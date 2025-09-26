using MyMonkeyApp.Models;
using MyMonkeyApp.Data;

namespace MyMonkeyApp.Services;

/// <summary>
/// 정적 원숭이 데이터 헬퍼.
/// - Monkey MCP 서버(향후 연동) 또는 시드 데이터로부터 목록을 로드
/// - 모든 원숭이 조회 / 이름 검색 / 랜덤 선택
/// - 랜덤 선택된 횟수 추적
/// </summary>
public static class MonkeyHelper
{
    private static readonly object _lock = new();
    private static IReadOnlyList<MonkeyExtended>? _cache;
    private static IMonkeyDataSource _dataSource = new SeedMonkeyDataSource();
    private static readonly Random _rng = new();
    private static readonly Dictionary<Guid, int> _randomPickCounts = new();
    private static bool _isLoading;

    /// <summary>
    /// 데이터 소스를 교체합니다 (예: Monkey MCP 서버 구현체).
    /// 교체 후 즉시 기존 캐시는 무효화됩니다.
    /// </summary>
    public static void ConfigureDataSource(IMonkeyDataSource source, bool preload = false, CancellationToken ct = default)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        lock (_lock)
        {
            _dataSource = source;
            _cache = null; // invalidate
            _randomPickCounts.Clear();
        }
        if (preload)
        {
            // Fire & forget preload (best-effort). For simplicity, block here.
            _ = GetAllMonkeysAsync(ct).GetAwaiter().GetResult();
        }
    }

    /// <summary>모든 원숭이 목록을 반환 (캐시).</summary>
    public static async Task<IReadOnlyList<MonkeyExtended>> GetAllMonkeysAsync(CancellationToken ct = default)
    {
        if (_cache is not null) return _cache;
        await EnsureLoadedAsync(ct).ConfigureAwait(false);
        return _cache!;
    }

    /// <summary>이름(대소문자 무시)으로 단일 원숭이를 찾습니다.</summary>
    public static async Task<MonkeyExtended?> FindByNameAsync(string name, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        var all = await GetAllMonkeysAsync(ct).ConfigureAwait(false);
        return all.FirstOrDefault(m => string.Equals(m.CommonName, name, StringComparison.OrdinalIgnoreCase) ||
                                       string.Equals(m.ScientificName, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 랜덤 원숭이를 반환하고 해당 개체의 선택 횟수를 1 증가시킵니다.
    /// </summary>
    public static async Task<MonkeyExtended> GetRandomMonkeyAsync(CancellationToken ct = default)
    {
        var all = await GetAllMonkeysAsync(ct).ConfigureAwait(false);
        if (all.Count == 0) throw new InvalidOperationException("No monkey data available.");
        MonkeyExtended picked;
        lock (_lock)
        {
            picked = all[_rng.Next(all.Count)];
            if (_randomPickCounts.TryGetValue(picked.Id, out var cnt))
                _randomPickCounts[picked.Id] = cnt + 1;
            else
                _randomPickCounts[picked.Id] = 1;
        }
        return picked;
    }

    /// <summary>특정 원숭이(Id)의 랜덤 선택 누계 횟수.</summary>
    public static int GetRandomPickCount(Guid monkeyId)
    {
        lock (_lock)
            return _randomPickCounts.TryGetValue(monkeyId, out var cnt) ? cnt : 0;
    }

    /// <summary>랜덤 선택 카운터 전체를 반환 (스냅샷).</summary>
    public static IReadOnlyDictionary<Guid, int> GetRandomPickCountsSnapshot()
    {
        lock (_lock)
            return new Dictionary<Guid, int>(_randomPickCounts);
    }

    /// <summary>캐시 무효화 (다음 호출 시 재로딩).</summary>
    public static void InvalidateCache()
    {
        lock (_lock)
        {
            _cache = null;
            _randomPickCounts.Clear();
        }
    }

    private static async Task EnsureLoadedAsync(CancellationToken ct)
    {
        if (_cache is not null) return;
        lock (_lock)
        {
            if (_cache is not null || _isLoading) return;
            _isLoading = true;
        }
        try
        {
            var data = await _dataSource.FetchMonkeysAsync(ct).ConfigureAwait(false);
            // 방어적 복사
            var frozen = data.ToList().AsReadOnly();
            lock (_lock)
            {
                _cache = frozen;
            }
        }
        finally
        {
            lock (_lock) _isLoading = false;
        }
    }
}

/// <summary>
/// 원숭이 데이터를 비동기로 공급하는 추상화.
/// Monkey MCP 서버 클라이언트가 이를 구현하여 <see cref="MonkeyHelper.ConfigureDataSource"/>로 주입될 예정입니다.
/// </summary>
public interface IMonkeyDataSource
{
    Task<IReadOnlyList<MonkeyExtended>> FetchMonkeysAsync(CancellationToken ct = default);
}

/// <summary>
/// 임시 시드 데이터 소스: 기존 <see cref="MonkeyData"/> + 최소 매핑을 이용하여 MonkeyExtended 생성.
/// MCP 서버 연동 전까지 기본 동작을 제공.
/// </summary>
internal sealed class SeedMonkeyDataSource : IMonkeyDataSource
{
    public Task<IReadOnlyList<MonkeyExtended>> FetchMonkeysAsync(CancellationToken ct = default)
    {
        // MonkeyData (간단 모델) -> MonkeyExtended 매핑.
        var list = MonkeyData.All.Select(m => new MonkeyExtended
        {
            CommonName = m.Name,
            ScientificName = m.ScientificName,
            Region = m.Region,
            Description = m.Description,
            Habitat = null,
            Diet = null,
            WeightMinKg = null,
            WeightMaxKg = null,
            Status = ConservationStatus.NotEvaluated,
            ImageUrl = null,
            Tags = Array.Empty<string>(),
            UpdatedUtc = null
        }).ToList().AsReadOnly();
        return Task.FromResult<IReadOnlyList<MonkeyExtended>>(list);
    }
}

/*
 TODO: Monkey MCP Server 연동 예시 스케치
 public sealed class McpMonkeyDataSource : IMonkeyDataSource
 {
     private readonly IMonkeyMcpClient _client; // (가칭) MCP 프로토콜 클라이언트
     public McpMonkeyDataSource(IMonkeyMcpClient client) => _client = client;
     public async Task<IReadOnlyList<MonkeyExtended>> FetchMonkeysAsync(CancellationToken ct = default)
     {
         var response = await _client.ListMonkeysAsync(ct); // MCP 메서드 호출 (미구현)
         return response.Select(r => new MonkeyExtended { ...매핑... }).ToList();
     }
 }
*/
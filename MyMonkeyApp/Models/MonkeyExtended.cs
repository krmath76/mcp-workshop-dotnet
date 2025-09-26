namespace MyMonkeyApp.Models;

/// <summary>
/// 확장된 원숭이 도메인 모델. 기존 <see cref="Monkey"/> (간단 목록 표시용) 과 달리
/// 식별자/보존등급/태그/서식/식단 등 추가 메타데이터를 포함합니다.
/// 추후 Persistence/DTO 계층으로 발전시키기 위한 초기 형태입니다.
/// </summary>
public enum ConservationStatus
{
    NotEvaluated,
    LeastConcern,
    NearThreatened,
    Vulnerable,
    Endangered,
    CriticallyEndangered
}

public sealed class MonkeyExtended
{
    /// <summary>내부 전역 식별자.</summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>일반(통용) 이름.</summary>
    public required string CommonName { get; init; }

    /// <summary>이명(학명). 예: Mandrillus sphinx</summary>
    public required string ScientificName { get; init; }

    /// <summary>주 서식 지역(간단 문자열 — 추후 Region 엔터티 분리 가능).</summary>
    public required string Region { get; init; }

    /// <summary>서식 환경 (예: Rainforest, Mangrove, Montane 등).</summary>
    public string? Habitat { get; init; }

    /// <summary>식단(간단 기술 / 콤마 구분 값). 복잡해지면 컬렉션으로 확장.</summary>
    public string? Diet { get; init; }

    /// <summary>체중 범위(kg). 둘 중 하나만 제공 시 Min = Max 로 간주 가능.</summary>
    public double? WeightMinKg { get; init; }
    public double? WeightMaxKg { get; init; }

    /// <summary>IUCN 등급 등 보존 상태(단순 enum 표현).</summary>
    public ConservationStatus Status { get; init; } = ConservationStatus.NotEvaluated;

    /// <summary>자유 기술 설명.</summary>
    public string? Description { get; init; }

    /// <summary>대표 이미지/참조 링크.</summary>
    public string? ImageUrl { get; init; }

    /// <summary>특성 태그(arboreal, nocturnal 등).</summary>
    public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();

    /// <summary>갱신 시각(UTC). 정적 데이터면 null.</summary>
    public DateTime? UpdatedUtc { get; init; }

    /// <summary>파생 속성: 속(Genus) (ScientificName의 첫 토큰).</summary>
    public string Genus => ScientificName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
}

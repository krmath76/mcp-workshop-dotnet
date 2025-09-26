using MyMonkeyApp.Models;
using MyMonkeyApp.Data;

namespace MyMonkeyApp.Services;

public sealed class MonkeyService
{
    private readonly IReadOnlyList<Monkey> _monkeys = MonkeyData.All;
    private readonly Random _rng = new();

    public IReadOnlyList<Monkey> GetAll() => _monkeys;
    public Monkey? FindByName(string name) => _monkeys.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
    public Monkey GetRandom() => _monkeys[_rng.Next(_monkeys.Count)];
}

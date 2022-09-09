using BenchmarkDotNet.Engines;

namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class LookupVsList
{
    private readonly List<Person> _list;
    private readonly Guid _target;
    private readonly ILookup<Guid, Person> _lookup;
    private readonly Consumer _consumer = new();

    public LookupVsList()
    {
        var random = new Bogus.DataSets.Name();

        var initial = Enumerable.Range(1, 1000)
            .Select(_ => new Person { Id = Guid.NewGuid(), Name = random.FullName() })
            .ToList();

        _list = initial.Concat(initial).ToList();

        _lookup = _list.ToLookup(x => x.Id, x => x);

        _target = _list[500].Id;
    }

    [Benchmark]
    public void List() => _list.Where(x => x.Id == _target).Consume(_consumer);

    [Benchmark]
    public void Lookup() => _lookup[_target].Consume(_consumer);
}
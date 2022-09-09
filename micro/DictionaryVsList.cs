namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class DictionaryVsList
{
    private readonly List<Person> _list;
    private readonly Dictionary<Guid, Person> _dictionary;
    private readonly Guid _target;

    public DictionaryVsList()
    {
        var random = new Bogus.DataSets.Name();

        _list = Enumerable.Range(1, 1000)
            .Select(x => new Person { Id = Guid.NewGuid(), Name = random.FullName() })
            .ToList();

        _dictionary = _list.ToDictionary(x => x.Id, x => x);

        _target = _list[500].Id;
    }

    [Benchmark]
    public Person? List() => _list.FirstOrDefault(x => x.Id == _target);

    [Benchmark]
    public bool Dictionary() => _dictionary.TryGetValue(_target, out var value);
}
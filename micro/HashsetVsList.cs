namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class HashsetVsList
{
    private readonly List<Guid> _list;
    private readonly Guid _target;
    private readonly HashSet<Guid> _hashset;

    public HashsetVsList()
    {

        _list = Enumerable.Range(1, 1000)
            .Select(x => Guid.NewGuid())
            .ToList();

        _hashset = _list.ToHashSet();

        _target = _list[500];
    }

    [Benchmark]
    public bool List() => _list.Contains(_target);

    [Benchmark]
    public bool Hashset() => _hashset.Contains(_target);
}
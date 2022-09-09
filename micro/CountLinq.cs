namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class LinqCountMethodVsProp
{
    private List<int> _items;

    [Params(100, 500, 1000, 5000, 10000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup() => _items = Enumerable.Range(0, Size).Select(i => i).ToList();

    [Benchmark(Description = ".Count()")]
    public int Count() => _items.Count();

    [Benchmark(Description = ".Count")]
    public int RawCount() => _items.Count;
}
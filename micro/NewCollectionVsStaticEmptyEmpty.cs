using BenchmarkDotNet.Engines;

namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class NewCollectionVsStaticEmptyEmpty
{
    private readonly Consumer _consumer = new();

    [Benchmark(Description = "new List<string>(0)")]
    public void EmptyList() => new List<string>(0).Consume(_consumer);

    [Benchmark(Description = "new string[0]")]
    // ReSharper disable once UseArrayEmptyMethod
    public void EmptyArray() => new string[0].Consume(_consumer);

    [Benchmark(Description = "Array.Empty<string>()")]
    public void StaticEmptyArray() => Array.Empty<string>().Consume(_consumer);

    [Benchmark(Description = "Enumerable.Empty<string>")]
    public void StaticEmptyList() => Enumerable.Empty<string>().Consume(_consumer);
}
using BenchmarkDotNet.Engines;

namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class ListVsListCapacityAdd
{
    private readonly Consumer _consumer = new();

    [Params(10, 10000, 100000)]
    public int Capacity { get; set; }

    [Benchmark(Baseline = true)]
    public void AllocWithoutDefiningCapacity_FillList()
    {
        var testList = new List<int>();

        for (var i = 0; i < Capacity; i++)
        {
            testList.Add(i);
        }
    }

    [Benchmark]
    public void AllocDefineCapacity_FillList()
    {
        var testList = new List<int>(Capacity);

        for (var i = 0; i < Capacity; i++)
        {
            testList.Add(i);
        }
    }

    [Benchmark]
    public void Select() => Enumerable.Range(0, Capacity)
        .Select(x => x)
        .Consume(_consumer);

}
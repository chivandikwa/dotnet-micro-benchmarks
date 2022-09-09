using System.Buffers;

namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class ArrayPoolVsRegularArrayAllocation
{
    [Params(100, 500, 1000, 5000, 10000)]
    public int Size { get; set; }

    [Benchmark(Baseline = true)]
    public int[] AllocateArrayWithNew() => new int[Size];

    [Benchmark]
    public int[] ArrayPool() => ArrayPool<int>.Shared.Rent(Size);
}
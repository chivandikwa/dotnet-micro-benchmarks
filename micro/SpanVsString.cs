namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class SpanVsString
{
    [Benchmark]
    public bool StringAllocation() => "let's benchmark stuff".Substring(5, 14).SequenceEqual("benchmark");

    [Benchmark]
    public bool AllocationFree() => "let's benchmark stuff".AsSpan().Slice(5, 14).SequenceEqual("benchmark");
}
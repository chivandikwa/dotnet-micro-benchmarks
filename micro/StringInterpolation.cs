namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class StringInterpolation
{
    private const string A = "A";
    private const string B = "B";
    private const string C = "C";

    [Benchmark]
    public string Interpolation() => $"{A}{B}{C}";

    [Benchmark]
    public string Concatenation() => A + B + C;
}
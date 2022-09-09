namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class IntParseVsIntTryParse
{
    [Params("1", "2147483647", "X")]
    public string Data { get; set; }

    [Benchmark(Baseline = true)]
    public void IntParse()
    {
        int result;
        try
        {
            result = int.Parse(Data);
        }
        catch
        {
            // Write a message in the log.
        }
    }

    [Benchmark]
    public void IntTryParse()
    {
        var succedded = int.TryParse(Data, out var result);
    }
}
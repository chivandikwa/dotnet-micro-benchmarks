using System.Runtime;

namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class ExceptionBlockVsContinueWith
{
    [Benchmark]
    public async Task<string> TryCatch()
    {
        try
        {
            await Sample();
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return null;
    }


    [Benchmark]
    public async Task<string?> ContinueWith()
    {
        return await Sample()
            .ContinueWith(task => task?.Exception?.Message);
    }


    public async Task Sample()
    {
        await Task.Delay(10);
        throw new AmbiguousImplementationException("For Gags");
    }
}
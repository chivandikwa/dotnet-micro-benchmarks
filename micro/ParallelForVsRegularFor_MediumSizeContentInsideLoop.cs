using System.Text.RegularExpressions;

namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class ParallelForVsRegularFor_MediumSizeContentInsideLoop
{
    [Params(100, 1000, 10000, 200000)]
    public int Count { get; set; }

    public List<string> AllLines { get; set; }

    [GlobalSetup]
    public void PrepareData()
    {
        AllLines = new();

        for (var i = 0; i < Count; i++)
        {
            AllLines.Add("This is a #test line that contains multiple #test hashtags.");
        }
    }

    [Benchmark(Baseline = true)]
    public void RegularFor()
    {
        var counter = 0;

        for (var i = 0; i < Count; i++)
        {
            if (DoesContainMultipleHashtags(AllLines[i]))
                counter++;
        }
    }

    [Benchmark]
    public void ParallelFor()
    {
        var counter = 0;

        Parallel.For(0, Count, (i) =>
        {
            if (DoesContainMultipleHashtags(AllLines[i]))
                Interlocked.Increment(ref counter);
        });
    }

    private bool DoesContainMultipleHashtags(string text)
    {
        Regex regexHashtag = new(@"(#[a-zA-Z0-9_-]+).*(\1)", RegexOptions.IgnoreCase);
        var count = regexHashtag.Matches(text).Count;

        return count > 0;
    }
}
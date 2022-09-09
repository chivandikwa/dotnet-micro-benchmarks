using System.Text;

namespace micro;


[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class StringVsStringBuilder
{
    [Params(2, 3, 5, 7, 10, 15, 100, 1000)]
    public int ElementsCount { get; set; }

    [Params("a", "abcde")]
    public string SingleElement { get; set; }

    [Benchmark(Baseline = true)]
    public void StringConcatenation()
    {
        var result = string.Empty;
        for (var i = 0; i < ElementsCount; i++)
        {
            result += SingleElement;
        }
    }

    [Benchmark]
    public void StringBuilder()
    {
        StringBuilder result = new();
        for (var i = 0; i < ElementsCount; i++)
        {
            result.Append(SingleElement);
        }
    }

    [Benchmark]
    public void StringBuilderCapacity()
    {
        var result = new StringBuilder(ElementsCount);
        for (var i = 0; i < ElementsCount; i++)
        {
            result.Append(SingleElement);
        }
    }
}
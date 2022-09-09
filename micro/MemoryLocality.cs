namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class MemoryLocality
{
    [Params(100, 500, 1000, 5000, 10000)]
    public int Size { get; set; }
    private int[,] data;

    [GlobalSetup]
    public void PrepareData()
    {
        data = new int[Size, Size];

        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                data[i, j] = Random.Shared.Next();
            }
        }
    }

    [Benchmark(Baseline = true)]
    public void AccessArray_RowByRow()
    {
        var counter = 0;

        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                if (data[i, j] == 0)
                    counter++;
            }
        }
    }

    [Benchmark]
    public void AccessArray_JumpBetweenRows()
    {
        var counter = 0;

        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                if (data[j, i] == 0)
                    counter++;
            }
        }
    }
}
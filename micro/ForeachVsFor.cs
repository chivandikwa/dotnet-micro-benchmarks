namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class ForeachVsFor
{
    [Params(10, 100, 1000, 10000, 1000000)]
    public int DataSize { get; set; }
    List<int> _data1;
    int[] _data2;

    [GlobalSetup]
    public void PrepareData()
    {
        _data1 = new List<int>(DataSize);

        for (var i = 0; i < DataSize; i++)
        {
            _data1.Add(Random.Shared.Next(minValue: 0, maxValue: 10));
        }

        _data2 = new int[DataSize];

        for (var i = 0; i < DataSize; i++)
        {
            _data2[i] = Random.Shared.Next(minValue: 0, maxValue: 10);
        }
    }

    [Benchmark(Baseline = true)]
    public int ForEachList()
    {
        var sum = 0;
        foreach (var dataElement in _data1)
        {
            sum += dataElement;
        }

        return sum;
    }

    [Benchmark]
    public int ForList()
    {
        var sum = 0;
        for (var i = 0; i < _data1.Count; i++)
        {
            sum += _data1[i];
        }

        return sum;
    }

    [Benchmark(Baseline = true)]
    public int ForEachArray()
    {
        var sum = 0;
        foreach (var dataElement in _data2)
        {
            sum += dataElement;
        }

        return sum;
    }

    [Benchmark]
    public int ForArray()
    {
        var sum = 0;
        for (var i = 0; i < _data2.Length; i++)
        {
            sum += _data2[i];
        }

        return sum;
    }
}
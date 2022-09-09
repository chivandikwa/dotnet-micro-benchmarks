using System.Runtime.CompilerServices;

namespace micro;

/// <summary>
/// SkipLocalsInit attribute stops the compiler from inserting .locals init directive,
/// and without that directive JIT will not add a code that initializes local variables to default values.
/// </summary>
/// [MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class SkipLocalsInit
{
    [Params(100, 500, 1000, 5000, 10000)]
    public int Size { get; set; }

    [Benchmark(Baseline = true)]
    public void WithoutSkipLocalsInit()
    {
        Span<int> data = stackalloc int[Size];
    }

    [Benchmark]
    [SkipLocalsInit]
    // Note: if want to use [SkipLocalsInit] we need to compile with /unsafe 
    public void AddSkipLocalsInit()
    {
        Span<int> data = stackalloc int[Size];
    }
}

[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class SingleVsFirst
{
    public Random Random;

    public int Length = 10000;

    public List<string> List;
    public List<string> Targets => new List<string> { "StartTarget", "MiddleTarget", "EndTarget" };


    [ParamsSource(nameof(Targets))] public string Target { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        Random = new Random();

        List = new List<string>();
        for (int i = 0; i < Length; i++)
        {
            var number = Random.Next(0, 100);
            List.Add(number.ToString());
        }

        List.Insert(0, Targets[0]);
        List.Insert(Length / 2, Targets[1]);
        List.Insert(List.Count - 1, Targets[2]);
    }

    [Benchmark]
    public string Single() => List.SingleOrDefault(x => x == Target);

    [Benchmark]
    public string First() => List.FirstOrDefault(x => x == Target);

    [Benchmark]
    public string WhereAndFirst() => List.Where(x => x == Target).First();
}
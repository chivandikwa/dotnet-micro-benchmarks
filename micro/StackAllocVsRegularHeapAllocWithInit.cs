namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class StackAllocVsRegularHeapAllocWithInit
{
    [Params(5, 10, 100, 1000, 10000)]
    public int Size { get; set; }

    [Benchmark(Baseline = true)]
    public void RegularHeapAlloc()
    {
        var array = new Point3D[Size];

        for (var i = 0; i < Size; i++)
        {
            array[i].X = 1;
            array[i].Y = 2;
            array[i].Z = 3;
        }
    }

    [Benchmark]
    public unsafe void StackAllocWithPointer()
    {
        var array = stackalloc Point3D[Size];

        for (var i = 0; i < Size; i++)
        {
            array[i].X = 1;
            array[i].Y = 2;
            array[i].Z = 3;
        }
    }

    [Benchmark]
    public void StackAllocWithSpan()
    {
        Span<Point3D> array = stackalloc Point3D[Size];

        for (var i = 0; i < Size; i++)
        {
            array[i].X = 1;
            array[i].Y = 2;
            array[i].Z = 3;
        }
    }

    public struct Point3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}
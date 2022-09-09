using System.Numerics;

namespace micro;

[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class SIMDSumVsForSum
{
    [Params(8, 16, 800, 80000, 800000)]
    public int DataCount { get; set; }
    public double[] Data;

    double maxDataValue = 100.0;

    [GlobalSetup]
    public void PrepData()
    {
        Data = new double[DataCount];
        for (var i = 0; i < DataCount; i++)
        {
            Data[i] = Random.Shared.NextDouble() * maxDataValue;
        }
    }

    [Benchmark(Baseline = true)]
    public double ForSum()
    {
        double totalSum = 0;
        for (var i = 0; i < DataCount; i++)
        {
            totalSum += Data[i];
        }

        return totalSum;
    }

    [Benchmark]
    public double SIMDSum()
    {
        if (!Vector.IsHardwareAccelerated)
            return 0;

        var vectorSize = Vector<double>.Count;
        var vectorIntermediateSums = Vector<double>.Zero;

        // Add multiple data at the same time using SIMD instructions.
        // Result is in the vector that contains intermediate sums.
        for (var i = 0; i <= DataCount - vectorSize; i += vectorSize)
        {
            var vectorNextDataGroup = new Vector<double>(Data, i);
            vectorIntermediateSums = Vector.Add(vectorIntermediateSums, vectorNextDataGroup);
        }

        // Total sum = sum of intermediate sums contained in a vectorSum.
        double totalSum = 0;
        for (var i = 0; i < vectorSize; i++)
        {
            totalSum += vectorIntermediateSums[i];
        }

        return totalSum;
    }
}
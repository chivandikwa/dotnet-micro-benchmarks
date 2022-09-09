using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Engines;

namespace micro;

[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class MultiEnumeration
{
    private readonly List<Person> _list;
    private readonly Consumer _consumer = new();

    public MultiEnumeration()
    {
        var random = new Bogus.DataSets.Name();

        _list = Enumerable.Range(1, 1000)
            .Select(x => new Person { Id = Guid.NewGuid(), Name = random.FullName() })
            .ToList();
    }

    [Benchmark]
    public void SingleToList()
    {
        var result = _list.Select(x => x.Name.ToLower()).ToList();

        result.Consume(_consumer);
        result.Consume(_consumer);
        result.Consume(_consumer);
    }

    [Benchmark]
    public void SingleToArray()
    {
        var result = _list.Select(x => x.Name.ToLower()).ToArray();

        result.Consume(_consumer);
        result.Consume(_consumer);
        result.Consume(_consumer);
    }

    [Benchmark]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public void Multi()
    {
        var result = _list.Select(x => x.Name.ToLower());

        result.Consume(_consumer);
        result.Consume(_consumer);
        result.Consume(_consumer);
    }
}
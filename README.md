# Consider use of array pools for large and regular array allocation in loops

```csharp
[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class ArrayPoolVsRegularArrayAllocation
{
    [Params(100, 500, 1000, 5000, 10000)]
    public int Size { get; set; }

    [Benchmark(Baseline = true)]
    public int[] AllocateArrayWithNew() => new int[Size];

    [Benchmark]
    public int[] ArrayPool() => ArrayPool<int>.Shared.Rent(Size);
}
```

| Method                   | Runtime      | Size      |            Mean |  RatioSD |       Gen0 |       Gen1 |   Allocated | Alloc Ratio |
| ------------------------ | ------------ | --------- | --------------: | -------: | ---------: | ---------: | ----------: | ----------: |
| **AllocateArrayWithNew** | **.NET 6.0** | **100**   |    **21.54 ns** | **0.00** | **0.0676** | **0.0001** |   **424 B** |    **1.00** |
| ArrayPool                | .NET 6.0     | 100       |        33.79 ns |     0.38 |     0.0854 |     0.0001 |       536 B |        1.26 |
| AllocateArrayWithNew     | .NET 7.0     | 100       |        29.68 ns |     0.19 |     0.0676 |     0.0001 |       424 B |        1.00 |
| ArrayPool                | .NET 7.0     | 100       |        40.38 ns |     0.66 |     0.0854 |     0.0001 |       536 B |        1.26 |
|                          |              |           |                 |          |            |            |             |             |
| **AllocateArrayWithNew** | **.NET 6.0** | **500**   |   **148.86 ns** | **0.00** | **0.3226** | **0.0024** |  **2024 B** |    **1.00** |
| ArrayPool                | .NET 6.0     | 500       |        90.09 ns |     0.16 |     0.3293 |     0.0025 |      2072 B |        1.02 |
| AllocateArrayWithNew     | .NET 7.0     | 500       |        83.15 ns |     0.14 |     0.3226 |     0.0024 |      2024 B |        1.00 |
| ArrayPool                | .NET 7.0     | 500       |        77.54 ns |     0.14 |     0.3293 |     0.0025 |      2072 B |        1.02 |
|                          |              |           |                 |          |            |            |             |             |
| **AllocateArrayWithNew** | **.NET 6.0** | **1000**  |   **247.14 ns** | **0.00** | **0.6413** | **0.0095** |  **4024 B** |    **1.00** |
| ArrayPool                | .NET 6.0     | 1000      |       107.95 ns |     0.06 |     0.6545 |     0.0100 |      4120 B |        1.02 |
| AllocateArrayWithNew     | .NET 7.0     | 1000      |       203.24 ns |     0.04 |     0.6413 |     0.0095 |      4024 B |        1.00 |
| ArrayPool                | .NET 7.0     | 1000      |        89.59 ns |     0.04 |     0.6543 |     0.0101 |      4120 B |        1.02 |
|                          |              |           |                 |          |            |            |             |             |
| **AllocateArrayWithNew** | **.NET 6.0** | **5000**  | **1,353.94 ns** | **0.00** | **3.1834** | **0.2117** | **20024 B** |    **1.00** |
| ArrayPool                | .NET 6.0     | 5000      |       281.01 ns |     0.02 |     5.2080 |     0.5784 |     32792 B |        1.64 |
| AllocateArrayWithNew     | .NET 7.0     | 5000      |     1,013.94 ns |     0.09 |     3.1834 |     0.2270 |     20024 B |        1.00 |
| ArrayPool                | .NET 7.0     | 5000      |       285.51 ns |     0.03 |     5.2080 |     0.6509 |     32792 B |        1.64 |
|                          |              |           |                 |          |            |            |             |             |
| **AllocateArrayWithNew** | **.NET 6.0** | **10000** | **2,320.97 ns** | **0.00** | **6.3286** | **0.7896** | **40024 B** |    **1.00** |
| ArrayPool                | .NET 6.0     | 10000     |       408.67 ns |     0.02 |    10.4165 |     2.0833 |     65560 B |        1.64 |
| AllocateArrayWithNew     | .NET 7.0     | 10000     |     2,306.57 ns |     0.05 |     6.3286 |     0.9003 |     40024 B |        1.00 |
| ArrayPool                | .NET 7.0     | 10000     |       528.44 ns |     0.02 |    10.4165 |     2.6040 |     65560 B |        1.64 |

# Favor Count property over Count() method

```csharp
[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class LinqCountMethodVsProp
{
    private List<int> _items;

    [Params(100, 500, 1000, 5000, 10000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup() => _items = Enumerable.Range(0, Size).Select(i => i).ToList();

    [Benchmark(Description = ".Count()")]
    public int Count() => _items.Count();

    [Benchmark(Description = ".Count")]
    public int RawCount() => _items.Count;
}
```

| Method       | Runtime      | Size      |          Mean |  RatioSD | Allocated | Alloc Ratio |
| ------------ | ------------ | --------- | ------------: | -------: | --------: | ----------: |
| **.Count()** | **.NET 6.0** | **100**   | **4.4440 ns** | **0.00** |     **-** |      **NA** |
| .Count()     | .NET 7.0     | 100       |     2.9954 ns |     0.07 |         - |          NA |
|              |              |           |               |          |           |             |
| .Count       | .NET 6.0     | 100       |     0.0000 ns |        ? |         - |           ? |
| .Count       | .NET 7.0     | 100       |     0.0735 ns |        ? |         - |           ? |
|              |              |           |               |          |           |             |
| **.Count()** | **.NET 6.0** | **500**   | **3.7110 ns** | **0.00** |     **-** |      **NA** |
| .Count()     | .NET 7.0     | 500       |     3.1521 ns |     0.03 |         - |          NA |
|              |              |           |               |          |           |             |
| .Count       | .NET 6.0     | 500       |     0.0076 ns |        ? |         - |           ? |
| .Count       | .NET 7.0     | 500       |     0.0002 ns |        ? |         - |           ? |
|              |              |           |               |          |           |             |
| **.Count()** | **.NET 6.0** | **1000**  | **3.1150 ns** | **0.00** |     **-** |      **NA** |
| .Count()     | .NET 7.0     | 1000      |     3.4346 ns |     0.17 |         - |          NA |
|              |              |           |               |          |           |             |
| .Count       | .NET 6.0     | 1000      |     0.0388 ns |        ? |         - |           ? |
| .Count       | .NET 7.0     | 1000      |     0.1320 ns |        ? |         - |           ? |
|              |              |           |               |          |           |             |
| **.Count()** | **.NET 6.0** | **5000**  | **3.6008 ns** | **0.00** |     **-** |      **NA** |
| .Count()     | .NET 7.0     | 5000      |     2.9886 ns |     0.11 |         - |          NA |
|              |              |           |               |          |           |             |
| .Count       | .NET 6.0     | 5000      |     0.0026 ns |        ? |         - |           ? |
| .Count       | .NET 7.0     | 5000      |     0.0196 ns |        ? |         - |           ? |
|              |              |           |               |          |           |             |
| **.Count()** | **.NET 6.0** | **10000** | **3.5470 ns** | **0.00** |     **-** |      **NA** |
| .Count()     | .NET 7.0     | 10000     |     3.6048 ns |     0.09 |         - |          NA |
|              |              |           |               |          |           |             |
| .Count       | .NET 6.0     | 10000     |     0.0076 ns |        ? |         - |           ? |
| .Count       | .NET 7.0     | 10000     |     0.0000 ns |        ? |         - |           ? |

# Favor Dictionary over List for lookup by key

```csharp
[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class DictionaryVsList
{
    private readonly List<Person> _list;
    private readonly Dictionary<Guid, Person> _dictionary;
    private readonly Guid _target;

    public DictionaryVsList()
    {
        var random = new Bogus.DataSets.Name();

        _list = Enumerable.Range(1, 1000)
            .Select(x => new Person { Id = Guid.NewGuid(), Name = random.FullName() })
            .ToList();

        _dictionary = _list.ToDictionary(x => x.Id, x => x);

        _target = _list[500].Id;
    }

    [Benchmark]
    public Person? List() => _list.FirstOrDefault(x => x.Id == _target);

    [Benchmark]
    public bool Dictionary() => _dictionary.TryGetValue(_target, out var value);
}
```

| Method     | Runtime  |         Mean | RatioSD |   Gen0 | Allocated | Alloc Ratio |
| ---------- | -------- | -----------: | ------: | -----: | --------: | ----------: |
| List       | .NET 6.0 | 4,459.294 ns |    0.00 | 0.0153 |     104 B |        1.00 |
| List       | .NET 7.0 | 4,833.701 ns |    0.02 | 0.0153 |     104 B |        1.00 |
|            |          |              |         |        |           |             |
| Dictionary | .NET 6.0 |     7.960 ns |    0.00 |      - |         - |          NA |
| Dictionary | .NET 7.0 |     5.114 ns |    0.02 |      - |         - |          NA |

# Favor Hashset over List for lookup by contains and unique sets

```csharp
[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class HashsetVsList
{
    private readonly List<Guid> _list;
    private readonly Guid _target;
    private readonly HashSet<Guid> _hashset;

    public HashsetVsList()
    {

        _list = Enumerable.Range(1, 1000)
            .Select(x => Guid.NewGuid())
            .ToList();

        _hashset = _list.ToHashSet();

        _target = _list[500];
    }

    [Benchmark]
    public bool List() => _list.Contains(_target);

    [Benchmark]
    public bool Hashset() => _hashset.Contains(_target);
}
```

| Method  | Runtime  |         Mean | RatioSD | Allocated | Alloc Ratio |
| ------- | -------- | -----------: | ------: | --------: | ----------: |
| List    | .NET 6.0 | 1,135.671 ns |    0.00 |         - |          NA |
| List    | .NET 7.0 | 1,022.555 ns |    0.31 |         - |          NA |
|         |          |              |         |           |             |
| Hashset | .NET 6.0 |     7.829 ns |    0.00 |         - |          NA |
| Hashset | .NET 7.0 |     6.243 ns |    0.08 |         - |          NA |

# Favor a lookup over a list for lookup via key where there can be multple matches

```csharp
[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class LookupVsList
{
    private readonly List<Person> _list;
    private readonly Guid _target;
    private readonly ILookup<Guid, Person> _lookup;
    private readonly Consumer _consumer = new();

    public LookupVsList()
    {
        var random = new Bogus.DataSets.Name();

        var initial = Enumerable.Range(1, 1000)
            .Select(_ => new Person { Id = Guid.NewGuid(), Name = random.FullName() })
            .ToList();

        _list = initial.Concat(initial).ToList();

        _lookup = _list.ToLookup(x => x.Id, x => x);

        _target = _list[500].Id;
    }

    [Benchmark]
    public void List() => _list.Where(x => x.Id == _target).Consume(_consumer);

    [Benchmark]
    public void Lookup() => _lookup[_target].Consume(_consumer);
}
```

| Method | Runtime  |         Mean | RatioSD |   Gen0 | Allocated | Alloc Ratio |
| ------ | -------- | -----------: | ------: | -----: | --------: | ----------: |
| List   | .NET 6.0 | 11,260.77 ns |    0.00 | 0.0153 |     136 B |        1.00 |
| List   | .NET 7.0 | 11,074.25 ns |    0.09 | 0.0153 |     136 B |        1.00 |
|        |          |              |         |        |           |             |
| Lookup | .NET 6.0 |     37.89 ns |    0.00 | 0.0063 |      40 B |        1.00 |
| Lookup | .NET 7.0 |     35.47 ns |    0.16 | 0.0063 |      40 B |        1.00 |

# Carefully consider index order when iterating over multidimensional array to benefit from memory locality

```csharp
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
```

| Method                      | Runtime      | Size      |              Mean |  RatioSD | Allocated | Alloc Ratio |
| --------------------------- | ------------ | --------- | ----------------: | -------: | --------: | ----------: |
| **AccessArray_RowByRow**    | **.NET 6.0** | **100**   |      **10.85 μs** | **0.00** |     **-** |      **NA** |
| AccessArray_JumpBetweenRows | .NET 6.0     | 100       |          13.85 μs |     0.15 |         - |          NA |
| AccessArray_RowByRow        | .NET 7.0     | 100       |          12.07 μs |     0.09 |         - |          NA |
| AccessArray_JumpBetweenRows | .NET 7.0     | 100       |          12.11 μs |     0.00 |         - |          NA |
|                             |              |           |                   |          |           |             |
| **AccessArray_RowByRow**    | **.NET 6.0** | **500**   |     **268.13 μs** | **0.00** |     **-** |      **NA** |
| AccessArray_JumpBetweenRows | .NET 6.0     | 500       |         260.48 μs |     0.19 |         - |          NA |
| AccessArray_RowByRow        | .NET 7.0     | 500       |         255.04 μs |     0.16 |         - |          NA |
| AccessArray_JumpBetweenRows | .NET 7.0     | 500       |         265.15 μs |     0.06 |         - |          NA |
|                             |              |           |                   |          |           |             |
| **AccessArray_RowByRow**    | **.NET 6.0** | **1000**  |     **952.77 μs** | **0.00** |   **1 B** |    **1.00** |
| AccessArray_JumpBetweenRows | .NET 6.0     | 1000      |       1,621.52 μs |     0.01 |       1 B |        1.00 |
| AccessArray_RowByRow        | .NET 7.0     | 1000      |         924.57 μs |     0.10 |       1 B |        1.00 |
| AccessArray_JumpBetweenRows | .NET 7.0     | 1000      |       1,079.96 μs |     0.05 |       1 B |        1.00 |
|                             |              |           |                   |          |           |             |
| **AccessArray_RowByRow**    | **.NET 6.0** | **5000**  |  **21,893.60 μs** | **0.00** |  **15 B** |    **1.00** |
| AccessArray_JumpBetweenRows | .NET 6.0     | 5000      |     156,773.64 μs |     0.52 |     120 B |        8.00 |
| AccessArray_RowByRow        | .NET 7.0     | 5000      |      29,522.98 μs |     0.10 |      15 B |        1.00 |
| AccessArray_JumpBetweenRows | .NET 7.0     | 5000      |     154,573.32 μs |     0.34 |     120 B |        8.00 |
|                             |              |           |                   |          |           |             |
| **AccessArray_RowByRow**    | **.NET 6.0** | **10000** | **105,212.60 μs** | **0.00** |  **96 B** |    **1.00** |
| AccessArray_JumpBetweenRows | .NET 6.0     | 10000     |     885,650.03 μs |     1.81 |     480 B |        5.00 |
| AccessArray_RowByRow        | .NET 7.0     | 10000     |     126,800.19 μs |     0.06 |      96 B |        1.00 |
| AccessArray_JumpBetweenRows | .NET 7.0     | 10000     |     758,349.13 μs |     0.27 |     480 B |        5.00 |

# Be mindful of the impacts of multi enumeration

```csharp
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
```

| Method        | Runtime  |      Mean | RatioSD |    Gen0 |   Gen1 | Allocated | Alloc Ratio |
| ------------- | -------- | --------: | ------: | ------: | -----: | --------: | ----------: |
| SingleToList  | .NET 6.0 |  60.90 μs |    0.00 |  9.5825 | 1.4648 |   58.9 KB |        1.00 |
| SingleToList  | .NET 7.0 |  62.09 μs |    0.14 |  9.6436 | 1.5869 |  59.07 KB |        1.00 |
|               |          |           |         |         |        |           |             |
| SingleToArray | .NET 6.0 |  51.46 μs |    0.00 |  9.5825 | 1.2207 |  58.98 KB |        1.00 |
| SingleToArray | .NET 7.0 |  47.74 μs |    0.18 |  9.5215 | 1.4648 |  58.86 KB |        1.00 |
|               |          |           |         |         |        |           |             |
| Multi         | .NET 6.0 | 112.36 μs |    0.00 | 24.7803 |      - | 152.27 KB |        1.00 |
| Multi         | .NET 7.0 | 119.17 μs |    0.15 | 24.6582 |      - | 151.97 KB |        1.00 |

# Favor Array.Empty<T>() and Enumerable.Empty<T> over new new List<T>(), new List<T>(0) or new T[0]

```csharp
[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class NewCollectionVsStaticEmptyEmpty
{
    private readonly Consumer _consumer = new();

    [Benchmark(Description = "new List<string>(0)")]
    public void EmptyList() => new List<string>(0).Consume(_consumer);

    [Benchmark(Description = "new string[0]")]
    // ReSharper disable once UseArrayEmptyMethod
    public void EmptyArray() => new string[0].Consume(_consumer);

    [Benchmark(Description = "Array.Empty<string>()")]
    public void StaticEmptyArray() => Array.Empty<string>().Consume(_consumer);

    [Benchmark(Description = "Enumerable.Empty<string>")]
    public void StaticEmptyList() => Enumerable.Empty<string>().Consume(_consumer);
}
```

| Method                              | Runtime  |      Mean | RatioSD |   Gen0 | Allocated | Alloc Ratio |
| ----------------------------------- | -------- | --------: | ------: | -----: | --------: | ----------: |
| &#39;new List&lt;string&gt;(0)&#39; | .NET 6.0 | 18.321 ns |    0.00 | 0.0115 |      72 B |        1.00 |
| &#39;new List&lt;string&gt;(0)&#39; | .NET 7.0 | 19.297 ns |    0.06 | 0.0115 |      72 B |        1.00 |
|                                     |          |           |         |        |           |             |
| &#39;new string[0]&#39;             | .NET 6.0 | 11.860 ns |    0.00 | 0.0038 |      24 B |        1.00 |
| &#39;new string[0]&#39;             | .NET 7.0 | 12.186 ns |    0.03 | 0.0038 |      24 B |        1.00 |
|                                     |          |           |         |        |           |             |
| Array.Empty&lt;string&gt;()         | .NET 6.0 |  8.563 ns |    0.00 |      - |         - |          NA |
| Array.Empty&lt;string&gt;()         | .NET 7.0 |  9.688 ns |    0.06 |      - |         - |          NA |
|                                     |          |           |         |        |           |             |
| Enumerable.Empty&lt;string&gt;      | .NET 6.0 |  5.590 ns |    0.00 |      - |         - |          NA |
| Enumerable.Empty&lt;string&gt;      | .NET 7.0 |  6.030 ns |    0.08 |      - |         - |          NA |

# Make use of Span in any case applicable to avoid allocations

```csharp
[MemoryDiagnoser]
[RPlotExporter]
[HideColumns("Error", "StdDev", "Median", "Ratio", "RationSD")]
public class SpanVsString
{
    [Benchmark]
    public bool StringAllocation() => "let's benchmark stuff".Substring(5, 14).SequenceEqual("benchmark");

    [Benchmark]
    public bool AllocationFree() => "let's benchmark stuff".AsSpan().Slice(5, 14).SequenceEqual("benchmark");
}
```

| Method           | Runtime  |       Mean | RatioSD |   Gen0 | Allocated | Alloc Ratio |
| ---------------- | -------- | ---------: | ------: | -----: | --------: | ----------: |
| StringAllocation | .NET 6.0 | 34.3354 ns |    0.00 | 0.0191 |     120 B |        1.00 |
| StringAllocation | .NET 7.0 | 40.2530 ns |    0.13 | 0.0191 |     120 B |        1.00 |
|                  |          |            |         |        |           |             |
| AllocationFree   | .NET 6.0 |  0.0070 ns |       ? |      - |         - |           ? |
| AllocationFree   | .NET 7.0 |  0.8205 ns |       ? |      - |         - |           ? |

# Favor StringBuilder over string for multiple concatenation

```csharp
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
```

| Method                  | Runtime      | ElementsCount | SingleElement |              Mean |  RatioSD |         Gen0 |        Gen1 |     Allocated | Alloc Ratio |
| ----------------------- | ------------ | ------------- | ------------- | ----------------: | -------: | -----------: | ----------: | ------------: | ----------: |
| **StringConcatenation** | **.NET 6.0** | **2**         | **a**         |      **10.89 ns** | **0.00** |   **0.0051** |       **-** |      **32 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 2             | a             |          17.32 ns |     0.10 |       0.0166 |           - |         104 B |        3.25 |
| StringBuilderCapacity   | .NET 6.0     | 2             | a             |          18.07 ns |     0.10 |       0.0127 |           - |          80 B |        2.50 |
| StringConcatenation     | .NET 7.0     | 2             | a             |          36.66 ns |     0.05 |       0.0051 |           - |          32 B |        1.00 |
| StringBuilder           | .NET 7.0     | 2             | a             |          49.42 ns |     0.31 |       0.0166 |           - |         104 B |        3.25 |
| StringBuilderCapacity   | .NET 7.0     | 2             | a             |          46.41 ns |     0.43 |       0.0127 |           - |          80 B |        2.50 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **2**         | **abcde**     |      **29.91 ns** | **0.00** |   **0.0076** |       **-** |      **48 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 2             | abcde         |          41.19 ns |     0.01 |       0.0166 |           - |         104 B |        2.17 |
| StringBuilderCapacity   | .NET 6.0     | 2             | abcde         |         131.41 ns |     0.35 |       0.0393 |           - |         248 B |        5.17 |
| StringConcatenation     | .NET 7.0     | 2             | abcde         |          32.19 ns |     0.20 |       0.0076 |           - |          48 B |        1.00 |
| StringBuilder           | .NET 7.0     | 2             | abcde         |          53.93 ns |     0.05 |       0.0166 |           - |         104 B |        2.17 |
| StringBuilderCapacity   | .NET 7.0     | 2             | abcde         |         196.78 ns |     0.48 |       0.0393 |           - |         248 B |        5.17 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **3**         | **a**         |      **28.59 ns** | **0.00** |   **0.0102** |       **-** |      **64 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 3             | a             |          30.22 ns |     0.45 |       0.0166 |           - |         104 B |        1.62 |
| StringBuilderCapacity   | .NET 6.0     | 3             | a             |          55.48 ns |     0.44 |       0.0127 |           - |          80 B |        1.25 |
| StringConcatenation     | .NET 7.0     | 3             | a             |          58.59 ns |     0.32 |       0.0101 |           - |          64 B |        1.00 |
| StringBuilder           | .NET 7.0     | 3             | a             |          28.74 ns |     0.24 |       0.0166 |           - |         104 B |        1.62 |
| StringBuilderCapacity   | .NET 7.0     | 3             | a             |          26.00 ns |     0.10 |       0.0127 |           - |          80 B |        1.25 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **3**         | **abcde**     |      **28.35 ns** | **0.00** |   **0.0166** |       **-** |     **104 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 3             | abcde         |          26.01 ns |     0.17 |       0.0166 |           - |         104 B |        1.00 |
| StringBuilderCapacity   | .NET 6.0     | 3             | abcde         |         130.65 ns |     0.90 |       0.0548 |           - |         344 B |        3.31 |
| StringConcatenation     | .NET 7.0     | 3             | abcde         |          38.44 ns |     0.22 |       0.0166 |           - |         104 B |        1.00 |
| StringBuilder           | .NET 7.0     | 3             | abcde         |          41.92 ns |     0.54 |       0.0166 |           - |         104 B |        1.00 |
| StringBuilderCapacity   | .NET 7.0     | 3             | abcde         |         114.91 ns |     0.98 |       0.0548 |           - |         344 B |        3.31 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **5**         | **a**         |      **74.67 ns** | **0.00** |   **0.0204** |       **-** |     **128 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 5             | a             |          42.05 ns |     0.08 |       0.0166 |           - |         104 B |        0.81 |
| StringBuilderCapacity   | .NET 6.0     | 5             | a             |          62.57 ns |     0.10 |       0.0140 |           - |          88 B |        0.69 |
| StringConcatenation     | .NET 7.0     | 5             | a             |          68.52 ns |     0.32 |       0.0204 |           - |         128 B |        1.00 |
| StringBuilder           | .NET 7.0     | 5             | a             |          39.11 ns |     0.08 |       0.0166 |           - |         104 B |        0.81 |
| StringBuilderCapacity   | .NET 7.0     | 5             | a             |          35.31 ns |     0.04 |       0.0140 |           - |          88 B |        0.69 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **5**         | **abcde**     |      **69.38 ns** | **0.00** |   **0.0381** |       **-** |     **240 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 5             | abcde         |          68.75 ns |     0.09 |       0.0331 |           - |         208 B |        0.87 |
| StringBuilderCapacity   | .NET 6.0     | 5             | abcde         |         127.59 ns |     0.33 |       0.0612 |           - |         384 B |        1.60 |
| StringConcatenation     | .NET 7.0     | 5             | abcde         |          54.27 ns |     0.06 |       0.0382 |           - |         240 B |        1.00 |
| StringBuilder           | .NET 7.0     | 5             | abcde         |          80.17 ns |     0.39 |       0.0331 |           - |         208 B |        0.87 |
| StringBuilderCapacity   | .NET 7.0     | 5             | abcde         |         115.97 ns |     0.10 |       0.0610 |           - |         384 B |        1.60 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **7**         | **a**         |      **78.85 ns** | **0.00** |   **0.0331** |       **-** |     **208 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 7             | a             |          30.01 ns |     0.03 |       0.0166 |           - |         104 B |        0.50 |
| StringBuilderCapacity   | .NET 6.0     | 7             | a             |          34.28 ns |     0.03 |       0.0140 |           - |          88 B |        0.42 |
| StringConcatenation     | .NET 7.0     | 7             | a             |          55.06 ns |     0.02 |       0.0331 |           - |         208 B |        1.00 |
| StringBuilder           | .NET 7.0     | 7             | a             |          23.99 ns |     0.03 |       0.0166 |           - |         104 B |        0.50 |
| StringBuilderCapacity   | .NET 7.0     | 7             | a             |          60.79 ns |     0.07 |       0.0139 |           - |          88 B |        0.42 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **7**         | **abcde**     |     **120.37 ns** | **0.00** |   **0.0675** |       **-** |     **424 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 7             | abcde         |         141.63 ns |     0.24 |       0.0548 |           - |         344 B |        0.81 |
| StringBuilderCapacity   | .NET 6.0     | 7             | abcde         |          98.58 ns |     0.19 |       0.0650 |           - |         408 B |        0.96 |
| StringConcatenation     | .NET 7.0     | 7             | abcde         |         103.34 ns |     0.27 |       0.0675 |           - |         424 B |        1.00 |
| StringBuilder           | .NET 7.0     | 7             | abcde         |         126.00 ns |     0.29 |       0.0548 |           - |         344 B |        0.81 |
| StringBuilderCapacity   | .NET 7.0     | 7             | abcde         |          86.22 ns |     0.14 |       0.0650 |           - |         408 B |        0.96 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **10**        | **a**         |     **124.99 ns** | **0.00** |   **0.0535** |       **-** |     **336 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 10            | a             |          52.72 ns |     0.03 |       0.0166 |           - |         104 B |        0.31 |
| StringBuilderCapacity   | .NET 6.0     | 10            | a             |          63.81 ns |     0.05 |       0.0153 |           - |          96 B |        0.29 |
| StringConcatenation     | .NET 7.0     | 10            | a             |         129.49 ns |     0.05 |       0.0534 |           - |         336 B |        1.00 |
| StringBuilder           | .NET 7.0     | 10            | a             |          30.99 ns |     0.03 |       0.0166 |           - |         104 B |        0.31 |
| StringBuilderCapacity   | .NET 7.0     | 10            | a             |          50.71 ns |     0.10 |       0.0153 |           - |          96 B |        0.29 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **10**        | **abcde**     |     **149.26 ns** | **0.00** |   **0.1223** |       **-** |     **768 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 10            | abcde         |         136.21 ns |     0.08 |       0.0548 |           - |         344 B |        0.45 |
| StringBuilderCapacity   | .NET 6.0     | 10            | abcde         |         112.35 ns |     0.10 |       0.0725 |           - |         456 B |        0.59 |
| StringConcatenation     | .NET 7.0     | 10            | abcde         |         132.77 ns |     0.14 |       0.1223 |           - |         768 B |        1.00 |
| StringBuilder           | .NET 7.0     | 10            | abcde         |         140.28 ns |     0.36 |       0.0548 |           - |         344 B |        0.45 |
| StringBuilderCapacity   | .NET 7.0     | 10            | abcde         |         105.65 ns |     0.19 |       0.0725 |           - |         456 B |        0.59 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **15**        | **a**         |     **225.82 ns** | **0.00** |   **0.0942** |       **-** |     **592 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 15            | a             |          85.64 ns |     0.07 |       0.0166 |           - |         104 B |        0.18 |
| StringBuilderCapacity   | .NET 6.0     | 15            | a             |          99.93 ns |     0.19 |       0.0166 |           - |         104 B |        0.18 |
| StringConcatenation     | .NET 7.0     | 15            | a             |         149.57 ns |     0.17 |       0.0939 |           - |         592 B |        1.00 |
| StringBuilder           | .NET 7.0     | 15            | a             |          67.21 ns |     0.04 |       0.0166 |           - |         104 B |        0.18 |
| StringBuilderCapacity   | .NET 7.0     | 15            | a             |          85.14 ns |     0.13 |       0.0166 |           - |         104 B |        0.18 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **15**        | **abcde**     |     **423.49 ns** | **0.00** |   **0.2460** |       **-** |    **1544 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 15            | abcde         |         282.95 ns |     0.04 |       0.0863 |           - |         544 B |        0.35 |
| StringBuilderCapacity   | .NET 6.0     | 15            | abcde         |         174.25 ns |     0.06 |       0.0854 |           - |         536 B |        0.35 |
| StringConcatenation     | .NET 7.0     | 15            | abcde         |         371.24 ns |     0.25 |       0.2460 |           - |        1544 B |        1.00 |
| StringBuilder           | .NET 7.0     | 15            | abcde         |         170.99 ns |     0.04 |       0.0865 |           - |         544 B |        0.35 |
| StringBuilderCapacity   | .NET 7.0     | 15            | abcde         |         170.25 ns |     0.08 |       0.0854 |           - |         536 B |        0.35 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **100**       | **a**         |   **2,318.31 ns** | **0.00** |   **2.0027** |       **-** |   **12576 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 100           | a             |         677.10 ns |     0.03 |       0.0858 |           - |         544 B |        0.04 |
| StringBuilderCapacity   | .NET 6.0     | 100           | a             |         493.17 ns |     0.02 |       0.0429 |           - |         272 B |        0.02 |
| StringConcatenation     | .NET 7.0     | 100           | a             |       2,038.73 ns |     0.22 |       2.0027 |           - |       12576 B |        1.00 |
| StringBuilder           | .NET 7.0     | 100           | a             |         496.92 ns |     0.02 |       0.0863 |           - |         544 B |        0.04 |
| StringBuilderCapacity   | .NET 7.0     | 100           | a             |         452.35 ns |     0.01 |       0.0429 |           - |         272 B |        0.02 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **100**       | **abcde**     |   **7,410.56 ns** | **0.00** |   **8.4381** |  **0.0153** |   **52968 B** |    **1.00** |
| StringBuilder           | .NET 6.0     | 100           | abcde         |         690.72 ns |     0.01 |       0.2317 |           - |        1456 B |        0.03 |
| StringBuilderCapacity   | .NET 6.0     | 100           | abcde         |       1,194.76 ns |     0.01 |       0.2995 |      0.0019 |        1888 B |        0.04 |
| StringConcatenation     | .NET 7.0     | 100           | abcde         |       7,421.12 ns |     0.07 |       8.4381 |      0.0153 |       52968 B |        1.00 |
| StringBuilder           | .NET 7.0     | 100           | abcde         |       1,086.90 ns |     0.03 |       0.2308 |           - |        1456 B |        0.03 |
| StringBuilderCapacity   | .NET 7.0     | 100           | abcde         |       1,085.80 ns |     0.01 |       0.2995 |      0.0019 |        1888 B |        0.04 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **1000**      | **a**         |  **94,694.85 ns** | **0.00** | **163.3301** |  **0.7324** | **1025976 B** |   **1.000** |
| StringBuilder           | .NET 6.0     | 1000          | a             |       2,827.98 ns |     0.00 |       0.4044 |      0.0038 |        2552 B |       0.002 |
| StringBuilderCapacity   | .NET 6.0     | 1000          | a             |       4,126.54 ns |     0.01 |       0.3281 |           - |        2072 B |       0.002 |
| StringConcatenation     | .NET 7.0     | 1000          | a             |      58,202.88 ns |     0.01 |     163.4521 |      0.7324 |     1025976 B |       1.000 |
| StringBuilder           | .NET 7.0     | 1000          | a             |       3,933.24 ns |     0.01 |       0.4044 |      0.0038 |        2552 B |       0.002 |
| StringBuilderCapacity   | .NET 7.0     | 1000          | a             |       3,709.91 ns |     0.01 |       0.3281 |           - |        2072 B |       0.002 |
|                         |              |               |               |                   |          |              |             |               |             |
| **StringConcatenation** | **.NET 6.0** | **1000**      | **abcde**     | **384,717.89 ns** | **0.00** | **801.2695** | **19.5313** | **5029968 B** |   **1.000** |
| StringBuilder           | .NET 6.0     | 1000          | abcde         |      11,191.69 ns |     0.00 |       2.7237 |      0.1602 |       17104 B |       0.003 |
| StringBuilderCapacity   | .NET 6.0     | 1000          | abcde         |      11,209.16 ns |     0.00 |       2.5940 |      0.1373 |       16288 B |       0.003 |
| StringConcatenation     | .NET 7.0     | 1000          | abcde         |     742,568.23 ns |     0.19 |     801.7578 |     19.5313 |     5029968 B |       1.000 |
| StringBuilder           | .NET 7.0     | 1000          | abcde         |      11,519.04 ns |     0.00 |       2.7161 |      0.1678 |       17104 B |       0.003 |
| StringBuilderCapacity   | .NET 7.0     | 1000          | abcde         |      11,613.95 ns |     0.00 |       2.5940 |      0.1373 |       16288 B |       0.003 |

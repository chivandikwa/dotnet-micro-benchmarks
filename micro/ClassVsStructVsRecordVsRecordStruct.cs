namespace micro;

public class ClassVsStructVsRecordVsRecordStruct
{
    [Benchmark(Baseline = true)]
    public ClassPoint3D ClassPoint3D() => new ClassPoint3D() { X = 1, Y = 2, Z = 3 };

    [Benchmark]
    public StructPoint3D StructPoint3D() => new StructPoint3D() { X = 1, Y = 2, Z = 3 };

    [Benchmark]
    public RecordPoint3D RecordPoint3D() => new RecordPoint3D(1, 2, 3);

    [Benchmark]
    public RecordStrutPoint3D RecordStrutPoint3D() => new RecordStrutPoint3D(1, 2, 3);
}
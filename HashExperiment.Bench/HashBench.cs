using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace HashExperiment.Bench;

[MemoryDiagnoser(false)]
public class HashBench
{
    private static readonly byte[] _data = RandomNumberGenerator.GetBytes(640_000);

    private readonly HashAlgorithm _fastHashAlgo = FasterQuickAndDirtyHash.Create();
    private readonly HashAlgorithm _slowHashAlgo = QuickAndDirtyHash.Create();

    [Benchmark]
    public void FastHash() => _fastHashAlgo.ComputeHash(_data);

    [Benchmark]
    public void SlowHash() => _slowHashAlgo.ComputeHash(_data);
}

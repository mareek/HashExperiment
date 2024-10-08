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
    public void FastHash()
    {
        Span<byte> buffer = stackalloc byte[16];
        _fastHashAlgo.TryComputeHash(_data, buffer, out var _);
    }

    [Benchmark]
    public void SlowHash()
    {
        Span<byte> buffer =  stackalloc byte[16];
        _slowHashAlgo.TryComputeHash(_data, buffer, out var _);
    }
}

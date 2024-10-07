﻿using System.Runtime.Intrinsics;
using System.Security.Cryptography;

namespace HashExperiment;

public class FasterQuickAndDirtyHash : HashAlgorithm
{
    private const int HashSizeInBytes = 16;

    // random string of bytes generated by System.Security.Cryptography.RandomNumberGenerator.GetBytes(16)
    private static readonly byte[] _key = [91, 64, 130, 201, 187, 50, 33, 90, 248, 238, 23, 162, 202, 150, 75, 184];

    private readonly byte[] _hash;

    private FasterQuickAndDirtyHash()
    {
        HashSizeValue = HashSizeInBytes * 8;
        _hash = new byte[HashSizeInBytes];
    }

    public new static HashAlgorithm Create()
    {
        FasterQuickAndDirtyHash result = new();
        result.Initialize();
        return result;
    }

    public override void Initialize() => _key.CopyTo(_hash, 0);

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
        => HashCore(array.AsSpan(ibStart, cbSize));

    protected override void HashCore(ReadOnlySpan<byte> source)
    {
        if (!Vector128<byte>.IsSupported)
        {
            for (int i = 0; i < source.Length; i++)
                _hash[i % HashSizeInBytes] ^= source[i];
        }
        else
        {
            int tailLength = source.Length % HashSizeInBytes;

            int vectorizableLength = source.Length - tailLength;
            var buffer = Vector128.Create(_hash);
            for (int i = 0; i < vectorizableLength; i += HashSizeInBytes)
                buffer ^= Vector128.Create(source.Slice(i, HashSizeInBytes));

            buffer.CopyTo(_hash);

            for (int i = source.Length - tailLength; i < source.Length; i++)
                _hash[i % HashSizeInBytes] ^= source[i];
        }
    }

    protected override byte[] HashFinal() => _hash;
}

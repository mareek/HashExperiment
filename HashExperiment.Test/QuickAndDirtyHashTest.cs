using System.Security.Cryptography;
using NFluent;
using Xunit;

namespace HashExperiment.Test;

public class QuickAndDirtyHashTest
{
    public static readonly byte[][] TestSet =
        [
            [0],
            [1],
            [255],
            [1, 2, 3, 4],
            [5, 6, 7, 8],
            RandomNumberGenerator.GetBytes(16),
            RandomNumberGenerator.GetBytes(16),
            RandomNumberGenerator.GetBytes(256),
            RandomNumberGenerator.GetBytes(1337),
            RandomNumberGenerator.GetBytes(2048),
        ];

    [Fact]
    public void TestHash()
    {
        List<byte[]> resultSet = [];

        HashAlgorithm hashAlgorithm = QuickAndDirtyHash.Create();
        foreach (var testArray in TestSet)
        {
            var firstHash = hashAlgorithm.ComputeHash(testArray);
            var secondHash = hashAlgorithm.ComputeHash(testArray);
            Check.That(firstHash).IsEqualTo(secondHash);
            resultSet.Add(firstHash);
        }

        CheckUnicity(resultSet);
    }

    [Theory]
    [InlineData(16)]
    [InlineData(4096)]
    public void UnicityStressTest(int sizeInBytes)
    {
        const int setSize = 2048;
        var rng = new Random(sizeInBytes);

        List<byte[]> resultSet = new(setSize);
        byte[] buffer = new byte[sizeInBytes];
        var hashAlgorithm = QuickAndDirtyHash.Create();

        for (int i = 0; i < setSize; i++)
        {
            rng.NextBytes(buffer);
            var hash = hashAlgorithm.ComputeHash(buffer);
            resultSet.Add(hash);
        }

        CheckUnicity(resultSet);
    }

    internal static void CheckUnicity(List<byte[]> resultSet)
    {
        for (int i = 0; i < resultSet.Count; i++)
        {
            byte[] reference = resultSet[i];
            for (int j = i + 1; j < resultSet.Count; j++)
            {
                byte[] target = resultSet[j];
                bool areEqual = reference.SequenceEqual(target);
                Assert.False(areEqual);
            }
        }
    }
}

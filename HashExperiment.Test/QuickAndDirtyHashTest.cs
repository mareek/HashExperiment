using System.Security.Cryptography;
using NFluent;
using Xunit;

namespace HashExperiment.Test;

public class QuickAndDirtyHashTest
{
    [Fact]
    public void TestHash()
    {
        Span<byte[]> testSet =
        [
            [0],
            [1],
            [255],
            [1,2,3,4],
            RandomNumberGenerator.GetBytes(16),
            RandomNumberGenerator.GetBytes(16),
            RandomNumberGenerator.GetBytes(256),
            RandomNumberGenerator.GetBytes(2048),
        ];

        List<byte[]> resultSet = [];

        foreach (var testArray in testSet)
        {
            var firstHash = new QuickAndDirtyHash().Hash(testArray);
            var secondHash = new QuickAndDirtyHash().Hash(testArray);
            Check.That(firstHash).IsEqualTo(secondHash);
            resultSet.Add(firstHash);
        }

        for (int i = 0; i < resultSet.Count; i++)
        {
            for (int j = i + 1; j < resultSet.Count; j++)
            {
                Check.That(resultSet[i]).IsNotEqualTo(resultSet[j]);
            }
        }
    }
}

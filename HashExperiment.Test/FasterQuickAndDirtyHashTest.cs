using System.Security.Cryptography;
using NFluent;
using Xunit;

namespace HashExperiment.Test;

public class FasterQuickAndDirtyHashTest
{
    [Fact]
    public void TestHash()
    {
        HashAlgorithm slowHashAlgorithm = QuickAndDirtyHash.Create();
        HashAlgorithm fastHashAlgorithm = FasterQuickAndDirtyHash.Create();

        foreach (var testArray in QuickAndDirtyHashTest.TestSet)
        {
            var fastHash = fastHashAlgorithm.ComputeHash(testArray);
            var slowHash = slowHashAlgorithm.ComputeHash(testArray);

            Check.That(fastHash).IsEqualTo(slowHash);
        }
    }
}

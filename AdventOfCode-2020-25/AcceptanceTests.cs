using System.Numerics;
using Xunit;
using FluentAssertions;

namespace AdventOfCode_2020_25;

public class AcceptanceTests
{
    private readonly BigInteger _mod = 20201227;

    [Fact]
    public void Example1_DoorKey_Break()
    {
        var b = BabyStepGiantStep(7, 5764801, _mod);

        b.Should().Be(8);

        BigInteger.ModPow(17807724, b, _mod).Should().Be(14897079);
    }

    [Fact]
    public void Example1_CardKey_Break()
    {
        var b = BabyStepGiantStep(7, 17807724, _mod);

        b.Should().Be(11);

        BigInteger.ModPow(5764801, b, _mod).Should().Be(14897079);
    }

    [Fact]
    public void Solution_CardKey_Break()
    {
        var b = BabyStepGiantStep(7, 14012298, _mod);
        b.Should().Be(597630);
    }

    [Fact]
    public void Solution_DoorKey_Break()
    {
        var b = BabyStepGiantStep(7, 74241, _mod);
        b.Should().Be(5888191);
    }

    [Fact]
    public void Solution_CalculateSecret()
    {
        BigInteger.ModPow(14012298, 5888191, _mod).Should().Be(18608573);
    }

    private static BigInteger BabyStepGiantStep(BigInteger b, BigInteger h, BigInteger p)
    {
        
        var n = (BigInteger)Math.Ceiling(Math.Sqrt((double)p - 1.0));
        var lookup = new Dictionary<BigInteger, BigInteger>();
        
        for (BigInteger i = 0; i < n; i++)
        {
            lookup.Add(BigInteger.ModPow(b, i, p), i);
        }
        
        var c = BigInteger.ModPow(b, n * (p - 2), p);
        for (BigInteger j = 0; j < n; j++)
        {
            var y = (h * BigInteger.ModPow(c, j, p)) % p;
            if (lookup.TryGetValue(y, out var i))
            {
                return j * n + i;
            }
        }

        return -1; // solution not found
    }
}
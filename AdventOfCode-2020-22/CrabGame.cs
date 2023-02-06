using System.Collections.Immutable;

namespace AdventOfCode_2020_22;

public sealed record CrabGame(Player Player1, Player Player2)
{
    public static CrabGame From(string input)
    {
        var lines = input.Split("\n");
        var halfAmount = (lines.Length-1) / 2;
        var firstHalf = lines.Take(halfAmount).Skip(1).Select(int.Parse).Select(value => new Card(value));
        var secondHalf = lines.Skip(halfAmount+2).Select(int.Parse).Select(value => new Card(value));

        return new CrabGame(
            new Player(ImmutableQueue.CreateRange(firstHalf)),
            new Player(ImmutableQueue.CreateRange(secondHalf))
        );
    }

    public bool GameIsOver => !(Player1.HeldCards.Any() && Player2.HeldCards.Any());

    public bool Player1Won => GameIsOver && Player1.HeldCards.Any();
};

public sealed record Card(int Value);

public sealed record Player(IImmutableQueue<Card> HeldCards)
{
    public override string ToString()
    {
        return string.Join(",", HeldCards.Select(card => card.Value.ToString()));
    }

    public override int GetHashCode()
    {
        return HeldCards.Aggregate(0x2D2816FE, (current, item) => current * 31 + (item.GetHashCode()));
    }

    public bool Equals(Player? other)
    {
        if (other is null) return false;
        return HeldCards.SequenceEqual(other.HeldCards);
    }
};
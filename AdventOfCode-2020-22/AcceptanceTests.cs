using System.Collections.Immutable;
using FluentAssertions;
using Xunit;
using static AdventOfCode_2020_22.CrabGameRun;

namespace AdventOfCode_2020_22;

public class AcceptanceTests
{
    private const string Example1 = 
@"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";

    [Fact]
    public void Example1_CrabGameFrom_LoadedGame()
    {
        var expectedPlayer1 = new[] { 9, 2, 6 ,3 , 1 };
        var expectedPlayer2 = new[] { 5, 8, 4 ,7 , 10 };
        var game = CrabGame.From(Example1);

        game.Player1.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer1);
        game.Player2.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer2);
    }

    [Fact]
    public void CrabGame_PlayOneRound_FirstPlayerWonRound()
    {
        var expectedPlayer1 = new[] { 2, 6, 3, 1, 9, 5 };
        var expectedPlayer2 = new[] { 8, 4, 7, 10 };
        var game = CrabGame.From(Example1);

        var gameAfterOneRound = PlayOneRound(game);

        gameAfterOneRound.Player1.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer1);
        gameAfterOneRound.Player2.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer2);
    }

    [Fact]
    public void CrabGame_PlayTwoRound_SecondPlayerWon()
    {
        var expectedPlayer1 = new[] { 6, 3, 1, 9, 5 };
        var expectedPlayer2 = new[] { 4, 7, 10, 8, 2 };
        var game = CrabGame.From(Example1);

        var gameAfterOneRound = PlayOneRound(PlayOneRound(game));

        gameAfterOneRound.Player1.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer1);
        gameAfterOneRound.Player2.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer2);
    }

    [Fact]
    public void CrabGame_PlayGame_SecondPlayerWonGame()
    {
        var expectedPlayer2 = new[] { 3, 2, 10, 6, 8, 5, 9, 4, 7, 1 };
        var game = CrabGame.From(Example1);

        var endGame = PlayGame(game);

        endGame.Player1.HeldCards.Select(card => card.Value).Should().BeEmpty();
        endGame.Player2.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer2);
    }

    [Fact]
    public void CrabGame_ScoreWinner_Score()
    {
        var game = CrabGame.From(Example1);

        var endScore = ScoreWinner(PlayGame(game));

        endScore.Should().Be(306);
    }

    [Fact]
    public void Puzzle1()
    {
        var game = CrabGame.From(File.ReadAllText("puzzle-input.txt"));

        var endScore = ScoreWinner(PlayGame(game));

        endScore.Should().Be(32366);
    }

    [Fact]
    public void RecursiveCrabGame_ScoreWinner_Score()
    {
        var game = CrabGame.From(Example1);

        var endScore = ScoreWinner(PlayRecursiveGame(game));

        endScore.Should().Be(291);
    }

    [Fact]
    public void CrabGame_PlayOneRecursiveRound_FirstPlayerWonRound()
    {
        var expectedPlayer1 = new[] { 2, 6, 3, 1, 9, 5 };
        var expectedPlayer2 = new[] { 8, 4, 7, 10 };
        var game = CrabGame.From(Example1);

        var gameAfterOneRound = PlayOneRound(game);

        gameAfterOneRound.Player1.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer1);
        gameAfterOneRound.Player2.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer2);
    }

    [Fact]
    public void CrabGame_Play8RecursiveRound_FirstPlayerWonRound()
    {
        var expectedPlayer1 = new[] { 4, 9, 8, 5, 2 };
        var expectedPlayer2 = new[] { 3, 10, 1, 7, 6 };
        var game = CrabGame.From(Example1);

        var gameAfter8Rounds = ApplyNTimes(game, PlayOneRecursiveRound, 8);

        gameAfter8Rounds.Player1.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer1);
        gameAfter8Rounds.Player2.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer2);
    }

    [Fact]
    public void CrabGame_SubGameCanBeTriggered_True()
    {
        var game = new CrabGame(
            new Player(ImmutableQueue.Create(new Card(9), new Card(8), new Card(5), new Card(2))),    
            new Player(ImmutableQueue.Create(new Card(10), new Card(1), new Card(7), new Card(6)))    
        );

        SubGameCanBeTriggered(game, new Card(4), new Card(3)).Should().BeTrue();
    }

    [Fact]
    public void CrabGame_GetSubGame_SubGame()
    {
        var expectedSubGame = new CrabGame(
            new Player(ImmutableQueue.Create(new Card(9), new Card(8), new Card(5), new Card(2))),    
            new Player(ImmutableQueue.Create(new Card(10), new Card(1), new Card(7)))    
        );
        var game = new CrabGame(
            new Player(ImmutableQueue.Create(new Card(9), new Card(8), new Card(5), new Card(2))),    
            new Player(ImmutableQueue.Create(new Card(10), new Card(1), new Card(7), new Card(6)))    
        );

        var actual = GetSubGameOf(game, new Card(4), new Card(3));

        actual.Should().BeEquivalentTo(expectedSubGame);
    }

    [Fact]
    public void CrabGame_PlaySubGame_Player2Won()
    {
        var game = new CrabGame(
            new Player(ImmutableQueue.Create(new Card(9), new Card(8), new Card(5), new Card(2))),    
            new Player(ImmutableQueue.Create(new Card(10), new Card(1), new Card(7)))    
        );

        var actual = PlayRecursiveGame(game).Player1Won;

        actual.Should().BeFalse();
    }

    [Fact]
    public void CrabGame_Play9RecursiveRound_FirstPlayerWonRound()
    {
        var expectedPlayer1 = new[] { 9, 8, 5, 2 };
        var expectedPlayer2 = new[] { 10, 1, 7, 6, 3, 4 };
        var game = CrabGame.From(Example1);

        var gameAfter9Rounds = ApplyNTimes(game, PlayOneRecursiveRound, 9);

        gameAfter9Rounds.Player1.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer1);
        gameAfter9Rounds.Player2.HeldCards.Select(card => card.Value).Should().BeEquivalentTo(expectedPlayer2);
    }

    [Fact]
    public void TwoGamesWithSameValues_Equals_True()
    {
        var game = new CrabGame(
            new Player(ImmutableQueue.Create(new Card(43), new Card(29))),   
            new Player(ImmutableQueue.Create(new Card(2), new Card(29), new Card(14)))    
        );

        var game2 = new CrabGame(
            new Player(ImmutableQueue.Create(new Card(43), new Card(29))),   
            new Player(ImmutableQueue.Create(new Card(2), new Card(29), new Card(14)))    
        );

        var actual = game.Equals(game2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void TwoPlayersWithSameValues_Equals_True()
    {
        var player = new Player(ImmutableQueue.Create(new Card(43), new Card(29)));
        var player2 = new Player(ImmutableQueue.Create(new Card(43), new Card(29)));

        var actual = player.Equals(player2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void TwoCardsWithSameValues_Equals_True()
    {
        var card = new Card(43);
        var card2 = new Card(43);

        var actual = card.Equals(card2);

        actual.Should().BeTrue();
    }

    [Fact]
    public void EndlessLoop_PlayGame_Terminated()
    {
        var game = new CrabGame(
            new Player(ImmutableQueue.Create(new Card(43), new Card(29))),   
            new Player(ImmutableQueue.Create(new Card(2), new Card(29), new Card(14)))    
        );

        var actual = PlayRecursiveGame(game).Player1Won;

        actual.Should().BeTrue();
    }

    [Fact]
    public void Puzzle2()
    {
        var game = CrabGame.From(File.ReadAllText("puzzle-input.txt"));

        var endScore = ScoreWinner(PlayRecursiveGame(game));

        endScore.Should().Be(30891);
    }

    private static T ApplyNTimes<T>(T seed, Func<T, T> function, int n) =>
        Enumerable.Repeat(function, n).Aggregate(seed, (item, func) => func(item));
}
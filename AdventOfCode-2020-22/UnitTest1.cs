using FluentAssertions;
using Xunit;
using static AdventOfCode_2020_22.CrabGameRun;

namespace AdventOfCode_2020_22;

public class UnitTest1
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
}
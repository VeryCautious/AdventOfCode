using System.Collections.Immutable;
using System.Diagnostics;

namespace AdventOfCode_2020_22;

public static class CrabGameRun
{
    public static CrabGame PlayOneRound(CrabGame game)
    {
        var firstPlayersCard = game.Player1.HeldCards.First();
        var secondPlayersCard = game.Player2.HeldCards.First();

        var newGame = new CrabGame(
            new Player(game.Player1.HeldCards.Dequeue()),
            new Player(game.Player2.HeldCards.Dequeue())
        );

        if (firstPlayersCard.Value > secondPlayersCard.Value)
        {
            newGame = newGame with
            {
                Player1 = new Player(
                    newGame.Player1.HeldCards.
                        Enqueue(firstPlayersCard).
                        Enqueue(secondPlayersCard)
                )
            };
        }
        else
        {
            newGame = newGame with
            {
                Player2 = new Player(
                    newGame.Player2.HeldCards.
                        Enqueue(secondPlayersCard).
                        Enqueue(firstPlayersCard)
                )
            };
        }

        return newGame;
    }

    public static CrabGame PlayOneRecursiveRound(CrabGame game) => 
        PlayOneRecursiveRound(game, new RecursionCarry());

    private class RecursionCarry
    {
        public int RecursionDepth;
    }

    private static CrabGame PlayOneRecursiveRound(CrabGame game, RecursionCarry carry)
    {
        var firstPlayersCard = game.Player1.HeldCards.First();
        var secondPlayersCard = game.Player2.HeldCards.First();

        var newGame = new CrabGame(
            new Player(game.Player1.HeldCards.Dequeue()),
            new Player(game.Player2.HeldCards.Dequeue())
        );

        bool player1Won;

        if (SubGameCanBeTriggered(newGame, firstPlayersCard, secondPlayersCard))
        {
            carry.RecursionDepth++;
            Debug.Write($"recursion-depth:{carry.RecursionDepth}");
            Debug.Write(new string(Enumerable.Repeat('-', carry.RecursionDepth + 1).ToArray()));
            
            var subGameResult = PlayRecursiveGame(GetSubGameOf(newGame, firstPlayersCard, secondPlayersCard), carry);
            player1Won = subGameResult.Player1Won;
            
            Debug.Write(new string(Enumerable.Repeat('-', carry.RecursionDepth + 1).ToArray()));
            carry.RecursionDepth--;
        }
        else
        {
            player1Won = firstPlayersCard.Value > secondPlayersCard.Value;
        }

        return player1Won ? GivePlayer1(newGame, firstPlayersCard, secondPlayersCard) : GivePlayer2(newGame, secondPlayersCard, firstPlayersCard);
    }

    private static CrabGame GivePlayer1(CrabGame game, Card firstCard, Card secondCard) => game with
    {
        Player1 = new Player(
            game.Player1.HeldCards.
                Enqueue(firstCard).
                Enqueue(secondCard)
        )
    };

    private static CrabGame GivePlayer2(CrabGame game, Card firstCard, Card secondCard) => game with
    {
        Player2 = new Player(
            game.Player2.HeldCards.
                Enqueue(firstCard).
                Enqueue(secondCard)
        )
    };

    internal static bool SubGameCanBeTriggered(CrabGame game, Card firstPlayersCard, Card secondPlayersCard) =>
        game.Player1.HeldCards.Count() >= firstPlayersCard.Value &&
        game.Player2.HeldCards.Count() >= secondPlayersCard.Value;

    internal static CrabGame GetSubGameOf(CrabGame game, Card firstPlayersCard, Card secondPlayersCard) =>
        new (
            new Player(ImmutableQueue.CreateRange(game.Player1.HeldCards.Take(firstPlayersCard.Value))),
            new Player(ImmutableQueue.CreateRange(game.Player2.HeldCards.Take(secondPlayersCard.Value)))
        );

    public static CrabGame PlayGame(CrabGame game)
    {
        var newGame = game;

        while (!newGame.GameIsOver)
        {
            newGame = PlayOneRound(newGame);
        }

        return newGame;
    }

    public static CrabGame PlayRecursiveGame(CrabGame game)
    {
        return PlayRecursiveGame(game, new RecursionCarry());
    }

    private static CrabGame PlayRecursiveGame(CrabGame game, RecursionCarry carry)
    {
        var newGame = game;
        var visitedStatesPlayer1 = new List<Player>();
        var visitedStatesPlayer2 = new List<Player>();

        while (!newGame.GameIsOver)
        {
            newGame = PlayOneRecursiveRound(newGame, carry);

            if (visitedStatesPlayer1.Contains(newGame.Player1) || visitedStatesPlayer2.Contains(newGame.Player2))
            {
                return newGame with
                {
                    Player2 = new Player(ImmutableQueue<Card>.Empty)
                };
            }

            visitedStatesPlayer1.Add(newGame.Player1);
            visitedStatesPlayer2.Add(newGame.Player2);
        }

        return newGame;
    }

    public static int ScoreWinner(CrabGame game)
    {
        if (!game.GameIsOver) throw new ArgumentException("The game can only be scored when its over", nameof(game));

        var winningHand = game.Player1.HeldCards.Any() ? game.Player1.HeldCards : game.Player2.HeldCards;

        return winningHand.Reverse().Select((card, i) => card.Value * (i+1)).Sum();
    }
}
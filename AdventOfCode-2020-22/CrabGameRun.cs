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

    public static CrabGame PlayGame(CrabGame game)
    {
        var newGame = game;

        while (newGame.Player1.HeldCards.Any() && newGame.Player2.HeldCards.Any())
        {
            newGame = PlayOneRound(newGame);
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
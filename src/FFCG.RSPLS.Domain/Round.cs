using System;
using System.Collections.Generic;
using System.Linq;

namespace FFCG.RSPLS.Domain
{
    public class Round
    {
        public Round(Game game)
        {
            Game = game;
            Started = DateTime.Now;
            Outcome = RoundOutcome.NotFinished;
        }

        public Game Game { get; private set; }
        public Move Player1Move { get; private set; }
        public Move Player2Move { get; private set; }
        public DateTime Started { get; private set; }
        public DateTime? Completed { get; private set; }

        public bool IsCompleted
        {
            get { return Completed != null; }
        }

        public RoundOutcome Outcome { get; private set; }
        public Player Winner { get; private set; }
        public Rule WinnerRule { get; private set; }

        internal void MakeMove(Player player, Move move)
        {
            if (player == Game.Player1)
            {
                if (Player1Move != null)
                    throw new GameException("Player 1 has already made a move this round");

                Player1Move = move;
            }
            else if (player == Game.Player2)
            {
                if (Player2Move != null)
                    throw new GameException("Player 2 has already made a move this round");

                Player2Move = move;
            }
            else
            {
                throw new GameException("Trying to make a move with a player thats not in the game");
            }

            if (Player1Move != null && Player2Move != null)
            {
                Completed = DateTime.Now;
            }
        }

        public bool HasMadeMove(Player player)
        {
            if (player == Game.Player1)
            {
                return (Player1Move != null);
            }
            else if (player == Game.Player2)
            {
                return (Player2Move != null);
            }
            else
            {
                throw new GameException("Player is not in this game");
            }
        }

        internal void Check(IEnumerable<Rule> rules)
        {
            if( Player1Move == null || Player2Move == null) return;

            var winningRule = rules.GetWinningRule(Player1Move.Type, Player2Move.Type);
            if (winningRule == null)
            {
                winningRule = rules.GetWinningRule(Player2Move.Type, Player1Move.Type);
                if (winningRule != null)
                {
                    Win(Game.Player1, winningRule);
                }
                else
                {
                    Draw();
                }
            }
            else
            {
                Win(Game.Player2, winningRule);
            }
        }

        private void Win(Player winner, Rule winnerRule)
        {
            Winner = winner;
            Outcome = winner == Game.Player1 ? RoundOutcome.Player1Wins : RoundOutcome.Player2Wins;
            WinnerRule = winnerRule;
        }

        private void Draw()
        {
            Outcome = RoundOutcome.Draw;
        }
    }

    public enum RoundOutcome
    {
        NotFinished = 0,
        Draw = 1,
        Player1Wins = 2,
        Player2Wins = 3,
    }

    public static class RoundExtensions
    {
        public static int NoPlayerWins(this IEnumerable<Round> rounds, Player player)
        {
            var count = rounds.Count(round => round.Winner == player);
            return count;
        }
    }
}
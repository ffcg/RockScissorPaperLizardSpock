using System;
using System.Collections.Generic;
using System.Linq;

namespace FFCG.RSPLS.Domain
{
    public class Game
    {
        private readonly Player _player1;
        private readonly Player _player2;
        private Player _winner;
        private readonly int _bestOf;
        private readonly int _roundsNeededToWin;
        private readonly List<Round> _rounds;
        private Round _nextRound;
        private GameStatus _status;
        private readonly IEnumerable<Rule> _rules;

        /// <summary>
        /// Starts a new game with the players given and optionaly a best of count
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <param name="bestOf"></param>
        /// <returns></returns>
        public static Game StartGame(Player player1, Player player2, int bestOf = 3)
        {
            var game = new Game(player1, player2, bestOf);
            return game;
        }

        private Game(Player player1, Player player2, int bestOf)
        {
            _player1 = player1;
            _player2 = player2;
            _winner = null;
            _bestOf = bestOf;
            _roundsNeededToWin = (_bestOf / 2);
            _rounds = new List<Round>();

            StartNextRound();

            _rules = Rule.DefaultRules;
        }

        public Player Player1 { get { return _player1; } }
        public Player Player2 { get { return _player2; } }
        public Player Winner { get { return _winner; } }
        public GameStatus Status { get { return _status;  } }
        public int BestOf { get { return _bestOf; } }
        public DateTime Started { get; private set ; }
        public DateTime Finished { get; private set; }
        public IEnumerable<Round> Rounds 
        {
            get { return _rounds.ToArray(); }
        }
        public Round NextRound 
        {
            get { return _nextRound; }
        }

        /// <summary>
        /// Makes a move for player. 
        /// Throws <see cref="GameException"/> if player is not in the game, or player has already made a move this round
        /// </summary>
        /// <param name="player"></param>
        /// <param name="moveType"></param>
        public void MakeMove(Player player, MoveType moveType)
        {
            if (_status != GameStatus.ReadyForNextMove) 
                throw new GameException("Game is not ready for next move");
            
            if (_nextRound == null)
                throw new GameException("No new round started");

            var move = new Move(player, moveType);
            _nextRound.MakeMove(player, move);
            _nextRound.Check(_rules);

            if (_nextRound.IsCompleted)
            {
                _rounds.Add(_nextRound);
                var player1Wins = _rounds.NoPlayerWins(_player1);
                var player2Wins = _rounds.NoPlayerWins(_player2);

                if ((player1Wins > _roundsNeededToWin) && (player1Wins > player2Wins))
                {
                    // Player 1 wins game
                    Win(_player1);
                    return;
                }
                else if ((player2Wins > _roundsNeededToWin) && (player2Wins > player1Wins))
                {
                    // Player 2 wins game
                    Win(_player2);
                    return;
                }

                StartNextRound();
            }
        }

        /// <summary>
        /// Player abandons the game
        /// </summary>
        /// <param name="player"></param>
        public void Abandon(Player player)
        {
            _status = GameStatus.Abandoned;
            Finished = DateTime.Now;
        }

        private void StartNextRound()
        {
            _nextRound = new Round(this);
            _status = GameStatus.ReadyForNextMove;
        }

        private void Win(Player player)
        {
            _winner = _player1;
            _status = GameStatus.Completed;
            Finished = DateTime.Now;
        }
    }
}

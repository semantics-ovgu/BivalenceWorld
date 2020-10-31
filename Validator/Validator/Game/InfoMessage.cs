using System;
using System.Collections.Generic;

namespace Validator.Game
{
    public class InfoMessage : AMove
    {
        private readonly AMove _nextMove = null;

        public InfoMessage(Game game, Formula formula, string message, AMove nextMove) : base(game, formula, message)
        {
            _nextMove = nextMove;
        }

        internal override AMove NextMove(Game game)
        {
            return _nextMove;
        }

        internal override bool CanBePlayed()
        {
            return true;
        }

        internal override void Play()
        {
            Game.SetNextMove(_nextMove);
        }
    }
}
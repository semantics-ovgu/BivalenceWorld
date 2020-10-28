using System;
using System.Collections.Generic;

namespace Validator.Game
{
    public class ErrorMessage : AMove
    {
        public ErrorMessage(Game game, Formula formula, string message) : base(game, formula, message)
        {
        }

        internal override AMove NextMove(Game game)
        {
            return this;
        }

        internal override bool CanBePlayed()
        {
            return true;
        }

        internal override void Play()
        {
            
        }
    }
}
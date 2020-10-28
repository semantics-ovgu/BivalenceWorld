using System;
using System.Collections.Generic;

namespace Validator.Game
{
    public class EndMessage : AMove
    {
        public bool GuessWasRight { get; private set; }

        public EndMessage(Game game, Formula formula, string message, bool guessWasRight) : base(game, formula, message)
        {
            GuessWasRight = guessWasRight;
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

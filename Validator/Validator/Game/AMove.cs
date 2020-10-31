using System.Collections.Generic;

namespace Validator.Game
{
    public abstract class AMove
    {
        internal AMove(Game game, Formula formula, string message)
        {
            Formula = formula;
            Game = game;
            Message = message;
        }

        public Formula Formula { get; }

        public string Message { get; private set; } = "";

        protected Game Game { get; } = null;

        public bool IsPlayed { get; internal set; } = false;

        internal abstract AMove NextMove(Game game);

        internal abstract bool CanBePlayed();
        internal abstract void Play();
    }
}
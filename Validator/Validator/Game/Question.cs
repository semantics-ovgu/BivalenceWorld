using System.Collections.Generic;

namespace Validator.Game
{
    public class Question : AMove
    {
        private readonly List<Selection> _possibleAnswers;

        public Question(Game game, Formula formula, string message, List<Selection> possibleAnswers) : base(game, formula, message)
        {
            _possibleAnswers = possibleAnswers;
        }

        public Selection Answer { get; private set; } = null;

        public List<Selection> PossibleAnswers => _possibleAnswers;

        internal override AMove NextMove(Game game)
        {
            return Answer.GetNextMove(game);
        }

        internal override bool CanBePlayed()
        {
            return Answer != null;
        }

        internal override void Play()
        {
            if (CanBePlayed())
            {
                IsPlayed = true;
                Game.SetNextMove(NextMove(Game));
            }
        }

        public void SetAnswers(Selection answer)
        {
            if (_possibleAnswers.Contains(answer))
                Answer = answer;
        }

        public class Selection
        {
            private Formula _formula;
            private Dictionary<string, string> _dictVariables;

            public Selection(Formula formula, Dictionary<string, string> dictVariables)
            {
                _formula = formula;
                _dictVariables = dictVariables;
            }

            internal AMove GetNextMove(Game game)
            {
                return _formula.CreateNextMove(game, _dictVariables);
            }
        }
    }
}
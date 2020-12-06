using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

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
            private WorldObject _worldObject;
            public WorldObject WorldObject => _worldObject;
            private Formula _formula;
            public Formula Formula => _formula;
            private Dictionary<string, string> _dictVariables;
            private SelectionTypes _selectionType;
            public SelectionTypes SelectionType => _selectionType;
            private string _variable = "";
            public string Message => _formula.ReformatFormula(_dictVariables);

            public Selection(Formula formula, Dictionary<string, string> dictVariables, WorldObject obj, string variable) : this(formula, dictVariables)
            {
                _worldObject = new WorldObject(obj);
                _variable = variable;
                _selectionType = SelectionTypes.WorldObject;
            }

            public Selection(Formula formula, Dictionary<string, string> dictVariables)
            {
                _formula = formula;
                _selectionType = SelectionTypes.Formula;
                _dictVariables = new Dictionary<string, string>();

                foreach (KeyValuePair<string, string> valuePair in dictVariables)
                {
                    _dictVariables.Add(valuePair.Key, valuePair.Value);
                }
            }

            private void ChangeWorldObjects(Game game)
            {
                if (_worldObject.Consts.Any())
                {
                    if (_dictVariables.ContainsKey(_variable))
                    {
                        _dictVariables[_variable] = _worldObject.Consts[0];
                    }
                    else
                    {
                        _dictVariables.Add(_variable, _worldObject.Consts[0]);
                    }
                }
                else
                {
                    var value = game.AddGenericConstString();
                    _dictVariables.Add(_variable, value);

                    game.ReplaceWorldObject(_worldObject);
                }
            }

            internal AMove GetNextMove(Game game)
            {
                if (_selectionType == SelectionTypes.WorldObject)
                {
                    ChangeWorldObjects(game);
                }

                return _formula.CreateNextMove(game, _dictVariables);
            }

            public enum SelectionTypes
            {
                Formula,
                WorldObject
            }
        }
    }
}

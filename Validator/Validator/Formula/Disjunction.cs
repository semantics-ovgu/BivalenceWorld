using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pegasus.Common.Tracing;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Disjunction : GenericFormula<Formula>, IFormulaValidate
    {
        private Formula _invalidFormula = null;
        private Formula _validFormula = null;

        public Disjunction(List<Formula> arguments, string name, string formattedFormula) : base(arguments, name, formattedFormula)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var argument in arguments)
            {
                if (argument != arguments.First())
                    builder.Append(" ");

                builder.Append(argument.FormattedFormula);

                if (argument != arguments.Last())
                    builder.Append(" ∨ ");
            }
            SetFormattedFormula(builder.ToString());

            ReorderConjunctionParts();
        }

        private void ReorderConjunctionParts()
        {
            for (var i = 0; i < Arguments.Count; i++)
            {
                var argument = Arguments[i];
                if (argument is Disjunction disj)
                {
                    Arguments.RemoveAt(i--);
                    Arguments.AddRange(disj.Arguments);
                }
            }
        }

        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<EValidationResult> result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
            foreach (var conjunctionPart in GetArgumentsOfType<IFormulaValidate>())
            {
                ResultSentence<EValidationResult> validate = conjunctionPart.Validate(pL1Structure, dictVariables);
                if (validate.IsValid)
                {
                    if (validate.Value == EValidationResult.True)
                    {
                        result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.True);
                        _validFormula = conjunctionPart as Formula;
                    }
                    else
                    {
                        _invalidFormula = conjunctionPart as Formula;
                    }
                }
                else
                {
                    result = validate;
                    break;
                }
            }

            return result;
        }

        private List<Question.Selection> CreatePossibleSelection(Dictionary<string, string> dictVariables)
        {
            var selection = new List<Question.Selection>();
            foreach (var argument in Arguments)
            {
                selection.Add(new Question.Selection(argument, dictVariables));
            }

            return selection;
        }

        public override string ReformatFormula(Dictionary<string, string> variables)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var argument in Arguments)
            {
                if (argument != Arguments.First())
                    builder.Append(" ");

                builder.Append(argument.ReformatFormula(variables));

                if (argument != Arguments.Last())
                    builder.Append(" ∨ ");
            }

            return builder.ToString();
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            var result = Validate(game.World, dictVariables);

            if (game.Guess)
            {
                var questionMessage = new Question(game, this, "Choose a formula that you believe to be true.", CreatePossibleSelection(dictVariables));
                var allTrueInfo = new InfoMessage(game, this, $"So you believe that at least one of these formula is true:\n{ArgumentsToString(dictVariables)}\n[You will try to choose a true formula]", questionMessage);
                return new InfoMessage(game, this, $"So you believe that\n{ReformatFormula(dictVariables)}\nis true?", allTrueInfo);
            }
            else
            {
                AMove invalidMove = null;
                if (result.Value == EValidationResult.False)
                {
                    invalidMove = _invalidFormula.CreateNextMove(game, dictVariables);
                }
                else
                {
                    invalidMove = _validFormula.CreateNextMove(game, dictVariables);
                }
                var allTrueInfo = new InfoMessage(game, this, $"So you believe that all of these formula are false:\n{ArgumentsToString(dictVariables)}\n[Bivalence World will try to choose a true formula]", invalidMove);
                return new InfoMessage(game, this, $"So you believe that\n{ReformatFormula(dictVariables)}\nis false?", allTrueInfo);
            }
        }
    }
}

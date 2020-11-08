using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Conjunction : GenericFormula<Formula>, IFormulaValidate
    {
        private Formula _invalidFormula = null;

        public Conjunction(List<Formula> arguments, string name, string formattedFormula) : base(arguments, name, formattedFormula)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var argument in arguments)
            {
                if (argument != arguments.First())
                    builder.Append(" ");

                builder.Append(argument.FormattedFormula);

                if (argument != arguments.Last())
                    builder.Append(" ∧ ");
            }
            SetFormattedFormula(builder.ToString());

            ReorderConjunctionParts();
        }

        private void ReorderConjunctionParts()
        {
            for (var i = 0; i < Arguments.Count; i++)
            {
                var argument = Arguments[i];
                if (argument is Conjunction conj)
                {
                    Arguments.RemoveAt(i--);
                    Arguments.AddRange(conj.Arguments);
                }
            }
        }

        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<EValidationResult> result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.True);
            foreach (var conjunctionPart in GetArgumentsOfType<IFormulaValidate>())
            {
                ResultSentence<EValidationResult> validate = conjunctionPart.Validate(pL1Structure, dictVariables);
                if (validate.IsValid)
                {
                    if (validate.Value != EValidationResult.True)
                    {
                        _invalidFormula = conjunctionPart as Formula;
                        result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
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
                    builder.Append(" ∧ ");
            }

            return builder.ToString();
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            var result = Validate(game.World, dictVariables);

            if (game.Guess)
            {
                var invalidMove = Arguments[0].CreateNextMove(game, dictVariables);
                if (result.Value == EValidationResult.False)
                {
                    invalidMove = _invalidFormula.CreateNextMove(game, dictVariables);
                }
                var allTrueInfo = new InfoMessage(game, this, $"So you believe that all of these formula are true:\n{ArgumentsToString(dictVariables)}\n[Bivalence World will try to choose a false formula]", invalidMove);
                return new InfoMessage(game, this, $"So you believe that\n{ReformatFormula(dictVariables)}\nis true?", allTrueInfo);
            }
            else
            {
                var questionMessage = new Question(game, this, "Choose a formula that you believe to be false.", CreatePossibleSelection(dictVariables));
                var allTrueInfo = new InfoMessage(game, this, $"So you believe that at least one of these formula is false:\n{ArgumentsToString(dictVariables)}\n[You will try to choose a false formula]", questionMessage);
                return new InfoMessage(game, this, $"So you believe that\n{ReformatFormula(dictVariables)}\nis false?", allTrueInfo);
            }
        }
    }
}

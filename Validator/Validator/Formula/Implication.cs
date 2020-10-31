using System.Collections.Generic;
using System.Linq;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Implication : GenericFormula<Formula>, IFormulaValidate
    {
        private Formula _rewrittenFormula = null;

        public Implication(Formula first, Formula second, string name, string formattedFormula) : base(new List<Formula> { first, second }, name, formattedFormula)
        {
            var rewrittenArguments = new List<Formula>()
            {
                new Negation(first),
                second
            };
            _rewrittenFormula = new Disjunction(rewrittenArguments, name, formattedFormula);

            SetFormattedFormula(first.FormattedFormula + "\u2192" + second.FormattedFormula);
        }

        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<EValidationResult> result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
            var arguments = GetArgumentsOfType<IFormulaValidate>().ToList();
            if (arguments.Count != 2)
            {
                result = ResultSentence<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult,
                        "Implication has invalid amount of arguments " + arguments.Count);
            }
            else
            {
                var result1 = arguments[0].Validate(pL1Structure, dictVariables);
                var result2 = arguments[1].Validate(pL1Structure, dictVariables);
                if (result1.IsValid && result2.IsValid)
                {
                    if (result1.Value == EValidationResult.False || result2.Value == EValidationResult.True)
                    {
                        result = ResultSentence<EValidationResult>.CreateResult(EValidationResult.True);
                    }
                    else
                    {
                        result = ResultSentence<EValidationResult>.CreateResult(EValidationResult.False);
                    }
                }
                else
                {
                    if (!result1.IsValid)
                    {
                        result = result1;
                    }
                    else
                    {
                        result = result2;
                    }
                }
            }

            return result;
        }

        public override string ReformatFormula(Dictionary<string, string> variables)
        {
            return Arguments[0].ReformatFormula(variables) + "\u2192" + Arguments[1].ReformatFormula(variables);
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            return new InfoMessage(game, this, $"{ReformatFormula(dictVariables)}\ncan be rewritten as\n{_rewrittenFormula.ReformatFormula(dictVariables)}", _rewrittenFormula.CreateNextMove(game, dictVariables));
        }
    }
}

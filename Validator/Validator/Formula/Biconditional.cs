using System.Collections.Generic;
using System.Linq;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Biconditional : GenericFormula<Formula>, IFormulaValidate
    {
        private Formula _rewrittenFormula = null;


        public Biconditional(Formula first, Formula second, string name, string formattedFormula) : base(new List<Formula> { first, second }, name, formattedFormula)
        {
            var rewrittenArguments = new List<Formula>()
            {
                    new Conjunction(new List<Formula>{new Implication(first, second, name, FormattedFormula), new Implication(second, first, name, formattedFormula)  },name, formattedFormula )
            };
            _rewrittenFormula = new Disjunction(rewrittenArguments, name, formattedFormula);

            SetFormattedFormula(first.FormattedFormula + "\u2194" + second.FormattedFormula);
        }

        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<EValidationResult> result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
            var arguments = GetArgumentsOfType<IFormulaValidate>().ToList();
            if (arguments.Count != 2)
            {
                result = ResultSentence<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult,
                        "Biconditional has invalid amount of arguments " + arguments.Count);
            }
            else
            {
                var result1 = arguments[0].Validate(pL1Structure, dictVariables);
                var result2 = arguments[1].Validate(pL1Structure, dictVariables);
                if (result1.IsValid && result2.IsValid)
                {
                    if (result1.Value == result2.Value)
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
            return Arguments[0].ReformatFormula(variables) + "\u2194" + Arguments[1].ReformatFormula(variables);
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            return new InfoMessage(game, this, $"{ReformatFormula(dictVariables)}\ncan be rewritten as\n{_rewrittenFormula.ReformatFormula(dictVariables)}", _rewrittenFormula.CreateNextMove(game, dictVariables));
        }
    }
}

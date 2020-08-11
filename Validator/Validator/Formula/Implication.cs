using System.Collections.Generic;
using System.Linq;
using Validator.World;

namespace Validator
{
    public class Implication : GenericFormula<Formula>, IFormulaValidate
    {
        public Implication(Formula first, Formula second, string name, string rawFormula) : base(new List<Formula> { first, second }, name, rawFormula)
        {
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
    }
}

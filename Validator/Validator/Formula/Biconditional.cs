using System.Collections.Generic;
using System.Linq;
using Validator.World;

namespace Validator
{
    public class Biconditional : GenericFormula<Formula>, IFormulaValidate
    {
        public Biconditional(Formula first, Formula second, string name, string rawFormula) : base(new List<Formula> { first, second }, name, rawFormula)
        {
        }

        public Result<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            Result<EValidationResult> result = Result<EValidationResult>.CreateResult(true, EValidationResult.False);
            var arguments = GetArgumentsOfType<IFormulaValidate>().ToList();
            if (arguments.Count != 2)
            {
                result = Result<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult,
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
                        result = Result<EValidationResult>.CreateResult(EValidationResult.True);
                    }
                    else
                    {
                        result = Result<EValidationResult>.CreateResult(EValidationResult.False);
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

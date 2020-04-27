using System.Collections.Generic;
using Validator.World;

namespace Validator
{
    public class Conjunction : GenericFormula<Formula>, IFormulaValidate
    {
        public Conjunction(List<Formula> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }

        public Result<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            Result<EValidationResult> result = Result<EValidationResult>.CreateResult(true, EValidationResult.True);
            foreach (var conjunctionPart in GetArgumentsOfType<IFormulaValidate>())
            {
                Result<EValidationResult> validate = conjunctionPart.Validate(pL1Structure, dictVariables);
                if (validate.IsValid)
                {
                    if (validate.Value != EValidationResult.True)
                    {
                        result = Result<EValidationResult>.CreateResult(true, EValidationResult.False);
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
    }
}

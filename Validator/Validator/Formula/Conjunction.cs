using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    public class Conjunction : GenericFormula<Formula>, IFormulaValidate
    {
        public Conjunction(List<Formula> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }


        public Result<bool> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            Result<bool> result = Result<bool>.CreateResult(true, true);
            foreach (var conjunctionPart in GetArgumentsOfType<IFormulaValidate>())
            {
                Result<bool> validate = conjunctionPart.Validate(pL1Structure, dictVariables);
                if (!validate.Value)
                {
                    result = Result<bool>.CreateResult(true, false);
                    break;
                }
            }

            return result;
        }
    }
}

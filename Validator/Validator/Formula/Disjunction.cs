using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    internal class Disjunction : GenericFormula<Formula>, IFormulaValidate
    {
        public Disjunction(List<Formula> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }


        public Result<bool> Validate(IWorldPL1Structure pL1Structure)
        {
            Result<bool> result = Result<bool>.CreateResult(true, false);
            foreach (var conjunctionPart in GetArgumentsOfType<IFormulaValidate>())
            {
                Result<bool> validate = conjunctionPart.Validate(pL1Structure);
                if (validate.Value)
                {
                    result = Result<bool>.CreateResult(true, true);
                    break;
                }
            }

            return result;
        }
    }
}

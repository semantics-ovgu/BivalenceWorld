using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    internal class Implication : GenericFormula<Formula>, IFormulaValidate
    {
        public Implication(Formula first, Formula second, string name, string rawFormula) : base(new List<Formula> { first, second }, name, rawFormula)
        {
        }


        public Result<bool> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            Result<bool> result = Result<bool>.CreateResult(true, false);
            var arguments = GetArgumentsOfType<IFormulaValidate>().ToList();

            if (arguments.Count != 2)
                result = Result<bool>.CreateResult(false, false, "Implication has invalid amount of arguments " + arguments.Count);
            else
            {
                var result1 = arguments[0].Validate(pL1Structure, dictVariables);
                var result2 = arguments[1].Validate(pL1Structure, dictVariables);

                if (result1.IsValid && result2.IsValid)
                {
                    if (!result1.Value || result2.Value)
                        result = Result<bool>.CreateResult(true, true);
                    else
                        result = Result<bool>.CreateResult(true, false);
                }
                else
                    result = Result<bool>.CreateResult(false, false, result1.Message + "\n" + result2.Message);
            }

            return result;
        }
    }
}

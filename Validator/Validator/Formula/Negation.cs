using System.Collections.Generic;

namespace Validator
{
    public class Negation : Formula, IFormulaValidate
    {
        private Formula _formula = null;


        public Negation(Formula formula) : base(formula.Name, formula.RawFormula)
        {
            _formula = formula;
        }

        public Result<bool> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (_formula != null && _formula is IFormulaValidate formulaValidate)
            {
                var res = formulaValidate.Validate(pL1Structure, dictVariables);
                if (res.IsValid)
                {
                    return Result<bool>.CreateResult(true, !res.Value);
                }
                else
                {
                    return res;
                }
            }

            return Result<bool>.CreateResult(false, false, "No Formula in Parenthese");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public class Parenthese : Formula, IFormulaValidate
    {
        private Formula _formula = null;


        public Parenthese(Formula formula) : base(formula.Name, formula.RawFormula)
        {
            _formula = formula;
        }

        public Result<bool> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (_formula != null && _formula is IFormulaValidate formulaValidate)
                return formulaValidate.Validate(pL1Structure, dictVariables);
            else
                return Result<bool>.CreateResult(false, false, "No Formula in Parenthese");
        }
    }
}

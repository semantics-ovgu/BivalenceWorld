﻿using System.Collections.Generic;
using Validator.World;

namespace Validator
{
    public class Negation : Formula, IFormulaValidate
    {
        private Formula _formula = null;


        public Negation(Formula formula) : base(formula.Name, formula.RawFormula)
        {
            _formula = formula;
        }

        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (_formula != null && _formula is IFormulaValidate formulaValidate)
            {
                var res = formulaValidate.Validate(pL1Structure, dictVariables);
                if (res.IsValid)
                {
                    if (res.Value == EValidationResult.True)
                    {
                        return ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
                    }
                    else
                    {
                        return ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.True);
                    }
                }
                else
                {
                    return res;
                }
            }

            return ResultSentence<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult, "No Formula in Negation");
        }
    }
}

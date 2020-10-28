﻿using System.Collections.Generic;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Parenthese : Formula, IFormulaValidate
    {
        private Formula _formula = null;

        public Parenthese(Formula formula) : base(formula.Name, formula.FormattedFormula)
        {
            _formula = formula;
        }

        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (_formula != null && _formula is IFormulaValidate formulaValidate)
            {
                return formulaValidate.Validate(pL1Structure, dictVariables);
            }

            return ResultSentence<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult, "No Formula in Parenthese");
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            throw new System.NotImplementedException();
        }
    }
}

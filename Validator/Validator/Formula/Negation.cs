﻿using System.Collections.Generic;
using System.Text;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Negation : Formula, IFormulaValidate
    {
        private Formula _formula = null;


        public Negation(Formula formula) : base(formula.Name, formula.FormattedFormula)
        {
            _formula = formula;

            SetFormattedFormula("¬" + formula.FormattedFormula);
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

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            var result = Validate(game.World, dictVariables);

            if (game.Guess)
            {
                game.SetGuess(!game.Guess);
                return new InfoMessage(game, this, $"So you believe that \n{FormattedFormula}\n is true", _formula.CreateNextMove(game, dictVariables));
            }
            else
            {
                game.SetGuess(!game.Guess);
                return new InfoMessage(game, this, $"So you believe that \n{FormattedFormula}\n is false", _formula.CreateNextMove(game, dictVariables));
            }
        }
    }
}

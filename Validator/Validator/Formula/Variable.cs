using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Variable : Argument
    {
        public Variable(string name, string formattedFormula) : base(new List<Argument>(), name, formattedFormula)
        {
        }

        public override ResultSentence<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (dictVariables.ContainsKey(FormattedFormula))
            {
                return ResultSentence<string>.CreateResult(true, dictVariables[FormattedFormula]);
            }

            if (pL1Structure is IWorldSignature worldSignature)
            {
                if (worldSignature.GetSignature().Variables.Any(s => s == FormattedFormula))
                {
                    return ResultSentence<string>.CreateResult(EValidationResult.ContainsFreeVariable, false, FormattedFormula, ErrorLogFields.VALIDATION_FREEVARIABLES + $"[{FormattedFormula}]");
                }
                else
                {
                    return ResultSentence<string>.CreateResult(EValidationResult.UnknownSymbol, false, FormattedFormula, ErrorLogFields.VALIDATION_ARGUMENTUNKNOWN + $"[{FormattedFormula}]");
                }
            }

            return ResultSentence<string>.CreateResult(EValidationResult.UnexpectedResult, false, FormattedFormula, "Could not find the signature: \n" + Environment.StackTrace);
        }

        public override string ReformatFormula(Dictionary<string, string> variables)
        {
            if (variables.ContainsKey(Name))
            {
                return variables[Name];
            }
            else
            {
                return Name;
            }
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            throw new NotImplementedException();
        }
    }
}

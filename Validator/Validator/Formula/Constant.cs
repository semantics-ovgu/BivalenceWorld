using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Constant : Argument
    {
        public Constant(string name, string formattedFormula) : base(new List<Argument>(), name, formattedFormula)
        {
        }

        public override ResultSentence<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (pL1Structure.GetPl1Structure().GetConsts().ContainsKey(Name))
            {
                return ResultSentence<string>.CreateResult(true, Name);
            }
            if (pL1Structure is IWorldSignature worldSignature)
            {
                if (worldSignature.GetSignature().Consts.Any(s => s == FormattedFormula))
                {
                    return ResultSentence<string>.CreateResult(EValidationResult.ConstantNotUsed, false, FormattedFormula, ErrorLogFields.VALIDATION_CONSTANTNOTINWORLD + $"[{FormattedFormula}]");
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
            return Name;
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            throw new NotImplementedException();
        }
    }
}

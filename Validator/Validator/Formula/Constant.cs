using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validator.World;

namespace Validator
{
    public class Constant : Argument
    {
        public Constant(string name, string rawFormula) : base(new List<Argument>(), name, rawFormula)
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
                if (worldSignature.GetSignature().Consts.Any(s => s == RawFormula))
                {
                    return ResultSentence<string>.CreateResult(EValidationResult.ConstantNotUsed, false, RawFormula, ErrorLogFields.VALIDATION_CONSTANTNOTINWORLD + $"[{RawFormula}]");
                }
                else
                {
                    return ResultSentence<string>.CreateResult(EValidationResult.UnknownSymbol, false, RawFormula, ErrorLogFields.VALIDATION_ARGUMENTUNKNOWN + $"[{RawFormula}]");
                }
            }

            return ResultSentence<string>.CreateResult(EValidationResult.UnexpectedResult, false, RawFormula, "Could not find the signature: \n" + Environment.StackTrace);
        }
    }
}

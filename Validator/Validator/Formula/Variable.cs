using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validator.World;

namespace Validator
{
    public class Variable : Argument
    {
        public Variable(string name, string rawFormula) : base(new List<Argument>(), name, rawFormula)
        {
        }

        public override ResultSentence<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (dictVariables.ContainsKey(RawFormula))
            {
                return ResultSentence<string>.CreateResult(true, dictVariables[RawFormula]);
            }

            if (pL1Structure is IWorldSignature worldSignature)
            {
                if (worldSignature.GetSignature().Variables.Any(s => s == RawFormula))
                {
                    return ResultSentence<string>.CreateResult(EValidationResult.ContainsFreeVariable, false, RawFormula, ErrorLogFields.VALIDATION_FREEVARIABLES + $"[{RawFormula}]");
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

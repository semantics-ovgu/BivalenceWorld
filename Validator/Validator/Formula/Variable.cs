using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    public class Variable : Argument
    {
        public Variable(string name, string rawFormula) : base(new List<Argument>(), name, rawFormula)
        {
        }

        public override Result<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (dictVariables.ContainsKey(Name))
            {
                return Result<string>.CreateResult(true, dictVariables[Name]);
            }
            else if (pL1Structure.GetPl1Structure().GetConsts().ContainsKey(Name))
            {
                return Result<string>.CreateResult(true, Name);
            }
            else
            {
                return Result<string>.CreateResult(false, "");
            }
        }
    }
}

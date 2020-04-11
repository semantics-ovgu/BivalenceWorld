using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    public class Constant : Argument
    {
        public Constant(string name, string rawFormula) : base(new List<Argument>(), name, rawFormula)
        {
        }

        public override Result<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (dictVariables.ContainsKey(Name))
            {
                return Result<string>.CreateResult(true, dictVariables[Name]);
            }
            else
            {
                return Result<string>.CreateResult(true, Name);
            }
        }
    }
}

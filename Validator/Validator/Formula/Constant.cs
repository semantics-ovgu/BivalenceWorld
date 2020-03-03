using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    internal class Constant : Argument
    {
        public Constant(string name, string rawFormula) : base(new List<Argument>(), name, rawFormula)
        {
        }

        public override Result<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure)
        {
            List<string> constList = GetUniverseIdentifier(Arguments, pL1Structure);

            if (constList.Any())
                return Result<string>.CreateResult(true, constList[0]);
            else
                return Result<string>.CreateResult(true, Name);
        }
    }
}

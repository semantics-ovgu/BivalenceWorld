using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;

namespace Validator
{
    public class Equals : GenericFormula<Argument>, IFormulaValidate
    {
        public Equals(Argument first, Argument second) : base(new List<Argument>() { first, second }, "", "")
        {
        }


        public Result<bool> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            List<string> universeIdentifier = Argument.GetUniverseIdentifier(Arguments, pL1Structure, dictVariables);

            string u1 = universeIdentifier[0];
            string u2 = universeIdentifier[1];

            if (u1 == u2)
            {
                return Result<bool>.CreateResult(true, true);
            }
            else
            {
                return Result<bool>.CreateResult(true, false);
            }
        }
    }
}

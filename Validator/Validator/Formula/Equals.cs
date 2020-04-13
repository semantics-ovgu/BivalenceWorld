using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using Validator.World;

namespace Validator
{
    public class Equals : GenericFormula<Argument>, IFormulaValidate
    {
        public Equals(Argument first, Argument second) : base(new List<Argument>() { first, second }, "", "")
        {
        }


        public Result<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            List<string> universeIdentifier = Argument.GetUniverseIdentifier(Arguments, pL1Structure, dictVariables);

            string u1 = universeIdentifier[0];
            string u2 = universeIdentifier[1];

            if (u1 == u2)
            {
                return Result<EValidationResult>.CreateResult(true, EValidationResult.True);
            }
            else
            {
                return Result<EValidationResult>.CreateResult(true, EValidationResult.False);
            }
        }
    }
}

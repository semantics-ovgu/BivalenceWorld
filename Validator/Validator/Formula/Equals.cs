using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using Validator.Game;
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
            Result<List<string>> universeIdentifier = Argument.GetUniverseIdentifier(Arguments, pL1Structure, dictVariables);

            if (!universeIdentifier.IsValid)
            {
                return Result<EValidationResult>.CreateResult(false, EValidationResult.CanNotBeValidated);
            }

            string u1 = universeIdentifier.Value[0];
            string u2 = universeIdentifier.Value[1];

            if (u1 == u2)
            {
                return Result<EValidationResult>.CreateResult(true, EValidationResult.True);
            }
            else
            {
                return Result<EValidationResult>.CreateResult(true, EValidationResult.False);
            }
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            throw new NotImplementedException();
        }
    }
}

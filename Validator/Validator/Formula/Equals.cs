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
            SetFormattedFormula(first.FormattedFormula + "=" + second.FormattedFormula);
        }


        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<List<string>> universeIdentifier = Argument.GetUniverseIdentifier(Arguments, pL1Structure, dictVariables);

            if (!universeIdentifier.IsValid)
            {
                return ResultSentence<EValidationResult>.CreateResult(false, EValidationResult.CanNotBeValidated);
            }

            string u1 = universeIdentifier.Value[0];
            string u2 = universeIdentifier.Value[1];

            if (u1 == u2)
            {
                return ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.True);
            }
            else
            {
                return ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
            }
        }

        public override string ReformatFormula(Dictionary<string, string> variables)
        {
            return Arguments[0].ReformatFormula(variables) + "=" + Arguments[1].ReformatFormula(variables);
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            throw new NotImplementedException();
        }
    }
}

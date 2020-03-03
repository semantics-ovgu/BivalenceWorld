using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    internal class Predicate : GenericFormula<Argument>, IFormulaValidate
    {
        public Predicate(List<Argument> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }

        public Result<bool> Validate(IWorldPL1Structure pL1Structure)
        {
            Result<bool> result = Result<bool>.CreateResult(true, false);
            ConstDictionary constDict = pL1Structure.GetPl1Structure().GetConsts();
            PredicateDictionary predDict = pL1Structure.GetPl1Structure().GetPredicates();

            List<string> universeArguments = Argument.GetUniverseIdentifier(Arguments, pL1Structure);
            List<List<string>> predicateList = predDict.TryGetValue(Name);

            foreach (var predElem in predicateList)
            {
                if (predElem.SequenceEqual(universeArguments))
                {
                    result = Result<bool>.CreateResult(true, true);
                    break;
                }
            }

            return result;
        }
    }
}

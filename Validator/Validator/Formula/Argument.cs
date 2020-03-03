using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal abstract class Argument : GenericFormula<Argument>
    {
        public Argument(List<Argument> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {

        }

        public abstract Result<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure);

        public static List<string> GetUniverseIdentifier(List<Argument> arguments, IWorldPL1Structure pL1WorldStructure)
        {
            List<string> universeArguments = new List<string>();
            ConstDictionary constDict = pL1WorldStructure.GetPl1Structure().GetConsts();

            foreach (var item in arguments)
            {
                Result<string> universeIdentifier = item.GetPL1UniverseIdentifier(pL1WorldStructure);
                if (universeIdentifier.IsValid)
                    universeArguments.Add(constDict.TryGetValue(universeIdentifier.Value));
            }

            return universeArguments;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class Function : Argument
    {
        public Function(List<Argument> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }

        public override Result<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1WorldStructure)
        {
            PL1Structure pl1Structure = pL1WorldStructure.GetPl1Structure();
            ConstDictionary constDict = pl1Structure.GetConsts();
            FunctionDictionary funcDict = pl1Structure.GetFunctions();
            List<string> universeIdentifier = GetUniverseIdentifier(Arguments, pL1WorldStructure);
            ListDictionary listDict = funcDict.TryGetValue(Name);

            string result = listDict.TryGetValue(universeIdentifier);

            return Result<string>.CreateResult(true, result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    internal class Function : Argument
    {
        public Function(List<Argument> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }

        public override Result<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1WorldStructure, Dictionary<string, string> dictVariables)
        {
            PL1Structure pl1Structure = pL1WorldStructure.GetPl1Structure();
            ConstDictionary constDict = pl1Structure.GetConsts();
            FunctionDictionary funcDict = pl1Structure.GetFunctions();
            List<string> universeIdentifier = GetUniverseIdentifier(Arguments, pL1WorldStructure, dictVariables);
            ListDictionary listDict = funcDict.TryGetValue(Name);

            if (pL1WorldStructure is IWorldSignature worldSignature)
            {
                Signature signature = worldSignature.GetSignature();
                if (!signature.Functions.Any(elem => elem.Item1 == Name && elem.Item2 == Arguments.Count))
                {
                    return Result<string>.CreateResult(false, "", "Function " + Name + " not found in signature.");
                }
            }

            string result = listDict.TryGetValue(universeIdentifier);

            return Result<string>.CreateResult(true, result);
        }
    }
}

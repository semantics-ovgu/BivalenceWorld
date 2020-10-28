using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validator.Game;

namespace Validator
{
    public class Function : Argument
    {
        public Function(List<Argument> arguments, string name, string formattedFormula) : base(arguments, name, formattedFormula)
        {
        }

        public override Result<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1WorldStructure, Dictionary<string, string> dictVariables)
        {
            PL1Structure pl1Structure = pL1WorldStructure.GetPl1Structure();
            ConstDictionary constDict = pl1Structure.GetConsts();
            FunctionDictionary funcDict = pl1Structure.GetFunctions();
            Result<List<string>> universeIdentifier = GetUniverseIdentifier(Arguments, pL1WorldStructure, dictVariables);
            ListDictionary listDict = funcDict.TryGetValue(Name);

            if (pL1WorldStructure is IWorldSignature worldSignature)
            {
                Signature signature = worldSignature.GetSignature();
                if (!signature.Functions.Any(elem => elem.Item1 == Name && elem.Item2 == Arguments.Count))
                {
                    return Result<string>.CreateResult(false, "", "Function " + Name + " not found in signature.");
                }
            }

            if (!universeIdentifier.IsValid)
            {
                return Result<string>.CreateResult(false, universeIdentifier.Value.FirstOrDefault());
            }

            string result = listDict.TryGetValue(universeIdentifier.Value);
            return Result<string>.CreateResult(true, result);
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            throw new NotImplementedException();
        }
    }
}

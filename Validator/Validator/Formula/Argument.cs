﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public abstract class Argument : GenericFormula<Argument>
    {
        public Argument(List<Argument> arguments, string name, string formattedFormula) : base(arguments, name, formattedFormula)
        {

        }

        public abstract Result<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables);

        public static Result<List<string>> GetUniverseIdentifier(List<Argument> arguments, IWorldPL1Structure pL1WorldStructure, Dictionary<string, string> dictVariables)
        {
            Result<List<string>> universeArguments = Result<List<string>>.CreateResult(true, new List<string>());
            ConstDictionary constDict = pL1WorldStructure.GetPl1Structure().GetConsts();

            foreach (var item in arguments)
            {
                Result<string> universeIdentifier = item.GetPL1UniverseIdentifier(pL1WorldStructure, dictVariables);
                if (universeIdentifier.IsValid)
                {
                    universeArguments.Value.Add(constDict.TryGetValue(universeIdentifier.Value));
                }
                else
                {
                    universeArguments = Result<List<string>>.CreateResult(false, new List<string>() { universeIdentifier.Value });
                    break;
                };
            }
            return universeArguments;
        }
    }
}

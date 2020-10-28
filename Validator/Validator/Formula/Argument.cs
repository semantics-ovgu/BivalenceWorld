using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public abstract class Argument : GenericFormula<Argument>
    {
        public Argument(List<Argument> arguments, string name, string formattedFormula) : base(arguments, name, formattedFormula)
        {

        }

        public abstract ResultSentence<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables);

        public static ResultSentence<List<string>> GetUniverseIdentifier(List<Argument> arguments, IWorldPL1Structure pL1WorldStructure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<List<string>> universeArguments = ResultSentence<List<string>>.CreateResult(true, new List<string>());
            ConstDictionary constDict = pL1WorldStructure.GetPl1Structure().GetConsts();

            foreach (var item in arguments)
            {
                ResultSentence<string> universeIdentifier = item.GetPL1UniverseIdentifier(pL1WorldStructure, dictVariables);
                if (universeIdentifier.IsValid)
                {
                    universeArguments.Value.Add(constDict.TryGetValue(universeIdentifier.Value));
                }
                else
                {
                    universeArguments = ResultSentence<List<string>>.CreateResult(universeIdentifier.ValidationResult, false, new List<string>() { universeIdentifier.Value }, universeIdentifier.ErrorMessage);
                    break;
                };
            }
            return universeArguments;
        }
    }
}

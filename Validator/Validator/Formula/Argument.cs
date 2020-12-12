using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public Argument CreateArgumentWithoutFunctions(Game.Game game, Dictionary<string, string> dictVariables)
        {
            if (this is Function function)
            {
                var worldConstant = function.GetWorldConstant(game.World, dictVariables);
                var worldObject = game.WorldObjects.Find(obj => obj.Consts.Contains(worldConstant));
                if (worldObject.Tags != null)
                {
                    worldObject.Consts.Add(worldConstant);
                }
                game.AddTemporaryWorldObject(worldObject);

                return new Constant(worldConstant, worldConstant);
            }
            else
            {
                return this;
            }
        }
    }
}

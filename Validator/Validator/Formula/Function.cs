using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Function : Argument
    {
        public Function(List<Argument> arguments, string name, string formattedFormula) : base(arguments, name, formattedFormula)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(formattedFormula + "(");
            foreach (var argument in arguments)
            {
                if (argument != arguments.First())
                    builder.Append(" ");

                builder.Append(argument.FormattedFormula);

                if (argument != arguments.Last())
                    builder.Append(",");
            }

            builder.Append(")");
            SetFormattedFormula(builder.ToString());
        }

        public override ResultSentence<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1WorldStructure, Dictionary<string, string> dictVariables)
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
                    return ResultSentence<string>.CreateResult(EValidationResult.UnknownSymbol, false, "", "Function " + Name + " not found in signature.");
                }
            }

            if (!universeIdentifier.IsValid)
            {
                return ResultSentence<string>.CreateResult(false, universeIdentifier.Value.FirstOrDefault(), universeIdentifier.ErrorMessage);
            }

            string result = listDict.TryGetValue(universeIdentifier.Value);
            return ResultSentence<string>.CreateResult(true, result);
        }

        public string GetWorldConstant(IWorldPL1Structure pL1WorldStructure, Dictionary<string, string> dictVariables)
        {
            PL1Structure pl1Structure = pL1WorldStructure.GetPl1Structure();
            ConstDictionary constDict = pl1Structure.GetConsts();
            FunctionDictionary funcDict = pl1Structure.GetFunctions();
            Result<List<string>> universeIdentifier = GetUniverseIdentifier(Arguments, pL1WorldStructure, dictVariables);
            ListDictionary listDict = funcDict.TryGetValue(Name);

            string result = listDict.TryGetValue(universeIdentifier.Value);

            foreach (KeyValuePair<string, string> valuePair in constDict)
            {
                if (result == valuePair.Value)
                {
                    return valuePair.Key;
                }
            }

            return "WorldConstant not found";
        }

        public override string ReformatFormula(Dictionary<string, string> variables)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name + "(");
            foreach (var argument in Arguments)
            {
                if (argument != Arguments.First())
                    builder.Append(" ");

                builder.Append(argument.ReformatFormula(variables));

                if (argument != Arguments.Last())
                    builder.Append(",");
            }

            builder.Append(")");

            return builder.ToString();
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            throw new NotImplementedException();
        }
    }
}

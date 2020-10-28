using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Predicate : GenericFormula<Argument>, IFormulaValidate
    {
        public Predicate(List<Argument> arguments, string name, string formattedFormula) : base(arguments, name, formattedFormula)
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

        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<EValidationResult> result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
            ConstDictionary constDict = pL1Structure.GetPl1Structure().GetConsts();
            PredicateDictionary predDict = pL1Structure.GetPl1Structure().GetPredicates();

            if (pL1Structure is IWorldSignature worldSignature)
            {
                Signature signature = worldSignature.GetSignature();
                if (!signature.Predicates.Any(elem => elem.Item1 == Name && elem.Item2 == Arguments.Count))
                {
                    return ResultSentence<EValidationResult>.CreateResult(false, EValidationResult.UnknownSymbol, "Predicate " + Name + " not found in signature.");
                }
            }

            ResultSentence<List<string>> universeArguments = Argument.GetUniverseIdentifier(Arguments, pL1Structure, dictVariables);
            List<List<string>> predicateList = predDict.TryGetValue(Name);

            if (!universeArguments.IsValid)
            {
                result = ResultSentence<EValidationResult>.CreateResult(false, universeArguments.ValidationResult, universeArguments.ErrorMessage);
            }
            else
            {
                foreach (var predElem in predicateList)
                {
                    if (predElem.SequenceEqual(universeArguments.Value))
                    {
                        result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.True);
                        break;
                    }
                }
            }

            return result;
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            var result = Validate(game.World, dictVariables);

            if (game.Guess)
            {
                var endMessage = new EndMessage(game, this, "You win:\n" + FormattedFormula + "\n is true in this world", true);
                if (result.Value == EValidationResult.False)
                {
                    endMessage = new EndMessage(game, this, "You lose:\n" + FormattedFormula + "\n is false, not true, in this world", false);
                }
                return new InfoMessage(game, this, $"So you believe that \n{FormattedFormula}\n is true", endMessage);
            }
            else
            {
                var endMessage = new EndMessage(game, this, "You win:\n" + FormattedFormula + "\n is false in this world", true);
                if (result.Value == EValidationResult.True)
                {
                    endMessage = new EndMessage(game, this, "You lose:\n" + FormattedFormula + "\n is true, not false, in this world", false);
                }
                return new InfoMessage(game, this, $"So you believe that \n{FormattedFormula}\n is false", endMessage);
            }
        }
    }
}

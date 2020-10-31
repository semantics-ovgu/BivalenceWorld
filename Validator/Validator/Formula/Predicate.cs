using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validator.World;

namespace Validator
{
    public class Predicate : GenericFormula<Argument>, IFormulaValidate
    {
        public Predicate(List<Argument> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
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
    }
}

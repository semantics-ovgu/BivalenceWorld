﻿using System.Collections.Generic;
using System.Linq;
using Validator.World;

namespace Validator
{
    public class Quantum : GenericFormula<Formula>, IFormulaValidate
    {
        private EQuantumType _type = EQuantumType.None;
        private Variable _variable = null;

        public Quantum(EQuantumType type, Formula argument, Variable variable, string name, string rawFormula) : base(new List<Formula> { argument }, name,
                rawFormula)
        {
            _type = type;
            _variable = variable;
        }

        public Result<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            Result<EValidationResult> result = Result<EValidationResult>.CreateResult(true, EValidationResult.True);
            if (_type == EQuantumType.None)
            {
                result = Result<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult, "Parser failed. Quantumtype not detected");
            }

            var arguments = GetArgumentsOfType<IFormulaValidate>().ToList();
            if (arguments.Count != 1)
            {
                result = Result<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult,
                        "Invalid amount of arguments in quantum : " + arguments.Count);
            }
            else
            {
                if (_type == EQuantumType.All)
                {
                    result = ValidateAllQuantum(pL1Structure, dictVariables);
                }
                else if (_type == EQuantumType.Exist)
                {
                    result = ValidateExistQuantum(pL1Structure, dictVariables);
                }
            }

            return result;
        }

        private Result<EValidationResult> ValidateAllQuantum(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            var arguments = GetArgumentsOfType<IFormulaValidate>().First();
            var result = Result<EValidationResult>.CreateResult(true, EValidationResult.True);
            foreach (var identifier in pL1Structure.GetPl1Structure().GetConsts())
            {
                var dict = new Dictionary<string, string>(dictVariables);
                if (dict.ContainsKey(_variable.RawFormula))
                {
                    dict.Remove(_variable.RawFormula);
                };
                dict.Add(_variable.RawFormula, identifier.Key);

                var helpResult = arguments.Validate(pL1Structure, dict);
                if (!helpResult.IsValid)
                {
                    result = helpResult;
                    break;
                }

                if (helpResult.Value != EValidationResult.True)
                {
                    result = helpResult;
                }
            }

            return result;
        }

        private Result<EValidationResult> ValidateExistQuantum(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            var arguments = GetArgumentsOfType<IFormulaValidate>().First();
            var result = Result<EValidationResult>.CreateResult(true, EValidationResult.False);
            foreach (var identifier in pL1Structure.GetPl1Structure().GetConsts())
            {
                var dict = new Dictionary<string, string>(dictVariables);
                if (dict.ContainsKey(_variable.RawFormula))
                {
                    dict.Remove(_variable.RawFormula);
                };
                dict.Add(_variable.RawFormula, identifier.Key);

                var helpResult = arguments.Validate(pL1Structure, dict);
                if (!helpResult.IsValid)
                {
                    result = helpResult;
                    break;
                }

                if (helpResult.Value == EValidationResult.True)
                {
                    result = helpResult;
                }
            }

            return result;
        }
    }
}

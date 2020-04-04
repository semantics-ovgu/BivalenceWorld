using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    public class Quantum : GenericFormula<Formula>, IFormulaValidate
    {
        private EQuantumType _type = EQuantumType.None;
        private string _variable = "";


        public Quantum(EQuantumType type, Formula argument, string variable, string name, string rawFormula) : base(new List<Formula> { argument }, name, rawFormula)
        {
            _type = type;
            _variable = variable;
        }


        public Result<bool> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            Result<bool> result = Result<bool>.CreateResult(true, true);

            if (_type == EQuantumType.None)
                result = Result<bool>.CreateResult(false, false, "Parser failed. Quantumtype not detected");

            var arguments = GetArgumentsOfType<IFormulaValidate>().ToList();
            if (arguments.Count != 1)
                result = Result<bool>.CreateResult(false, false, "Invalid amount of arguments in quantum : " + arguments.Count);
            else
            {
                if (_type == EQuantumType.All)
                    result = ValidateAllQuantum(pL1Structure, dictVariables);
                else if (_type == EQuantumType.Exist)
                    result = ValidateExistQuantum(pL1Structure, dictVariables);
            }

            return result;
        }


        private Result<bool> ValidateExistQuantum(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            var arguments = GetArgumentsOfType<IFormulaValidate>().First();
            var result = Result<bool>.CreateResult(true, false);

            foreach (var identifier in pL1Structure.GetPl1Structure().GetConsts())
            {
                var dict = new Dictionary<string, string>(dictVariables)
                {
                    { _variable, identifier.Key }
                };
                var helpResult = arguments.Validate(pL1Structure, dict);

                if (!helpResult.IsValid)
                {
                    result = helpResult;
                    break;
                }

                if (helpResult.Value)
                {
                    result = helpResult;
                    break;
                }
            }

            return result;
        }

        private Result<bool> ValidateAllQuantum(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            var arguments = GetArgumentsOfType<IFormulaValidate>().First();
            var result = Result<bool>.CreateResult(true, true);

            foreach (var identifier in pL1Structure.GetPl1Structure().GetConsts())
            {
                var dict = new Dictionary<string, string>(dictVariables)
                {
                    { _variable, identifier.Key }
                };

                var helpResult = arguments.Validate(pL1Structure, dict);

                if (!helpResult.IsValid)
                {
                    result = helpResult;
                    break;
                }

                if (!helpResult.Value)
                {
                    result = helpResult;
                    break;
                }
            }

            return result;
        }
    }
}

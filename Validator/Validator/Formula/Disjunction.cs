using System.Collections.Generic;
using Validator.World;

namespace Validator
{
    public class Disjunction : GenericFormula<Formula>, IFormulaValidate
    {
        public Disjunction(List<Formula> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }

        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<EValidationResult> result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
            foreach (var conjunctionPart in GetArgumentsOfType<IFormulaValidate>())
            {
                ResultSentence<EValidationResult> validate = conjunctionPart.Validate(pL1Structure, dictVariables);
                if (validate.IsValid)
                {
                    if (validate.Value == EValidationResult.True)
                    {
                        result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.True);
                    }
                }
                else
                {
                    result = validate;
                    break;
                }
            }

            return result;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Quantum : GenericFormula<Formula>, IFormulaValidate
    {
        private EQuantumType _type = EQuantumType.None;
        private Variable _variable = null;

        public Quantum(EQuantumType type, Formula argument, Variable variable, string name, string formattedFormula) : base(new List<Formula> { argument }, name,
                formattedFormula)
        {
            _type = type;
            _variable = variable;

            string quantum = _type == EQuantumType.All ? "\u2200x" : "\u2203";

            SetFormattedFormula(quantum + variable.FormattedFormula + argument.FormattedFormula);
        }

        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<EValidationResult> result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.True);
            if (_type == EQuantumType.None)
            {
                result = ResultSentence<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult, "Parser failed. Quantumtype not detected");
            }

            var arguments = GetArgumentsOfType<IFormulaValidate>().ToList();
            if (arguments.Count != 1)
            {
                result = ResultSentence<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult,
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

        private ResultSentence<EValidationResult> ValidateAllQuantum(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            var arguments = GetArgumentsOfType<IFormulaValidate>().First();
            var result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.True);
            foreach (var identifier in pL1Structure.GetPl1Structure().GetConsts())
            {
                var dict = new Dictionary<string, string>(dictVariables);
                if (!dict.ContainsKey(_variable.FormattedFormula))
                {
                    dict.Add(_variable.FormattedFormula, identifier.Key);
                }
                else
                {
                    dict[_variable.FormattedFormula] = identifier.Key;
                }

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

        private ResultSentence<EValidationResult> ValidateExistQuantum(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            var arguments = GetArgumentsOfType<IFormulaValidate>().First();
            var result = ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
            foreach (var identifier in pL1Structure.GetPl1Structure().GetConsts())
            {
                var dict = new Dictionary<string, string>(dictVariables);
                if (!dict.ContainsKey(_variable.FormattedFormula))
                {
                    dict.Add(_variable.FormattedFormula, identifier.Key);
                }
                else
                {
                    dict[_variable.FormattedFormula] = identifier.Key;
                }

                var helpResult = arguments.Validate(pL1Structure, dict);
                if (!helpResult.IsValid)
                {
                    result = helpResult;
                    break;
                }

                if (helpResult.Value == EValidationResult.True)
                {
                    result = helpResult;
                    break;
                }
            }

            return result;
        }

        private List<Question.Selection> CreatePossibleSelection(Game.Game game, Dictionary<string, string> dictVariables)
        {
            var selection = new List<Question.Selection>();
            foreach (var combination in game.WorldObjects)
            {
                selection.Add(new Question.Selection(Arguments[0], dictVariables, combination, _variable.Name));
            }

            return selection;
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
                var questionMessage = new Question(game, this, $"Choose a block that satisfies:\n{Arguments[0].FormattedFormula}", CreatePossibleSelection(game, dictVariables));
                var infoVariable = new InfoMessage(game, this, $"So you believe that some object [{_variable.FormattedFormula}] satisfies\n{Arguments[0].FormattedFormula}\nYou will try to find an instance", endMessage);
                return new InfoMessage(game, this, $"So you believe that \n{FormattedFormula}\n is true", infoVariable);
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

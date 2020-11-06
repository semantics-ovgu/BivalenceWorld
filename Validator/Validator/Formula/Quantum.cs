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
        private KeyValuePair<string, string> _invalidConstPair = new KeyValuePair<string, string>();
        private KeyValuePair<string, string> _validConstPair = new KeyValuePair<string, string>();

        public Quantum(EQuantumType type, Formula argument, Variable variable, string name, string formattedFormula) : base(new List<Formula> { argument }, name,
                formattedFormula)
        {
            _type = type;
            _variable = variable;

            string quantum = _type == EQuantumType.All ? "\u2200" : "\u2203";

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
                    _invalidConstPair = new KeyValuePair<string, string>(_variable.Name, identifier.Key);
                }
                else
                {
                    _validConstPair = new KeyValuePair<string, string>(_variable.Name, identifier.Key);
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
                    _validConstPair = new KeyValuePair<string, string>(_variable.Name, identifier.Key);
                    break;
                }
                else
                {
                    _invalidConstPair = new KeyValuePair<string, string>(_variable.Name, identifier.Key);
                }
            }

            return result;
        }

        private List<Question.Selection> CreatePossibleSelection(Game.Game game, Dictionary<string, string> dictVariables)
        {
            var selection = new List<Question.Selection>();
            foreach (var worldObject in game.WorldObjects)
            {
                selection.Add(new Question.Selection(Arguments[0], dictVariables, worldObject, _variable.Name));
            }

            return selection;
        }

        public override string ReformatFormula(Dictionary<string, string> variables)
        {
            string quantum = _type == EQuantumType.All ? "\u2200" : "\u2203";

            return quantum + _variable.FormattedFormula + Arguments[0].ReformatFormula(variables);
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            if (_type == EQuantumType.Exist)
            {
                return CreateMoveExistQuantum(game, dictVariables);
            }
            else
            {
                return CreateMoveAllQuantum(game, dictVariables);
            }
        }

        private AMove CreateMoveExistQuantum(Game.Game game, Dictionary<string, string> dictVariables)
        {
            var result = Validate(game.World, dictVariables);

            if (game.Guess)
            {
                var questionMessage = new Question(game, this, $"Choose a block that satisfies:\n{Arguments[0].ReformatFormula(dictVariables)}", CreatePossibleSelection(game, dictVariables));
                var infoVariable = new InfoMessage(game, this, $"So you believe that some object [{_variable.ReformatFormula(dictVariables)}] satisfies\n{Arguments[0].ReformatFormula(dictVariables)}\nYou will try to find an instance", questionMessage);
                return new InfoMessage(game, this, $"So you believe that \n{ReformatFormula(dictVariables)}\n is true", infoVariable);
            }
            else
            {
                var infoVariable = new InfoMessage(game, this, $"So you believe that some object [{_variable.ReformatFormula(dictVariables)}] satisfies\n{Arguments[0].ReformatFormula(dictVariables)}\nBivalence World will try to find a counterexample", Arguments[0].CreateNextMove(game, SetResultVariableConstValue(result.Value, dictVariables)));
                return new InfoMessage(game, this, $"So you believe that \n{ReformatFormula(dictVariables)}\n is true", infoVariable);
            }
        }

        private AMove CreateMoveAllQuantum(Game.Game game, Dictionary<string, string> dictVariables)
        {
            var result = Validate(game.World, dictVariables);

            if (game.Guess)
            {
                var infoVariable = new InfoMessage(game, this, $"So you believe that every object [{_variable.ReformatFormula(dictVariables)}] satisfies\n{Arguments[0].ReformatFormula(dictVariables)}\nBivalence World will try to find a counterexample", Arguments[0].CreateNextMove(game, SetResultVariableConstValue(result.Value, dictVariables)));
                return new InfoMessage(game, this, $"So you believe that \n{ReformatFormula(dictVariables)}\n is true", infoVariable);
            }
            else
            {
                var questionMessage = new Question(game, this, $"Choose a block that satisfies:\n{Arguments[0].ReformatFormula(dictVariables)}", CreatePossibleSelection(game, dictVariables));
                var infoVariable = new InfoMessage(game, this, $"So you believe that every object [{_variable.ReformatFormula(dictVariables)}] satisfies\n{Arguments[0].ReformatFormula(dictVariables)}\nYou will try to find an instance", questionMessage);
                return new InfoMessage(game, this, $"So you believe that \n{ReformatFormula(dictVariables)}\n is true", infoVariable);
            }
        }

        private Dictionary<string, string> SetResultVariableConstValue(EValidationResult result, Dictionary<string, string> dictVariables)
        {
            Dictionary<string, string> newDictionary = new Dictionary<string, string>();

            var keyValue = new KeyValuePair<string, string>();
            if (result == EValidationResult.True)
            {
                keyValue = _validConstPair;
            }
            else
            {
                keyValue = _invalidConstPair;
            }

            foreach (KeyValuePair<string, string> pair in dictVariables)
            {
                newDictionary.Add(pair.Key, pair.Value);
            }

            if (newDictionary.ContainsKey(keyValue.Key))
            {
                newDictionary[keyValue.Key] = keyValue.Value;
            }
            else
            {
                newDictionary.Add(keyValue.Key, keyValue.Value);
            }

            return newDictionary;
        }
    }
}

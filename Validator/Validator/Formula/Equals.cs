using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using Validator.Game;
using Validator.World;

namespace Validator
{
    public class Equals : GenericFormula<Argument>, IFormulaValidate
    {
        public Equals(Argument first, Argument second) : base(new List<Argument>() { first, second }, "", "")
        {
            SetFormattedFormula(first.FormattedFormula + "=" + second.FormattedFormula);
        }


        public ResultSentence<EValidationResult> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            ResultSentence<List<string>> universeIdentifier = Argument.GetUniverseIdentifier(Arguments, pL1Structure, dictVariables);

            if (!universeIdentifier.IsValid)
            {
                return ResultSentence<EValidationResult>.CreateResult(false, EValidationResult.CanNotBeValidated, universeIdentifier.ErrorMessage);
            }

            string u1 = universeIdentifier.Value[0];
            string u2 = universeIdentifier.Value[1];

            if (u1 == u2)
            {
                return ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.True);
            }
            else
            {
                return ResultSentence<EValidationResult>.CreateResult(true, EValidationResult.False);
            }
        }

        public override string ReformatFormula(Dictionary<string, string> variables)
        {
            return Arguments[0].ReformatFormula(variables) + "=" + Arguments[1].ReformatFormula(variables);
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            var result = Validate(game.World, dictVariables);
            var withoutFunctionFormula = CreateFormulaWithoutFunctions(game, dictVariables);

            if (game.Guess)
            {
                if (result.Value == EValidationResult.True)
                {
                    if (withoutFunctionFormula != null)
                    {
                        var endMessage = new EndMessage(game, this, $"You win:\n{withoutFunctionFormula.ReformatFormula(dictVariables)}\nis true in this world.", true);
                        var infoFunctionFormula = new InfoMessage(game, withoutFunctionFormula, $"So you believe that \n{withoutFunctionFormula.ReformatFormula(dictVariables)}\n is true", endMessage);
                        var infoMessage = new InfoMessage(game, withoutFunctionFormula, $"So you believe that \n{ReformatFormula(dictVariables)}\n is true", infoFunctionFormula);
                        return infoMessage;
                    }
                    else
                    {
                        var endMessage = new EndMessage(game, this, $"You win:\n{ReformatFormula(dictVariables)}\nis true in this world.", true);
                        var infoMessage = new InfoMessage(game, this, $"So you believe that \n{ReformatFormula(dictVariables)}\n is true", endMessage);
                        return infoMessage;
                    }
                }
                else
                {
                    if (withoutFunctionFormula != null)
                    {
                        var endMessage = new EndMessage(game, this, $"You lose:\n{withoutFunctionFormula.ReformatFormula(dictVariables)}\nis false, not true, in this world.", false);
                        var infoFunctionFormula = new InfoMessage(game, withoutFunctionFormula, $"So you believe that \n{withoutFunctionFormula.ReformatFormula(dictVariables)}\n is true", endMessage);
                        var infoMessage = new InfoMessage(game, withoutFunctionFormula, $"So you believe that \n{ReformatFormula(dictVariables)}\n is false", infoFunctionFormula);
                        return infoMessage;
                    }
                    else
                    {
                        var endMessage = new EndMessage(game, this, $"You lose:\n{ReformatFormula(dictVariables)}\nis false, not true, in this world.", false);
                        var infoMessage = new InfoMessage(game, this, $"So you believe that \n{ReformatFormula(dictVariables)}\n is true", endMessage);
                        return infoMessage;
                    }
                }
            }
            else
            {
                if (result.Value == EValidationResult.True)
                {
                    if (withoutFunctionFormula != null)
                    {
                        var endMessage = new EndMessage(game, this, $"You lose:\n{withoutFunctionFormula.ReformatFormula(dictVariables)}\nis true, not false, in this world.", false);
                        var infoFunctionFormula = new InfoMessage(game, withoutFunctionFormula, $"So you believe that \n{withoutFunctionFormula.ReformatFormula(dictVariables)}\n is false", endMessage);
                        var infoMessage = new InfoMessage(game, withoutFunctionFormula, $"So you believe that \n{ReformatFormula(dictVariables)}\n is false", infoFunctionFormula);
                        return infoMessage;
                    }
                    else
                    {
                        var endMessage = new EndMessage(game, this, $"You lose:\n{ReformatFormula(dictVariables)}\nis true, not false, in this world.", false);
                        var infoMessage = new InfoMessage(game, this, $"So you believe that \n{ReformatFormula(dictVariables)}\n is false", endMessage);
                        return infoMessage;
                    }
                }
                else
                {
                    if (withoutFunctionFormula != null)
                    {
                        var endMessage = new EndMessage(game, this, $"You win:\n{withoutFunctionFormula.ReformatFormula(dictVariables)}\nis false in this world.", true);
                        var infoFunctionFormula = new InfoMessage(game, withoutFunctionFormula, $"So you believe that \n{withoutFunctionFormula.ReformatFormula(dictVariables)}\n is false", endMessage);
                        var infoMessage = new InfoMessage(game, withoutFunctionFormula, $"So you believe that \n{ReformatFormula(dictVariables)}\n is false", infoFunctionFormula);
                        return infoMessage;
                    }
                    else
                    {
                        var endMessage = new EndMessage(game, this, $"You win:\n{ReformatFormula(dictVariables)}\nis false in this world.", true);
                        var infoMessage = new InfoMessage(game, this, $"So you believe that \n{ReformatFormula(dictVariables)}\n is false", endMessage);
                        return infoMessage;
                    }
                }
            }
        }

        private Formula CreateFormulaWithoutFunctions(Game.Game game, Dictionary<string, string> dictVariables)
        {
            if (Arguments.Any(a => a is Function))
            {
                return new Equals(Arguments[0].CreateArgumentWithoutFunctions(game, dictVariables), Arguments[1].CreateArgumentWithoutFunctions(game, dictVariables));
            }
            else
            {
                return null;
            }
        }
    }
}

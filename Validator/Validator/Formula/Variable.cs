﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validator.Game;

namespace Validator
{
    public class Variable : Argument
    {
        public Variable(string name, string formattedFormula) : base(new List<Argument>(), name, formattedFormula)
        {
        }

        public override Result<string> GetPL1UniverseIdentifier(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables)
        {
            if (dictVariables.ContainsKey(Name))
            {
                return Result<string>.CreateResult(true, dictVariables[Name]);
            }
            else if (pL1Structure.GetPl1Structure().GetConsts().ContainsKey(Name))
            {
                return Result<string>.CreateResult(true, Name);
            }
            else
            {
                return Result<string>.CreateResult(false, "");
            }
        }

        public override AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables)
        {
            throw new NotImplementedException();
        }
    }
}

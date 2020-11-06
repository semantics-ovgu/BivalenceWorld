using System;
using System.Collections.Generic;
using System.Text;
using Validator.Game;

namespace Validator
{
    public abstract class Formula
    {
        private string _name = "";
        private string _formattedFormula = "";


        public Formula(string name, string formattedFormula)
        {
            _name = name;
            _formattedFormula = formattedFormula;
        }

        public string Name => _name;
        public string FormattedFormula => _formattedFormula;

        public abstract string ReformatFormula(Dictionary<string, string> variables);

        protected void SetFormattedFormula(string str) => _formattedFormula = str;

        public abstract AMove CreateNextMove(Game.Game game, Dictionary<string, string> dictVariables);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public abstract class Formula
    {
        private string _name = "";
        private string _rawFormula = "";


        public Formula(string name, string rawFormula)
        {
            _name = name;
            _rawFormula = rawFormula;
        }

        public string Name => _name;
        public string RawFormula => _rawFormula;
    }
}

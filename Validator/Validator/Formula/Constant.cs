using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class Constant : Argument
    {
        public Constant(string name, string rawFormula) : base(new List<Argument>(), name, rawFormula)
        {
        }
    }
}

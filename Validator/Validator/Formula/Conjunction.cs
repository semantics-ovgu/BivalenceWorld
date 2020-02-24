using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class Conjunction : GenericFormula<Formula>
    {
        public Conjunction(List<Formula> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }
    }
}

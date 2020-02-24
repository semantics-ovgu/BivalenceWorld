using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class Predicate : GenericFormula<Argument>
    {
        public Predicate(List<Argument> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }
    }
}

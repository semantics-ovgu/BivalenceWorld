using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal abstract class Argument : GenericFormula<Argument>
    {
        public Argument(List<Argument> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class Function : Argument
    {
        public Function(List<Argument> arguments, string name, string rawFormula) : base(arguments, name, rawFormula)
        {
        }
    }
}

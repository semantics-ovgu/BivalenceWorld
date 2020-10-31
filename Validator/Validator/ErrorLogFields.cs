using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class ErrorLogFields
    {
        public const string VALIDATION_FREEVARIABLES = "The sentence contains a free variable.";
        public const string VALIDATION_ARGUMENTUNKNOWN = "The sentence contains an unknown symbol.";
        public const string VALIDATION_CONSTANTNOTINWORLD = "The constant symbol is not used.";
    }
}

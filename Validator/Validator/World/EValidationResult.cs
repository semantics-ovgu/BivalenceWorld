using System;
using System.Collections.Generic;
using System.Text;

namespace Validator.World
{
    public enum EValidationResult
    {
        UnexpectedResult,
        ParserFailed,
        CanNotBeValidated,
        False,
        True,
        ContainsFreeVariable,
        ConstantNotUsed,
        UnknownSymbol
    }
}

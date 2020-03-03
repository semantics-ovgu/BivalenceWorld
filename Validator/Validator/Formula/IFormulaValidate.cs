using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal interface IFormulaValidate
    {
        Result<bool> Validate(IWorldPL1Structure pL1Structure);
    }
}

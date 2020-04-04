using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public interface IFormulaValidate
    {
        Result<bool> Validate(IWorldPL1Structure pL1Structure, Dictionary<string, string> dictVariables);
    }
}

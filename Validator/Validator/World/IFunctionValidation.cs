using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public interface IFunctionValidation
    {
        WorldObject Check(List<WorldObject> objs, List<WorldObject> others);
    }
}

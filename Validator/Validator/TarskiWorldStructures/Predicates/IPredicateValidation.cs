using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    interface IPredicateValidation
    {
        bool Check(List<WorldObject> obj);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public interface IPredicateValidation
    {
        bool Check(List<WorldObject> obj);
    }
}

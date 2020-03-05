using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    internal class LeftOf : IPredicateValidation
    {
        public bool Check(List<WorldObject> objs)
        {
            if (objs.Count != 2)
            {
                throw new Exception("Invalid input parameter: " + nameof(LeftOf));
            }
            else
            {
                bool result = false;
                WorldObject obj1 = objs[0];
                WorldObject obj2 = objs[1];

                var pos1 = obj1.TryGetPosition();
                var pos2 = obj2.TryGetPosition();

                if (pos1.IsValid && pos2.IsValid && pos1.Value.X < pos2.Value.X)
                    result = true;

                return result;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class SameSize : IPredicateValidation
    {
        public bool Check(List<WorldObject> obj)
        {
            if (obj.Count != 2)
            {
                throw new Exception("Invalid input parameter: " + nameof(SameSize));
            }
            else
            {
                bool result = false;
                WorldObject obj1 = obj[0];
                WorldObject obj2 = obj[1];

                if (obj1.CheckPredicate(obj2, TarskiWorldDataFields.SMALL, TarskiWorldDataFields.SMALL) ||
                    obj1.CheckPredicate(obj2, TarskiWorldDataFields.MEDIUM, TarskiWorldDataFields.MEDIUM) ||
                    obj1.CheckPredicate(obj2, TarskiWorldDataFields.LARGE, TarskiWorldDataFields.LARGE))
                {
                    result = true;
                }

                return result;
            }
        }
    }
}

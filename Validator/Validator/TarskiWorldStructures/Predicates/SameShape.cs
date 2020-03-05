using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class SameShape : IPredicateValidation
    {
        public bool Check(List<WorldObject> obj)
        {
            if (obj.Count != 2)
            {
                throw new Exception("Invalid input parameter: " + nameof(SameShape));
            }
            else
            {
                bool result = false;
                WorldObject obj1 = obj[0];
                WorldObject obj2 = obj[1];

                if (obj1.CheckPredicate(obj2, TarskiWorldDataFields.CUBE, TarskiWorldDataFields.CUBE) ||
                    obj1.CheckPredicate(obj2, TarskiWorldDataFields.TET, TarskiWorldDataFields.TET) ||
                    obj1.CheckPredicate(obj2, TarskiWorldDataFields.DODEC, TarskiWorldDataFields.DODEC))
                {
                    result = true;
                }

                return result;
            }
        }
    }
}

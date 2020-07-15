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

                if (obj1.CheckPredicate(obj2, BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.CUBE) ||
                    obj1.CheckPredicate(obj2, BivalenceWorldDataFields.TET, BivalenceWorldDataFields.TET) ||
                    obj1.CheckPredicate(obj2, BivalenceWorldDataFields.DODEC, BivalenceWorldDataFields.DODEC))
                {
                    result = true;
                }

                return result;
            }
        }
    }
}

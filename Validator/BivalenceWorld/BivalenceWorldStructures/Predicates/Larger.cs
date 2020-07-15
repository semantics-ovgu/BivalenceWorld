using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class Larger : IPredicateValidation
    {
        public bool Check(List<WorldObject> obj)
        {
            if (obj.Count != 2)
            {
                throw new Exception("Invalid input parameter: " + nameof(Larger));
            }
            else
            {
                WorldObject obj1 = obj[0];
                WorldObject obj2 = obj[1];

                if (obj1.CheckPredicate(obj2, BivalenceWorldDataFields.LARGE, BivalenceWorldDataFields.MEDIUM) ||
                    obj1.CheckPredicate(obj2, BivalenceWorldDataFields.LARGE, BivalenceWorldDataFields.SMALL) ||
                    obj1.CheckPredicate(obj2, BivalenceWorldDataFields.MEDIUM, BivalenceWorldDataFields.SMALL))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}

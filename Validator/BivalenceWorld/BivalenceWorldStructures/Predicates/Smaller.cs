using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class Smaller : IPredicateValidation
    {
        public bool Check(List<WorldObject> obj)
        {
            if (obj.Count != 2)
            {
                throw new Exception("Invalid input parameter: " + nameof(Smaller));
            }
            else
            {
                WorldObject obj1 = obj[0];
                WorldObject obj2 = obj[1];

                if (obj1.CheckPredicate(obj2, BivalenceWorldDataFields.MEDIUM, BivalenceWorldDataFields.LARGE) ||
                    obj1.CheckPredicate(obj2, BivalenceWorldDataFields.SMALL, BivalenceWorldDataFields.LARGE) ||
                    obj1.CheckPredicate(obj2, BivalenceWorldDataFields.SMALL, BivalenceWorldDataFields.MEDIUM))
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

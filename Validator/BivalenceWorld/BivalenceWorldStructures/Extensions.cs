using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public static class Extensions
    {
        public static Result<(int X, int Y)> TryGetPosition(this WorldObject obj)
        {
            Result<(int X, int Y)> result = Result<(int, int)>.CreateResult(false, (0, 0), "Obj has no [X,Y] Position");

            if (obj.Tags != null && obj.Tags.Count > 1)
            {
                if (obj.Tags[0] is int x && obj.Tags[1] is int y)
                {
                    result = Result<(int, int)>.CreateResult(true, (x, y));
                }
            }

            return result;
        }

        public static bool CheckPredicate(this WorldObject o1, WorldObject o2, string p1, string p2)
        {
            return o1.Predicates.Contains(p1) && o2.Predicates.Contains(p2);
        }
    }
}

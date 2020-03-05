using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal static class Extensions
    {
        public static Result<(int X, int Y)> TryGetPosition(this WorldObject obj)
        {
            Result<(int X, int Y)> result = Result<(int, int)>.CreateResult(false, (0, 0), "Obj has no [X,Y] Position");

            if (obj.Tags.Count > 1)
            {
                int? x = obj.Tags[0] as int?;
                int? y = obj.Tags[1] as int?;

                if (x.HasValue && y.HasValue)
                {
                    result = Result<(int, int)>.CreateResult(true, (x.Value, y.Value));
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

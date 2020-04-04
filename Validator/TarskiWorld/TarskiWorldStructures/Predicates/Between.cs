using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    internal class Between : IPredicateValidation
    {
        private bool SameDiagonal(int x1, int y1, int x2, int y2)
        {
            int diffX = Math.Abs(x2 - x1);
            int diffY = Math.Abs(y2 - y1);

            if (diffX == diffY)
                return true;
            else
                return false;
        }


        public bool Check(List<WorldObject> objs)
        {
            if (objs.Count != 3)
            {
                throw new Exception("Invalid input parameter: " + nameof(Between));
            }
            else
            {
                bool result = false;
                WorldObject obj1 = objs[0];
                WorldObject obj2 = objs[1];
                WorldObject obj3 = objs[2];

                var pos1 = obj1.TryGetPosition();
                var pos2 = obj2.TryGetPosition();
                var pos3 = obj3.TryGetPosition();

                if (pos1.IsValid && pos2.IsValid && pos3.IsValid)
                {
                    int x1 = pos1.Value.X;
                    int x2 = pos2.Value.X;
                    int x3 = pos3.Value.X;

                    int y1 = pos1.Value.Y;
                    int y2 = pos2.Value.Y;
                    int y3 = pos3.Value.Y;

                    //Column
                    if (x1 == x2 && x2 == x3 && (y1 > y2 && y1 < y3 || y1 < y2 && y1 > y3))
                        result = true;
                    else if (y1 == y2 && y2 == y3 && (x1 > x2 && x1 < x3 || x1 < x2 && x1 > x3)) //Row
                        result = true;
                    else if (SameDiagonal(x1, y1, x2, y2) && SameDiagonal(x2, y2, x3, y3) && //Diagonal
                             (x1 > x2 && x1 < x3 || x1 < x2 && x1 > x3))
                    {
                        result = true;
                    }
                }

                return result;
            }
        }
    }
}

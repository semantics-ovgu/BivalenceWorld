using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    internal class LeftMost : IFunctionValidation
    {
        public WorldObject Check(List<WorldObject> objs, List<WorldObject> others)
        {
            if (objs.Count != 1)
            {
                throw new Exception("Invalid input parameter: " + nameof(FrontOf));
            }
            else
            {
                WorldObject curr = objs[0];
                var currPos = curr.TryGetPosition().Value;

                foreach (var worldObject in others)
                {
                    var objPos = worldObject.TryGetPosition().Value;

                    if (currPos.Y == objPos.Y && objPos.X < currPos.X)
                    {
                        curr = worldObject;
                        currPos = objPos;
                    }
                }

                return curr;
            }
        }
    }
}

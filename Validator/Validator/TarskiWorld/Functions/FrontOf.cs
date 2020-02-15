using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    internal class FrontOf : IFunctionValidation
    {
        public WorldObject Check(List<WorldObject> objs, List<WorldObject> others)
        {
            if (objs.Count != 1)
            {
                throw new Exception("Invalid input parameter: " + nameof(FrontOf));
            }
            else
            {
                WorldObject result = objs[0];
                if (result.Tags != null && result.Tags.Count > 1)
                {
                    int posY = (int)result.Tags[1];
                    int posX = (int)result.Tags[0];

                    foreach (var other in others)
                    {
                        int helpY = (int)other.Tags[1];
                        int helpX = (int)other.Tags[0];

                        if (helpY < posY && helpX == posX)
                        {
                            posY = helpY;
                            result = other;
                        }
                    } 
                }

                return result;
            }
        }
    }
}

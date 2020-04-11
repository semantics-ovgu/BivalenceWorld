using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validator;

public class NotEquals : IPredicateValidation
{
    public bool Check(List<WorldObject> obj)
    {
        if (obj.Count != 2)
        {
            throw new Exception("Invalid NotEquals Parameter");
        }
        else
        {
            WorldObject o1 = obj[0];
            WorldObject o2 = obj[1];

            string id1 = o1.Consts.FirstOrDefault();
            string id2 = o2.Consts.FirstOrDefault();

            return id1 != id2;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class FunctionHelpers
    {
        public static bool CheckSameConstants(List<string> s1, List<string> s2)
        {
            if (s1.Count != s2.Count)
                return false;

            for (int i = 0; i < s1.Count; i++)
                if (s1[i] != s2[i])
                    return false;

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public struct Signature
    {
        public List<string> Consts;
        public List<(string, int)> Functions;
        public List<(string, int)> Predicates;


        public Signature(List<string> consts, List<(string, int)> functions, List<(string, int)> predicates)
        {
            Consts = consts;
            Functions = functions;
            Predicates = predicates;
        }


        public static Signature CreateTarskiWorld()
        {
            return new Signature();
        }
    }
}

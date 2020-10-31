using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    public struct Signature
    {
        public List<string> Consts;
        public List<string> Variables;
        public List<(string, int)> Functions;
        public List<(string, int)> Predicates;


        public Signature(List<string> consts, List<string> variables, List<(string, int)> predicates, List<(string, int)> functions)
        {
            Variables = variables;
            Consts = consts;
            Functions = functions;
            Predicates = predicates;
        }
    }
}

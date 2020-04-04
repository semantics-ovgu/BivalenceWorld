using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Validator
{
    public class PL1Parser
    {
        public static Formula Parse(string sentence)
        {
            PegPL1Parser parser = new PegPL1Parser();
            return parser.Parse(sentence);
        }
    }
}

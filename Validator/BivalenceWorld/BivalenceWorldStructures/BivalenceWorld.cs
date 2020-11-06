using System;
using System.Collections.Generic;

namespace Validator
{
    public class BivalenceWorld : AWorld
    {
        protected override Signature CreateSignature()
        {
            List<string> consts = new List<string>()
            {
                "a",
                "b",
                "c",
                "d",
                "e",
                "f",
            };

            List<string> variables = new List<string>()
            {
                    "u",
                    "v",
                    "w",
                    "x",
                    "y",
                    "z"
            };

            List<(string, int)> predicates = new List<(string, int)>
            {
                (BivalenceWorldDataFields.TET, 1),
                (BivalenceWorldDataFields.CUBE, 1),
                (BivalenceWorldDataFields.DODEC, 1),
                (BivalenceWorldDataFields.SMALL, 1),
                (BivalenceWorldDataFields.MEDIUM, 1),
                (BivalenceWorldDataFields.LARGE, 1),
                (BivalenceWorldDataFields.ADJOINS, 2),
                (BivalenceWorldDataFields.BACKOF, 2),
                (BivalenceWorldDataFields.FRONTOF, 2),
                (BivalenceWorldDataFields.LARGER, 2),
                (BivalenceWorldDataFields.LEFTOF, 2),
                (BivalenceWorldDataFields.RIGHTOF, 2),
                (BivalenceWorldDataFields.SAMECOL, 2),
                (BivalenceWorldDataFields.SAMEROW, 2),
                (BivalenceWorldDataFields.SAMESHAPE, 2),
                (BivalenceWorldDataFields.SAMESIZE, 2),
                (BivalenceWorldDataFields.SMALLER, 2),

                (BivalenceWorldDataFields.BETWEEN, 3)
            };

            List<(string, int)> functions = new List<(string, int)>
            {
                (BivalenceWorldDataFields.FRONTMOST, 1),
                (BivalenceWorldDataFields.BACKMOST, 1),
                (BivalenceWorldDataFields.RIGHTMOST, 1),
                (BivalenceWorldDataFields.LEFTMOST, 1),
            };

            return new Signature(consts, variables, predicates, functions);
        }

        protected override AWorld CloneWorld()
        {
            return new BivalenceWorld()
            {
                
            };
        }

        protected override Dictionary<string, IPredicateValidation> CreatePredicateDictionary()
        {
            return new Dictionary<string, IPredicateValidation>
            {
                { BivalenceWorldDataFields.ADJOINS, new Adjoins() },
                { BivalenceWorldDataFields.BACKOF, new BackOf() },
                { BivalenceWorldDataFields.BETWEEN, new Between() },
                { BivalenceWorldDataFields.FRONTOF, new FrontOf() },
                { BivalenceWorldDataFields.LARGER, new Larger() },
                { BivalenceWorldDataFields.LEFTOF, new LeftOf() },
                { BivalenceWorldDataFields.RIGHTOF, new RightOf() },
                { BivalenceWorldDataFields.SAMECOL, new SameCol() },
                { BivalenceWorldDataFields.SAMEROW, new SameRow() },
                { BivalenceWorldDataFields.SAMESHAPE, new SameShape() },
                { BivalenceWorldDataFields.SAMESIZE, new SameSize() },
                { BivalenceWorldDataFields.SMALLER, new Smaller() },
            };
        }

        protected override Dictionary<string, IFunctionValidation> CreateFunctionDictionary()
        {
            return new Dictionary<string, IFunctionValidation>()
            {
                {BivalenceWorldDataFields.BACKMOST, new BackMost() },
                {BivalenceWorldDataFields.LEFTMOST, new LeftMost() },
                {BivalenceWorldDataFields.RIGHTMOST, new RightMost() },
                {BivalenceWorldDataFields.FRONTMOST, new FrontMost() }
            };
        }
    }
}

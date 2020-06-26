using System.Collections.Generic;

namespace Validator
{
    public class TarskiWorld : AWorld
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
                "u",
                "v",
                "w",
                "x",
                "y",
                "z"
            };

            List<(string, int)> predicates = new List<(string, int)>
            {
                (TarskiWorldDataFields.TET, 1),
                (TarskiWorldDataFields.CUBE, 1),
                (TarskiWorldDataFields.DODEC, 1),
                (TarskiWorldDataFields.SMALL, 1),
                (TarskiWorldDataFields.MEDIUM, 1),
                (TarskiWorldDataFields.LARGE, 1),
                (TarskiWorldDataFields.ADJOINS, 2),
                (TarskiWorldDataFields.BACKOF, 2),
                (TarskiWorldDataFields.FRONTOF, 2),
                (TarskiWorldDataFields.LARGER, 2),
                (TarskiWorldDataFields.LEFTOF, 2),
                (TarskiWorldDataFields.RIGHTOF, 2),
                (TarskiWorldDataFields.SAMECOL, 2),
                (TarskiWorldDataFields.SAMEROW, 2),
                (TarskiWorldDataFields.SAMESHAPE, 2),
                (TarskiWorldDataFields.SAMESIZE, 2),
                (TarskiWorldDataFields.SMALLER, 2),

                (TarskiWorldDataFields.BETWEEN, 3)
            };

            List<(string, int)> functions = new List<(string, int)>
            {
                (TarskiWorldDataFields.FRONTMOST, 1),
                (TarskiWorldDataFields.BACKMOST, 1),
                (TarskiWorldDataFields.RIGHTMOST, 1),
                (TarskiWorldDataFields.LEFTMOST, 1),
            };

            return new Signature(consts, predicates, functions);
        }

        protected override Dictionary<string, IPredicateValidation> CreatePredicateDictionary()
        {
            return new Dictionary<string, IPredicateValidation>
            {
                { TarskiWorldDataFields.ADJOINS, new Adjoins() },
                { TarskiWorldDataFields.BACKOF, new BackOf() },
                { TarskiWorldDataFields.BETWEEN, new Between() },
                { TarskiWorldDataFields.FRONTOF, new FrontOf() },
                { TarskiWorldDataFields.LARGER, new Larger() },
                { TarskiWorldDataFields.LEFTOF, new LeftOf() },
                { TarskiWorldDataFields.RIGHTOF, new RightOf() },
                { TarskiWorldDataFields.SAMECOL, new SameCol() },
                { TarskiWorldDataFields.SAMEROW, new SameRow() },
                { TarskiWorldDataFields.SAMESHAPE, new SameShape() },
                { TarskiWorldDataFields.SAMESIZE, new SameSize() },
                { TarskiWorldDataFields.SMALLER, new Smaller() },
            };
        }

        protected override Dictionary<string, IFunctionValidation> CreateFunctionDictionary()
        {
            return new Dictionary<string, IFunctionValidation>()
            {
                {TarskiWorldDataFields.BACKMOST, new BackMost() },
                {TarskiWorldDataFields.LEFTMOST, new LeftMost() },
                {TarskiWorldDataFields.RIGHTMOST, new RightMost() },
                {TarskiWorldDataFields.FRONTMOST, new FrontMost() }
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validator;


public class GameWorld : AWorld
{
    protected override Dictionary<string, IFunctionValidation> CreateFunctionDictionary()
    {
        return new Dictionary<string, IFunctionValidation>
        {

        };
    }

    protected override Dictionary<string, IPredicateValidation> CreatePredicateDictionary()
    {
        return new Dictionary<string, IPredicateValidation>
        {
            { GameWorldFields.NOTEQUALS, new NotEquals() }
        };
    }

    protected override Signature CreateSignature()
    {
        return new Signature
        {
            Consts = new List<string>
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6"
            },
            Predicates = new List<(string, int)>
            {
                (GameWorldFields.DICEONE, 1),
                (GameWorldFields.DICETWO, 1),
                (GameWorldFields.DICETHREE, 1),
                (GameWorldFields.DICEFOUR, 1),
                (GameWorldFields.DICEFIVE, 1),
                (GameWorldFields.DICESIX, 1),
                (GameWorldFields.SELECTED, 1),
                (GameWorldFields.NOTSELECTED, 1),
                (GameWorldFields.NOTEQUALS, 2)
            },
            Functions = new List<(string, int)>
            {

            }
        };
    }
}

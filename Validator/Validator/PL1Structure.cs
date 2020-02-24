using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    public class PL1Structure
    {
        private PL1DataStructure _modelDataStructure;

        private string _genericIdentifier = "n";
        private int _genericIndex = 0;

        private List<string> _universe = new List<string>();
        private string _universeIdentifier = "u";


        public PL1Structure()
        {
            _modelDataStructure = new PL1DataStructure(new ConstDictionary(), new PredicateDictionary(), new FunctionDictionary());
        }


        private bool CheckPredicateList(List<string> identifier, List<string> checkList)
        {
            if (identifier.Count != checkList.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < identifier.Count; i++)
                {
                    if (identifier[i] != checkList[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private List<string> CreateUniverseConsts(params string[] arguments)
        {
            List<string> universeIdentifier = new List<string>();

            foreach (var argument in arguments)
            {
                if (_modelDataStructure.Consts.ContainsKey(argument))
                {
                    universeIdentifier.Add(_modelDataStructure.Consts[argument]);
                }
                else
                {
                    throw new Exception("Argument " + argument + " is not in Dictionary");
                }
            }

            return universeIdentifier;
        }

        private void AddFunctionKeyToDictionary(ListDictionary dictionary, string function, List<string> arguments, string resultArgument)
        {
            if (!dictionary.ContainsKey(arguments))
            {
                dictionary[arguments] = resultArgument;
            }
        }

        public string AddConst()
        {
            ConstDictionary consts = _modelDataStructure.Consts;
            string key = _genericIdentifier + _genericIndex;
            _genericIndex++;
            consts.Add(key, _universeIdentifier + consts.CurrIndex++);

            return key;
        }

        public void AddConst(string key, string keySameUniverseIdentifier = null)
        {
            ConstDictionary consts = _modelDataStructure.Consts;

            if (!consts.ContainsKey(key))
            {
                if (keySameUniverseIdentifier == null)
                {
                    consts.Add(key, _universeIdentifier + consts.CurrIndex);
                    consts.CurrIndex++;
                }
                else
                    consts.Add(key, consts[keySameUniverseIdentifier]);
            }
        }


        public void AddFunctions(string function, List<string> arguments, string resultArgument)
        {
            FunctionDictionary functions = _modelDataStructure.Functions;
            List<string> unviverseIdentifier = CreateUniverseConsts(arguments.ToArray());
            List<string> unviverseIdentifierResult = CreateUniverseConsts(resultArgument);

            if (functions.ContainsKey(function))
            {
                AddFunctionKeyToDictionary(functions[function], function, unviverseIdentifier, unviverseIdentifierResult.First());
            }
            else
            {
                functions.Add(function, new ListDictionary());
                AddFunctionKeyToDictionary(functions[function], function, unviverseIdentifier, unviverseIdentifierResult.First());
            }
        }

        public void AddPredicate(string predicate, List<string> arguments)
        {
            PredicateDictionary predicates = _modelDataStructure.Predicates;

            List<string> universeIdentifier = CreateUniverseConsts(arguments.ToArray());

            if (predicates.ContainsKey(predicate))
            {
                //--Refactorn--//
                if (!(predicates[predicate].Any(p => CheckPredicateList(universeIdentifier, p))))
                {
                    predicates[predicate].Add(universeIdentifier);
                }
            }
            else
            {
                predicates.Add(predicate, new List<List<string>>() { universeIdentifier });
            }
        }

        //--Refactorn--//
        public ConstDictionary GetConsts() => _modelDataStructure.Consts;
        public PredicateDictionary GetPredicates() => _modelDataStructure.Predicates;
        public FunctionDictionary GetFunctions() => _modelDataStructure.Functions;
    }


    public class ConstDictionary : Dictionary<string, string>
    {
        public int CurrIndex = 0;
    }

    public class PredicateDictionary : Dictionary<string, List<List<string>>>
    {

    }

    public class FunctionDictionary : Dictionary<string, ListDictionary>
    {

    }

    public class ListDictionary : Dictionary<List<string>, string>
    {
        public ListDictionary() : base(new ListComparer())
        {

        }

        private class ListComparer : IEqualityComparer<List<string>>
        {
            public bool Equals(List<string> x, List<string> y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(List<string> obj)
            {
                int hashcode = 0;
                foreach (string t in obj)
                {
                    hashcode ^= t.GetHashCode();
                }
                return hashcode;
            }
        }
    }
}

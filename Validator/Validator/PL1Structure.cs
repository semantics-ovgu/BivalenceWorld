using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    public class PL1Structure
    {
        public const string GENERIC_IDENTIFIER = "n";
        private int _genericIndex = 0;
        private PL1DataStructure _modelDataStructure;

        private List<string> _universe = new List<string>();
        private string _universeIdentifier = "u";

        public PL1Structure()
        {
            _modelDataStructure = new PL1DataStructure(new ConstDictionary(), new PredicateDictionary(), new FunctionDictionary());
        }

        //--Refactorn--//
        public ConstDictionary GetConsts() => _modelDataStructure.Consts;

        public FunctionDictionary GetFunctions() => _modelDataStructure.Functions;

        public PredicateDictionary GetPredicates() => _modelDataStructure.Predicates;

        public void Clear()
        {
            _modelDataStructure.Clear();
            _universe.Clear();
        }

        public string GetModelRepresentation(List<string> sentences, bool showGenericIdentifier = false)
        {
            StringBuilder builder = new StringBuilder();
            const int padding = 10;

            builder.AppendLine("---Universe---");
            builder.Append("Universe |-> {");
            builder.Append(string.Join(",", _universe));
            builder.AppendLine("}");

            builder.AppendLine("---Constants---");
            foreach (var constValue in GetConsts())
            {
                if (showGenericIdentifier || !constValue.Key.StartsWith(GENERIC_IDENTIFIER))
                {
                    builder.AppendLine($"{constValue.Key.PadRight(5)} |-> {constValue.Value}");
                }
            }

            builder.AppendLine("---Predicates---");
            foreach (var predicateValuePair in GetPredicates())
            {
                if (sentences.Any(s => s.Contains(predicateValuePair.Key)))
                {
                    builder.Append($"{predicateValuePair.Key.PadRight(padding)} |-> ");
                    for (var i = 0; i < predicateValuePair.Value.Count; i++)
                    {
                        var predicateValue = predicateValuePair.Value[i];
                        if (i != 0)
                        {
                            //builder.AppendFormat($"{{0,{padding + 4}}}", " ");
                        }
                        else
                        {
                            builder.Append("{");
                        }

                        if (i < predicateValuePair.Value.Count - 1)
                        {
                            if (predicateValue.Count > 1)
                            {
                                builder.Append($"({string.Join(",", predicateValue)}),");
                            }
                            else
                            {
                                builder.Append($"{string.Join(",", predicateValue)},");
                            }
                        }
                        else
                        {
                            if (predicateValue.Count > 1)
                            {
                                builder.Append($"({string.Join(",", predicateValue)})}}");
                            }
                            else
                            {
                                builder.Append($"{string.Join(",", predicateValue)}}}");
                            }
                        }
                    }

                    builder.AppendLine();
                }
            }

            builder.AppendLine("---Functions---");
            foreach (var functionValuePair in GetFunctions())
            {
                if (sentences.Any(s => s.Contains(functionValuePair.Key)))
                {
                    builder.Append($"{functionValuePair.Key.PadRight(padding)} |-> ");
                    int index = 0;
                    int maxIndex = functionValuePair.Value.Count;
                    foreach (var arguments in functionValuePair.Value)
                    {
                        if (index != 0)
                        {
                            builder.AppendFormat($"{{0,{padding + 6}}}", " ");
                        }
                        else
                        {
                            builder.Append("{");
                        }

                        if (index < maxIndex - 1)
                        {
                            if (arguments.Key.Count > 1)
                            {
                                builder.AppendLine($"({string.Join(",", arguments.Key)}) |-> {arguments.Value},");
                            }
                            else
                            {
                                builder.AppendLine($"{string.Join(",", arguments.Key)} |-> {arguments.Value},");
                            }
                        }
                        else
                        {
                            if (arguments.Key.Count > 1)
                            {
                                builder.AppendLine($"({string.Join(",", arguments.Key)}) |-> {arguments.Value}}}");
                            }
                            else
                            {
                                builder.AppendLine($"{string.Join(",", arguments.Key)} |-> {arguments.Value}}}");
                            }
                        }

                        index++;
                    }

                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }

        private void AddFunctionKeyToDictionary(ListDictionary dictionary, string function, List<string> arguments, string resultArgument)
        {
            if (!dictionary.ContainsKey(arguments))
            {
                dictionary[arguments] = resultArgument;
            }
        }


        private bool CheckPredicateList(List<string> identifier, List<string> checkList)
        {
            if (identifier.Count != checkList.Count)
            {
                return false;
            }

            for (int i = 0; i < identifier.Count; i++)
            {
                if (identifier[i] != checkList[i])
                {
                    return false;
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

        internal string AddConst()
        {
            ConstDictionary consts = _modelDataStructure.Consts;
            string key = GENERIC_IDENTIFIER + _genericIndex;
            _genericIndex++;
            string uni = _universeIdentifier + consts.CurrIndex++;
            consts.Add(key, uni);
            _universe.Add(uni);

            return key;
        }

        internal void AddConst(string key, string keySameUniverseIdentifier = null)
        {
            ConstDictionary consts = _modelDataStructure.Consts;

            if (!consts.ContainsKey(key))
            {
                if (keySameUniverseIdentifier == null)
                {
                    string uni = _universeIdentifier + consts.CurrIndex;
                    consts.Add(key, uni);
                    consts.CurrIndex++;
                    _universe.Add(uni);
                }
                else
                {
                    consts.Add(key, consts[keySameUniverseIdentifier]);
                }
            }
        }

        internal void RemoveConst(string key)
        {
            ConstDictionary consts = _modelDataStructure.Consts;
            consts.Remove(key);
            _genericIndex--;
        }


        internal void AddFunctions(string function, List<string> arguments, string resultArgument)
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

        internal void AddPredicate(string predicate, List<string> arguments)
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
                predicates.Add(predicate, new List<List<string>> { universeIdentifier });
            }
        }
    }


    public class ConstDictionary : Dictionary<string, string>
    {
        public string TryGetValue(string key)
        {
            if (ContainsKey(key))
            {
                return this[key];
            }

            return key;
        }

        public int CurrIndex = 0;
    }

    public class PredicateDictionary : Dictionary<string, List<List<string>>>
    {
        public List<List<string>> TryGetValue(string key)
        {
            if (ContainsKey(key))
            {
                return this[key];
            }

            return new List<List<string>>();
        }
    }

    public class FunctionDictionary : Dictionary<string, ListDictionary>
    {
        public ListDictionary TryGetValue(string key)
        {
            if (ContainsKey(key))
            {
                return this[key];
            }

            return new ListDictionary();
        }
    }

    public class ListDictionary : Dictionary<List<string>, string>
    {
        public ListDictionary() : base(new ListComparer())
        {

        }

        public string TryGetValue(List<string> key)
        {
            if (ContainsKey(key))
            {
                return this[key];
            }

            return "";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    public class PL1Structure
    {
        public const string GENERIC_IDENTIFIER = "n";
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

        public void Clear(WorldParameter parameter)
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
            builder.Append(string.Join(",", _universe.OrderBy(x => x)));
            builder.AppendLine("}");

            builder.AppendLine("---Constants---");
            foreach (var constValue in GetConsts().ToList().OrderBy(x => x.Key))
            {
                if (showGenericIdentifier || !constValue.Key.StartsWith(GENERIC_IDENTIFIER))
                {
                    builder.AppendLine($"{constValue.Key.PadRight(5)} |-> {constValue.Value}");
                }
            }

            builder.AppendLine("---Predicates---");
            foreach (var predicateValuePair in GetPredicates().ToList())
            {
                if (sentences.Any(s => s.Contains(predicateValuePair.Key)))
                {
                    builder.Append($"{predicateValuePair.Key.PadRight(padding)} |-> ");
                    var valueList = predicateValuePair.Value.Where(x => x.Any()).ToList();
                    var amountParameters = valueList.Count;

                    if (amountParameters > 0)
                    {

                        for (var i = 0; i < amountParameters; i++)
                        {
                            var predicateValue = valueList[i].Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x).ToList();
                            if (i != 0)
                            {
                                //builder.AppendFormat($"{{0,{padding + 4}}}", " ");
                            }
                            else
                            {
                                builder.Append("{");
                            }

                            if (i < amountParameters - 1)
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
                    }
                    else
                    {
                        builder.Append("{}");
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
            string key = GENERIC_IDENTIFIER + consts.GenericIndex;
            string uni = _universeIdentifier + consts.CurrIndex;
            consts.Add(key, uni);
            _universe.Add(uni);
            return key;
        }

        internal void AddGenericKey(WorldObject obj)
        {
            ConstDictionary consts = _modelDataStructure.Consts;
            string key = GENERIC_IDENTIFIER + consts.GenericIndex;
            obj.Consts.Add(key);
        }

        internal void AddConstAndUniverseKey(string key, string universeKey)
        {
            ConstDictionary consts = _modelDataStructure.Consts;

            if (!consts.ContainsKey(key))
            {
                consts.Add(key, universeKey);
                _universe.Add(universeKey);

                if (key.StartsWith("n"))
                {
                    consts.AddIndex(key);
                }

                consts.AddIndex(universeKey);
            }
        }

        internal void AddConst(string key, string keySameUniverseIdentifier = null)
        {
            ConstDictionary consts = _modelDataStructure.Consts;

            if (!consts.ContainsKey(key))
            {
                if (keySameUniverseIdentifier == null || !consts.ContainsKey(keySameUniverseIdentifier))
                {
                    string uni = _universeIdentifier + consts.CurrIndex;
                    consts.Add(key, uni);
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
            consts.RemoveIndex(key);
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

        public int CurrIndex
        {
            get
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (!_availableIndexes.Contains(i))
                    {
                        _availableIndexes.Add(i);
                        return i;
                    }
                }

                return -1;
            }
        }

        public int GenericIndex
        {
            get
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (!_availableGenericIndexes.Contains(i))
                    {
                        _availableGenericIndexes.Add(i);
                        return i;
                    }
                }

                return -1;
            }
        }

        public void ClearIndexes()
        {
            _availableIndexes.Clear();
            _availableGenericIndexes.Clear();
        }

        private HashSet<int> _availableIndexes = new HashSet<int>();
        private HashSet<int> _availableGenericIndexes = new HashSet<int>();

        public void AddIndex(string key)
        {
            var index = key.Substring(1);
            var indexKey = key[0];
            var indexValue = int.Parse(index);

            if (indexKey == 'u')
            {
                _availableIndexes.Add(indexValue);
            }
            else if (indexKey == 'n')
            {
                _availableGenericIndexes.Add(indexValue);
            }
        }

        public void RemoveIndex(string key)
        {
            var index = key.Substring(1);
            var indexKey = key[0];
            var indexValue = int.Parse(index);

            if (indexKey == 'u')
            {
                _availableIndexes.Remove(indexKey);
            }
            else
            {
                _availableGenericIndexes.Remove(indexValue);
            }
        }
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

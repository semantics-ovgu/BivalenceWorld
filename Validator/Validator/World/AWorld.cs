using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Validator.World;

namespace Validator
{
    public abstract class AWorld : IWorldPL1Structure, IWorldSignature, ICloneable
    {
        private Dictionary<string, IFunctionValidation> _funcValidation = new Dictionary<string, IFunctionValidation>();
        private PL1Structure _pl1Structure = new PL1Structure();
        private Dictionary<string, IPredicateValidation> _predValidation = new Dictionary<string, IPredicateValidation>();
        private Signature _signature = new Signature();
        private WorldParameter _worldParameter = new WorldParameter();

        public AWorld()
        {
            _signature = CreateSignature();
            _predValidation = CreatePredicateDictionary();
            _funcValidation = CreateFunctionDictionary();
        }

        public WorldResult<EValidationResult> Check(WorldParameter parameter)
        {
            _worldParameter = parameter;
            _pl1Structure.Clear();

            WorldResult<EValidationResult> worldResult =
                    new WorldResult<EValidationResult>(Result<List<Result<EValidationResult>>>.CreateResult(true, new List<Result<EValidationResult>>()));
            try
            {
                //--Initialize consts and static predicates--//
                foreach (var data in parameter.Data)
                {
                    string keyUniverseIdentifier = _pl1Structure.AddConst();
                    bool removeKeyUniverse = true;
                    foreach (var pred in data.Predicates)
                    {
                        if (_signature.Predicates.Any(s => s.Item1 == pred))
                        {
                            if (data.Consts.Any() && !string.IsNullOrEmpty(data.Consts.FirstOrDefault()))
                            {
                                foreach (var con in data.Consts)
                                {
                                    _pl1Structure.AddConst(con, keyUniverseIdentifier);
                                    _pl1Structure.AddPredicate(pred, new List<string> { con });
                                }
                            }
                            else
                            {
                                if (!data.Consts.Any())
                                {
                                    data.Consts.Add(keyUniverseIdentifier);
                                }
                                else
                                {
                                    data.Consts[0] = keyUniverseIdentifier;
                                }

                                _pl1Structure.AddConst(keyUniverseIdentifier, keyUniverseIdentifier);
                                _pl1Structure.AddPredicate(pred, new List<string> { keyUniverseIdentifier });
                                removeKeyUniverse = false;
                            }
                        }
                    }

                    if (removeKeyUniverse)
                    {
                        _pl1Structure.RemoveConst(keyUniverseIdentifier);
                    }
                }

                //--Calc <dynamic> predicates--//
                foreach (var pred in _signature.Predicates)
                {
                    if (_predValidation.ContainsKey(pred.Item1))
                    {
                        IPredicateValidation predValidation = _predValidation[pred.Item1];
                        IEnumerable<List<WorldObject>> worldObjects = AllWorldObjectsCombinations(parameter.Data, pred.Item2);
                        foreach (var objects in worldObjects)
                        {
                            if (predValidation.Check(objects))
                            {
                                IEnumerable<List<string>> consts = AllConstCombinations(objects);
                                foreach (var constList in consts)
                                {
                                    _pl1Structure.AddPredicate(pred.Item1, constList);
                                }
                            }
                        }
                    }
                }

                //--Calc functions--//
                foreach (var func in _signature.Functions)
                {
                    if (_funcValidation.ContainsKey(func.Item1))
                    {
                        IFunctionValidation funcValidation = _funcValidation[func.Item1];
                        IEnumerable<IEnumerable<string>> allConsts =
                                AllConstCombinations(parameter.Data.Where(s => s.Consts.Count > 0).Select(s => s.Consts.First()).ToList(), func.Item2);
                        foreach (var objects in allConsts)
                        {
                            List<WorldObject> inputObjects = new List<WorldObject>();
                            foreach (var con in objects)
                            {
                                inputObjects.Add(parameter.Data.Find(x => x.Consts.Contains(con)));
                            }

                            WorldObject result = funcValidation.Check(inputObjects.ToList(), parameter.Data);
                            _pl1Structure.AddFunctions(func.Item1, objects.ToList(), result.Consts.First());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                worldResult = new WorldResult<EValidationResult>(
                        Result<List<Result<EValidationResult>>>.CreateResult(false, new List<Result<EValidationResult>>() { Result<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult) }, e.Message));
            }

            if (worldResult.Result.IsValid && parameter.Sentences != null)
            {
                List<Result<EValidationResult>> sentences = new List<Result<EValidationResult>>();
                foreach (var item in parameter.Sentences)
                {
                    try
                    {
                        Formula formula = PL1Parser.Parse(item);
                        sentences.Add(ValidateFormula(formula));
                    }
                    catch (Exception e)
                    {
                        sentences.Add(Result<EValidationResult>.CreateResult(false, EValidationResult.ParserFailed, e.Message));
                    }
                }

                worldResult = new WorldResult<EValidationResult>(Result<List<Result<EValidationResult>>>.CreateResult(true, sentences));
            }

            return worldResult;
        }

        public PL1Structure GetPl1Structure()
        {
            return _pl1Structure;
        }

        public Signature GetSignature()
        {
            return _signature;
        }

        protected abstract Dictionary<string, IFunctionValidation> CreateFunctionDictionary();

        protected abstract Dictionary<string, IPredicateValidation> CreatePredicateDictionary();

        protected abstract Signature CreateSignature();

        private Result<EValidationResult> ValidateFormula(Formula formula)
        {
            if (formula is IFormulaValidate validateFormula)
            {
                return validateFormula.Validate(this, new Dictionary<string, string>());
            }

            return Result<EValidationResult>.CreateResult(false, EValidationResult.UnexpectedResult, "Formula has no IFormulaValidate interface");
        }

        internal IEnumerable<List<string>> AllConstCombinations(List<string> worldObjects, int length)
        {
            if (length > 1)
            {
                foreach (var objList in AllConstCombinations(worldObjects, length - 1))
                    foreach (var obj in worldObjects)
                    {
                        List<string> result = new List<string> { obj };
                        result.AddRange(objList);
                        yield return result;
                    }
            }
            else
            {
                foreach (var obj in worldObjects)
                {
                    yield return new List<string> { obj };
                }
            }
        }

        internal IEnumerable<List<string>> AllConstCombinations(List<WorldObject> worldObjects)
        {
            foreach (var con in worldObjects.First().Consts)
            {
                if (worldObjects.Count > 1)
                {
                    foreach (var conList in AllConstCombinations(worldObjects.Skip(1).ToList()))
                    {
                        List<string> result = new List<string>();
                        result.Add(con);
                        result.AddRange(conList);
                        yield return result;
                    }
                }
                else
                {
                    yield return new List<string> { con };
                }
            }
        }

        internal IEnumerable<List<WorldObject>> AllWorldObjectsCombinations(List<WorldObject> worldObjects, int length)
        {
            if (length > 1)
            {
                foreach (var objList in AllWorldObjectsCombinations(worldObjects, length - 1))
                    foreach (var obj in worldObjects)
                    {
                        List<WorldObject> result = new List<WorldObject> { obj };
                        result.AddRange(objList);
                        yield return result;
                    }
            }
            else
            {
                foreach (var obj in worldObjects)
                {
                    yield return new List<WorldObject> { obj };
                }
            }
        }

        public WorldParameter WorldParameter => _worldParameter;

        protected abstract AWorld CloneWorld();

        public object Clone()
        {
            var world = CloneWorld();

            return world;
        }
    }
}

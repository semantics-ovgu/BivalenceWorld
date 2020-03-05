using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_PL1_DynamicPred
    {
        private ConstDictionary _constDictionary = new ConstDictionary();
        private FunctionDictionary _funcDictionary = new FunctionDictionary();
        private PredicateDictionary _predDictionary = new PredicateDictionary();


        private WorldParameter CreateBiggerParameter()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.BIG }, null),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, null),
                new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.SMALL }, null),
            };

            return new WorldParameter(worldObjects, null);
        }


        [TestInitialize]
        public void PL1_Setup()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = CreateBiggerParameter();
            world.Check(parameter);

            PL1Structure structure = world.GetPl1Structure();

            _predDictionary = structure.GetPredicates();
            _constDictionary = structure.GetConsts();
            _funcDictionary = structure.GetFunctions();
        }

        [TestMethod]
        public void PL1_PredBigger_True()
        {
            Assert.IsTrue(_predDictionary.ContainsKey(TarskiWorldDataFields.LARGER));

            List<List<string>> preds = _predDictionary[TarskiWorldDataFields.LARGER];

            Assert.IsTrue(preds.Any(x => x[0] == "u0" && x[1] == "u1"));
            Assert.IsTrue(preds.Any(x => x[0] == "u0" && x[1] == "u2"));
            Assert.IsTrue(preds.Any(x => x[0] == "u1" && x[1] == "u2"));
        }

        [TestMethod]
        public void PL1_PredSmaller_True()
        {
            Assert.IsTrue(_predDictionary.ContainsKey(TarskiWorldDataFields.SMALLER));

            List<List<string>> preds = _predDictionary[TarskiWorldDataFields.SMALLER];

            Assert.IsTrue(preds.Any(x => x[0] == "u1" && x[1] == "u0"));
            Assert.IsTrue(preds.Any(x => x[0] == "u2" && x[1] == "u0"));
            Assert.IsTrue(preds.Any(x => x[0] == "u2" && x[1] == "u1"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_PL1_ConstPred_Constant
    {
        private ConstDictionary _constDictionary = new ConstDictionary();
        private FunctionDictionary _funcDictionary = new FunctionDictionary();
        private PredicateDictionary _predDictionary = new PredicateDictionary();


        private WorldParameter CreateTestParmeter()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a", "c" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, null),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.SMALL }, null),
                new WorldObject(new List<string> { "d" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE }, null),
                new WorldObject(new List<string> { }, new List<string> {BivalenceWorldDataFields.CUBE }, null)
            };

            return new WorldParameter(worldObjects, null);
        }


        [TestInitialize]
        public void PL1_Setup()
        {
            BivalenceWorld world = new BivalenceWorld();
            WorldParameter parameter = CreateTestParmeter();
            world.Check(parameter);

            PL1Structure structure = world.GetPl1Structure();

            _predDictionary = structure.GetPredicates();
            _constDictionary = structure.GetConsts();
            _funcDictionary = structure.GetFunctions();
        }

        [TestMethod]
        public void PL1_Constants_True()
        {
            BivalenceWorld world = new BivalenceWorld();
            WorldParameter parameter = CreateTestParmeter();

            world.Check(parameter);

            PL1Structure structure = world.GetPl1Structure();

            ConstDictionary consts = structure.GetConsts();
            PredicateDictionary preds = structure.GetPredicates();

            Assert.AreEqual(consts["a"], "u0");
            Assert.AreEqual(consts["c"], "u0");
            Assert.AreEqual(consts["b"], "u1");
            Assert.AreEqual(consts["d"], "u2");
        }

        [TestMethod]
        public void PL1_NoConstants()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, null),
                new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.SMALL }, null),
                new WorldObject(new List<string> { }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE }, null),
                new WorldObject(new List<string> { }, new List<string> {BivalenceWorldDataFields.CUBE }, null)
            };


            BivalenceWorld world = new BivalenceWorld();
            world.Check(new WorldParameter(worldObjects, null));

            PL1Structure structure = world.GetPl1Structure();
            ConstDictionary consts = structure.GetConsts();
            PredicateDictionary preds = structure.GetPredicates();

            Assert.AreEqual(consts["n0"], "u0");
            Assert.AreEqual(consts["n1"], "u1");
            Assert.AreEqual(consts["n2"], "u2");
            Assert.AreEqual(consts["n3"], "u3");
        }

        [TestMethod]
        public void PL1_Predicates_ContainsKey_True()
        {
            Assert.IsTrue(_predDictionary.ContainsKey(BivalenceWorldDataFields.TET));
            Assert.IsTrue(_predDictionary.ContainsKey(BivalenceWorldDataFields.LARGE));
            Assert.IsTrue(_predDictionary.ContainsKey(BivalenceWorldDataFields.SMALL));
            Assert.IsTrue(_predDictionary.ContainsKey(BivalenceWorldDataFields.CUBE));
        }

        [TestMethod]
        public void PL1_Predicates_ContainsValue_True()
        {
            List<List<string>> consts = _predDictionary[BivalenceWorldDataFields.TET];

            Assert.IsTrue(consts.Any(c => c.Contains("u0")));
            Assert.IsTrue(consts.Any(c => c.Contains("u1")));
        }
    }
}

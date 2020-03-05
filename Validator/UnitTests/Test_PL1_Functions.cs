using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_PL1_Functions
    {
        private ConstDictionary _constDictionary = new ConstDictionary();
        private FunctionDictionary _funcDictionary = new FunctionDictionary();
        private PredicateDictionary _predDictionary = new PredicateDictionary();


        private WorldParameter CreateFrontOfParameter()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.LARGE }, new List<object> { 2, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> { 2, 1 }),
                new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.SMALL }, new List<object> { 3, 0 }),
            };

            return new WorldParameter(worldObjects, null);
        }


        [TestInitialize]
        public void PL1_Setup()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = CreateFrontOfParameter();
            world.Check(parameter);

            PL1Structure structure = world.GetPl1Structure();

            _predDictionary = structure.GetPredicates();
            _constDictionary = structure.GetConsts();
            _funcDictionary = structure.GetFunctions();
        }

        //[TestMethod]
        public void PL1_FuncFrontOf_True()
        {
            Assert.IsTrue(_funcDictionary.ContainsKey(TarskiWorldDataFields.FRONTOF));

            Dictionary<List<string>, string> functions = _funcDictionary[TarskiWorldDataFields.FRONTOF];

            var o1 = functions[new List<string> { "u0" }];
            Assert.AreEqual(o1, "u1");

            var o2 = functions[new List<string> { "u1" }];
            Assert.AreEqual(o2, "u1");

            var o3 = functions[new List<string> { "u2" }];
            Assert.AreEqual(o3, "u2");
        }
    }
}

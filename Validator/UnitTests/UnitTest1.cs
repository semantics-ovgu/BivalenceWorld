using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private WorldParameter CreateTestParmeter()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a", "c" }, new List<string> {"Tet", "Big" }, null),
                new WorldObject(new List<string> { "b", "c" }, new List<string> {"Tet", "Small" }, null),
                new WorldObject(new List<string> { "b", "d" }, new List<string> {"Cube", "Big" }, null),
                new WorldObject(new List<string> { "c" }, new List<string> {"Cube" }, null)
            };

            return new WorldParameter(worldObjects, null);
        }



        [TestMethod]
        public void TestMethod1()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = CreateTestParmeter();


            world.Check(parameter);

            PL1Structure structure = world.GetPl1Structure();

            ConstDictionary consts = structure.GetConsts();
            PredicateDictionary preds = structure.GetPredicates();

            Assert.AreEqual(consts.Count, 4);

            Assert.AreEqual(consts["a"], "u0");
            Assert.AreEqual(consts["c"], "u1");
            Assert.AreEqual(consts["b"], "u2");
            Assert.AreEqual(consts["d"], "u3");
        }
    }
}

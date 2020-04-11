using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_Tarski_Functions
    {
        [TestMethod]
        public void Test_Functions()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.CUBE, TarskiWorldDataFields.LARGE }, new List<object> { 1, 1 }),
                    new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> { 2, 1 }),
                    new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.SMALL }, new List<object> { 2, 4 }),
            };
            List<string> sentences = new List<string>()
            {
                "Cube(lm(b))",
                "Medium(rm(b))",
                "fm(c) = c",
                "bm(c) = b",
                "Cube(fm(c))"
            };

            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.IsTrue(result.Result.Value[0].Value);
            Assert.IsTrue(result.Result.Value[1].Value);
            Assert.IsTrue(result.Result.Value[2].Value);
            Assert.IsTrue(result.Result.Value[3].Value);
            Assert.IsFalse(result.Result.Value[4].Value);
        }
    }
}

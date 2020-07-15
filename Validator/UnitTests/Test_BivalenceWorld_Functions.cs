using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_Functions
    {
        [TestMethod]
        public void Test_Functions()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE }, new List<object> { 1, 1 }),
                    new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> { 2, 1 }),
                    new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.SMALL }, new List<object> { 2, 4 }),
            };
            List<string> sentences = new List<string>()
            {
                "Cube(lm(b))",
                "Medium(rm(b))",
                "fm(c) = b",
                "bm(c) = c",
                "Cube(fm(c))"
            };

            BivalenceWorld world = new BivalenceWorld();
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[2].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[3].Value == EValidationResult.True);
            Assert.IsFalse(result.Result.Value[4].Value == EValidationResult.True);
        }
    }
}

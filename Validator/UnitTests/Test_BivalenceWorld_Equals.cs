using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_Equals
    {
        [TestMethod]
        public void TarskiWorld_Equals()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "  a  =  d ",
                "\u2200x (c = x \u2192 Cube(x))",
                "b = a"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a", "d" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.True);
            Assert.IsFalse(result.Result.Value[2].Value == EValidationResult.True);
        }

        [TestMethod]
        public void TarskiWorld_NotEquals()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "  b  \u2260  d ",
                    "\u2200x (c \u2260 x \u2192 Tet(x))",
                    "a \u2260 d"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a", "d" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }),
                    new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                    new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.True);
            Assert.IsFalse(result.Result.Value[2].Value == EValidationResult.True);
        }
    }
}

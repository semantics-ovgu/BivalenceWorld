using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_Negation
    {
        [TestMethod]
        public void TarskiWorld_Negation()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "\u00ac Tet(a)",
                "\u00ac ( Cube(b) \u2227 Medium(a))",
                "Tet(c) \u2228 \u00ac Tet(c)",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 3);
            Assert.IsFalse(result.Result.Value[0].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[2].Value == EValidationResult.True);
        }
    }
}

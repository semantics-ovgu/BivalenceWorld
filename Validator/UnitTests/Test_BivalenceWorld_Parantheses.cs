using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_Parantheses
    {
        [TestMethod]
        public void TarskiWorld_Right()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "Tet(a) \u2227 (  Tet(b) \u2228 Tet(c))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 1);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
        }

        [TestMethod]
        public void TarskiWorld_Left()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "(Tet(b) \u2228 Tet(c)) \u2227 Tet(a) "
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 1);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
        }

        [TestMethod]
        public void TarskiWorld_CNF()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "(Tet(a) \u2228 Tet(b)) \u2227 Tet(a) \u2227 (Tet(d) \u2228 Tet(c)) \u2227 Tet(a) "
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                new WorldObject(new List<string> { "d" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 1);
            Assert.IsFalse(result.Result.Value[0].Value == EValidationResult.True);
        }

        [TestMethod]
        public void TarskiWorld_Implication()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "(Tet(a) \u2228 Tet(b)) \u2192 (Tet(a) \u2227 Tet(d) \u2227 (Cube(d) \u2228 Cube(a)))",
                "(Tet(a) \u2228 Tet(b)) \u2192 (Tet(a) \u2227 (Cube(d) \u2228 Cube(a)))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                new WorldObject(new List<string> { "d" }, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 2);
            Assert.IsFalse(result.Result.Value[0].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.True);
        }
    }
}

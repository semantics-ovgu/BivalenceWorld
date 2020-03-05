using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_TarskiWorld_Predicate
    {

        [TestMethod]
        public void TarskiWorld_Adjoins()
        {
            TarskiWorld world = new TarskiWorld();
            List<string> sentences = new List<string>
            {
                "Adjoins(a,b)",
                "Adjoins(b,a)",
                "Adjoins(a,c)",
                "Adjoins(a,d)",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {3, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {4, 3 }),
                new WorldObject(new List<string> { "d" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 4);
            Assert.IsTrue(result.Result.Value[0].Value);
            Assert.IsTrue(result.Result.Value[1].Value);
            Assert.IsTrue(result.Result.Value[2].Value);
            Assert.IsFalse(result.Result.Value[3].Value);
        }

        [TestMethod]
        public void TarskiWorld_Between()
        {
            TarskiWorld world = new TarskiWorld();
            List<string> sentences = new List<string>
            {
                "Between(a,b,c)",
                "Between(a,c,b)",
                "Between(a,d,e)",
                "Between(a,b,e)",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                new WorldObject(new List<string> { "d" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {3, 4 }),
                new WorldObject(new List<string> { "e" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {3, 2 }),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 4);
            Assert.IsTrue(result.Result.Value[0].Value);
            Assert.IsTrue(result.Result.Value[1].Value);
            Assert.IsTrue(result.Result.Value[2].Value);
            Assert.IsFalse(result.Result.Value[3].Value);
        }

        [TestMethod]
        public void TarskiWorld_BackOf()
        {
            TarskiWorld world = new TarskiWorld();
            List<string> sentences = new List<string>
            {
                "BackOf(a,b)",
                "BackOf(a,c)",
                "BackOf(a,d)",
                "BackOf(a,e)"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                new WorldObject(new List<string> { "d" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {3, 4 }),
                new WorldObject(new List<string> { "e" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {3, 2 }),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 4);
            Assert.IsFalse(result.Result.Value[0].Value);
            Assert.IsTrue(result.Result.Value[1].Value);
            Assert.IsFalse(result.Result.Value[2].Value);
            Assert.IsTrue(result.Result.Value[3].Value);
        }
    }
}

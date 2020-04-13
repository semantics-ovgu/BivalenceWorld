using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_TarskiWorld_Quantum
    {
        [TestMethod]
        public void TarskiWorld_AllQuantum()
        {
            TarskiWorld world = new TarskiWorld();
            List<string> sentences = new List<string>
            {
                "\u2200x Tet(x)",
                "\u2200 x Tet(x)",
                "\u2200x (Tet(x) \u2227 Small(x))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 3);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.True);
            Assert.IsFalse(result.Result.Value[2].Value == EValidationResult.True);
        }


        [TestMethod]
        public void TarskiWorld_ExistQuantum()
        {
            TarskiWorld world = new TarskiWorld();
            List<string> sentences = new List<string>
            {
                "\u2203x Tet(x)",
                "\u2203 x Cube(x)",
                "\u2203x (Tet(x) \u2227 Small(x))",
                "\u2203x(Cube(x) \u2227 Medium(x))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> {  }, new List<string> {TarskiWorldDataFields.CUBE, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 4);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.True);
            Assert.IsFalse(result.Result.Value[2].Value == EValidationResult.True);
            Assert.IsTrue(result.Result.Value[3].Value == EValidationResult.True);
        }
    }
}

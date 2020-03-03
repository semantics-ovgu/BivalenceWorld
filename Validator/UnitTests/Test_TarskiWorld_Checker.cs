using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_TarskiWorld_Checker
    {
        private List<WorldObject> CreateWorldObject()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.BIG }, new List<object> {1,2 }),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {1,4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.SMALL }, new List<object> {2,4 }),
            };

            return worldObjects;
        }

        private List<string> Sentences_Atomar()
        {
            List<string> sentences = new List<string>
            {
                "Tet(a)",
                "Tet(b)",
                "Cube(a)"
            };
            return sentences;
        }

        private List<string> Sentences_Conjunction()
        {
            List<string> sentences = new List<string>
            {
                "Tet(a) \u2227 Tet(b) \u2227 Cube(c)",
                "Tet(b    ) \u2227 Cube( a )",
            };
            return sentences;
        }

        private List<string> Sentences_InvalidOperator()
        {
            List<string> sentences = new List<string>
            {
                "Tet(a) + Tet(b)",
                "Tet(   b) + Cube(   a)",
            };
            return sentences;
        }


        [TestMethod]
        public void TarskiWorld_Atomar()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = new WorldParameter(CreateWorldObject(), Sentences_Atomar());
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 3);
            Assert.IsTrue(result.Result.Value[0].Value);
            Assert.IsTrue(result.Result.Value[1].Value);
            Assert.IsFalse(result.Result.Value[2].Value);
        }
    }
}

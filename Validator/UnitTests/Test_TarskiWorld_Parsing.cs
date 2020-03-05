using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_TarskiWorld_Parsing
    {
        private List<WorldObject> CreateWorldObject()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.LARGE }, new List<object> {1,2 }),
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
                "Tet(b) \u2227 Tet(a)",
                "Cube(c) \u2227 Tet(b)",
                "Tet(a) \u2227 Tet(b) \u2227 Tet(c)",
            };
            return sentences;
        }

        private List<string> Sentences_Disjunction()
        {
            List<string> sentences = new List<string>
            {
                "Tet(a) \u2228 Cube(a) \u2228 Cube(c)",
                "Tet(b) \u2228 Tet(a)",
                "Cube(c) \u2228 Tet(b)",
                "Tet(a) \u2228 Tet(b) \u2228 Tet(c)",
                "Cube(b) \u2228 Tet(d) \u2228 Tet(f)",
            };
            return sentences;
        }

        private List<string> Sentences_DisAndConjunction()
        {
            List<string> sentences = new List<string>
            {
                "Tet(a) \u2228 Cube(a) \u2227 Cube(c)",
                "Cube(a) \u2228 Cube(b) \u2227 Tet(b)",
                "Tet(b) \u2227 Tet(a) \u2228 Tet(c)",
                "Tet(a) \u2228 Cube(b) \u2227 Tet(b)"
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
        public void TarskiWorld_Atomar_Valid()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = new WorldParameter(CreateWorldObject(), Sentences_Atomar());
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 3);
            Assert.IsTrue(result.Result.Value[0].Value);
            Assert.IsTrue(result.Result.Value[1].Value);
            Assert.IsFalse(result.Result.Value[2].Value);
        }

        [TestMethod]
        public void TarskiWorld_Conjunction_Valid()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = new WorldParameter(CreateWorldObject(), Sentences_Conjunction());
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 4);
            Assert.IsFalse(result.Result.Value[0].Value);
            Assert.IsTrue(result.Result.Value[1].Value);
            Assert.IsFalse(result.Result.Value[2].Value);
            Assert.IsTrue(result.Result.Value[3].Value);
        }

        [TestMethod]
        public void TarskiWorld_Disjunction_Valid()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = new WorldParameter(CreateWorldObject(), Sentences_Disjunction());
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 5);
            Assert.IsTrue(result.Result.Value[0].Value);
            Assert.IsTrue(result.Result.Value[1].Value);
            Assert.IsTrue(result.Result.Value[2].Value);
            Assert.IsTrue(result.Result.Value[3].Value);
            Assert.IsFalse(result.Result.Value[4].Value);
        }

        [TestMethod]
        public void TarskiWorld_DisAndConjunction_Valid()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = new WorldParameter(CreateWorldObject(), Sentences_DisAndConjunction());
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 4);
            Assert.IsTrue(result.Result.Value[0].Value);
            Assert.IsFalse(result.Result.Value[1].Value);
            Assert.IsTrue(result.Result.Value[2].Value);
            Assert.IsTrue(result.Result.Value[3].Value);
        }

        [TestMethod]
        public void TarskiWorld_InvalidOperator()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = new WorldParameter(CreateWorldObject(), Sentences_InvalidOperator());
            var result = world.Check(parameter);

            Assert.AreEqual(result.Result.Value.Count, 2);
            foreach (var obj in result.Result.Value)
                Assert.IsFalse(obj.IsValid);
        }
    }
}

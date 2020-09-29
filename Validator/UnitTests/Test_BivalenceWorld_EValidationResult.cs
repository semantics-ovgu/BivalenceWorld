using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_EValidationResult
    {
        private const string AllQuantum = "\u2200";
        private const string ExistQuantum = "\u2203";
        private const string Implication = "\u2192";
        private const string AND = "\u2227";
        private const string OR = "\u2228";

        [TestMethod]
        public void TarskiWorld_ParserError()
        {
            List<string> sentences = new List<string>
            {
                "Tesa)",
                "(Tet(a)"
            };
            List<WorldObject> worldObjects = new List<WorldObject>()
            {
                    new WorldObject(new List<string>() { "a" }, new List<string>() {"Tet"}, new List<object>() )
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.ParserFailed);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.ParserFailed);
        }

        public void TarskiWorld_CanNotBeValidated()
        {
            List<string> sentences = new List<string>
            {
                    "Tet(b)",
                    "Tes(a)",
                    "Tet(rds(a))",
                    $"∀x (Cube(x) ∧ Cube(y))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>()
            {
                    new WorldObject(new List<string>() { "a", "c" }, new List<string>() {"Tet", "Large"}, new List<object> {3,2} )
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.CanNotBeValidated);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.CanNotBeValidated);
            Assert.IsTrue(result.Result.Value[2].Value == EValidationResult.CanNotBeValidated);
            Assert.IsTrue(result.Result.Value[3].Value == EValidationResult.CanNotBeValidated);
        }

        [TestMethod]
        public void TarskiWorld_FreeVariable()
        {
            List<string> sentences = new List<string>
            {
                    $"∀x (Cube(x) ∧ Cube(y))",
                    $"∀y (Cube(x) ∧ Cube(y))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>()
            {
                    new WorldObject(new List<string>() { "a", "c" }, new List<string>() {"Tet", "Large"}, new List<object> {3,2} )
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.ContainsFreeVariable);
            Assert.IsTrue(result.Result.Value[0].ErrorMessage.Length > 0);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.ContainsFreeVariable);
            Assert.IsTrue(result.Result.Value[1].ErrorMessage.Length > 0);
        }

        [TestMethod]
        public void TarskiWorld_UknownArgumentSymbol()
        {
            List<string> sentences = new List<string>
            {
                    $"(Cube(a) ∧ Cube(l))",
                    $"(Cube(m) ∧ Cube(n))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>()
            {
                    new WorldObject(new List<string>() { "a", "c" }, new List<string>() {"Tet", "Large"}, new List<object> {3,2} )
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.UnknownSymbol);
            Assert.IsTrue(result.Result.Value[0].ErrorMessage.Length > 0);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.UnknownSymbol);
            Assert.IsTrue(result.Result.Value[1].ErrorMessage.Length > 0);
        }

        [TestMethod]
        public void TarskiWorld_ConstantNotInWorld()
        {
            List<string> sentences = new List<string>
            {
                    $"(Cube(a) ∧ Cube(b))",
                    $"(Cube(e) ∧ Cube(d))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>()
            {
                    new WorldObject(new List<string>() { "a", "c" }, new List<string>() {"Tet", "Large"}, new List<object> {3,2} )
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.ConstantNotUsed);
            Assert.IsTrue(result.Result.Value[0].ErrorMessage.Length > 0);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.ConstantNotUsed);
            Assert.IsTrue(result.Result.Value[1].ErrorMessage.Length > 0);
        }

        [TestMethod]
        public void TarskiWorld_PredicateUnknown()
        {
            List<string> sentences = new List<string>
            {
                    $"(Cub(a) ∧ Cube(c))",
                    $"(Tes(a) ∧ Tat(c))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>()
            {
                    new WorldObject(new List<string>() { "a", "c" }, new List<string>() {"Tet", "Large"}, new List<object> {3,2} )
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.UnknownSymbol);
            Assert.IsTrue(result.Result.Value[0].ErrorMessage.Length > 0);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.UnknownSymbol);
            Assert.IsTrue(result.Result.Value[1].ErrorMessage.Length > 0);
        }

        [TestMethod]
        public void TarskiWorld_FunctionUnknown()
        {
            List<string> sentences = new List<string>
            {
                    $"(Cub(func(a)) ∧ Cube(c))",
                    $"(Tes(a) ∧ Tat(taz(c)))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>()
            {
                    new WorldObject(new List<string>() { "a", "c" }, new List<string>() {"Tet", "Large"}, new List<object> {3,2} )
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.UnknownSymbol);
            Assert.IsTrue(result.Result.Value[0].ErrorMessage.Length > 0);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.UnknownSymbol);
            Assert.IsTrue(result.Result.Value[1].ErrorMessage.Length > 0);
        }
    }
}

using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_TarskiWorld_EValidationResult
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
            TarskiWorld world = new TarskiWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.ParserFailed);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.ParserFailed);
        }

        [TestMethod]
        public void TarskiWorld_CanNotBeValidated()
        {
            List<string> sentences = new List<string>
            {
                    "Tet(b)",
                    "Tes(a)",
                    "Tet(rds(a))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>()
            {
                    new WorldObject(new List<string>() { "a" }, new List<string>() {"Tet"}, new List<object>() )
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            TarskiWorld world = new TarskiWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.CanNotBeValidated);
            Assert.IsTrue(result.Result.Value[1].Value == EValidationResult.CanNotBeValidated);
            Assert.IsTrue(result.Result.Value[2].Value == EValidationResult.CanNotBeValidated);
        }
    }
}

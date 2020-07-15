using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_IntegrationTests
    {
        private const string AllQuantum = "\u2200";
        private const string ExistQuantum = "\u2203";
        private const string Implication = "\u2192";
        private const string AND = "\u2227";
        private const string OR = "\u2228";

        [TestMethod]
        public void TarskiWorld_ComplexSentences()
        {
            List<string> sentences = new List<string>
            {
                    "(\u2203x Dodec(x)) \u2192 ((\u2200y (Tet(y) \u2192 Medium(y))) \u2227 Dodec(f))",
                    "(\u2203x Dodec(x)) \u2192 ((\u2200y (Tet(y) \u2192 Medium(y))) \u2227 Dodec(e))"
            };
            WorldParameter parameter = new WorldParameter(CreateWorld(), sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
            Assert.IsFalse(result.Result.Value[1].Value == EValidationResult.True);
        }

        [TestMethod]
        public void TarskiWorld_IntegrationRound01()
        {
            List<string> sentences = new List<string>
            {
                    "Tet(a) \u2192  (∃x (Dodec(x)  ∧ Large(x)))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> {"a"}, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.SMALL}, new List<object> {3, 3}),
                    new WorldObject(new List<string>(), new List<string> {BivalenceWorldDataFields.DODEC, BivalenceWorldDataFields.LARGE}, new List<object> {4, 4})
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
        }

        [TestMethod]
        public void TarskiWorld_IntegrationRound02()
        {
            List<string> sentences = new List<string>
            {
                    "\u2200 x (Tet(x) \u2192  Large(x))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> {"a"}, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE},
                            new List<object> {3, 3}),
                    new WorldObject(new List<string>(), new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE}, new List<object> {3, 3})
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
        }

        [TestMethod]
        public void TarskiWorld_IntegrationRound03()
        {
            List<string> sentences = new List<string>
            {
                    $"{AllQuantum}x (Large(x) {Implication} (Tet(x)" +
                    $"{OR} {ExistQuantum}y (Cube(x) {AND} Cube(y))" +
                    $"{OR} {ExistQuantum}y (Dodec(x) {AND} Dodec(y))))"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> {"a"}, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE},
                            new List<object> {3, 3}),
                    new WorldObject(new List<string> {"b"}, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE},
                            new List<object> {3, 3}),
                    new WorldObject(new List<string>(), new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM}, new List<object> {3, 3})
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            Assert.IsTrue(result.Result.Value[0].Value == EValidationResult.True);
        }

        [TestMethod]
        public void TarskiWorld_ModelRepresentation()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> {"a"}, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE},
                            new List<object> {3, 3}),
                    new WorldObject(new List<string> {"b"}, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE},
                            new List<object> {5, 3}),
                    new WorldObject(new List<string> {"c"}, new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.LARGE},
                            new List<object> {4, 3}),
                    new WorldObject(new List<string>(), new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM}, new List<object> {3, 5})
            };
            WorldParameter parameter = new WorldParameter(worldObjects, new List<string>());
            BivalenceWorld world = new BivalenceWorld();
            var result = world.Check(parameter);
            var representation = world.GetPl1Structure().GetModelRepresentation();
        }

        private List<WorldObject> CreateWorld()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> {"a"}, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM},
                            new List<object> {3, 3}),
                    new WorldObject(new List<string> {"b"}, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM},
                            new List<object> {4, 4}),
                    new WorldObject(new List<string> {"c"}, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM},
                            new List<object> {2, 2}),
                    new WorldObject(new List<string> {"d"}, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM},
                            new List<object> {2, 2}),
                    new WorldObject(new List<string>(), new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM}, new List<object> {2, 2}),
                    new WorldObject(new List<string>(), new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM}, new List<object> {2, 2}),
                    new WorldObject(new List<string>(), new List<string> {BivalenceWorldDataFields.CUBE, BivalenceWorldDataFields.MEDIUM}, new List<object> {2, 2}),
                    new WorldObject(new List<string> {"f"}, new List<string> {BivalenceWorldDataFields.DODEC, BivalenceWorldDataFields.MEDIUM},
                            new List<object> {2, 2})
            };
            return worldObjects;
        }
    }
}

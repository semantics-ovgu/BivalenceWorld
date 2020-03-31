using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_TarskiWorld_IntegrationTests
    {
        private List<WorldObject> CreateWorld()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                new WorldObject(new List<string> { "d" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                new WorldObject(new List<string> { }, new List<string> {TarskiWorldDataFields.CUBE, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                new WorldObject(new List<string> { }, new List<string> {TarskiWorldDataFields.CUBE, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                new WorldObject(new List<string> { }, new List<string> {TarskiWorldDataFields.CUBE, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                new WorldObject(new List<string> { "f" }, new List<string> {TarskiWorldDataFields.DODEC, TarskiWorldDataFields.MEDIUM }, new List<object> {2, 2 })
            };

            return worldObjects;
        }


        [TestMethod]
        public void TarskiWorld_ComplexSentences()
        {
            List<string> sentences = new List<string>
            {
                "(\u2203x Dodec(x)) \u21D2 ((\u2200y (Tet(y) \u21D2 Medium(y))) \u2227 Dodec(f))",
                "(\u2203x Dodec(x)) \u21D2 ((\u2200y (Tet(y) \u21D2 Medium(y))) \u2227 Dodec(e))"
            };
            WorldParameter parameter = new WorldParameter(CreateWorld(), sentences);

            TarskiWorld world = new TarskiWorld();
            var result = world.Check(parameter);

            Assert.IsTrue(result.Result.Value[0].Value);
            Assert.IsFalse(result.Result.Value[1].Value);
        }


        [TestMethod]
        public void TarskiWorld_IntegrationRound01()
        {
            List<string> sentences = new List<string>
            {
                "Tet(a) \u21d2  (∃x (Dodec(x)  ∧ Large(x)))"                
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.SMALL }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { }, new List<string> {TarskiWorldDataFields.DODEC, TarskiWorldDataFields.LARGE }, new List<object> {4, 4 }),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);

            TarskiWorld world = new TarskiWorld();
            var result = world.Check(parameter);

            Assert.IsTrue(result.Result.Value[0].Value);
        }
    }
}

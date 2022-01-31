using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;
using Validator.World;

namespace UnitTests
{
    [TestClass]
    public class Test_BivalenceWorld_IDs
    {
        [TestMethod]
        public void TarskiWorld_AfterChangedWorld_SameIds()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "\u2200x Tet(x)",
                "\u2200 x Tet(x)",
                "\u2200x (Tet(x) \u2227 Small(x))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }, 2),
            };
            List<WorldObject> worldObjects2 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }, 2),
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1),
            };
            List<WorldObject> worldObjects3 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {1, 1 }, 1),
                    new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }, 2),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            WorldParameter parameter2 = new WorldParameter(worldObjects2, sentences);
            WorldParameter parameter3 = new WorldParameter(worldObjects3, sentences);

            var result = world.Check(parameter);
            var consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);

            result = world.Check(parameter2);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);

            result = world.Check(parameter3);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);
        }

        [TestMethod]
        public void TarskiWorld_AfterChangedWorld_OnlyTwoIds()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "\u2200x Tet(x)",
                "\u2200 x Tet(x)",
                "\u2200x (Tet(x) \u2227 Small(x))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }),
                new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }),
            };
            List<WorldObject> worldObjects2 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }),
            };
            List<WorldObject> worldObjects3 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {1, 1 }),
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }),
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            WorldParameter parameter2 = new WorldParameter(worldObjects2, sentences);
            WorldParameter parameter3 = new WorldParameter(worldObjects3, sentences);

            var result = world.Check(parameter);
            var consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual(2, consts.Count);

            result = world.Check(parameter2);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual(2, consts.Count);

            result = world.Check(parameter3);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual(2, consts.Count);
        }

        [TestMethod]
        public void TarskiWorld_AfterChangedWorld_SameIds_WithUnknownConstants()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "\u2200x Tet(x)",
                "\u2200 x Tet(x)",
                "\u2200x (Tet(x) \u2227 Small(x))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1),
                new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }, 2),
                new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }, 3)
            };
            List<WorldObject> worldObjects2 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }, 2),
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1),
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }, 3)
            };
            List<WorldObject> worldObjects3 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {1, 1 }, 1),
                    new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }, 2),
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }, 3)
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            WorldParameter parameter2 = new WorldParameter(worldObjects2, sentences);
            WorldParameter parameter3 = new WorldParameter(worldObjects3, sentences);

            var result = world.Check(parameter);
            var consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);

            result = world.Check(parameter2);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);

            result = world.Check(parameter3);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);
        }

        [TestMethod]
        public void TarskiWorld_AfterChangedWorld_UniverseKeyCorrect()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "\u2200x Tet(x)",
                "\u2200 x Tet(x)",
                "\u2200x (Tet(x) \u2227 Small(x))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1)
            };
            List<WorldObject> worldObjects2 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1),
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }, 2)
            };
            List<WorldObject> worldObjects3 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1),
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }, 2),
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {5, 5 }, 3)
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            WorldParameter parameter2 = new WorldParameter(worldObjects2, sentences);
            WorldParameter parameter3 = new WorldParameter(worldObjects3, sentences);

            var result = world.Check(parameter);
            var consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["n0"]);

            result = world.Check(parameter2);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["n0"]);
            Assert.AreEqual("u1", consts["n1"]);

            result = world.Check(parameter3);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["n0"]);
            Assert.AreEqual("u1", consts["n1"]);
            Assert.AreEqual("u2", consts["n2"]);
        }

        [TestMethod]
        public void TarskiWorld_AfterChangedWorld_ConstantMappingAreEqual()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "\u2200x Tet(x)",
                "\u2200 x Tet(x)",
                "\u2200x (Tet(x) \u2227 Small(x))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1)
            };
            List<WorldObject> worldObjects2 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1),
                    new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {2, 2 }, 2)
            };
            List<WorldObject> worldObjects3 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1),
                    new WorldObject(new List<string> { "b" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }, 2),
                    new WorldObject(new List<string> { "c" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {1, 1 }, 3)
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            WorldParameter parameter2 = new WorldParameter(worldObjects2, sentences);
            WorldParameter parameter3 = new WorldParameter(worldObjects3, sentences);

            var result = world.Check(parameter);
            var consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);

            result = world.Check(parameter2);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);
            Assert.AreEqual("u1", consts["b"]);

            result = world.Check(parameter3);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);
            Assert.AreEqual("u1", consts["b"]);
            Assert.AreEqual("u2", consts["c"]);
        }

        [TestMethod]
        public void TarskiWorld_AfterChangedWorld_ConstantRemoved()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                "\u2200x Tet(x)",
                "\u2200 x Tet(x)",
                "\u2200x (Tet(x) \u2227 Small(x))",
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1)
            };
            List<WorldObject> worldObjects2 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1)
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            WorldParameter parameter2 = new WorldParameter(worldObjects2, sentences);

            var result = world.Check(parameter);
            var consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["a"]);

            result = world.Check(parameter2);
            consts = world.GetPl1Structure().GetConsts();

            Assert.AreEqual("u0", consts["n0"]);
        }

        [TestMethod]
        public void TarskiWorld_AfterChangedWorld_PredicatesEqual()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "Large(a)"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string>(), new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {3, 3 }, 1)
            };
            List<WorldObject> worldObjects2 = new List<WorldObject>
            {
                    new WorldObject(new List<string>(), new List<string> {BivalenceWorldDataFields.TET, BivalenceWorldDataFields.LARGE }, new List<object> {5, 5 }, 1)
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            WorldParameter parameter2 = new WorldParameter(worldObjects2, sentences);

            var result = world.Check(parameter);
            var pred = world.GetPl1Structure().GetPredicates();

            Assert.IsTrue(pred["Large"].Any(x => x.Any(y => y == "u0")));

            result = world.Check(parameter2);
            pred = world.GetPl1Structure().GetPredicates();

            Assert.IsTrue(pred["Large"].Any(x => x.Any(y => y == "u0")));
        }

        [TestMethod]
        public void TarskiWorld_AfterChangedWorld_PredicatesEqualConstants()
        {
            BivalenceWorld world = new BivalenceWorld();
            List<string> sentences = new List<string>
            {
                    "Large(a)"
            };
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.DODEC, BivalenceWorldDataFields.MEDIUM }, new List<object> {4, 4 }, 146812)
            };
            List<WorldObject> worldObjects2 = new List<WorldObject>
            {
                    new WorldObject(new List<string> { "" }, new List<string> {BivalenceWorldDataFields.DODEC, BivalenceWorldDataFields.LARGE }, new List<object> {4, 4 }, 146812)
            };
            WorldParameter parameter = new WorldParameter(worldObjects, sentences);
            WorldParameter parameter2 = new WorldParameter(worldObjects2, sentences);

            var result = world.Check(parameter);
            var pred = world.GetPl1Structure().GetPredicates();

            Assert.IsTrue(pred["Medium"].Any(x => x.Any(y => y == "u0")));

            result = world.Check(parameter2);
            pred = world.GetPl1Structure().GetPredicates();

            Assert.IsTrue(pred["Large"].Any(x => x.Any(y => y == "u0")));
        }
    }
}

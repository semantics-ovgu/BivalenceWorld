using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_TarskiWorld
    {
        private WorldParameter CreateBiggerParameter()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.BIG }, new List<object> {1,2 }),
                new WorldObject(new List<string> { "b" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.MEDIUM }, new List<object> {1,4 }),
                new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.SMALL }, new List<object> {2,4 }),
            };

            return new WorldParameter(worldObjects, null);
        }

        [TestInitialize]
        public void PL1_Setup()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = CreateBiggerParameter();
            world.Check(parameter);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_World_Objects
    {
        [TestMethod]
        public void PL1_WorldObject_Equals()
        {
            WorldObject obj1 = new WorldObject(new List<string>() { "a" }, new List<string>() { "Tet", "Medium" }, new List<object>() { 3, 5 });
            WorldObject obj2 = new WorldObject(new List<string>() { "a" }, new List<string>() { "Tet", "Larger" }, new List<object>() { 3, 5 });
            WorldObject obj3 = new WorldObject(new List<string>() { "a" }, new List<string>() { "Tet", "Medium" }, new List<object>() { 4, 5 });

            Assert.IsTrue(obj1.Equals(obj2));
            Assert.IsFalse(obj1.Equals(obj3));
        }

        [TestMethod]
        public void PL1_WorldObject_DictionaryContains()
        {
            WorldObject obj1 = new WorldObject(new List<string>() { "a" }, new List<string>() { "Tet", "Medium" }, new List<object>() { 3, 5 });
            WorldObject obj2 = new WorldObject(new List<string>() { "a" }, new List<string>() { "Tet", "Large" }, new List<object>() { 3, 5 });
            WorldObject obj3 = new WorldObject(new List<string>() { "a" }, new List<string>() { "Tet", "Medium" }, new List<object>() { 4, 5 });

            List<WorldObject> list = new List<WorldObject>();
            list.Add(obj1);

            Assert.IsTrue(list.Contains(obj2));
            Assert.IsFalse(list.Contains(obj3));
        }
    }
}

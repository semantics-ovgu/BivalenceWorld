using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_Parser_Atoms
    {
        [TestMethod]
        public void PL1_Predicate_Single()
        {
            string atom = "Tet(a)";

            Predicate predicate = PL1Parser.Parse(atom) as Predicate;
            Assert.AreEqual(predicate.Name, "Tet");

            Constant constant = predicate.Arguments[0] as Constant;
            Assert.IsNotNull(constant);

            Assert.AreEqual(constant.Name, "a");
        }

        [TestMethod]
        public void PL1_Predicate_Whitespaces()
        {
            string atom = "Tet(  a   )";

            Predicate predicate = PL1Parser.Parse(atom) as Predicate;
            Assert.AreEqual(predicate.Name, "Tet");

            Constant constant = predicate.Arguments[0] as Constant;
            Assert.IsNotNull(constant);

            Assert.AreEqual(constant.Name, "a");
        }

        [TestMethod]
        public void PL1_Predicate_Multi()
        {
            string atom = "Between(a,b,c)";

            Predicate predicate = PL1Parser.Parse(atom) as Predicate;
            Assert.AreEqual(predicate.Name, "Between");

            Constant constant = predicate.Arguments[0] as Constant;
            Assert.IsNotNull(constant);
            Assert.AreEqual(constant.Name, "a");

            constant = predicate.Arguments[1] as Constant;
            Assert.IsNotNull(constant);
            Assert.AreEqual(constant.Name, "b");

            constant = predicate.Arguments[2] as Constant;
            Assert.IsNotNull(constant);
            Assert.AreEqual(constant.Name, "c");
        }
    }
}

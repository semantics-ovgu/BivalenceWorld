using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_Parser_Conjunctions
    {
        [TestMethod]
        public void PL1_Conjunction_Atoms()
        {
            string atom = "Tet(a) \u2227 Tet(b)";

            Conjunction conjunction = PL1Parser.Parse(atom) as Conjunction;
            Assert.IsNotNull(conjunction);

            Predicate predicate = conjunction.Arguments[0] as Predicate;

            Assert.IsNotNull(predicate);
            Assert.AreEqual(predicate.Name, "Tet");

            Constant constant = predicate.Arguments[0] as Constant;
            Assert.IsNotNull(constant);
            Assert.AreEqual(constant.Name, "a");


            predicate = conjunction.Arguments[1] as Predicate;
            Assert.IsNotNull(predicate);

            Assert.AreEqual(predicate.Name, "Tet");

            constant = predicate.Arguments[0] as Constant;
            Assert.IsNotNull(constant);
            Assert.AreEqual(constant.Name, "b");
        }
    }
}

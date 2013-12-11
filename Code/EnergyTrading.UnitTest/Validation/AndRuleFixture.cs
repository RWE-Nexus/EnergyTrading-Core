namespace EnergyTrading.UnitTest.Validation
{
    using System;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Validation;

    [TestClass]
    public class AndRuleFixture
    {
        [TestMethod]
        public void IsValidIfAllRulesAreValid()
        {
            var r1 = new Mock<IRule<int>>();
            var r2 = new Mock<IRule<int>>();
            r1.Setup(r => r.IsValid(It.IsAny<int>())).Returns(true);
            r2.Setup(r => r.IsValid(It.IsAny<int>())).Returns(true);
            var combination = new AndRule<int>(new List<IRule<int>> { r1.Object, r2.Object });
            var candidate = combination.IsValid(2);
            Assert.IsTrue(candidate);
            r1.Verify(r => r.IsValid(It.IsAny<int>()), Times.Once());
            r2.Verify(r => r.IsValid(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void NotValidIfARuleFails()
        {
            var msg = "r2 message";
            var r1 = new Mock<IRule<int>>();
            var r2 = new Mock<IRule<int>>();
            var r3 = new Mock<IRule<int>>();
            r1.Setup(r => r.IsValid(It.IsAny<int>())).Returns(true);
            r2.Setup(r => r.IsValid(It.IsAny<int>())).Returns(false);
            r2.Setup(r => r.Message).Returns(msg);
            r3.Setup(r => r.IsValid(It.IsAny<int>())).Returns(true);
            var combination = new AndRule<int>(new List<IRule<int>> { r1.Object, r2.Object, r3.Object });
            var candidate = combination.IsValid(2);
            Assert.IsFalse(candidate);
            Assert.AreEqual(msg + Environment.NewLine, combination.Message);
            r1.Verify(r => r.IsValid(It.IsAny<int>()), Times.Once());
            r2.Verify(r => r.IsValid(It.IsAny<int>()), Times.Once());
            r3.Verify(r => r.IsValid(It.IsAny<int>()), Times.Once());
        }
    }
}
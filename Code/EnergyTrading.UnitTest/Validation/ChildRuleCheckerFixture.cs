namespace EnergyTrading.UnitTest.Validation
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Validation;

    [TestClass]
    public class ChildRuleCheckerFixture
    {
        private class Parent
        {
            public string Child { get; set; }
        }

        [TestMethod]
        public void ValidWhenRuleIsValid()
        {
            var innerRule = new Mock<IRule<string>>();
            innerRule.Setup(r => r.IsValid(It.IsAny<string>())).Returns(true);
            var count = 0;
            var parent = new Parent { Child = "test" };
            var prefix = "starts with";
            var rule = new ChildRuleChecker<Parent, string>(innerRule.Object,
                                                            p =>
                                                            {
                                                                count++;
                                                                return p.Child;
                                                            },
                                                            prefix);
            var candidate = rule.IsValid(parent);
            Assert.IsTrue(candidate);
            innerRule.Verify(r => r.IsValid("test"), Times.Once());
            Assert.AreEqual(1, count);
            Assert.IsTrue(string.IsNullOrEmpty(rule.Message));
        }

        [TestMethod]
        public void InvalidWhenRuleIsInvalid()
        {
            var innerRule = new Mock<IRule<string>>();
            var msg = "innerRule message";
            innerRule.Setup(r => r.IsValid(It.IsAny<string>())).Returns(false);
            innerRule.Setup(r => r.Message).Returns(msg);
            var count = 0;
            var parent = new Parent { Child = "test" };
            var prefix = "starts with";
            var rule = new ChildRuleChecker<Parent, string>(innerRule.Object,
                                                            p =>
                                                            {
                                                                count++;
                                                                return p.Child;
                                                            },
                                                            prefix);
            var candidate = rule.IsValid(parent);
            Assert.IsFalse(candidate);
            Assert.AreEqual(1, count);
            innerRule.Verify(r => r.IsValid("test"), Times.Once());
            Assert.AreEqual(prefix + " " + msg, rule.Message);
        }
    }
}
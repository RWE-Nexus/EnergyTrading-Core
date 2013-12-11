namespace EnergyTrading.UnitTest.Validation
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Validation;

    [TestClass]
    public class NotRuleFixture
    {
        [TestMethod]
        public void DefaultMessageIsEmpty()
        {
            var innerRule = new Mock<IRule<string>>();
            var rule = new NotRule<string>(innerRule.Object);
            Assert.IsTrue(string.IsNullOrEmpty(rule.Message));
        }

        [TestMethod]
        public void MessagePassedToConstructorIsReturned()
        {
            const string Msg = "msg";
            var innerRule = new Mock<IRule<string>>();
            var rule = new NotRule<string>(innerRule.Object, Msg);
            Assert.AreEqual(Msg, rule.Message);
        }

        [TestMethod]
        public void ResultIsNegationOfInnerRuleResult()
        {
            var innerRule = new Mock<IRule<string>>();
            innerRule.Setup(r => r.IsValid(bool.TrueString)).Returns(true);
            innerRule.Setup(r => r.IsValid(bool.FalseString)).Returns(false);
            var rule = new NotRule<string>(innerRule.Object);
            var candidate1 = rule.IsValid(bool.TrueString);
            var candidate2 = rule.IsValid(bool.FalseString);
            Assert.IsFalse(candidate1);
            Assert.IsTrue(candidate2);
            innerRule.Verify(r => r.IsValid(bool.TrueString), Times.Once());
            innerRule.Verify(r => r.IsValid(bool.FalseString), Times.Once());
        }
    }
}

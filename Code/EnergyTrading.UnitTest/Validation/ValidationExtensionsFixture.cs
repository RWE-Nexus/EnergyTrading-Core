namespace EnergyTrading.UnitTest.Validation
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Validation;

    [TestClass]
    public class ValidationExtensionsFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateExtensionThrowsIfInvalid()
        {
            var mockValidator = new Mock<IValidator<string>>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<IList<IRule>>())).Returns(false);

            mockValidator.Object.Validate("some value");
        }

        [TestMethod]
        public void ValidateExtensionReturnsIfValid()
        {
            var mockValidator = new Mock<IValidator<string>>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<IList<IRule>>())).Returns(true);

            mockValidator.Object.Validate("some value");
        }
    }
}
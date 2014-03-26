namespace EnergyTrading.UnitTest.Validation
{
    using System.Collections.Generic;

    using EnergyTrading.Validation;

    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class ValidationExtensionsFixture
    {
        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void ValidateExtensionThrowsIfInvalid()
        {
            var mockValidator = new Mock<IValidator<string>>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<IList<IRule>>())).Returns(false);

            mockValidator.Object.Validate("some value");
        }

        [Test]
        public void ValidateExtensionReturnsIfValid()
        {
            var mockValidator = new Mock<IValidator<string>>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), It.IsAny<IList<IRule>>())).Returns(true);

            mockValidator.Object.Validate("some value");
        }
    }
}
namespace TradeEnrichment.BusAdapter.UnitTests.MessageFiltering.ByTrader
{
    using System;
    using System.Collections.Generic;

    using EnergyTrading.Filtering;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class TraderFilterFixture
    {
        private Mock<IFilterRepository> mockTraderRepository;

        [SetUp]
        public void SetUp()
        {
            mockTraderRepository = new Mock<IFilterRepository>();
        }

        private ItemFilter CreateFilter()
        {
            return new ItemFilter(mockTraderRepository.Object);
        }

        [Test]
        public void ConstructionTests()
        {
            Assert.That(() => new ItemFilter(null), Throws.TypeOf<ArgumentNullException>());
            Assert.That(this.CreateFilter(), Is.Not.Null);
        }

        [Test]
        public void FalseIfTraderIsNull()
        {
            var candidate = this.CreateFilter().IsExcluded(null);
            Assert.IsFalse(candidate);
            mockTraderRepository.Verify(x => x.Included, Times.Never());
            mockTraderRepository.Verify(x => x.Excluded, Times.Never());
        }

        [Test]
        public void FalseIfRepositoryIsEmpty()
        {
            mockTraderRepository.Setup(x => x.Included).Returns(new List<string>());
            mockTraderRepository.Setup(x => x.Excluded).Returns(new List<string>());
            var candidate = this.CreateFilter().IsExcluded("anything");
            Assert.IsFalse(candidate);
            mockTraderRepository.Verify(x => x.Included, Times.Once());
            mockTraderRepository.Verify(x => x.Excluded, Times.Once());
        }

        [Test]
        public void TrueIfIdentifierNotInSuppliedIncludeList()
        {
            mockTraderRepository.Setup(x => x.Included).Returns(new List<string> { "identifier" });
            mockTraderRepository.Setup(x => x.Excluded).Returns(new List<string>());
            var candidate = this.CreateFilter().IsExcluded("suppliedIdentifier");
            Assert.IsTrue(candidate);
            mockTraderRepository.Verify(x => x.Included, Times.Exactly(2));
            mockTraderRepository.Verify(x => x.Excluded, Times.Never());
        }

        [Test]
        public void FalseIfIdentifierInSuppliedIncludeList()
        {
            mockTraderRepository.Setup(x => x.Included).Returns(new List<string> { "suppliedIdentifier" });
            mockTraderRepository.Setup(x => x.Excluded).Returns(new List<string>());
            var candidate = this.CreateFilter().IsExcluded("suppliedIdentifier");
            Assert.IsFalse(candidate);
            mockTraderRepository.Verify(x => x.Included, Times.Exactly(2));
            mockTraderRepository.Verify(x => x.Excluded, Times.Once());
        }

        [Test]
        public void TrueIfInExcludeList()
        {
            mockTraderRepository.Setup(x => x.Included).Returns(new List<string>());
            mockTraderRepository.Setup(x => x.Excluded).Returns(new List<string> { "suppliedIdentifier" });
            var candidate = this.CreateFilter().IsExcluded("suppliedIdentifier");
            Assert.IsTrue(candidate);
            mockTraderRepository.Verify(x => x.Included, Times.Once());
            mockTraderRepository.Verify(x => x.Excluded, Times.Exactly(2));
        }

        [Test]
        public void FalseIfNotInExcludeList()
        {
            mockTraderRepository.Setup(x => x.Included).Returns(new List<string>());
            mockTraderRepository.Setup(x => x.Excluded).Returns(new List<string> { "supplied" });
            var candidate = this.CreateFilter().IsExcluded("suppliedIdentifier");
            Assert.IsFalse(candidate);
            mockTraderRepository.Verify(x => x.Included, Times.Once());
            mockTraderRepository.Verify(x => x.Excluded, Times.Exactly(2));
        }
    }
}
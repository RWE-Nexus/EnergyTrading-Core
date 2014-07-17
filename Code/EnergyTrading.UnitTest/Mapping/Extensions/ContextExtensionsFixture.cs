namespace EnergyTrading.UnitTest.Mapping.Extensions
{
    using EnergyTrading.Mapping;
    using EnergyTrading.Mapping.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class ContextExtensionsFixture
    {
        [Test]
        public void CheckOrRetrieveValueWithNullContext()
        {
            var candidate = ContextExtensions.CheckOrRetrieveValue(null, "name", () => 3);
            Assert.That(candidate, Is.EqualTo(default(int)));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        public void CheckOrRetrieveValueWithInvalidNames(string name)
        {
            var candidate = new Context().CheckOrRetrieveValue(name, () => 3);
            Assert.That(candidate, Is.EqualTo(default(int)));
        }

        [Test]
        public void CheckOrRetrieveValueRetrieverIsNull()
        {
            var candidate = new Context().CheckOrRetrieveValue<int>("name");
            Assert.That(candidate, Is.EqualTo(default(int)));
        }

        [Test]
        public void ValueFromContextIsReturnedIfPresent()
        {
            var count = 0;
            var context = new Context();
            context.Set("name", 55);
            var candidate = context.CheckOrRetrieveValue("name", () => ++count);
            Assert.That(candidate, Is.EqualTo(55));
            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public void ValueFromRetrieverIsReturnedAndStoredInContext()
        {
            var count = 0;
            var context = new Context();
            var candidate = context.CheckOrRetrieveValue("name", () => ++count);
            Assert.That(candidate, Is.EqualTo(count));
            Assert.That(count, Is.EqualTo(1));
            Assert.That(context.Value<int>("name"), Is.EqualTo(1));
        }
    }
}
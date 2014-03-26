namespace EnergyTrading.Test
{
    using NUnit.Framework;

    [TestFixture]
    public abstract class SpecBase
    {
        [SetUp]
        public virtual void MainSetup()
        {
            this.Initialize();
            this.Establish_context();
            this.Because_of();
        }

        [TearDown]
        public virtual void MainTeardown()
        {
            this.Cleanup();
        }

        protected virtual void Because_of()
        {
        }

        protected virtual void Cleanup()
        {
        }

        protected virtual void Establish_context()
        {
        }

        protected virtual void Initialize()
        {
        }
    }
}
namespace EnergyTrading.Test
{
    using NUnit.Framework;

    [TestFixture]
    public abstract class SpecBase<TContext>
    {
        protected TContext Sut { get; private set; }

        [SetUp]
        public virtual void MainSetup()
        {
            this.Initialize();
            this.Sut = this.Establish_context();
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
            this.Sut = default(TContext);
        }

        protected abstract TContext Establish_context();

        protected virtual void Initialize()
        {
        }
    }
}
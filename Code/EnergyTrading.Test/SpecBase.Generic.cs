namespace EnergyTrading.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract class SpecBase<TContext>
    {
        protected TContext Sut { get; private set; }

        [TestInitialize]
        public virtual void MainSetup()
        {
            this.Initialize();
            this.Sut = this.Establish_context();
            this.Because_of();
        }

        [TestCleanup]
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
namespace EnergyTrading.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract class SpecBase
    {
        [TestInitialize]
        public virtual void MainSetup()
        {
            this.Initialize();
            this.Establish_context();
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
        }

        protected virtual void Establish_context()
        {
        }

        protected virtual void Initialize()
        {
        }
    }
}
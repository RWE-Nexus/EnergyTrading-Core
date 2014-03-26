namespace EnergyTrading.UnitTest.Threading
{
    using System;

    using EnergyTrading.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class CancellationTokenOwnerFixture
    {
        [Test]
        public void NewOwnerIsNotCancelled()
        {
            var owner = new CancellationTokenOwner();
            var token = owner.Token;

            Assert.IsFalse(owner.IsCancellationRequested, "Owner not cancelled");
            Assert.IsFalse(token.IsCancellationRequested, "Token not cancelled");
        }

        [Test]
        public void CancelSetsIsCancellationRequested()
        {
            var owner = new CancellationTokenOwner();
            var token = owner.Token;

            owner.Cancel();

            Assert.IsTrue(owner.IsCancellationRequested, "Owner not cancelled");
            Assert.IsTrue(token.IsCancellationRequested, "Token not cancelled");
        }

        [Test]
        public void ResetClearsIsCancellationRequested()
        {
            var owner = new CancellationTokenOwner();

            owner.Cancel();
            owner.Reset();

            var token = owner.Token;

            Assert.IsFalse(owner.IsCancellationRequested, "Owner not cancelled");
            Assert.IsFalse(token.IsCancellationRequested, "Token not cancelled");         
        }

        [Test]
        public void MustCancelBeforeReset()
        {
            var owner = new CancellationTokenOwner();

            try
            {
                owner.Reset();

                Assert.Fail("Expected I");
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
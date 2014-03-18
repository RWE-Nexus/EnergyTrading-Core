namespace EnergyTrading.UnitTest.Threading
{
    using System;

    using EnergyTrading.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class TaskExtensionsFixture
    {
        [Test]
        public void RunsFuncAtLeastOnce()
        {
            var count = 0;
            TaskExtensions.Retry(() =>
            {
                ++count;
                return 0;
            }, 3);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void RunsActionAtLeastOnce()
        {
            var count = 0;
            TaskExtensions.Retry(() =>
            {
                ++count;
            });
            Assert.AreEqual(1, count);
        }

        [Test]
        public void RunsFuncAtLeastOnceIfRetriesIsZero()
        {
            var count = 0;
            TaskExtensions.Retry(() =>
            {
                ++count;
                return 0;
            }, 0);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void RunsActionAtLeastOnceIfRetriesIsZero()
        {
            var count = 0;
            TaskExtensions.Retry(() =>
            {
                ++count;
            }, 0);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void RetryFuncThrowsImmediatelyIfRetriesIsZero()
        {
            var count = 0;
            try
            {
                TaskExtensions.Retry<int>(() =>
                {
                    ++count;
                    throw new InvalidOperationException();
                }, 0);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
            }
            Assert.AreEqual(1, count);
        }

        [Test]
        public void RetryActionThrowsImmediatelyIfRetriesIsZero()
        {
            var count = 0;
            try
            {
                TaskExtensions.Retry(() =>
                {
                    ++count;
                    throw new InvalidOperationException();
                }, 0);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
            }
            Assert.AreEqual(1, count);
        }

        [Test]
        public void RetryFuncThrowsAfterCorrectNumberOfAttempts()
        {
            var count = 0;
            try
            {
                TaskExtensions.Retry<int>(() =>
                {
                    ++count;
                    throw new InvalidOperationException();
                }, 3);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
            }
            Assert.AreEqual(4, count);
        }

        [Test]
        public void RetryActionThrowsAfterCorrectNumberOfAttempts()
        {
            var count = 0;
            try
            {
                TaskExtensions.Retry(() =>
                {
                    ++count;
                    throw new InvalidOperationException();
                }, 3);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
            }
            Assert.AreEqual(4, count);
        }

        [Test]
        public void RetryFuncDoesNotThrowIfSubsequentAttemptSucceeds()
        {
            var count = 0;
            TaskExtensions.Retry(() =>
            {
                ++count;
                if (count < 2)
                {
                    throw new InvalidOperationException();
                }
                return count;
            }, 3);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void RetryActionDoesNotThrowIfSubsequentAttemptSucceeds()
        {
            var count = 0;
            TaskExtensions.Retry(() =>
            {
                ++count;
                if (count < 2)
                {
                    throw new InvalidOperationException();
                }
            }, 3);
            Assert.AreEqual(2, count);
        }
    }
}
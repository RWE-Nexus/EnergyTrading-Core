namespace EnergyTrading.UnitTest.Threading
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Threading;

    [TestClass]
    public class TaskExtensionsFixture
    {
        [TestMethod]
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

        [TestMethod]
        public void RunsActionAtLeastOnce()
        {
            var count = 0;
            TaskExtensions.Retry(() =>
            {
                ++count;
            });
            Assert.AreEqual(1, count);
        }

        [TestMethod]
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

        [TestMethod]
        public void RunsActionAtLeastOnceIfRetriesIsZero()
        {
            var count = 0;
            TaskExtensions.Retry(() =>
            {
                ++count;
            }, 0);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
namespace EnergyTrading.Test.MoqSpecific
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;

    using Moq;

    /// <summary>
    /// Counts the number of times a Setup action is called on a mock
    /// Useful when test items are shared across individual tests (e.g. Assembly wide ILogger mock) 
    /// </summary>
    public class MockCounter<T> : IDisposable where T : class
    {
        private Expression<Action<T>> SetupAction { get; set; }
        private Mock<T> Mock { get; set; }
        private int count;

        public MockCounter(Mock<T> mock, Expression<Action<T>> setupAction)
        {
            if (mock == null)
            {
                throw new ArgumentNullException("mock");
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException("setupAction");
            }

            this.Mock = mock;
            this.SetupAction = setupAction;
            this.count = 0;
            this.Mock.Setup(this.SetupAction).Callback(() => Interlocked.Increment(ref this.count));
        }

        ~MockCounter()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                GC.SuppressFinalize(this);
            }

            this.Mock.Setup(this.SetupAction).Callback(() => { });
        }

        public int Count 
        { 
            get { return this.count; }
        }
    }
}
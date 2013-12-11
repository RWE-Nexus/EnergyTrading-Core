namespace EnergyTrading.Test
{
    using System;
    using System.Linq.Expressions;

    using Microsoft.Practices.Unity;
    using Moq;

    using EnergyTrading.Test.MoqSpecific;

    public static class MoqExtensions
    {
        public static Mock<T> RegisterMock<T>(this IUnityContainer container) where T : class
        {
            var mock = new Mock<T>();

            container.RegisterInstance<Mock<T>>(mock);
            container.RegisterInstance<T>(mock.Object);

            return mock;
        }

        public static void RegisterExistingMock(this IUnityContainer container, Mock mock, Type type)
        {
            container.RegisterInstance(type, mock);
            container.RegisterInstance(mock.Object);
        }

        /// <summary>
        /// Use this to add additional setups for a mock that is already registered
        /// </summary>
        public static Mock<T> ConfigureMockFor<T>(this IUnityContainer container) where T : class
        {
            return container.Resolve<Mock<T>>();
        }

        public static void VerifyMockFor<T>(this IUnityContainer container) where T : class
        {
            container.Resolve<Mock<T>>().VerifyAll();
        }

        public static MockCounter<T> StartCounting<T>(this Mock<T> mock, Expression<Action<T>> setupAction)
            where T : class
        {
            return new MockCounter<T>(mock, setupAction);
        }
    }
}
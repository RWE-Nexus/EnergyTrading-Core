namespace EnergyTrading.Test
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using Moq;

    public class AutoMockingContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var strategy = new AutoMockingBuilderStrategy(Container);

            Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }

        private class AutoMockingBuilderStrategy : BuilderStrategy
        {
            private readonly IUnityContainer container;

            public AutoMockingBuilderStrategy(IUnityContainer container)
            {
                this.container = container;
            }

            public override void PreBuildUp(IBuilderContext context)
            {
                var key = context.OriginalBuildKey;

                if (key.Type.IsInterface && !container.IsRegistered(key.Type))
                {
                    context.Existing = CreateDynamicMock(key.Type, container);
                }
            }

            private static object CreateDynamicMock(Type type, IUnityContainer container)
            {
                var genericMockType = typeof(Mock<>).MakeGenericType(type);

                var mock = (Mock)Activator.CreateInstance(genericMockType);

                container.RegisterExistingMock(mock, genericMockType);

                return mock.Object;
            }
        }
    }
}
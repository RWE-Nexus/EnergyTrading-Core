namespace EnergyTrading.UnitTest.Container.Unity.AutoRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Container.Unity.AutoRegistration;
    using EnergyTrading.UnitTest.Container.Unity.AutoRegistration.Contract;

    [TestClass]
    public class AutoRegistrationFixture
    {
        private Mock<IUnityContainer> containerMock;
        private List<RegisterEvent> registered;
        private IUnityContainer container;
        private delegate void RegistrationCallback(Type from, Type to, string name, LifetimeManager lifetime, InjectionMember[] ims);
        private IUnityContainer realContainer;

        [TestInitialize]
        public void SetUp()
        {
            this.realContainer = new UnityContainer();

            this.containerMock = new Mock<IUnityContainer>();
            this.registered = new List<RegisterEvent>();
            var setup = this.containerMock
                .Setup(c => c.RegisterType(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<LifetimeManager>()));
            var callback = new RegistrationCallback((from, to, name, lifetime, ips) =>
            {
                this.registered.Add(new RegisterEvent(from, to, name, lifetime));
                this.realContainer.RegisterType(from, to, name, lifetime);
            });

            // Using reflection, because current version of Moq doesn't support callbacks with more than 4 arguments
            setup
                .GetType()
                .GetMethod("SetCallbackWithArguments", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(setup, new object[] { callback });

            this.container = this.containerMock.Object;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenContainerIsNull_ThrowsException()
        {
            this.container = null;
            this.container
                .ConfigureAutoRegistration();
        }

        [TestMethod]
        public void WhenApplingAutoRegistrationWithoutAnyRules_NothingIsRegistered()
        {
            this.container
                .ConfigureAutoRegistration()
                .ApplyAutoRegistration();
            Assert.IsFalse(this.registered.Any());
        }

        [TestMethod]
        public void WhenApplingAutoRegistrationWithOnlyAssemblyRules_NothingIsRegistered()
        {
            this.container
                .ConfigureAutoRegistration()
                .ApplyAutoRegistration();
            Assert.IsFalse(this.registered.Any());
        }

        [TestMethod]
        public void WhenApplyMethodIsNotCalled_AutoRegistrationDoesNotHappen()
        {
            this.container
                .ConfigureAutoRegistration()
                .Include(If.Is<TestCache>, Then.Register());

            Assert.IsFalse(this.registered.Any());
        }

        [TestMethod]
        public void WhenAssemblyIsExcluded_AutoRegistrationDoesNotHappenForItsTypes()
        {
            this.container
                .ConfigureAutoRegistration()
                .Include(If.Is<TestCache>, Then.Register())
                .ExcludeAssemblies(If.ContainsType<TestCache>)
                .ApplyAutoRegistration();

            Assert.IsFalse(this.registered.Any());
        }

        //[TestMethod]
        //public void WhenExternalAssemblyIsLoaded_AutoRegistrationHappensForItsTypes()
        //{
        //    _container
        //        .ConfigureAutoRegistration()
        //        .LoadAssemblyFrom(String.Format("{0}.dll", KnownExternalAssembly))
        //        .ExcludeSystemAssemblies()
        //        .Include(If.Any, Then.Register())
        //        .ApplyAutoRegistration();

        //    Assert.IsTrue(_registered.Any());
        //}

        [TestMethod]
        public void WhenTypeIsExcluded_AutoRegistrationDoesNotHappenForIt()
        {
            this.container.ConfigureAutoRegistration().Exclude(If.Is<TestCache>).Include(
                If.Is<TestCache>, Then.Register()).ApplyAutoRegistration();

            Assert.IsFalse(this.registered.Any());
        }

        [TestMethod]
        public void WhenRegisterWithDefaultOptions_TypeMustBeRegisteredAsAllInterfacesItImplementsUsingPerCallLifetimeWithEmptyName()
        {
            this.container
                .ConfigureAutoRegistration()
                .Include(If.Is<TestCache>, Then.Register())
                .ApplyAutoRegistration();

            Assert.IsTrue(this.registered.Count == 2);

            var iCacheRegisterEvent = this.registered.SingleOrDefault(r => r.From == typeof(ICache));
            var iDisposableRegisterEvent = this.registered.SingleOrDefault(r => r.From == typeof(IDisposable));

            Assert.IsNotNull(iCacheRegisterEvent);
            Assert.IsNotNull(iDisposableRegisterEvent);
            Assert.AreEqual(typeof(TestCache), iCacheRegisterEvent.To);
            Assert.AreEqual(typeof(TransientLifetimeManager), iCacheRegisterEvent.Lifetime.GetType());
            Assert.AreEqual(string.Empty, iCacheRegisterEvent.Name);
            Assert.AreEqual(typeof(TestCache), iDisposableRegisterEvent.To);
            Assert.AreEqual(typeof(TransientLifetimeManager), iDisposableRegisterEvent.Lifetime.GetType());
            Assert.AreEqual(string.Empty, iDisposableRegisterEvent.Name);
        }

        [TestMethod]
        public void WhenRegistrationObjectIsPassed_RequestedTypeRegisteredAsExpected()
        {
            const string RegistrationName = "TestName";

            var registration = Then.Register();
            registration.Interfaces = new[] { typeof(ICache) };
            registration.LifetimeManager = new ContainerControlledLifetimeManager();
            registration.Name = RegistrationName;

            this.container
                .ConfigureAutoRegistration()
                .Include(If.Is<TestCache>, registration)
                .ApplyAutoRegistration();

            Assert.AreEqual(1, this.registered.Count);
            var registerEvent = this.registered.Single();
            Assert.AreEqual(typeof(TestCache), registerEvent.To);
            Assert.AreEqual(typeof(ContainerControlledLifetimeManager), registerEvent.Lifetime.GetType());
            Assert.AreEqual(RegistrationName, registerEvent.Name);
        }

        //[TestMethod]
        //public void WhenHaveMoreThanOneRegistrationRules_TypesRegisteredAsExpected()
        //{
        //    _container
        //        .ConfigureAutoRegistration()
        //        .Include(If.Implements<ICustomerRepository>,
        //                 Then.Register()
        //                     .AsSingleInterfaceOfType()
        //                     .WithTypeName()
        //                     .UsingPerThreadMode())
        //        .Include(If.DecoratedWith<LoggerAttribute>, Then.Register().AsAllInterfacesOfType())
        //        .ApplyAutoRegistration();

        //    // 2 types implement ICustomerRepository, LoggerAttribute decorated type implement 2 interfaces
        //    Assert.AreEqual(4, _registered.Count);
        //}

        [TestMethod]
        public void WhenImplementsITypeNameMethodCalled_ItWorksAsExpected()
        {
            Assert.IsTrue(typeof(CustomerRepository).ImplementsITypeName());
            Assert.IsTrue(typeof(Introduction).ImplementsITypeName());
        }

        [TestMethod]
        public void WhenImplementsOpenGenericTypes_RegisteredAsExpected()
        {
            this.container
                .ConfigureAutoRegistration()
                .Include(x => x.ImplementsOpenGeneric(typeof(IHandlerFor<>)),
                         Then.Register().AsFirstInterfaceOfType().WithTypeName())
                .ApplyAutoRegistration();

            Assert.AreEqual(2, this.registered.Count);
            Assert.IsTrue(this.registered
                .Select(r => r.From)
                .All(t => t == typeof(IHandlerFor<DomainEvent>)));

            Assert.AreEqual(2, this.realContainer.ResolveAll(typeof(IHandlerFor<DomainEvent>)).Count());
        }

        [TestMethod]
        public void WhenWithPartNameMehtodCalled_ItWorksAsExpected()
        {
            Assert.AreEqual(
                "Customer",
                new RegistrationOptions { Type = typeof(CustomerRepository) }
                    .WithPartName(WellKnownAppParts.Repository)
                    .Name);

            Assert.AreEqual(
                "Test",
                new RegistrationOptions { Type = typeof(TestCache) }
                    .WithPartName("Cache")
                    .Name);
        }

        private class RegisterEvent
        {
            public Type From { get; private set; }
            public Type To { get; private set; }
            public string Name { get; private set; }
            public LifetimeManager Lifetime { get; private set; }

            public RegisterEvent(Type from, Type to, string name, LifetimeManager lifetime)
            {
                From = from;
                To = to;
                Name = name;
                Lifetime = lifetime;
            }
        }

        public class Introduction : IIntroduction
        {
        }

        public interface IIntroduction
        {
        }

        private void Example()
        {
            var container = new UnityContainer();

            container
                .ConfigureAutoRegistration()
                .LoadAssemblyFrom("MyFancyPlugin.dll")
                .ExcludeSystemAssemblies()
                .ExcludeAssemblies(a => a.GetName().FullName.Contains("Test"))
                .Include(If.ImplementsSingleInterface, Then.Register().AsSingleInterfaceOfType().UsingSingletonMode())
                .Include(If.Implements<ILogger>, Then.Register().UsingPerCallMode())
                .Include(If.ImplementsITypeName, Then.Register().WithTypeName())
                .Include(If.Implements<ICustomerRepository>, Then.Register().WithName("Sample"))
                .Include(If.Implements<IOrderRepository>,
                         Then.Register().AsSingleInterfaceOfType().UsingPerCallMode())
                .Include(If.DecoratedWith<LoggerAttribute>,
                         Then.Register()
                             .As<IDisposable>()
                             .WithPartName(WellKnownAppParts.Logger)
                             .UsingLifetime<MyLifetimeManager>())
                .Exclude(t => t.Name.Contains("Trace"))
                .ApplyAutoRegistration();
        }
    }
}
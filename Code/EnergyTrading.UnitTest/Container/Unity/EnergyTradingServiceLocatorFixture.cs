using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyTrading.UnitTest.Container.Unity
{
    /// <summary>
    /// Base class for tests for an adapter for the <see cref="IServiceLocator"/>
    /// interface that are independent of the actual container. Subclass this
    /// to provide your actual container implementation to test.
    /// </summary>
    public abstract class EnergyTradingServiceLocatorFixture
    {
        protected IServiceLocator Locator { get; set; }

        protected abstract IServiceLocator CreateServiceLocator();

        public void GetInstance()
        {
            var instance = Locator.GetInstance<ITest>();
            Assert.IsNotNull(instance);
        }

        public void AskingForInvalidComponentShouldRaiseActivationException()
        {
            AssertThrows<ActivationException>(() => Locator.GetInstance<IDictionary>());
        }

        public void GetNamedInstance()
        {
            var instance = Locator.GetInstance<ITest>(typeof(TestClass).FullName);
            Assert.IsInstanceOfType(instance, typeof(TestClass));
        }

        public void GetNamedInstance2()
        {
            var instance = Locator.GetInstance<ITest>(typeof(AnotherTestClass).FullName);
            Assert.IsInstanceOfType(instance, typeof(AnotherTestClass));
        }

        public void GetUnknownInstance2()
        {
            AssertThrows<ActivationException>(() => Locator.GetInstance<ITest>("test"));
        }

        public void GetAllInstances()
        {
            IEnumerable<ITest> instances = Locator.GetAllInstances<ITest>();
            IList<ITest> list = new List<ITest>(instances);
            Assert.AreEqual(2, list.Count);
        }

        public void GetAllInstance_ForUnknownType_ReturnEmptyEnumerable()
        {
            IEnumerable<IDictionary> instances = Locator.GetAllInstances<IDictionary>();
            IList<IDictionary> list = new List<IDictionary>(instances);
            Assert.AreEqual(0, list.Count);
        }

        public void GenericOverload_GetInstance()
        {
            Assert.AreEqual(
                Locator.GetInstance<ITest>().GetType(),
                Locator.GetInstance(typeof(ITest), null).GetType());
        }

        public void GenericOverload_GetInstance_WithName()
        {
            Assert.AreEqual(
                Locator.GetInstance<ITest>(typeof(TestClass).FullName).GetType(),
                Locator.GetInstance(typeof(ITest), typeof(TestClass).FullName).GetType());
        }

        public void Overload_GetInstance_NoName_And_NullName()
        {
            Assert.AreEqual(
                Locator.GetInstance<ITest>().GetType(),
                Locator.GetInstance<ITest>(null).GetType());
        }

        public void GenericOverload_GetAllInstances()
        {
            var genericLoggers = new List<ITest>(Locator.GetAllInstances<ITest>());
            var plainLoggers = new List<object>(Locator.GetAllInstances(typeof(ITest)));
            Assert.AreEqual(genericLoggers.Count, plainLoggers.Count);
            for (var i = 0; i < genericLoggers.Count; i++)
            {
                Assert.AreEqual(
                    genericLoggers[i].GetType(),
                    plainLoggers[i].GetType());
            }
        }

        private static void AssertThrows<TException>(Action action)
            where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected exception {0}, but instead exception {1} was thrown",
                    typeof (TException).Name,
                    ex.GetType().Name);
            }
            Assert.Fail("Expected exception {0}, no exception thrown", typeof (TException).Name);
        }
    }
}         
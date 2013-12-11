namespace EnergyTrading.UnitTest.Extensions
{
    using System;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Extensions;

    [TestClass]
    public class ExceptionExtensionsFixture
    {
        [TestMethod]
        public void AddThrownNullListFuncReturns()
        {
            var count = 0;
            ExceptionExtensions.AddThrown(null, () => count++);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExceptionsAreThrownIfListIsNull()
        {
            ExceptionExtensions.AddThrown(null, () => { throw new ArgumentOutOfRangeException(); });
        }

        [TestMethod]
        public void ThrownExceptionsAreAddedToListByDefault()
        {
            var ex = new ArgumentOutOfRangeException();
            var list = new List<Exception>();
            list.AddThrown(() => { throw ex; });
            Assert.AreEqual(1, list.Count);
            Assert.AreSame(ex, list[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandledExceptionTypesCanBeRestricted()
        {
            var handledTypes = new List<Type> { typeof(ArgumentOutOfRangeException) };
            var ex = new ArgumentOutOfRangeException();
            var list = new List<Exception>();
            list.AddThrown(() => { throw ex; }, handledTypes);
            Assert.AreEqual(1, list.Count);
            Assert.AreSame(ex, list[0]);
            list.AddThrown(() => { throw new ArgumentNullException(); }, handledTypes);
        }
    }
}
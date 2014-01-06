namespace EnergyTrading.UnitTest.Extensions
{
    using System;

    using EnergyTrading.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MemoizationExtensionsFixture
    {
        private int doubleCounter;
        private int tripleCounter;

        [TestInitialize]
        public void Setup()
        {
            this.doubleCounter = 0;
            this.tripleCounter = 0;
        }

        [TestMethod]
        public void MemoizeSingleArgFunction()
        {
            Func<int, int> d = this.Double;
            Func<int, int> dm = d.Memoize();

            var x = dm(1);
            var y = dm(1);
            Assert.AreEqual(2, x, "First compute double incorrect");
            Assert.AreEqual(2, y, "Second compute double incorrect");
            Assert.AreEqual(1, this.doubleCounter, "Double counter incorrect");
        }

        [TestMethod]
        public void MemoizeDifferentFunctions()
        {
            Func<int, int> d = this.Double;
            Func<int, int> dm = d.Memoize();

            Func<int, int> t = this.Triple;
            Func<int, int> tm = t.Memoize();

            var x = dm(1);
            var y = dm(1);
            Assert.AreEqual(2, x, "First compute double incorrect");
            Assert.AreEqual(2, y, "Second compute double incorrect");
            Assert.AreEqual(1, this.doubleCounter, "Double counter incorrect");

            var p = tm(1);
            var q = tm(1);
            Assert.AreEqual(3, p, "First compute triple incorrect");
            Assert.AreEqual(3, q, "Second compute triple incorrect");
            Assert.AreEqual(1, this.tripleCounter, "Triple counter incorrect");
        }

        [TestMethod]
        public void MemoizeSameFunction()
        {
            Func<int, int> d = this.Double;
            Func<int, int> dm = d.Memoize();

            Func<int, int> t = this.Double;
            Func<int, int> tm = t.Memoize();

            var x = dm(1);
            var y = dm(1);
            Assert.AreEqual(2, x, "First compute double incorrect");
            Assert.AreEqual(2, y, "Second compute double incorrect");

            var p = tm(1);
            var q = tm(1);
            Assert.AreEqual(2, p, "Third compute double incorrect");
            Assert.AreEqual(2, q, "Fourth compute double incorrect");
            Assert.AreEqual(2, this.doubleCounter, "Double counter incorrect");
        }

        private int Double(int value)
        {
            this.doubleCounter += 1;
            return value * 2;
        }

        private int Triple(int value)
        {
            this.tripleCounter += 1;
            return value * 3;
        }
    }
}
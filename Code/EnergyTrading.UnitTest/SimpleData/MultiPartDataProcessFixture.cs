namespace RWEST.Nexus.UnitTest.SimpleData
{
    using System;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using global::Nexus.Data.SimpleData;

    [TestClass]
    public class MultiPartDataProcessFixture
    {
        private static readonly string TestData = "here is my test data, hopefully it will all get stored";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithoutProcessor()
        {
// ReSharper disable ObjectCreationAsStatement
            new MultiPartDataProcess(null);
// ReSharper restore ObjectCreationAsStatement
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructWithZeroPartSize()
        {
// ReSharper disable ObjectCreationAsStatement
            new MultiPartDataProcess((x, y, z) => true, null, null, 0);
// ReSharper restore ObjectCreationAsStatement
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructWithNegativePartSize()
        {
            // ReSharper disable ObjectCreationAsStatement
            new MultiPartDataProcess((x, y, z) => true, null, null, -1);
            // ReSharper restore ObjectCreationAsStatement
        }

        [TestMethod]
        public void TestSingleChunkIsProcessed()
        {
            var sb = new StringBuilder();
            var processor = new MultiPartDataProcess((x, y, z) =>
                {
                    Assert.AreEqual(1, y);
                    Assert.AreEqual(1, z);
                    sb.Append(x);
                    return true;
                });
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsTrue(candidate);
        }

        [TestMethod]
        public void MultiplePartsAreProcessed()
        {
            var sb = new StringBuilder();
            var count = 1;
            var processor = new MultiPartDataProcess((x, y, z) =>
            {
                Assert.AreEqual(count++, y);
                sb.Append(x);
                return true;
            }, null, null, 3);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsTrue(candidate);
        }

        [TestMethod]
        public void PartsAreNotProcessedIfStartProcessFails()
        {
            var processor = new MultiPartDataProcess((x, y, z) =>
                                    {
                                        Assert.Fail();
                                        return false;
                                    },
                                    x => false);
            var candidate = processor.ProcessData(TestData);
            Assert.IsFalse(candidate);
        }

        [TestMethod]
        public void PartsAreProcessedIfStartProcessSucceeds()
        {
            var sb = new StringBuilder();
            var processor = new MultiPartDataProcess((x, y, z) =>
                                    {
                                        sb.Append(x);
                                        return true;
                                    },
                                    x => true);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsTrue(candidate);
        }

        [TestMethod]
        public void StopsWithFalseIfPartFailsToProcess()
        {
            var sb = new StringBuilder();
            var count = 1;
            var processor = new MultiPartDataProcess((x, y, z) =>
            {
                Assert.AreEqual(count++, y);
                sb.Append(x);
                return y != 3;
            }, null, null, 3);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData.Substring(0, 3*3), sb.ToString());
            Assert.IsFalse(candidate);
        }

        [TestMethod]
        public void ReturnsFalseIfEndProcessFails()
        {
            var sb = new StringBuilder();
            var processor = new MultiPartDataProcess((x, y, z) =>
                                    {
                                        sb.Append(x);
                                        return true;
                                    },
                                    x => true,
                                    x => false);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsFalse(candidate);
        }

        [TestMethod]
        public void ReturnsTrueIfEndProcessSucceeds()
        {
            var sb = new StringBuilder();
            var processor = new MultiPartDataProcess((x, y, z) =>
            {
                sb.Append(x);
                return true;
            },
                                    x => true,
                                    x => true);
            var candidate = processor.ProcessData(TestData);
            Assert.AreEqual(TestData, sb.ToString());
            Assert.IsTrue(candidate);
        }
    }
}
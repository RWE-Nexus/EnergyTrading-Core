namespace EnergyTrading.UnitTest.FileProcessing
{
    using EnergyTrading.FileProcessing;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DefaultFileFilterFixture
    {
        [TestMethod]
        public void TestDefaultFilterAlwaysReturnsTrue()
        {
            this.TestDefaultFilter(null);
            this.TestDefaultFilter(string.Empty);
            this.TestDefaultFilter("anything");
        }

        private void TestDefaultFilter(string path)
        {
            var candidate = new DefaultFileFilter().IncludeFile(path);
            Assert.IsTrue(candidate);
        }
    }
}
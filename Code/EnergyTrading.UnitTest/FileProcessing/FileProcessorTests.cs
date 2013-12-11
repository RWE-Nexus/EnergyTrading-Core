namespace EnergyTrading.UnitTest.FileProcessing
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.FileProcessing;

    [TestClass]
    public class FileProcessorTests
    {
        [TestMethod]
        public void TestSuccessFileNameRetainsDirectoryStructure()
        {
            var helper = new FileProcessorHelper(new FileProcessorEndpoint { DropPath = @"c:\a", FailurePath = @"c:\b", ScavengeInterval= new TimeSpan(0, 0, 2), SuccessPath = @"c:\d" }, new FileHandler(), new NullPostProcessor());
            var result = helper.GenerateSuccessFileName(@"c:\a\e\f\g.txt");
            Assert.IsTrue(result.StartsWith(@"c:\d\e\f\g.txt"));
        }

        [TestMethod]
        public void TestErrorFileNameRetainsDirectoryStructure()
        {
            var helper = new FileProcessorHelper(new FileProcessorEndpoint { DropPath = @"c:\a", FailurePath = @"c:\b", ScavengeInterval = new TimeSpan(0, 0, 2), SuccessPath = @"c:\d" }, new FileHandler(), new NullPostProcessor());
            var result = helper.GenerateErrorFileName(@"c:\a\e\f\g.txt");
            Assert.IsTrue(result.StartsWith(@"c:\b\e\f\g.txt"));
        }

        [TestMethod]
        public void TestFilePathVariableInSuccessPath()
        {
            var helper = new FileProcessorHelper(new FileProcessorEndpoint { DropPath = @"c:\a", FailurePath = @"c:\b", ScavengeInterval = new TimeSpan(0, 0, 2), SuccessPath = @"%filepath%\d" }, new FileHandler(), new NullPostProcessor());
            var result = helper.GenerateSuccessFileName(@"c:\a\e\f\g.txt");
            Assert.IsTrue(result.StartsWith(@"c:\a\e\f\d\g.txt"));
        }

        [TestMethod]
        public void TestFilePathVariableInFailurePath()
        {
            var helper = new FileProcessorHelper(new FileProcessorEndpoint { DropPath = @"c:\a", FailurePath = @"%filepath%\b", ScavengeInterval = new TimeSpan(0, 0, 2), SuccessPath = @"C:\d" }, new FileHandler(), new NullPostProcessor());
            var result = helper.GenerateErrorFileName(@"c:\a\e\f\g.txt");
            Assert.IsTrue(result.StartsWith(@"c:\a\e\f\b\g.txt"));
        }
    }
}

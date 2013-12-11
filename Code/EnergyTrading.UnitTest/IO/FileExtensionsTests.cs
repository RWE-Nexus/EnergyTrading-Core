namespace EnergyTrading.UnitTest.IO
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    using EnergyTrading.IO;

    /// <summary>
    /// Summary description for FileExtensionsTests
    /// </summary>
    [DeploymentItem("TestFiles\\EssentEndurTestFile.xml", "TestFiles")]
    [TestClass]
    public class FileExtensionsTests
    {
        private const string TestFilePath = ".\\TestFiles\\EssentEndurTestFile.xml";
        private FileInfo fileInfo;
        private FileStream fileStream;

        [TestInitialize]
        public void SetUp()
        {
            fileInfo = new FileInfo(TestFilePath);
        }

        /// <summary>
        /// This is a successful test case with a test file. This test might file if the test file is locked by any other process.
        /// </summary>
        [TestMethod]
        public void ReturnReaderOnSuccessfulRetryFileOpenTextIfLocked()
        {
            fileInfo.RetryFileActionIfLocked(fi =>
            {
                var reader = fileInfo.OpenText();
                Assert.IsNotNull(reader);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void RetryFileActionThrowIOExceptionWhenFileLocked()
        {
            fileStream = fileInfo.OpenWrite();
            var fileInfoAnother = new FileInfo(TestFilePath);
            fileInfoAnother.RetryFileActionIfLocked(fi => fileInfoAnother.OpenText());
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void RetryFileActionThrowFileNotFoundException()
        {
            var fileName2 = ".\\test.xml";
            var fileInfo2 = new FileInfo(fileName2);

            fileInfo2.RetryFileActionIfLocked(fi =>{ var reader = fileInfo2.OpenText(); });
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void RetryFileActionThrowIOException()
        {
            var fileName2 = ".\\test.xml";         
            var fileInfo2 = new FileInfo(fileName2);

            fileInfo2.RetryFileActionIfLocked(fi =>{ throw new IOException(); });
        }      

        [TestCleanup]
        public void TearDown()
        {
            fileInfo = null;
            if (fileStream != null)
            {
                fileStream.Close();
            }
            fileStream = null;
        }
    }
}

namespace EnergyTrading.UnitTest.IO
{
    using System.IO;

    using EnergyTrading.IO;

    using NUnit.Framework;

    /// <summary>
    /// Summary description for FileExtensionsTests
    /// </summary>
    [TestFixture]
    public class FileExtensionsTests
    {
        private const string TestFilePath = ".\\TestFiles\\EssentEndurTestFile.xml";
        private FileInfo fileInfo;
        private FileStream fileStream;

        [SetUp]
        public void SetUp()
        {
            fileInfo = new FileInfo(TestFilePath);
        }

        /// <summary>
        /// This is a successful test case with a test file. This test might file if the test file is locked by any other process.
        /// </summary>
        [Test]
        public void ReturnReaderOnSuccessfulRetryFileOpenTextIfLocked()
        {
            fileInfo.RetryFileActionIfLocked(fi =>
            {
                var reader = fileInfo.OpenText();
                Assert.IsNotNull(reader);
            });
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void RetryFileActionThrowIOExceptionWhenFileLocked()
        {
            this.fileStream = this.fileInfo.OpenWrite();
            var fileInfoAnother = new FileInfo(TestFilePath);
            fileInfoAnother.RetryFileActionIfLocked(fi => fileInfoAnother.OpenText());
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void RetryFileActionThrowFileNotFoundException()
        {
            var fileName2 = ".\\test.xml";
            var fileInfo2 = new FileInfo(fileName2);

            fileInfo2.RetryFileActionIfLocked(fi =>{ var reader = fileInfo2.OpenText(); });
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void RetryFileActionThrowIOException()
        {
            var fileName2 = ".\\test.xml";         
            var fileInfo2 = new FileInfo(fileName2);

            fileInfo2.RetryFileActionIfLocked(fi =>{ throw new IOException(); });
        }      

        [TearDown]
        public void TearDown()
        {
            this.fileInfo = null;
            if (this.fileStream != null)
            {
                this.fileStream.Close();
            }
            this.fileStream = null;
        }
    }
}

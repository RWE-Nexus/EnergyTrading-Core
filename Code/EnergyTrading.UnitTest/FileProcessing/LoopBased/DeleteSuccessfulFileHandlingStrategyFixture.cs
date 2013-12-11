namespace EnergyTrading.UnitTest.FileProcessing.LoopBased
{
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using EnergyTrading.FileProcessing.FileHandling;

    [TestClass]
    public class DeleteSuccessfulFileHandlingStrategyFixture
    {
        [ClassInitialize]
        public static void CreateTestDirectory(TestContext context)
        {
            RemoveDirectory();
            testDirectory = Directory.CreateDirectory(TestDirectoryName).FullName;
        }

        [TestCleanup]
        public void EmptyTestDirectory()
        {
            foreach (var filePath in Directory.GetFiles(testDirectory))
            {
                new FileInfo(filePath).Delete();
            }
        }

        [ClassCleanup]
        public static void RemoveDirectory()
        {
            if (Directory.Exists(testDirectory))
            {
                Directory.Delete(testDirectory, true);
            }
        }

        private static string testDirectory;
        private const string TestDirectoryName = "Config";

        [TestMethod]
        public void HandleFileDoesNotExistDoNothing()
        {
            var handler = new DeleteSuccessfulFileHandlingStrategy();
            var processingFile = new ProcessingFile(Path.Combine(testDirectory, "somenonexistant.file.inprogress"), "somenonexistant.file", "nonexistantpath\\nonexistant.file");

            handler.Handle(processingFile);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void HandleFileExistsDeleteFile()
        {
            var file = new FileInfo(Path.Combine(testDirectory, "test.file"));
            var processingFile = new ProcessingFile(file.FullName, "original.name", "originalpath\\original.name");
            using (var sr = file.Create())
            {
            }
            var handler = new DeleteSuccessfulFileHandlingStrategy();

            handler.Handle(processingFile);

            Assert.IsFalse(file.Exists);
        }
    }
}
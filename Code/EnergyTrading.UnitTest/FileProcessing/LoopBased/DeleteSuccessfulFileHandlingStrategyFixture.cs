namespace EnergyTrading.UnitTest.FileProcessing.LoopBased
{
    using System.IO;

    using EnergyTrading.FileProcessing.FileHandling;

    using NUnit.Framework;

    [TestFixture]
    public class DeleteSuccessfulFileHandlingStrategyFixture
    {
        [TestFixtureSetUp]
        public static void CreateTestDirectory()
        {
            RemoveDirectory();
            testDirectory = Directory.CreateDirectory(TestDirectoryName).FullName;
        }

        [TearDown]
        public void EmptyTestDirectory()
        {
            foreach (var filePath in Directory.GetFiles(testDirectory))
            {
                new FileInfo(filePath).Delete();
            }
        }

        [TestFixtureTearDown]
        public static void RemoveDirectory()
        {
            if (Directory.Exists(testDirectory))
            {
                Directory.Delete(testDirectory, true);
            }
        }

        private static string testDirectory;
        private const string TestDirectoryName = "Config";

        [Test]
        public void HandleFileDoesNotExistDoNothing()
        {
            var handler = new DeleteSuccessfulFileHandlingStrategy();
            var processingFile = new ProcessingFile(Path.Combine(testDirectory, "somenonexistant.file.inprogress"), "somenonexistant.file", "nonexistantpath\\nonexistant.file");

            handler.Handle(processingFile);

            Assert.IsTrue(true);
        }

        [Test]
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
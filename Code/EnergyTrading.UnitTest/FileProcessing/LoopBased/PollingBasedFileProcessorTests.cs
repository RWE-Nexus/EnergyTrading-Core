namespace EnergyTrading.UnitTest.FileProcessing.LoopBased
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using EnergyTrading.FileProcessing;
    using EnergyTrading.FileProcessing.FileHandling;
    using EnergyTrading.FileProcessing.FileProcessors;

    using global::Rhino.Mocks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PollingBasedFileProcessorTests
    {
        private const string DropDirectoryName = "drop";
        private const string AcknowledgedDirectoryName = "acknowledged";
        private const string TempDirectoryName = "temp";

        private FileProcessorEndpoint endpoint; 
        private string dropLocation;
        private string acknowledgedLocation;
        private string tempLocation;

        [TestInitialize]
        public void CreateDirectories()
        {
            this.DeleteDirectories();

            this.dropLocation = Directory.CreateDirectory(DropDirectoryName).FullName;
            this.acknowledgedLocation = Directory.CreateDirectory(AcknowledgedDirectoryName).FullName;
            this.tempLocation = Directory.CreateDirectory(TempDirectoryName).FullName;

            this.endpoint = new FileProcessorEndpoint
                           {
                               DropPath = this.dropLocation,
                               InProgressPath = this.acknowledgedLocation,
                           };
        }

        [TestCleanup]
        public void DeleteDirectories()
        {
            RemoveDirectory(DropDirectoryName);
            RemoveDirectory(AcknowledgedDirectoryName);
            RemoveDirectory(TempDirectoryName);
        }

        [TestMethod]
        public void StartExistingFileInDropLocationProcessesFile()
        {
            var fileData = this.CreateTestFileIn(this.dropLocation);
            this.TestFilePickup(fileData.Filename, fileData.Content);
        }

        private void TestFilePickup(string originalFilename, string originalContent)
        {
            var resetEvent = new AutoResetEvent(false);
            var mockHandler = MockRepository.GenerateMock<IHandleFiles>();

            using (var listener = new PollingBasedFileProcessor(this.endpoint, mockHandler, new DefaultFileFilter()))
            {
                listener.Start();
                resetEvent.WaitOne(1000);
            }

            mockHandler.AssertWasCalled(x => x.Notify(Arg<ProcessingFile>.Is.NotNull));
            this.AssertDirectoryIsEmpty(this.dropLocation);

            var filePaths = Directory.GetFiles(this.acknowledgedLocation);
            Assert.AreEqual(filePaths.Length, 1);

            var actualFile = new FileInfo(filePaths[0]);
            AssertFileMatches(originalContent, originalFilename, actualFile);
        }

        [TestMethod]
        public void StartDropNewFileProcessFile()
        {
            var fileData = this.CreateTestFileIn(this.tempLocation);

            var resetEvent = new AutoResetEvent(false);
            var mockHandler = MockRepository.GenerateMock<IHandleFiles>();

            using (var listener = new PollingBasedFileProcessor(this.endpoint, mockHandler, new DefaultFileFilter()))
            {
                listener.Start();

                new FileInfo(Path.Combine(this.tempLocation, fileData.Filename)).MoveTo(Path.Combine(this.dropLocation, fileData.Filename));

                resetEvent.WaitOne(1000);
            }

            mockHandler.AssertWasCalled(x => x.Notify(Arg<ProcessingFile>.Is.NotNull));
            this.AssertDirectoryIsEmpty(this.dropLocation);

            var filePaths = Directory.GetFiles(this.acknowledgedLocation);
            Assert.AreEqual(filePaths.Length, 1);

            var actualFile = new FileInfo(filePaths[0]);
            AssertFileMatches(fileData.Content, fileData.Filename, actualFile);
        }

        [TestMethod]
        public void FilterPreventsProcessing()
        {
            var fileData = this.CreateTestFileIn(this.tempLocation);

            var resetEvent = new AutoResetEvent(false);
            var mockHandler = MockRepository.GenerateMock<IHandleFiles>();

            using (var listener = new PollingBasedFileProcessor(this.endpoint, mockHandler, new FalseFileFilter()))
            {
                listener.Start();

                new FileInfo(Path.Combine(this.tempLocation, fileData.Filename)).MoveTo(Path.Combine(this.dropLocation, fileData.Filename));

                resetEvent.WaitOne(1000);
            }

            mockHandler.AssertWasNotCalled(x => x.Notify(Arg<ProcessingFile>.Is.NotNull));

            var filePaths = Directory.GetFiles(this.dropLocation);
            Assert.AreEqual(filePaths.Length, 1);

            var actualFile = new FileInfo(filePaths[0]);
            AssertFileMatches(fileData.Content, fileData.Filename, actualFile);
        }

        private static void RemoveDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }
            foreach (var filePath in Directory.GetFiles(directory))
            {
                new FileInfo(filePath).Delete();
            }
            Directory.Delete(directory, true);
        }

        private FileMetaData CreateTestFileIn(string directory)
        {
            const string Filename = "test.file";
            var content = string.Format("{0} {1}", DateTime.UtcNow.ToLongDateString(), DateTime.UtcNow.ToLongTimeString());
            var file = new FileInfo(Path.Combine(directory, Filename));
            using (var s = file.CreateText())
            {
                s.Write(content);
            }

            return new FileMetaData { Filename = Filename, Content = content };
        }

        private void AssertDirectoryIsEmpty(string directory)
        {
            Assert.IsFalse(Directory.EnumerateFiles(directory).Any());
        }

        private static void AssertFileMatches(string originalContent, string originalFilename, FileInfo actualFile)
        {
            Assert.IsTrue(actualFile.Name.Contains(originalFilename));
            using (var sr = actualFile.OpenText())
            {
                Assert.AreEqual(sr.ReadToEnd(), originalContent);
            }
        }

        public class FalseFileFilter : IFileFilter
        {
            public bool IncludeFile(string fullFilePath)
            {
                return false;
            }
        }

        public class FileMetaData
        {
            public string Filename { get; set; }
            public string Content { get; set; }
        }
    }
}

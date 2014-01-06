namespace EnergyTrading.UnitTest.FileProcessing
{
    using EnergyTrading.FileProcessing;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class FileProcessorHostFixture
    {
        [TestMethod]
        public void StopInvokesProcessorsStop()
        {
            var processor = new Mock<IFileProcessor>();
            var host = new FileProcessorHost(new [] { processor.Object });

            host.Stop();

            processor.Verify(x => x.Stop());
        }

        [TestMethod]
        public void StartInvokesProcessorsStopAndStart()
        {
            var processor = new Mock<IFileProcessor>();
            var host = new FileProcessorHost(new [] { processor.Object });

            host.Start();

            processor.Verify(x => x.Stop());
            processor.Verify(x => x.Start());
        }
    }
}
namespace EnergyTrading.UnitTest.FileProcessing
{
    using EnergyTrading.FileProcessing;

    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class FileProcessorHostFixture
    {
        [Test]
        public void StopInvokesProcessorsStop()
        {
            var processor = new Mock<IFileProcessor>();
            var host = new FileProcessorHost(new [] { processor.Object });

            host.Stop();

            processor.Verify(x => x.Stop());
        }

        [Test]
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
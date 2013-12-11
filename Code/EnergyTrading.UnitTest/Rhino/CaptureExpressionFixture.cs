namespace EnergyTrading.UnitTest.Rhino
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using global::Rhino.Mocks;

    using EnergyTrading.Test.Rhino;

    [TestClass]
    public class CaptureExpressionFixture
    {
        [TestMethod]
        public void CaptureOneArgSingleCall()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .Args<int>((p, value) => p.Process(value));

            var wrapper = new Wrapper(processor);

            // Act
            wrapper.Process(1);

            // Assert
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(1, args[0]);
        }

        [TestMethod]
        public void CaptureTwoArgsSingleCall()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .Args<int, string>((p, v1, v2) => p.Process(v1, v2));

            var wrapper = new Wrapper(processor);

            // Act
            wrapper.Process(1, "a");

            // Assert
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(1, args[0].Item1);
            Assert.AreEqual("a", args[0].Item2);
        }

        [TestMethod]
        public void CaptureThreeArgsSingleCall()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .Args<int, string, double>((p, v1, v2, v3) => p.Process(v1, v2, v3));

            var wrapper = new Wrapper(processor);

            // Act
            wrapper.Process(1, "a", 1.0);

            // Assert
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(1, args[0].Item1);
            Assert.AreEqual("a", args[0].Item2);
            Assert.AreEqual(1.0, args[0].Item3);
        }

        [TestMethod]
        public void CaptureFourArgsSingleCall()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .Args<int, string, double, int>((p, v1, v2, v3, v4) => p.Process(v1, v2, v3, v4));

            var wrapper = new Wrapper(processor);

            // Act
            wrapper.Process(1, "a", 1.0, 2);

            // Assert
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(1, args[0].Item1);
            Assert.AreEqual("a", args[0].Item2);
            Assert.AreEqual(1.0, args[0].Item3);
            Assert.AreEqual(2, args[0].Item4);
        }

        [TestMethod]
        public void CaptureOneArgMultipleCalls()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .Args<int>((p, value) => p.Process(value));

            var wrapper = new Wrapper(processor);

            // Act
            wrapper.Process(1);
            wrapper.Process(2);

            // Assert
            Assert.AreEqual(2, args.Count);
            Assert.AreEqual(2, args[1]);
        }

        [TestMethod]
        public void CaptureMultipleArgsMultipleCalls()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .Args<int, string>((p, v1, v2) => p.Process(v1, v2));

            var wrapper = new Wrapper(processor);

            // Act
            wrapper.Process(1, "a");
            wrapper.Process(2, "b");

            // Assert
            Assert.AreEqual(2, args.Count);
            Assert.AreEqual(2, args[1].Item1);
            Assert.AreEqual("b", args[1].Item2);
        }

        [TestMethod]
        public void CaptureOneArgFuncSingleCall()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .FuncArgs<int, int>((p, value) => p.ProcessAndReturn(value), 3);

            var wrapper = new Wrapper(processor);

            // Act
            var candidate = wrapper.ProcessAndReturn(1);

            // Assert
            Assert.AreEqual(3, candidate, "Return values differ");
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(1, args[0]);
        }

        [TestMethod]
        public void CaptureTwoArgsFuncSingleCall()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .FuncArgs<int, string, int>((p, v1, v2) => p.ProcessAndReturn(v1, v2), 3);

            var wrapper = new Wrapper(processor);

            // Act
            var candidate = wrapper.ProcessAndReturn(1, "a");

            // Assert
            Assert.AreEqual(3, candidate, "Return values differ");
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(1, args[0].Item1);
            Assert.AreEqual("a", args[0].Item2);
        }

        [TestMethod]
        public void CaptureThreeArgsFuncSingleCall()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .FuncArgs<int, string, double, int>((p, v1, v2, v3) => p.ProcessAndReturn(v1, v2, v3), 3);

            var wrapper = new Wrapper(processor);

            // Act
            var candidate = wrapper.ProcessAndReturn(1, "a", 1.0);

            // Assert
            Assert.AreEqual(3, candidate, "Return values differ");
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(1, args[0].Item1);
            Assert.AreEqual("a", args[0].Item2);
            Assert.AreEqual(1.0, args[0].Item3);
        }

        [TestMethod]
        public void CaptureFourArgsFuncSingleCall()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .FuncArgs<int, string, double, int, int>((p, v1, v2, v3, v4) => p.ProcessAndReturn(v1, v2, v3, v4), 3);

            var wrapper = new Wrapper(processor);

            // Act
            var candidate = wrapper.ProcessAndReturn(1, "a", 1.0, 2);

            // Assert
            Assert.AreEqual(3, candidate, "Return values differ");
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(1, args[0].Item1);
            Assert.AreEqual("a", args[0].Item2);
            Assert.AreEqual(1.0, args[0].Item3);
            Assert.AreEqual(2, args[0].Item4);
        }

        [TestMethod]
        public void CaptureFiveArgsFuncSingleCall()
        {
            // Arrange
            var processor = MockRepository.GenerateStub<IProcessor>();
            var args = processor
                .Capture()
                .FuncArgs<int, string, double, int, bool, int>((p, v1, v2, v3, v4, v5) => p.ProcessAndReturn(v1, v2, v3, v4, v5), 3);

            var wrapper = new Wrapper(processor);

            // Act
            var candidate = wrapper.ProcessAndReturn(1, "a", 1.0, 2, true);

            // Assert
            Assert.AreEqual(3, candidate, "Return values differ");
            Assert.AreEqual(1, args.Count);
            Assert.AreEqual(1, args[0].Item1);
            Assert.AreEqual("a", args[0].Item2);
            Assert.AreEqual(1.0, args[0].Item3);
            Assert.AreEqual(2, args[0].Item4);
            Assert.IsTrue(args[0].Item5);
        }
        public class Wrapper
        {
            private readonly IProcessor processor;

            public Wrapper(IProcessor processor)
            {
                this.processor = processor;
            }

            public void Process(int i)
            {
                processor.Process(i);
            }

            public void Process(int i, string j)
            {
                processor.Process(i, j);
            }

            public void Process(int i, string j, double k)
            {
                processor.Process(i, j, k);
            }

            public void Process(int i, string j, double k, int l)
            {
                processor.Process(i, j, k, l);
            }

            public int ProcessAndReturn(int i)
            {
                return processor.ProcessAndReturn(i);
            }

            public int ProcessAndReturn(int i, string j)
            {
                return processor.ProcessAndReturn(i, j);
            }

            public int ProcessAndReturn(int i, string j, double k)
            {
                return processor.ProcessAndReturn(i, j, k);
            }

            public int ProcessAndReturn(int i, string j, double k, int l)
            {
                return processor.ProcessAndReturn(i, j, k, l);
            }

            public int ProcessAndReturn(int i, string j, double k, int l, bool m)
            {
                return processor.ProcessAndReturn(i, j, k, l, m);
            }
        }

        public interface IProcessor
        {
            void Process(int i);

            void Process(int i, string j);

            void Process(int i, string j, double k);

            void Process(int i, string j, double k, int l);

            int ProcessAndReturn(int i);

            int ProcessAndReturn(int i, string j);

            int ProcessAndReturn(int i, string j, double k);

            int ProcessAndReturn(int i, string j, double k, int l);

            int ProcessAndReturn(int i, string j, double k, int l, bool m);
        }
    }
}
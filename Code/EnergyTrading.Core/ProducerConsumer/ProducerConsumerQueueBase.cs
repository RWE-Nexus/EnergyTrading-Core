namespace EnergyTrading.ProducerConsumer
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EnergyTrading.Logging;

    public abstract class ProducerConsumerQueueBase : Disposable
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger<ProducerConsumerQueueBase>();

        private readonly BlockingCollection<WorkItem> queue;
        private readonly Task[] consumerTasks;
        private bool stopping;

        protected ProducerConsumerQueueBase(int numberOfConsumers)
        {
            queue = new BlockingCollection<WorkItem>(numberOfConsumers);

            consumerTasks =
                Enumerable.Range(1, numberOfConsumers)
                    .Select(_ => Task.Factory.StartNew(Consume))
                    .ToArray();
        }

        private void Consume()
        {
            foreach (var workItem in queue.GetConsumingEnumerable())
            {
                try
                {
                    if (stopping)
                    {
                        workItem.Cancel();
                    }
                    else
                    {
                        workItem.DoWork();
                    }
                }
                catch
                {
                    Logger.Error("WorkItem failed, exiting consuming enumerable");
                    throw;
                }
            }
        }

        protected Task EnqueueWork(Action workToPerform)
        {
            var taskCompletionSource = new TaskCompletionSource<object>();

            var workItem = new WorkItem(workToPerform, taskCompletionSource);
            queue.Add(workItem);

            return taskCompletionSource.Task;
        }


        protected override void DisposeManagedResources()
        {
            stopping = true;
            queue.CompleteAdding();
            SpinWait.SpinUntil(() => queue.IsCompleted);

            Task.WaitAll(consumerTasks);

            queue.Dispose();
        }
    }
}

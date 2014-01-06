namespace EnergyTrading.ProducerConsumer
{
    using System;
    using System.Threading.Tasks;

    public class WorkItem
    {
        private readonly TaskCompletionSource<object> taskSource;
        private readonly Action workToPerform;

        public WorkItem(Action workToPerform, TaskCompletionSource<object> taskSource)
        {
            if (taskSource == null)
            {
                throw new ArgumentNullException("taskSource");
            }

            this.workToPerform = workToPerform;
            this.taskSource = taskSource;
        }

        public void DoWork()
        {
            try
            {
                this.workToPerform();
                this.taskSource.SetResult(null);
            }
            catch (Exception ex)
            {
                this.taskSource.SetException(ex);
            }
        }

        public void Cancel()
        {
            this.taskSource.SetCanceled();
        }
    }
}
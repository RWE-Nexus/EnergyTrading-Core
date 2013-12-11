namespace EnergyTrading.ServiceModel
{
    using System;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;

    using EnergyTrading.EventArguments;

    public class WcfServiceClient<TClient> : ClientBase<TClient>, IDisposable 
        where TClient : class
    {
        public event EventHandler<GenericAsyncCompletedEventArgs> AsyncCompleted;

        internal WcfServiceClient()
        {            
        }

        internal WcfServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {            
        }

        internal WcfServiceClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {            
        }

        internal WcfServiceClient(InstanceContext callbackInstance) : base(callbackInstance)
        {            
        }

        public TClient Instance
        {
            get { return Channel; }
        }

        public void AsyncBegin(string methodName, object userState, params object[] inValues)
        {
            Console.WriteLine("AsyncBegin thread: {0}", Thread.CurrentThread.ManagedThreadId);
            if (string.IsNullOrEmpty(methodName))
            {
                throw new NullReferenceException("methodName cannot be null");
            }

            var mi = this.Instance.GetType().GetMethod(methodName);
            if (null == mi)
            {
                throw new TargetException(string.Format("methodName {0} not found on instance", methodName));
            }

            var func = new Func<MethodInfo, object[], object>(this.ExecuteAsyncMethod);
            func.BeginInvoke(
                mi,
                inValues,
                this.FuncCallback,
                new GenericAsyncState { UserState = userState, MethodName = methodName, InValues = inValues });
        }

        public void Dispose()
        {
            AbortClose();
        }

        public void AbortClose()
        {
            // Avoid the CommunicationObjectFaultedException 
            if (this.State != CommunicationState.Closed)
            {
                this.Abort();
            }

            // safe to close the client
            this.Close();
        }

        private object ExecuteAsyncMethod(MethodInfo mi, object[] inValues)
        {
            return mi.Invoke(this.Instance, inValues);
        }

        private void FuncCallback(IAsyncResult result)
        {
            Console.WriteLine("FuncCallback thread: {0}", Thread.CurrentThread.ManagedThreadId);
            var deleg = (Func<MethodInfo, object[], object>)((AsyncResult)result).AsyncDelegate;
            var state = result.AsyncState as GenericAsyncState;
            if (null != deleg)
            {
                Exception error = null;
                object retval = null;
                try
                {
                    retval = deleg.EndInvoke(result);
                }
                catch (Exception e)
                {
                    error = e;
                }
                var userState = state == null ? null : state.UserState;
                var methodName = state == null ? null : state.MethodName;
                var inValues = state == null ? null : state.InValues;

                var args = new GenericAsyncCompletedEventArgs(retval, error, methodName, userState, inValues);
                if (this.AsyncCompleted != null)
                {
                    this.AsyncCompleted(this, args);
                }
            }
        }

        private class GenericAsyncState
        {
            public object UserState { get; set; }
            public string MethodName { get; set; }
            public object[] InValues { get; set; }
        }
    }
}
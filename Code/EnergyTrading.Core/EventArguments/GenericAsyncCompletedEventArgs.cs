namespace EnergyTrading.EventArguments
{
    using System;

    public class GenericAsyncCompletedEventArgs : EventArgs
    {
        public GenericAsyncCompletedEventArgs(object result, Exception error, string methodName, object userState, object[] inValues)
        {
            this.Result = result;
            this.Error = error;
            this.MethodName = methodName;
            this.UserState = userState;
            this.InValues = inValues;
        }

        public object Result { get; private set; }
        public Exception Error { get; private set; }
        public string MethodName { get; private set; }
        public object UserState { get; private set; }
        public object[] InValues { get; private set; }
    }
}

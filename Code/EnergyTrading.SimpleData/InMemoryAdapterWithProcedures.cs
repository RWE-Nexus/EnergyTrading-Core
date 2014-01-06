namespace EnergyTrading.Data.SimpleData
{
    using System;
    using System.Collections.Generic;

    using Simple.Data;

    public class InMemoryAdapterWithProcedures : InMemoryAdapter, IAdapterWithFunctions
    {
        private readonly Dictionary<string, Delegate> procedures = new Dictionary<string, Delegate>();

        public void AddProcedure<TResult>(string procedureName, Func<IDictionary<string, object>, TResult> function)
        {
            this.procedures.Add(procedureName, function);
        }

        public new bool IsValidFunction(string functionName)
        {
            return this.procedures.ContainsKey(functionName) || base.IsValidFunction(functionName);
        }

        public new IEnumerable<IEnumerable<IEnumerable<KeyValuePair<string, object>>>> Execute(string functionName, IDictionary<string, object> parameters)
        {
            if (this.procedures.ContainsKey(functionName))
            {
                var obj = this.procedures[functionName].DynamicInvoke(parameters);

                var dict = obj as IDictionary<string, object>;
                if (dict != null)
                {
                    return new List<IEnumerable<IDictionary<string, object>>> { new List<IDictionary<string, object>> { dict } };
                }

                var list = obj as IEnumerable<IDictionary<string, object>>;
                if (list != null)
                {
                    return new List<IEnumerable<IDictionary<string, object>>> { list };
                }

                return obj as IEnumerable<IEnumerable<IDictionary<string, object>>>;
            }
            return base.Execute(functionName, parameters);
        }
    }
}
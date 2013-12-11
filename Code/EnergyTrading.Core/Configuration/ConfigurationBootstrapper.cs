namespace EnergyTrading.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    //using log4net;

    public static class ConfigurationBootStrapper
    {
        //private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Process a list of configuration tasks in order..
        /// <param name="tasks">Tasks to be processed, need not be in order, but must form a DAG</param>
        /// </summary>
        public static void Initialize(IEnumerable<IConfigurationTask> tasks)
        {
            // Form the graph
            var graph = tasks.ToDictionary(task => task, task => new List<Type>(task.DependsOn));

            // Now process each one that doesn't have any dependency
            var candidate = FindZeroDependencyTask(graph);
            while (candidate != null)
            {
                //logger.Debug("Processing: " + candidate.GetType().FullName);
                candidate.Configure();
                Remove(graph, candidate);

                // Get the next one
                candidate = FindZeroDependencyTask(graph);
            }

            // If we still have some left, there must be a cycle in the graph e.g. A -> B, B -> A
            if (graph.Keys.Count > 0)
            {
                //logger.Error("Cyclic dependency: " + graph.Keys.Count + " remaining");
                throw new ArgumentException("Cyclic dependency found in configuration tasks");
            }
        }

        private static IConfigurationTask FindZeroDependencyTask(IDictionary<IConfigurationTask, List<Type>> graph)
        {
            //logger.Debug("Searching: " + graph.Keys.Count + " remaining");

            foreach (var key in graph.Keys)
            {
                var depends = graph[key];
                if (depends.Count == 0)
                {
                    // Return the first task with no unresolved dependencies
                    //logger.Info("Found: " + key.GetType().FullName);
                    return key;
                }
            }

            return null;
        }

        private static void Remove(IDictionary<IConfigurationTask, List<Type>> graph, IConfigurationTask config)
        {
            var type = config.GetType();
            //logger.Debug("Removing: " + type.FullName);

            foreach (var values in graph.Values)
            {
                // Remove this type from the depenendencies, no impact if it's not there
                values.Remove(type);
            }

            // We're finished with this config class
            graph.Remove(config);
        }
    }
}

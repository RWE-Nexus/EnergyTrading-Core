namespace EnergyTrading.Extensions
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Simple extensions to standardize the caching of function results.
    /// <para>
    /// This functionality is built in to functional languages but we have to build
    /// it ourselves for other .NET languages.
    /// </para>
    /// <para>
    /// Functions used by this extension must be strict, i.e. no side effects, this
    /// is because for each argument value the function will only be called once, and if
    /// there were side-effects, different behaviour will result from calling the function
    /// natively or via Memoize.
    /// </para>
    /// <para>
    /// Uses a ConcurrentDictionary internally so is thread-safe, but
    /// doesn't lock so does allow for the same value to be calculated simultaneously.
    /// However, this is much cheaper (~20x) and has more concurrency than other techniques.   
    /// </para>
    /// </summary>
    public static class MemoizationExtensions
    {
        /// <summary>
        /// Memoize a function.
        /// </summary>
        /// <typeparam name="T1">Argument type of the function.</typeparam>
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute, needs to be a strict function i.e. no side-effects.</param>
        /// <returns>Result of the function</returns>
        public static Func<T1, R> Memoize<T1, R>(this Func<T1, R> f)
        {
            var cache = new ConcurrentDictionary<T1, R>();
            return a => cache.GetOrAdd(a, f);
        }

        /// <summary>
        /// Memoize a function.
        /// </summary>
        /// <typeparam name="T1">First argument type of the function.</typeparam>
        /// <typeparam name="T2">Second argument type of the function.</typeparam>         
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute, needs to be a strict function i.e. no side-effects.</param>
        /// <returns>Result of the function</returns>
        public static Func<T1, T2, R> Memoize<T1, T2, R>(this Func<T1, T2, R> f)
        {
            return f.Tuplify().Memoize().Detuplify();
        }

        /// <summary>
        /// Memoize a function.
        /// </summary>
        /// <typeparam name="T1">First argument type of the function.</typeparam>
        /// <typeparam name="T2">Second argument type of the function.</typeparam> 
        /// <typeparam name="T3">Third argument type of the function.</typeparam>        
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute, needs to be a strict function i.e. no side-effects.</param>
        /// <returns>Result of the function</returns>
        public static Func<T1, T2, T3, R> Memoize<T1, T2, T3, R>(this Func<T1, T2, T3, R> f)
        {
            return f.Tuplify().Memoize().Detuplify();
        }

        /// <summary>
        /// Memoize a function.
        /// </summary>
        /// <typeparam name="T1">First argument type of the function.</typeparam>
        /// <typeparam name="T2">Second argument type of the function.</typeparam> 
        /// <typeparam name="T3">Third argument type of the function.</typeparam>
        /// <typeparam name="T4">Fourth argument type of the function.</typeparam>        
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute, needs to be a strict function i.e. no side-effects.</param>
        /// <returns>Result of the function</returns>
        public static Func<T1, T2, T3, T4, R> Memoize<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> f)
        {
            return f.Tuplify().Memoize().Detuplify();
        }

        /// <summary>
        /// Apply a function to a tuple.
        /// </summary>
        /// <typeparam name="T1">First argument type of the function.</typeparam>
        /// <typeparam name="T2">Second argument type of the function.</typeparam>      
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute.</param>
        /// <returns>Result of the function</returns>
        public static Func<Tuple<T1, T2>, R> Tuplify<T1, T2, R>(this Func<T1, T2, R> f)
        {
            return t => f(t.Item1, t.Item2);
        }

        /// <summary>
        /// Create a tuple and apply a function.
        /// </summary>
        /// <typeparam name="T1">First argument type of the function.</typeparam>
        /// <typeparam name="T2">Second argument type of the function.</typeparam>      
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute.</param>
        /// <returns>Result of the function</returns>
        public static Func<T1, T2, R> Detuplify<T1, T2, R>(this Func<Tuple<T1, T2>, R> f)
        {
            return (a, b) => f(Tuple.Create(a, b));
        }

        /// <summary>
        /// Apply a function to a tuple.
        /// </summary>
        /// <typeparam name="T1">First argument type of the function.</typeparam>
        /// <typeparam name="T2">Second argument type of the function.</typeparam>
        /// <typeparam name="T3">Third argument type of the function.</typeparam>            
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute.</param>
        /// <returns>Result of the function</returns>
        public static Func<Tuple<T1, T2, T3>, R> Tuplify<T1, T2, T3, R>(this Func<T1, T2, T3, R> f)
        {
            return t => f(t.Item1, t.Item2, t.Item3);
        }

        /// <summary>
        /// Create a tuple and apply a function.
        /// </summary>
        /// <typeparam name="T1">First argument type of the function.</typeparam>
        /// <typeparam name="T2">Second argument type of the function.</typeparam>
        /// <typeparam name="T3">Third argument type of the function.</typeparam>    
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute.</param>
        /// <returns>Result of the function</returns>
        public static Func<T1, T2, T3, R> Detuplify<T1, T2, T3, R>(this Func<Tuple<T1, T2, T3>, R> f)
        {
            return (a, b, c) => f(Tuple.Create(a, b, c));
        }

        /// <summary>
        /// Apply a function to a tuple.
        /// </summary>
        /// <typeparam name="T1">First argument type of the function.</typeparam>
        /// <typeparam name="T2">Second argument type of the function.</typeparam>
        /// <typeparam name="T3">Third argument type of the function.</typeparam>
        /// <typeparam name="T4">Fourth argument type of the function.</typeparam>                    
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute.</param>
        /// <returns>Result of the function</returns>
        public static Func<Tuple<T1, T2, T3, T4>, R> Tuplify<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> f)
        {
            return t => f(t.Item1, t.Item2, t.Item3, t.Item4);
        }

        /// <summary>
        /// Create a tuple and apply a function.
        /// </summary>
        /// <typeparam name="T1">First argument type of the function.</typeparam>
        /// <typeparam name="T2">Second argument type of the function.</typeparam>
        /// <typeparam name="T3">Third argument type of the function.</typeparam>
        /// <typeparam name="T4">Fourth argument type of the function.</typeparam>  
        /// <typeparam name="R">Result type of the function.</typeparam>
        /// <param name="f">Function to execute.</param>
        /// <returns>Result of the function</returns>
        public static Func<T1, T2, T3, T4, R> Detuplify<T1, T2, T3, T4, R>(this Func<Tuple<T1, T2, T3, T4>, R> f)
        {
            return (a, b, c, d) => f(Tuple.Create(a, b, c, d));
        }
    }
}
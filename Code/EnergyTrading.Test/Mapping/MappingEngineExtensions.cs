namespace EnergyTrading.Test.Mapping
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    /// <summary>
    /// Helper methods for testing mapping engines.
    /// </summary>
    public static class MappingEngineExtensions
    {
        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <typeparam name="TMapper">Type of mapper we expect</typeparam>        
        /// <param name="engine">Engine to use.</param>        
        public static void ResolveMapper<TSource, TDestination, TMapper>(this IMappingEngine engine)
        {
            var simpleEngine = engine as SimpleMappingEngine;

            simpleEngine.ResolveMapper<TSource, TDestination>(typeof(TMapper));
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <param name="engine">Engine to use.</param>        
        /// <param name="implementation">Type of mapper we expect</param>
        public static void ResolveMapper<TSource, TDestination>(this IMappingEngine engine, Type implementation)
        {
            var simpleEngine = engine as SimpleMappingEngine;

            simpleEngine.ResolveMapper<TSource, TDestination>(implementation);
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <param name="engine">Engine to use.</param>        
        /// <param name="implementation">Type of mapper we expect</param>
        public static void ResolveMapper<TSource, TDestination>(this SimpleMappingEngine engine, Type implementation)
        {
            if (engine == null)
            {
                throw new NotSupportedException("Must supply engine");
            }
            var mapper = engine.Mapper<TSource, TDestination>();
            Assert.AreSame(mapper.GetType(), implementation, string.Format("{0} vs {1}", implementation.FullName, mapper.GetType().FullName));
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <typeparam name="TMapper">Type of mapper we expect</typeparam>
        /// <param name="engine">Engine to use.</param>  
        public static void ResolveMapper<TSource, TDestination, TMapper>(this IXmlMappingEngine engine)
        {
            var xmlEngine = engine as XmlMappingEngine;

            xmlEngine.ResolveMapper<TSource, TDestination>(typeof(TMapper));
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <param name="engine">Engine to use.</param>  
        /// <param name="implementation">Type of mapper we expect</param>
        public static void ResolveMapper<TSource, TDestination>(this IXmlMappingEngine engine, Type implementation)
        {
            var xmlEngine = engine as XmlMappingEngine;

            xmlEngine.ResolveMapper<TSource, TDestination>(implementation);
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <param name="engine">Engine to use.</param>  
        /// <param name="implementation">Type of mapper we expect</param>
        public static void ResolveMapper<TSource, TDestination>(this XmlMappingEngine engine, Type implementation)
        {
            if (engine == null)
            {
                throw new NotSupportedException("Must supply engine");
            }
            var mapper = engine.Mapper<TSource, TDestination>();            
            Assert.AreSame(mapper.GetType(), implementation, string.Format("{0} vs {1}", implementation.FullName, mapper.GetType().FullName));
        }
    }
}
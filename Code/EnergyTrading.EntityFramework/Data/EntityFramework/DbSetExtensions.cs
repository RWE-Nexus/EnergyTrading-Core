namespace EnergyTrading.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;

    using EnergyTrading.Logging;

    public static class DbSetExtensions
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Removes entities from a DbSet according to a predicate
        /// </summary>
        /// <remarks>
        /// One use of this is to clear out deleted members of child collections.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="predicate"></param>
        public static void RemoveLocals<T>(this IDbSet<T> dbSet, Func<T, bool> predicate)
            where T : class
        {
            new List<T>(dbSet.Local.Where(predicate).ToArray()).ForEach(entity => dbSet.Remove(entity));
        }

        /// <summary>
        /// Get entity by primary Key.
        /// If EF raises any materialized exceptions, then it catches and return null. Otherwise this behave exactly same as EF DbSet Find method.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="dbSet"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T FindEx<T>(this IDbSet<T> dbSet, object id) where T : class
        {
            try
            {
                return dbSet.Find(id);
            }
            catch (InvalidOperationException ex)
            {
                // EF throws this exception, only in case of finding a polymorphic entity
                // e.g.: If BusinessUnit and Broker (derived from PartyRole) are sharing the same table PartyRole, 
                // and if any client requests for a Broker with BusinessUnit Id
                // Then EF eagerly loads BusinessUnit proxy entity and try to cast it to Broker before it returns
                if (ex.Message.ToLower().Contains("the specified cast from a materialized"))
                {
                    Logger.Warn(string.Format("{0} identified by '{1}' not found", typeof(T).Name, id), ex);
                    return null; //EF return null if it didn't find any entity by id.(Need to be consistent)
                }

                throw;
            }
        }
    }
}
namespace EnergyTrading.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DistinctCollectionValuesRule<TEntity, TValueType> : Rule<IEnumerable<TEntity>>
    {
        private Func<TEntity, TValueType> Accessor { get; set; }

        private IEqualityComparer<TValueType> Comparer { get; set; }

        private string MessagePrefix { get; set; }

        public DistinctCollectionValuesRule(Func<TEntity, TValueType> accessor, IEqualityComparer<TValueType> comparer = null, string messagePrefix = "")
        {
            if (accessor == null)
            {
                throw new ArgumentNullException("accessor");
            }
            Accessor = accessor;
            Comparer = comparer;
            MessagePrefix = messagePrefix;
        }

        public override bool IsValid(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                return true;
            }
            var entityList = entities is IList<TEntity> ? entities as IList<TEntity> : entities.ToList();
            var fullCount = entityList.Select(Accessor).Count();
            var distinctCount = entityList.Select(Accessor).Distinct(Comparer).Count();
            if (fullCount != distinctCount)
            {
                Message = (MessagePrefix + " The list of items contains duplicate values").Trim();
            }
            return fullCount == distinctCount;
        }
    }
}
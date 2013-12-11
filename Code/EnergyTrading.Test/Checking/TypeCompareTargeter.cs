namespace EnergyTrading.Test.Checking
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Determines a compare target to use for a type.
    /// </summary>
    public class TypeCompareTargeter : ITypeCompareTargeter
    {
        private readonly Dictionary<Type, CompareTarget> types;

        public TypeCompareTargeter()
        {
            types = new Dictionary<Type, CompareTarget>();

            // Put some basic ones in there
            this.Register(typeof(Guid), CompareTarget.Value);
            this.Register(typeof(string), CompareTarget.Value);
            this.Register(typeof(decimal), CompareTarget.Value);
            this.Register(typeof(DateTime), CompareTarget.Value);
            this.Register(typeof(DateTimeOffset), CompareTarget.Value);
            this.Register(typeof(TimeSpan), CompareTarget.Value);
            this.Register(typeof(TimeZone), CompareTarget.Value);
            this.Register(typeof(TimeZoneInfo), CompareTarget.Value);
            // Type is abstract, get RuntimeType at r/t which is internal so can't handle easily
            this.Register(typeof(Type), CompareTarget.Value);
        }

        public CompareTarget DetermineCompareTarget(Type type)
        {
            CompareTarget compareTarget;

            if (types.TryGetValue(type, out compareTarget))
            {
                return compareTarget;
            }

            if (type.IsEnum)
            {
                compareTarget = CompareTarget.Value;
            }
            else if (type.IsValueType)
            {
                // Trying to make sure we process structs as entities, but exclude nullables
                if (!type.IsPrimitive && !type.FullName.StartsWith("System.Nullable"))
                {
                    compareTarget = CompareTarget.Entity;
                }
            }
            else
            {
                // Unless it's enumerable we will assume its an entity since all the primitive types
                // and exceptions have been dealt with above.
                compareTarget = typeof(IEnumerable).IsAssignableFrom(type) ? CompareTarget.Collection : CompareTarget.Entity;
            }

            return compareTarget;
        }

        public void Register(Type type, CompareTarget target)
        {
            types.Add(type, target);
        }
    }
}
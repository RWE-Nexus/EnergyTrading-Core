namespace EnergyTrading.Concurrency
{
    using System.Collections.Generic;

    /// <summary>
    /// Vector clock where the ids are integer.
    /// </summary>
    public class VectorClock : VectorClock<int>
    {
        public VectorClock()
        {            
        }

        public VectorClock(IDictionary<int, int> value) : base(value)
        {            
        }
    }
}
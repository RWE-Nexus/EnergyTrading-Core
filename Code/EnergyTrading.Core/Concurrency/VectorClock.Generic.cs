namespace EnergyTrading.Concurrency
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Generic vector clock
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// Presumption on the caller that it knows which id it is responsible for.
    /// <para>
    /// Either T is a string (which will lead to large increases in vector clock size,
    /// or it is an integer and the client is performing the translation from its own unique identity, e.g MAC address,
    /// to an integer for the vector clock. Whatever type T is, it implies some mechanism for global allocation of
    /// identity of the client, either intrinsic such as MAC address, or extrinsic by calling a service (same problem one remove!).
    /// </para>
    /// </remarks>    
    public class VectorClock<T> : IEquatable<VectorClock<T>>, ICloneable
        where T : IEquatable<T>
    {
        private readonly IDictionary<T, int> vector;
        private readonly ReaderWriterLockSlim syncLock;

        public VectorClock() : this(null)
        {
        }

        public VectorClock(IDictionary<T, int> value)
        {
            // NOTE: ConcurrentDictionary won't buy us anything since we need to lock 
            // across more than one primitive dictionary operation.
            vector = value != null ? new Dictionary<T, int>(value)
                                   : new Dictionary<T, int>();
            syncLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Increase the value associated with the id.
        /// </summary>
        /// <param name="id">Id to check for</param>
        public void Increment(T id)
        {           
            syncLock.EnterWriteLock();
            try
            {
                int value;
                if (vector.TryGetValue(id, out value))
                {
                    vector[id] = value + 1;
                }
                else
                {
                    vector.Add(id, 1);
                }
            }
            finally
            {
                syncLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Take the maximum vector from this and other vector clock. 
        /// </summary>
        /// <param name="value">Other vector clock</param>
        /// <returns></returns>
        public VectorClock<T> Max(VectorClock<T> value)
        {
            var max = Clone();

            if (value == null)
            {
                return max;
            }

            value.syncLock.EnterReadLock();
            try
            {
                // Get a copy as we will change vector during iteration
                var keys = new List<T>(max.vector.Keys);

                // Start from us
                foreach (var key in keys)
                {
                    int otherValue;
                    if (!value.vector.TryGetValue(key, out otherValue))
                    {
                        continue;
                    }

                    if (otherValue > max.vector[key])
                    {
                        max.vector[key] = otherValue;
                    }
                }

                // Check the other one
                // Only elements not in us need to be considered.
                foreach (var key in value.vector.Keys)
                {
                    if (!max.vector.ContainsKey(key))
                    {
                        max.vector.Add(key, value.vector[key]);
                    }
                }
            }
            finally
            {
                value.syncLock.ExitReadLock();
            }

            return max;
        }

        /// <summary>
        /// Clone the vector clock.
        /// </summary>
        /// <returns></returns>
        public VectorClock<T> Clone()
        {
            var clone = new VectorClock<T>();

            // Don't allow changes whilst cloning.
            syncLock.EnterReadLock();
            try
            {
                foreach (var entry in vector)
                {
                    clone.vector.Add(entry);
                }
            }
            finally
            {
                syncLock.ExitReadLock();
            }

            return clone;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <copydocfrom cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            syncLock.EnterReadLock();
            try
            {
                // State wholly contained in the vector.
                return vector.GetHashCode();
            }
            finally
            {
                syncLock.ExitReadLock();
            }
        }

        /// <copydocfrom cref="object.Equals(object)" />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as VectorClock<T>;

            return Equals(other);
        }

        /// <copydocfrom cref="object.Equals(object)" />
        public bool Equals(VectorClock<T> other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Compare(other) == ConcurrencyComparison.Equal;
        }

        /// <summary>
        /// Is this Reflexive, AntiSymmetric, and Transitive? Compare two VectorClocks,
        /// the outcomes will be one of the following: 
        /// -- Clock 1 is BEFORE clock 2       
        /// if there exists an i such that c1(i) &lt;= c(2) and there does not exist a j such that c1(j) &gt; c2(j). 
        /// -- Clock 1 is CONCURRENT to clock 2 
        /// if there exists an i, j such that c1(i) &lt; c2(i) and c1(j) &gt; c2(j) 
        /// -- Clock 1 is AFTER clock 2 otherwise 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public ConcurrencyComparison Compare(VectorClock<T> other)
        {
            if (other == null)
            {
                return ConcurrencyComparison.Concurrent; 
            }

            // Assume all things are possible
            var isEqual = true;
            var isGreater = true;
            var isSmaller = true;

            syncLock.EnterReadLock();
            other.syncLock.EnterReadLock();
            try
            {
                // Start from us
                foreach (var key in vector.Keys)
                {
                    int otherValue;
                    if (other.vector.TryGetValue(key, out otherValue))
                    {
                        // Present, so get the value
                        var value = vector[key];
                        if (value < otherValue)
                        {
                            isEqual = false;
                            isGreater = false;
                        }
                        else if (value > otherValue)
                        {
                            isEqual = false;
                            isSmaller = false;
                        }
                    }
                    else
                    {
                        isEqual = false;
                        isSmaller = false;
                    }
                }

                // Check the other one
                // Only elements not in us need to be considered.
                foreach (var key in other.vector.Keys)
                {
                    if (this.vector.ContainsKey(key))
                    {
                        continue;
                    }
                    isEqual = false;
                    isGreater = false;
                }
            }
            finally
            {
                syncLock.ExitReadLock();
                other.syncLock.ExitReadLock();
            }

            // Decision time..
            if (isEqual)
            {
                return ConcurrencyComparison.Equal;                
            }

            if (isGreater && !isSmaller)
            {
                return ConcurrencyComparison.After;                
            }

            if (!isGreater && isSmaller)
            {
                return ConcurrencyComparison.Before;
            }

            return ConcurrencyComparison.Concurrent;           
        }

        /// <copydocfrom cref="object.ToString" />
        public override string ToString()
        {
            var sb = new StringBuilder();

            syncLock.EnterReadLock();
            try
            {
                foreach (var key in this.SortedKeys())
                {
                    sb.AppendFormat("{0}[{1}]", key, vector[key]);
                    sb.Append(Environment.NewLine);
                }
            }
            finally
            {
                syncLock.ExitReadLock();
            }

            return sb.ToString();
        }

        private IEnumerable<T> SortedKeys()
        {
            var keys = new List<T>(vector.Keys);
            keys.Sort();

            return keys;
        }
    }
}
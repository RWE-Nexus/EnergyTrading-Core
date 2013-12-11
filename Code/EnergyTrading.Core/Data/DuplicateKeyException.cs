namespace EnergyTrading.Data
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown on attempted creating an entity with a duplicate key.
    /// </summary>
    [Serializable]
    public class DuplicateKeyException : Exception
    {
        public DuplicateKeyException()
            : this("Data with a duplicate key found")
        {
        }

        public DuplicateKeyException(string message)
            : base(message)
        {
        }

        public DuplicateKeyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DuplicateKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

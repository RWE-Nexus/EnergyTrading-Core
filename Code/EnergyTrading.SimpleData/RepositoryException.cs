namespace EnergyTrading.Data.SimpleData
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RepositoryException : Exception
    {
        public RepositoryException(string message)
            : base(message)
        {
        }

        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
            // Add any type-specific logic for inner exceptions.
        }

        protected RepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }
    }
}
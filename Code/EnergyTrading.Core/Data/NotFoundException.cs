namespace EnergyTrading.Data
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown on not finding an expected object
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : this("Could not find the data")
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
namespace EnergyTrading.Data
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown on an error in mapping the data to an entity.
    /// </summary>
    [Serializable]
    public class MappingException : Exception
    {
        public MappingException()
            : this("ORM Mapping problem")
        {
        }

        public MappingException(string message)
            : base(message)
        {
        }

        public MappingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MappingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

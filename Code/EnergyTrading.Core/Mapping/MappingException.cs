namespace EnergyTrading.Mapping
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception raised for errors in the mapping framework.
    /// </summary>
    [Serializable]
    public class MappingException : Exception
    {       
        public MappingException()
            : this("Unknown mapping exception")
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
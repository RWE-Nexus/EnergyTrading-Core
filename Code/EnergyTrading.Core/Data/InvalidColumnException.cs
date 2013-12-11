namespace EnergyTrading.Data
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown on attempted access to an invalid column
    /// </summary>
    [Serializable]
    public class InvalidColumnException : Exception
    {
        public InvalidColumnException()
            : this("ORM Invalid Column problem")
        {
        }

        public InvalidColumnException(string message)
            : base(message)
        {
        }

        public InvalidColumnException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidColumnException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
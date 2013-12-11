namespace EnergyTrading.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Raised for primary/foreign key and check constraint violations
    /// </summary>
    [Serializable]
    public class ConstraintViolationException : Exception
    {
        public ConstraintViolationException()
        {
            // Add any type-specific logic, and supply the default message.
        }

        public ConstraintViolationException(string message)
            : base(message)
        {
            // Add any type-specific logic.
        }

        public ConstraintViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
            // Add any type-specific logic for inner exceptions.
        }

        protected ConstraintViolationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }
    }
}

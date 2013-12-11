namespace EnergyTrading.FileProcessing.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class FileDeleteException : Exception
    {
        public FileDeleteException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FileDeleteException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
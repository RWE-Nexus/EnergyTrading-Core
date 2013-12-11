namespace EnergyTrading.Mapping
{
    using System.Collections.Generic;
    using System.Threading;

    /// <copydocfrom cref="IXmlSchemaRegistry" />
    public class XmlSchemaRegistry : IXmlSchemaRegistry
    {
        private readonly List<string> schemas;
        private readonly ReaderWriterLockSlim syncLock;

        public XmlSchemaRegistry()
        {
            schemas = new List<string>();
            syncLock = new ReaderWriterLockSlim();
        }

        /// <copydocfrom cref="IXmlSchemaRegistry.RegisterSchema" />
        public void RegisterSchema(string schema)
        {
            // Much cheaper than building our own ConcurrentList<T>
            try
            {
                syncLock.EnterWriteLock();
                if (!schemas.Contains(schema))
                {
                    schemas.Add(schema);
                }
            }
            finally
            {
                syncLock.ExitWriteLock();
            }
        }

        /// <copydocfrom cref="IXmlSchemaRegistry.GetSchemas" />
        public IEnumerable<string> GetSchemas()
        {
            try
            {
                syncLock.EnterReadLock();
                return new List<string>(schemas);
            }
            finally
            {
                syncLock.ExitReadLock();
            }
        }

        /// <copydocfrom cref="IXmlSchemaRegistry.SchemaExists" />
        public bool SchemaExists(string schema)
        {
            try
            {
                syncLock.EnterReadLock();
                return schemas.Contains(schema);
            }
            finally 
            {
                syncLock.ExitReadLock();   
            }
        }
    }
}
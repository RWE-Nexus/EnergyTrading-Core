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
            this.schemas = new List<string>();
            this.syncLock = new ReaderWriterLockSlim();
        }

        /// <copydocfrom cref="IXmlSchemaRegistry.RegisterSchema" />
        public void RegisterSchema(string schema)
        {
            // Much cheaper than building our own ConcurrentList<T>
            try
            {
                this.syncLock.EnterWriteLock();
                if (!this.schemas.Contains(schema))
                {
                    this.schemas.Add(schema);
                }
            }
            finally
            {
                this.syncLock.ExitWriteLock();
            }
        }

        /// <copydocfrom cref="IXmlSchemaRegistry.GetSchemas" />
        public IEnumerable<string> GetSchemas()
        {
            try
            {
                this.syncLock.EnterReadLock();
                return new List<string>(this.schemas);
            }
            finally
            {
                this.syncLock.ExitReadLock();
            }
        }

        /// <copydocfrom cref="IXmlSchemaRegistry.SchemaExists" />
        public bool SchemaExists(string schema)
        {
            try
            {
                this.syncLock.EnterReadLock();
                return this.schemas.Contains(schema);
            }
            finally 
            {
                this.syncLock.ExitReadLock();   
            }
        }
    }
}
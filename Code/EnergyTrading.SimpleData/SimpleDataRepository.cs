namespace EnergyTrading.Data.SimpleData
{
    using System;
    using System.Data;

    using EnergyTrading.Logging;
    using EnergyTrading.Threading;

    using Simple.Data;
    using Simple.Data.Ado;

    /// <summary>
    /// Simple.Data repository base class that provides connection retry attempts 
    /// </summary>
    public abstract class SimpleDataRepository
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger<SimpleDataRepository>();

        protected SimpleDataRepository(Database database, string schema = "", int maxRetries = 3, SimpleDataMode mode = SimpleDataMode.Live)
        {
            this.Database = database;
            // no schema then we try and get the value from the provider (so that we don't have to supply the appSetting unless we want to override it)
            if (string.IsNullOrWhiteSpace(schema))
            {
                var adapter = database.GetAdapter() as AdoAdapter;
                if (adapter != null)
                {
                    this.Schema = adapter.SchemaProvider.GetDefaultSchema();
                }
            }
            else
            {
                this.Schema = schema;
            }
            this.MaxRetries = maxRetries;
            this.Mode = mode;
        }

        /// <summary>
        /// Gets the sequence name for this entity.
        /// </summary>
        protected virtual string SequenceName
        {
            get { throw new NotImplementedException(); }
        }

        private string Schema { get; set; }

        private Database Database { get; set; }

        private int MaxRetries { get; set; }

        protected SimpleDataMode Mode { get; set; }

        protected dynamic GetSchemaObject()
        {
            return string.IsNullOrWhiteSpace(this.Schema) ? this.Database : this.Database[this.Schema];
        }

        protected string GetDottedSchema()
        {
            return string.IsNullOrEmpty(this.Schema) ? this.Schema : (this.Schema + ".");
        }

        protected virtual IDbConnection OpenConnection()
        {
            var adapter = this.GetAdapter();
            if (adapter == null && this.Mode == SimpleDataMode.Live)
            {
                throw new RepositoryException("Simple.Data must be using an AdoAdapter");
            }

            if (this.Mode == SimpleDataMode.Test)
            {
                return null;
            }

            var connection = adapter.ConnectionProvider.CreateConnection();
            connection.Open();

            return connection;
        }

        protected virtual IDbConnection RetryOpenConnection()
        {
            return TaskExtensions.Retry(() => this.OpenConnection(), this.MaxRetries);
        }

        protected void PerformDatabaseAction(Action action)
        {
            this.PerformDatabaseAction(() =>
            {
                action();
                return 0;
            });
        }

        protected T PerformDatabaseAction<T>(Func<T> func)
        {
            var ret = default(T);
            Logger.TryDatabaseTask(() =>
            {
                using (var connection = this.RetryOpenConnection())
                {
                    if (connection != null)
                    {
                        this.GetAdapter().UseSharedConnection(connection);
                    }
                    try
                    {
                        ret = func();
                    }
                    finally
                    {
                        if (connection != null)
                        {
                            this.GetAdapter().StopUsingSharedConnection();
                            connection.Close();
                        }
                    }
                }
            });

            return ret;
        }

        private AdoAdapter GetAdapter()
        {
            return this.Database.GetAdapter() as AdoAdapter;
        }
    }
}
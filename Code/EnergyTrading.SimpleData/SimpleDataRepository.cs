namespace EnergyTrading.Data.SimpleData
{
    using System;
    using System.Configuration;
    using System.Data;

    using EnergyTrading.Configuration;
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

        private string schema;
        private Database database;
        private bool shouldCloseDatabase;

        protected SimpleDataRepository(Database database, string schema = "", int maxRetries = 3, SimpleDataMode mode = SimpleDataMode.Live)
        {
            this.database = database;
            this.Schema = schema;
            this.MaxRetries = maxRetries;
            this.Mode = mode;
        }

        protected SimpleDataRepository(IConfigurationManager configurationManager, string connectionName = SimpleDataDatabaseProvider.DefaultConnectionName, string schema = "", int maxRetries = 3, SimpleDataMode mode = SimpleDataMode.Live)
        {
            this.ConnectionStringSettings = configurationManager.GetConnectionSettingsWithPassword(connectionName);
            this.Schema = schema;
            this.MaxRetries = maxRetries;
            this.Mode = mode;
            this.shouldCloseDatabase = true;
        }

        /// <summary>
        /// Gets the sequence name for this entity.
        /// </summary>
        protected virtual string SequenceName
        {
            get { throw new NotImplementedException(); }
        }

        private ConnectionStringSettings ConnectionStringSettings { get; set; }

        private Database Database
        {
            get
            {
                return this.database
                       ?? (this.database =
                           Database.Opener.OpenConnection(
                               this.ConnectionStringSettings.ConnectionString,
                               this.ConnectionStringSettings.ProviderName));
            }
        }

        private string Schema
        {
            get
            {
                if (schema == null)
                {
                    // no schema passed in to constructor so we try and get the value from the provider (so that we don't have to supply the appSetting unless we want to override it)
                    var adapter = Database.GetAdapter() as AdoAdapter;
                    if (adapter != null)
                    {
                        schema = adapter.SchemaProvider.GetDefaultSchema();
                    }
                }
                return this.schema;
            }
            set
            {
                this.schema = value;
            }
        }

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
            if (adapter == null && Mode == SimpleDataMode.Live)
            {
                throw new RepositoryException("Simple.Data must be using an AdoAdapter");
            }

            if (Mode == SimpleDataMode.Test)
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
                try
                {
                    using (var connection = RetryOpenConnection())
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
                }
                finally
                {
                    if (shouldCloseDatabase)
                    {
                        this.CloseDatabase();
                    }
                }
            });

            return ret;
        }

        private void CloseDatabase()
        {
            this.database = null;
        }

        private AdoAdapter GetAdapter()
        {
            return this.Database.GetAdapter() as AdoAdapter;
        }
    }
}
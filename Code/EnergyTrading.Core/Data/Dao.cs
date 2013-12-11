namespace EnergyTrading.Data
{
    using System.Data;

    /// <copydocfrom cref="IDao" />
    public class Dao : IDao
    {
        private readonly IDbConnection connection;

        /// <summary>
        /// Creates a new instance of the <see cref="Dao" /> class.
        /// </summary>
        /// <param name="connection"></param>
        public Dao(IDbConnection connection)
        {
            this.connection = connection;
        }

        /// <copydocfrom cref="IDao.Connection" />
        public IDbConnection Connection
        {
            get { return this.connection; }
        }

        /// <copydocfrom cref="IDao.ExecuteNonQuery" />
        public int ExecuteNonQuery(string sql, int timeout = 30)
        {
            var conn = this.Connection;
            var initialState = conn.State;
            try
            {
                if (initialState != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandTimeout = timeout;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    return cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (initialState != ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}

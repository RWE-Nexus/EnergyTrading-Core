namespace EnergyTrading.Test.Data
{
    using System.Collections.Generic;
    using System.Text;

    using EnergyTrading.Data;

    /// <summary>
    /// Cleans down the database
    /// </summary>
    public class Zapper
    {
        private readonly IDao dao;

        /// <summary>
        /// Create a new instance of the <see cref="Zapper"/> class.
        /// </summary>
        /// <param name="dao"></param>
        public Zapper(IDao dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// Execute any update statements required to clear data, followed by delete statements for all tables.
        /// </summary>
        /// <param name="updateCommands">Update commands to execute</param>
        /// <param name="tables">Tables to delete from</param>
        /// <param name="timeout">Maximum time for a statement</param>
        /// <param name="deleteBatch">Batch delete statements into groups, saves times on databases with large numbers of tables.</param>
        public void Zap(IEnumerable<string> updateCommands, IEnumerable<string> tables, int timeout = 30, int deleteBatch = 1)
        {
            foreach (var item in updateCommands)
            {
                dao.ExecuteNonQuery(item, timeout);
            }

            var sb = new StringBuilder();
            var count = 0;
            foreach (var item in tables)
            {
                sb.AppendLine(string.Format("DELETE FROM {0};", item));
                count++;
                if (count % deleteBatch != 0)
                {
                    continue;
                }

                dao.ExecuteNonQuery(sb.ToString(), timeout);
                sb.Clear();
            }

            var lastCmd = sb.ToString();
            if (!string.IsNullOrEmpty(lastCmd))
            {
                dao.ExecuteNonQuery(lastCmd, timeout);
            }
        }
    }
}
namespace EnergyTrading.Test.Data
{
    using System.Collections.Generic;

    using EnergyTrading.Data;

    /// <summary>
    /// Cleans down the database
    /// </summary>
    public class Zapper
    {
        private readonly IDao dao;

        public Zapper(IDao dao)
        {
            this.dao = dao;
        }

        public void Zap(IEnumerable<string> updateCommands, IEnumerable<string> tables)
        {
            foreach (var item in updateCommands)
            {
                this.dao.ExecuteNonQuery(item, 30);
            }

            foreach (var item in tables)
            {
                this.dao.ExecuteNonQuery(string.Format("DELETE FROM {0}", item), 30);
            }           
        }
    }
}
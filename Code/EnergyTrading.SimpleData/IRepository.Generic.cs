namespace EnergyTrading.Data.SimpleData
{
    using System.Collections.Generic;

    public interface IRepository<T> : IRepository
    {
        T Add(T entity);

        T Update(T entity);

        void Delete(T entity);

        IEnumerable<T> FindAll();
    }
}
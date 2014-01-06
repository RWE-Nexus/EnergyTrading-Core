namespace EnergyTrading.Data.SimpleData
{
    using System;

    using EnergyTrading.Configuration;

    public interface IRepositoryFactory<out T> where T : IRepository
    {
        T CreateRepository(Func<dynamic> getTransactionFunc, IConfigurationManager configurationManager);
    }
}
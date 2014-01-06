namespace EnergyTrading.Data.EntityFramework
{
    using EnergyTrading.Data.Sql;
    using EnergyTrading.Exceptions;

    public class EntityFrameworkExceptionFactory : ExceptionTranslator
    {
        public EntityFrameworkExceptionFactory()
        {
            this.AddFactory(new SqlExceptionFactory());
        }
    }
}
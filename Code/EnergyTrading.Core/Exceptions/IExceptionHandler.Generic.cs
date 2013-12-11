namespace EnergyTrading.Exceptions
{
    using System;

    public interface IExceptionHandler<T>
        where T : Exception
    {
    }
}

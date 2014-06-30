namespace EnergyTrading.Test
{
    using System;

    /// <summary>
    /// Comparator used to verify that two instances of <see typeparamref="T" /> are 
    /// the same on a per property basis.
    /// </summary>
    /// <typeparam name="T">Type whose instances we will check</typeparam>
    [Obsolete("Inherit from NCheck.Checker{T} instead")]
    public class Checker<T> : NCheck.Checker<T>
    {
    }
}
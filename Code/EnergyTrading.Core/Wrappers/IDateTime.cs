namespace EnergyTrading.Wrappers
{
    using System;

    /// <summary>
    /// Injection interface to allow for mocking when performing tasks that use the current time
    /// </summary>
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
        DateTime Today { get; }
    }
}
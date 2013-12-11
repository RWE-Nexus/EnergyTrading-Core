namespace EnergyTrading.Wrappers
{
    using System;

    using EnergyTrading.Attributes;

    [ExcludeFromCoverage]
    public class DateTimeWrapper : IDateTime
    {
        public DateTime Now { get { return DateTime.Now; } }

        public DateTime UtcNow { get { return DateTime.UtcNow; } }

        public DateTime Today { get { return DateTime.Today; } }
    }
}
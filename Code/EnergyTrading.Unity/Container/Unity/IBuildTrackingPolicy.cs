namespace EnergyTrading.Container.Unity
{
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    public interface IBuildTrackingPolicy : IBuilderPolicy
    {
        Stack<object> BuildKeys { get; }
    }
}
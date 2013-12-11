namespace EnergyTrading.Container.Unity
{
    using System.Collections.Generic;

    public class BuildTrackingPolicy : IBuildTrackingPolicy
    {
        public BuildTrackingPolicy()
        {
            this.BuildKeys = new Stack<object>();
        }

        public Stack<object> BuildKeys { get; private set; }
    }
}

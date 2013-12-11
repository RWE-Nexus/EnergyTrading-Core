namespace EnergyTrading.Container.Unity
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Implements a <see cref="UnityContainerExtension" /> that clears the list of build plan stratgies
    /// held by the container.
    /// </summary>
    public class UnityClearBuildPlanStrategies : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.BuildPlanStrategies.Clear();
        }
    }
}

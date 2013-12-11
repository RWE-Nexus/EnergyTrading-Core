namespace EnergyTrading.Container.Unity
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    /// <summary>
    /// Allow us to create an entity in the context of the actual type being resolved.
    /// <para>
    /// Solution taken from http://unity.codeplex.com/discussions/203744
    /// </para>
    /// </summary>
    public class BuildTracking : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.AddNew<BuildTrackingStrategy>(UnityBuildStage.TypeMapping);
        }

        public static IBuildTrackingPolicy GetPolicy(IBuilderContext context)
        {
            return context.Policies.Get<IBuildTrackingPolicy>(context.BuildKey, true);
        }

        public static IBuildTrackingPolicy SetPolicy(IBuilderContext context)
        {
            IBuildTrackingPolicy policy = new BuildTrackingPolicy();
            context.Policies.SetDefault<IBuildTrackingPolicy>(policy);
            return policy;
        }
    }
}
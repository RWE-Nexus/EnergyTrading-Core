namespace EnergyTrading.Container.Unity
{
    using Microsoft.Practices.ObjectBuilder2;

    public class BuildTrackingStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            var policy = BuildTracking.GetPolicy(context) ?? BuildTracking.SetPolicy(context);

            policy.BuildKeys.Push(context.BuildKey);
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            var policy = BuildTracking.GetPolicy(context);
            if ((policy != null) && (policy.BuildKeys.Count > 0))
            {
                policy.BuildKeys.Pop();
            }
        }
    }
}
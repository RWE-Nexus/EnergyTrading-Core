namespace EnergyTrading.ServiceModel
{
    using System.Collections.Generic;
    using System.Security;
    using System.Security.Permissions;
    using System.ServiceModel;

    using Microsoft.Practices.ServiceLocation;

#if(DEBUG)
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, MaxItemsInObjectGraph = 131072, IncludeExceptionDetailInFaults = true)]
#else
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, MaxItemsInObjectGraph = 131072, IncludeExceptionDetailInFaults = false)]
#endif
    public abstract class WcfServiceBase
    {
        private static IWcfConfig Config
        {
            get { return ServiceLocator.Current.GetInstance<IWcfConfig>(); }
        }

        public void Authorize()
        {
            var serviceType = this.GetType();
            var configItem = Config.Get(serviceType);

            IList<string> groups = null;
            if (configItem != null)
            {
                groups = configItem.AuthorizedGroups;
            }

            if (groups == null)
            {
                throw new SecurityException("Group is null");
            }

            var pps = new PrincipalPermission[groups.Count];
            for (var i = 0; i < groups.Count; i++)
            {
                pps[i] = new PrincipalPermission(null, groups[i]);
            }

            var pp = pps[0];
            if (groups.Count> 0)
            {
                for (var i = 1; i < groups.Count; i++)
                {
                    pp = (PrincipalPermission)pp.Union(pps[i]);
                }
            }
            pp.Demand();
        }
    }
}

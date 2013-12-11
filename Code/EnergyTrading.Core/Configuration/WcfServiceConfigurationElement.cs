namespace EnergyTrading.Configuration
{
    using System.Configuration;

    /*
    <wcfServices consoleMode="On">
        <services>
            <add key="test1" 
                  serviceAddressPort="localhost:2981" 
                  endpointName="Test1EndPoint" 
                  authorizedGroups="WcfServiceClients" 
                  hostType="Test1Service.MyService, Test1Service"
                  contractType="Test1Common.IMyService, Test1Common" />
            <add key="test2" 
                  serviceAddressPort="localhost:2981" 
                  endpointName="Test2EndPoint" 
                  authorizedGroups="WcfServiceClients" 
                  hostType="Test2Service.MyService, Test2Service"
                  contractType="Test2Common.IMyService, Test2Common" />
        </services>
    </wcfServices>
    */
    public class WcfServiceConfigurationElement : NamedConfigElement
    {
        public WcfServiceConfigurationElement()
        {           
        }

        public WcfServiceConfigurationElement(string name, string serviceAddressPort, string endpointName,
            string authorizedGroups, string hostType, string contractType)
        {
            this.Name = name;
            this.ServiceAddressPort = serviceAddressPort;
            this.EndpointName = endpointName;
            this.AuthorizedGroups = authorizedGroups;
            this.HostTypeDeclaration = hostType;
            this.ContractTypeDeclaration = contractType;
        }

        [ConfigurationProperty("serviceAddressPort", DefaultValue = "_", IsKey = false, IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&()[]{}/;'\"|\\", MinLength = 1, MaxLength = 260)]
        public string ServiceAddressPort
        {
            get
            {
                return (string)this["serviceAddressPort"];
            }
            set
            {
                this["serviceAddressPort"] = value;
            }
        }

        [ConfigurationProperty("endpointName", DefaultValue = "_", IsKey = false, IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&()[]{}/;'\"|\\", MinLength = 1, MaxLength = 260)]
        public string EndpointName
        {
            get
            {
                return (string)this["endpointName"];
            }
            set
            {
                this["endpointName"] = value;
            }
        }

        //, string type
        [ConfigurationProperty("authorizedGroups", DefaultValue = "_", IsKey = false, IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&()[]{}/;'\"|\\", MinLength = 0, MaxLength = 520)]
        public string AuthorizedGroups
        {
            get
            {
                return (string)this["authorizedGroups"];
            }
            set
            {
                this["authorizedGroups"] = value;
            }
        }

        [ConfigurationProperty("hostType", DefaultValue = "_", IsKey = false, IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&()/|\\", MinLength = 1, MaxLength = 520)]
        public string HostTypeDeclaration
        {
            get
            {
                return (string)this["hostType"];
            }
            set
            {
                this["hostType"] = value;
            }
        }

        public string HostTypeFullname
        {
            get
            {
                var parts = HostTypeDeclaration.Split(',');
                return parts[0].Trim();
            }
        }

        public string HostTypeAssembly
        {
            get
            {
                var parts = HostTypeDeclaration.Split(',');
                return (parts.Length > 1)
                    ? parts[1].Trim()
                    : null;
            }
        }

        [ConfigurationProperty("contractType", DefaultValue = "_", IsKey = false, IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&()/|\\", MinLength = 1, MaxLength = 520)]
        public string ContractTypeDeclaration
        {
            get
            {
                return (string)this["contractType"];
            }
            set
            {
                this["contractType"] = value;
            }
        }

        public string ContractTypeFullname
        {
            get
            {
                var parts = ContractTypeDeclaration.Split(',');
                return parts[0].Trim();
            }
        }

        public string ContractTypeAssembly
        {
            get
            {
                var parts = ContractTypeDeclaration.Split(',');
                return (parts.Length > 1)
                    ? parts[1].Trim()
                    : null;
            }
        }
    }
}

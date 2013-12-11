namespace EnergyTrading.UnitTest.Configuration
{
    using EnergyTrading.Configuration;

    public class ChildCollection : NamedConfigElementCollection<ChildElement>
    {
        protected override string ElementName
        {
            get { return "child"; }
        }
    }
}
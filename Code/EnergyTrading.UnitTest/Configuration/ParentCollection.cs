namespace EnergyTrading.UnitTest.Configuration
{
    using EnergyTrading.Configuration;

    public class ParentCollection : NamedConfigElementCollection<ParentElement>
    {
        protected override string ElementName
        {
            get { return "parent"; }
        }
    }
}

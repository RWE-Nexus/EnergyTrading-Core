namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Collections.Generic;

    public class Order
    {
        private List<Id> ids;

        public List<Id> Ids
        {
            get { return this.ids ?? (this.ids = new List<Id>()); }
            set { this.ids = value; }
        }
    }
}
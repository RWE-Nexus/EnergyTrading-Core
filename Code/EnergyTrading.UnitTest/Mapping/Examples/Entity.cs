namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public class Entity : INullableProperties
    {
        private int total;

        public Entity()
        {
            NullProperties = new NullPropertyBag();

            // NB Needed if we need the default to be "null" i.e. not emitted
            NullProperties["Total"] = true;
        }

        public NullPropertyBag NullProperties { get; private set; }

        public string Id { get; set; }

        public string Id2 { get; set; }

        public string Name { get; set; }

        public string Name2 { get; set; }

        public int Value { get; set; }

        public int Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
                NullProperties.Assigned("Total");
            }
        }
    }
}
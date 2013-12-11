namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Collections.Generic;

    public class Parent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }

        public List<Child> Children { get; set; }
    }
}

namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System;

    public class Child
    {
        public int Id { get; set; }

        public float Value { get; set; }

        public DateTime Start { get; set; }

        public Dog Dog { get; set; }
    }
}
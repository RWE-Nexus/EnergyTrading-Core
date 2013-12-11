namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Collections.Generic;

    public class Owner
    {
        public Owner()
        {
            Pets = new List<Animal>();    
        }

        public Identifier Id { get; set; }

        public string Name { get; set; }

        public List<Animal> Pets { get; set; }
    }
}

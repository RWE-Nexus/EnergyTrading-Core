namespace EnergyTrading.UnitTest.Mapping
{
    using System.Collections.Generic;

    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListXmlMapperFixture : MappingFixture<Order>
    {
        protected override string CreateExpectedXml()
        {
            return @"<order xmlns='http://www.sample.com/sales'>
                         <ids xmlns='http://www.sample.com/app'>
                             <id system='iso3167'>A</id>
                         </ids>
                     </order>";
        }

        protected override Order CreateExpectedDto()
        {
            return new Order
            {
                Ids = new List<Id>
                {
                    new Id { System = "iso3167", Value = "A" }
                }
            };
        }

        protected override XmlMapper<Order> CreateMapper(IXmlMappingEngine engine)
        {
            return new OrderXmlMapper(engine);
        }

        protected override void RegisterChildMappers(IXmlMappingEngine engine)
        {
            engine.RegisterMapper(new IdXmlMapper());
        }
    }
}
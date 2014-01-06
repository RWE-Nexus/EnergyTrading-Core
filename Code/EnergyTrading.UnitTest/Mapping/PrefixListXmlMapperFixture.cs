namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PrefixListXmlMapperFixture : ListXmlMapperFixture
    {
        protected override string CreateExpectedXml()
        {
            return @"<sales:order xmlns:sales='http://www.sample.com/sales' xmlns:app='http://www.sample.com/app'>
                         <app:ids>
                             <app:id system='iso3167'>A</app:id>
                         </app:ids>
                     </sales:order>";
        }
    }
}
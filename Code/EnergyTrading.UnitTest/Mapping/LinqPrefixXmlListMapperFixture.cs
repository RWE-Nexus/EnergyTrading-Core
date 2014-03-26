namespace EnergyTrading.UnitTest.Mapping
{
    using NUnit.Framework;

    [TestFixture]
    public class LinqPrefixXmlListMapperFixture : LinqListXmlMapperFixture
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
namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using NUnit.Framework;

    [TestFixture]
    [Ignore("Need example XML")]
    public class CatXmlMapperAllDetailsFixture : CatXmlMapperFixture
    {
        protected override string CreateExpectedXml()
        {
            return @"<Entity>
                        <Id>1</Id>
                        <Id2>A</Id2>
                        <Name>Test</Name>
                        <Name2>Bob</Name2>
                        <Total>3</Total>
                     </Entity>";
        }

        protected override Cat CreateExpectedDto()
        {
            return new Cat { Id = 1, Name = "Tabby", Spayed = false };
        }
    }
}
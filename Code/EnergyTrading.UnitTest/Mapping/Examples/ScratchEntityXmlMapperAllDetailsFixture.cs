namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ScratchEntityXmlMapperAllDetailsFixture : ScratchEntityXmlMapperFixture
    {
        protected override string CreateExpectedXml()
        {
            return @"<Entity xmlns=""http://www.sample.com/app"" Value=""5"">
                        <Id>1</Id>
                        <Id2>A</Id2>
                        <Name>Test</Name>
                        <Name2>Bob</Name2>
                        <Date>2012-05-13</Date>
                        <DateTime>2011-03-26T14:23:14</DateTime>
                        <DateTimeOffset>2011-03-26T14:23:14+02:00</DateTimeOffset>
                     </Entity>";
        }

        protected override ScratchEntity CreateExpectedDto()
        {
            return new ScratchEntity
            {
                Id = "1",
                Id2 = "A",
                Name = "Test",
                Name2 = "Bob",
                Date = new DateTime(2012, 5, 13),
                DateTime = new DateTime(2011, 3, 26, 14, 23, 14),
                DateTimeOffset = new DateTimeOffset(2011, 3, 26, 14, 23, 14, new TimeSpan(0, 2, 0, 0)),
                Value = 5,
                // NB Exclude total to check non-serialization of null
            };
        }
    }
}
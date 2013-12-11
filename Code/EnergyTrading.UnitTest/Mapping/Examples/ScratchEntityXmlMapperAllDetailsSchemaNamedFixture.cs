namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ScratchEntityXmlMapperAllDetailsSchemaNamedFixture : ScratchEntityXmlMapperAllDetailsFixture
    {
        protected override string CreateExpectedXml()
        {
            return @"<app:Entity xmlns:app=""http://www.sample.com/app"" Value=""5"">
                        <app:Id>1</app:Id>
                        <app:Id2>A</app:Id2>
                        <app:Name>Test</app:Name>
                        <app:Name2>Bob</app:Name2>
                        <app:Date>2012-05-13</app:Date>
                        <app:DateTime>2011-03-26T14:23:14</app:DateTime>
                        <app:DateTimeOffset>2011-03-26T14:23:14+02:00</app:DateTimeOffset>
                        <app:Total>0</app:Total>
                     </app:Entity>";
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
                // NB Include total to check serialization of default
                Total = 0
            };
        }
    }
}
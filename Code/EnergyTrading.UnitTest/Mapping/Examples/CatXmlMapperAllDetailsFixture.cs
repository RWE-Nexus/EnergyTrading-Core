namespace EnergyTrading.UnitTest.Mapping.Examples
{
    public class CatXmlMapperAllDetailsFixture : CatXmlMapperFixture
    {
        protected override string CreateExpectedXml()
        {
            return @"<monitoring:Metric monitoring:Channel=""Topic: DEV.MonitoredEvent.Unenriched.New"" xmlns:monitoring=""http://rwe.com/schema/monitoring/1"">
                       <monitoring:MessagesPublished xmlns:monitoring=""http://rwe.com/schema/monitoring/1"">5</monitoring:MessagesPublished>
                       <monitoring:MessagesReceived xmlns:monitoring=""http://rwe.com/schema/monitoring/1"">3</monitoring:MessagesReceived>
                     </monitoring:Metric>";
        }

        protected override Cat CreateExpectedDto()
        {
            return new Cat { Id = 1, Name = "Tabby", Spayed = false };
        }
    }
}
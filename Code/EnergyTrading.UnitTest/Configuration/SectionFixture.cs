namespace EnergyTrading.UnitTest.Configuration
{
    using System;
    using System.Configuration;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Test;

    [TestClass]
    public class SectionFixture : Fixture
    {
        [TestMethod]
        public void SimpleSection()
        {
            var section = (SampleSection)ConfigurationManager.GetSection("simpleSample");

            Assert.AreEqual(1, section.Parents.Count, "Parent count differs");
            Assert.AreEqual(1, section.Parents[0].Children.Count, "Children count differs");
            //this.DisplaySection(section);
        }

        private void DisplaySection(SampleSection section)
        {
            Console.WriteLine("Parents: " + section.Parents.Count);

            var j = 1;
            foreach (ParentElement subscriber in section.Parents)
            {
                Console.WriteLine("Parent " + j++);
                Console.WriteLine("Name: " + subscriber.Name);
                Console.WriteLine("Type: " + subscriber.Type);
                Console.WriteLine("Children: " + subscriber.Children.Count);

                int i = 1;
                foreach (ChildElement application in subscriber.Children)
                {
                    Console.WriteLine(string.Empty);
                    Console.WriteLine("Child " + i++ + ": " + application.Name);
                    //Console.WriteLine("Gateways: " + application.Gateways.Count);
                    //Console.Write("    ");

                    //foreach (GatewayElement gateway in application.Gateways)
                    //{
                    //    Console.Write(gateway.Name + " ");
                    //}

                    //Console.WriteLine("");

                    //Console.WriteLine("Events: " + application.Events.Count);
                    //Console.Write("    ");
                    //foreach (EventElement evt in application.Events)
                    //{
                    //    Console.Write(evt.Name + " ");
                    //}
                    //Console.WriteLine("");
                }
            }
        }
    }
}
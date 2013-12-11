namespace EnergyTrading.UnitTest.Mapping
{
    using System.Xml.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    [TestClass]
    public class XmlMapperFixture : XmlFixture
    {
        public const string AppNamespace = "http://www.sample.com/app";
        public const string AppNamespacePrefix = "app";

        protected IXmlMappingEngine Engine { get; set; }

        // NB Differences in the namespace prefix are reported as significant for xsi:type when they are not
        // since they are just surrogates for the real namespace e.g. Pet:Dog and pet:Dog are considered different
        // We align to the constants used in the app
        private string xml = @"
                <Owner xmlns='http://www.sample.com/app' xmlns:Pet='http://www.sample.com/pet' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                    <Identifier scheme='a'>b</Identifier>
                    <Name>Fred</Name>
                    <Pets>
                        <Animal xsi:type='Pet:Dog'>
                            <Id>1</Id>
                            <Name>Fido</Name>
                            <Pet:Tricks>Fetch</Pet:Tricks>
                        </Animal>
                        <Animal xsi:type='Pet:Cat'>
                            <Id>2</Id>
                            <Name>Tom</Name>
                            <Pet:Spayed>true</Pet:Spayed>
                        </Animal>
                        <Animal xsi:type='Animal'>
                            <Id>3</Id>
                            <Name>Birdie</Name>
                        </Animal>
                    </Pets>
                </Owner>";
        
        [TestMethod]
        public void MapPolymorphicList()
        {
            var processor = CreateProcessor();
            processor.Initialize(xml);

            var candidate = Engine.Map<XPathProcessor, Owner>(processor);

            var expectedDog = new Dog { Id = 1, Name = "Fido", Tricks = "Fetch" };
            var expectedCat = new Cat { Id = 2, Name = "Tom", Spayed = true };
            var expectedAnimal = new Animal { Id = 3, Name = "Birdie" };

            Assert.AreEqual(3, candidate.Pets.Count, "Pet count differs");
            var candidateDog = candidate.Pets[0] as Dog;
            var candidateCat = candidate.Pets[1] as Cat;
            var candidateAnimal = candidate.Pets[2];

            Check(expectedCat, candidateCat, "Cat");
            Check(expectedDog, candidateDog, "Dog");
            Check(expectedAnimal, candidateAnimal, "Animal");
        }

        [TestMethod]
        public void RoundTripMappingWithPolymorphicList()
        {
            var processor = CreateProcessor();
            processor.Initialize(xml);

            var candidate = Engine.Map<XPathProcessor, Owner>(processor);

            var candidateXml = Engine.CreateDocument(candidate);
            CheckXml(xml, candidateXml);
        }

        [TestMethod]
        public void ReadEmptyNodesAsEmptyStringMissingNodesAsNull()
        {
            var entityXml = @"
<Entity>
   <Id></Id>
   <Name></Name>
</Entity>";

            var expected = new Entity { Id = string.Empty, Name = string.Empty };
            ReadXml(entityXml, expected);
        }

        [TestMethod]
        public void ReadEmptyNodesAsEmptyString()
        {
            var entityXml = @"
<Entity>
   <Id></Id>
   <Id2></Id2>
   <Name></Name>
   <Name2></Name2>
</Entity>";

            var expected = new Entity { Id = string.Empty, Id2 = string.Empty, Name = string.Empty, Name2 = string.Empty };
            ReadXml(entityXml, expected);
        }

        [TestMethod]
        public void EmitCorrectDefaultsForNull()
        {
            var entityXml = @"
<Entity>
   <Id></Id>
   <Name></Name>
</Entity>";

            var expected = new Entity();
            EmitXml(expected, entityXml);
        }
  
        [TestMethod]
        public void EmitCorrectDefaultsForEmptyString()
        {
            var entityXml = @"
<Entity>
   <Id></Id>
   <Id2></Id2>
   <Name></Name>
   <Name2></Name2>
</Entity>";

            var expected = new Entity { Id = string.Empty, Id2 = string.Empty, Name = string.Empty, Name2 = string.Empty };
            EmitXml(expected, entityXml);
        }
      
        [TestMethod]
        public void RoundTripNullValues()
        {
            var entityXml = @"
<Entity>
   <Id></Id>
   <Name />
</Entity>";

            RoundTrip<Entity>(entityXml);
        }

        protected virtual XPathProcessor CreateProcessor()
        {
            var processor = new XPathProcessor();

            return processor;
        }

        protected void ReadXml<T>(string source, T expected)
        {
            var processor = CreateProcessor();
            processor.Initialize(source);

            var candidate = Engine.Map<XPathProcessor, T>(processor);   
         
            Check(expected, candidate);
        }

        protected void EmitXml<T>(T candidate, string expectedXml)
        {
            var candidateXml = Engine.Map<T, XElement>(candidate);

            CheckXml(candidateXml, expectedXml);   
        }

        protected void RoundTrip<T>(string source)
        {
            var processor = CreateProcessor();
            processor.Initialize(source);

            var candidate = Engine.Map<XPathProcessor, T>(processor);

            EmitXml(candidate, source);
        }

        protected override void OnSetup()
        {
            Container.RegisterType<IXmlMapper<XPathProcessor, Owner>, OwnerXmlMapper>();
            Container.RegisterType<IXmlMapper<XPathProcessor, Identifier>, IdentifierXmlMapper>();
            Container.RegisterType<IXmlMapper<XPathProcessor, Animal>, AnimalXmlMapper>();
            Container.RegisterType<IXmlMapper<XPathProcessor, Dog>, DogXmlMapper>();
            Container.RegisterType<IXmlMapper<XPathProcessor, Cat>, CatXmlMapper>();

            Container.RegisterType<IXmlMapper<Owner, XElement>, OwnerXmlMapper>();
            Container.RegisterType<IXmlMapper<Identifier, XElement>, IdentifierXmlMapper>();
            Container.RegisterType<IXmlMapper<Animal, XElement>, AnimalXmlMapper>();
            Container.RegisterType<IXmlMapper<Dog, XElement>, DogXmlMapper>();
            Container.RegisterType<IXmlMapper<Cat, XElement>, CatXmlMapper>();

            Container.RegisterType<IXmlMapper<XPathProcessor, Entity>, EntityXmlMapper>();
            Container.RegisterType<IXmlMapper<Entity, XElement>, EntityXmlMapper>();

            Container.RegisterType<IXmlMapperFactory, LocatorXmlMapperFactory>();
            Container.RegisterType<IXmlMappingEngine, XmlMappingEngine>(
                new InjectionConstructor(typeof(IXmlMapperFactory)));

            Engine = ServiceLocator.GetInstance<IXmlMappingEngine>();
        }
    }
}
namespace EnergyTrading.UnitTest.Mapping
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    [TestFixture]
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
        
        [Test]
        public void MapPolymorphicList()
        {
            var processor = this.CreateProcessor();
            processor.Initialize(this.xml);

            var candidate = this.Engine.Map<XPathProcessor, Owner>(processor);

            var expectedDog = new Dog { Id = 1, Name = "Fido", Tricks = "Fetch" };
            var expectedCat = new Cat { Id = 2, Name = "Tom", Spayed = true };
            var expectedAnimal = new Animal { Id = 3, Name = "Birdie" };

            Assert.AreEqual(3, candidate.Pets.Count, "Pet count differs");
            var candidateDog = candidate.Pets[0] as Dog;
            var candidateCat = candidate.Pets[1] as Cat;
            var candidateAnimal = candidate.Pets[2];

            this.Check(expectedCat, candidateCat, "Cat");
            this.Check(expectedDog, candidateDog, "Dog");
            this.Check(expectedAnimal, candidateAnimal, "Animal");
        }

        [Test]
        public void RoundTripMappingWithPolymorphicList()
        {
            var processor = this.CreateProcessor();
            processor.Initialize(this.xml);

            var candidate = this.Engine.Map<XPathProcessor, Owner>(processor);

            var candidateXml = this.Engine.CreateDocument(candidate);
            this.CheckXml(this.xml, candidateXml);
        }

        [Test]
        public void ReadEmptyNodesAsEmptyStringMissingNodesAsNull()
        {
            var entityXml = @"
<Entity>
   <Id></Id>
   <Name></Name>
</Entity>";

            var expected = new Entity { Id = string.Empty, Name = string.Empty };
            this.ReadXml(entityXml, expected);
        }

        [Test]
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
            this.ReadXml(entityXml, expected);
        }

        [Test]
        public void EmitCorrectDefaultsForNull()
        {
            var entityXml = @"
<Entity>
   <Id></Id>
   <Name></Name>
</Entity>";

            var expected = new Entity();
            this.EmitXml(expected, entityXml);
        }
  
        [Test]
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
            this.EmitXml(expected, entityXml);
        }
      
        [Test]
        public void RoundTripNullValues()
        {
            var entityXml = @"
<Entity>
   <Id></Id>
   <Name />
</Entity>";

            this.RoundTrip<Entity>(entityXml);
        }

        protected virtual XPathProcessor CreateProcessor()
        {
            var processor = new XPathProcessor();

            return processor;
        }

        protected void ReadXml<T>(string source, T expected)
        {
            var processor = this.CreateProcessor();
            processor.Initialize(source);

            var candidate = this.Engine.Map<XPathProcessor, T>(processor);   
         
            this.Check(expected, candidate);
        }

        protected void EmitXml<T>(T candidate, string expectedXml)
        {
            var candidateXml = this.Engine.Map<T, XElement>(candidate);

            this.CheckXml(candidateXml, expectedXml);   
        }

        protected void RoundTrip<T>(string source)
        {
            var processor = this.CreateProcessor();
            processor.Initialize(source);

            var candidate = this.Engine.Map<XPathProcessor, T>(processor);

            this.EmitXml(candidate, source);
        }

        protected override void OnSetup()
        {
            this.Container.RegisterType<IXmlMapper<XPathProcessor, Owner>, OwnerXmlMapper>();
            this.Container.RegisterType<IXmlMapper<XPathProcessor, Identifier>, IdentifierXmlMapper>();
            this.Container.RegisterType<IXmlMapper<XPathProcessor, Animal>, AnimalXmlMapper>();
            this.Container.RegisterType<IXmlMapper<XPathProcessor, Dog>, DogXmlMapper>();
            this.Container.RegisterType<IXmlMapper<XPathProcessor, Cat>, CatXmlMapper>();

            this.Container.RegisterType<IXmlMapper<Owner, XElement>, OwnerXmlMapper>();
            this.Container.RegisterType<IXmlMapper<Identifier, XElement>, IdentifierXmlMapper>();
            this.Container.RegisterType<IXmlMapper<Animal, XElement>, AnimalXmlMapper>();
            this.Container.RegisterType<IXmlMapper<Dog, XElement>, DogXmlMapper>();
            this.Container.RegisterType<IXmlMapper<Cat, XElement>, CatXmlMapper>();

            this.Container.RegisterType<IXmlMapper<XPathProcessor, Entity>, EntityXmlMapper>();
            this.Container.RegisterType<IXmlMapper<Entity, XElement>, EntityXmlMapper>();

            this.Container.RegisterType<IXmlMapperFactory, LocatorXmlMapperFactory>();
            this.Container.RegisterType<IXmlMappingEngine, XmlMappingEngine>(
                new InjectionConstructor(typeof(IXmlMapperFactory)));

            this.Engine = this.ServiceLocator.GetInstance<IXmlMappingEngine>();
        }
    }
}
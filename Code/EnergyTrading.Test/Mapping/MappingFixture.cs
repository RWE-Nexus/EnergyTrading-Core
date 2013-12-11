namespace EnergyTrading.Test.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Mapping;
    using EnergyTrading.Registrars;
    using EnergyTrading.Test;

    /// <summary>
    /// Base class for testing mappers.
    /// <para>
    /// Need to inherit class into assembly where used, overriding <see cref="Fixture.CreateCheckerFactory" /> to
    /// use the local <see cref="ICheckerFactory" /> and <see cref="ShouldMapToDto" />/<see cref="ShouldMapToXml" />
    /// due to MSTest not executing tests in different assemblies - implementations should just call the base method.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MappingFixture<T> : Xml.XmlFixture 
        where T : class, new()
    {
        /// <summary>
        /// Gets the XML mapping engine to use.
        /// </summary>
        protected XmlMappingEngine MappingEngine { get; private set; }

        /// <summary>
        /// Get or set the XML expected to be produced by the test.
        /// </summary>
        protected string ExpectedXml { get; set; }

        /// <summary>
        /// Get or set the DTO expected to be produced by the test.
        /// </summary>
        protected T ExpectedDto { get; set; }

        /// <summary>
        /// Get or set the XML mapper we are testing.
        /// </summary>
        protected XmlMapper<T> Mapper { get; set; }

        /// <summary>
        /// Get or set the XPath processor used.
        /// </summary>
        protected XPathProcessor XPathProcessor { get; set; }

        [TestInitialize]
        public virtual void Initialize()
        {
            ExpectedXml = CreateExpectedXml();
            ExpectedDto = CreateExpectedDto();

            var locator = new Mock<IServiceLocator>();
            var factory = new LocatorXmlMapperFactory(locator.Object);
            var cache = new CachingXmlMapperFactory(factory);
            MappingEngine = new XmlMappingEngine(cache);
            RegisterChildMappers(MappingEngine);

            XPathProcessor = new XPathProcessor();
            XPathProcessor.Initialize(ExpectedXml);

            Mapper = CreateMapper(MappingEngine);
            MappingEngine.RegisterMapper(Mapper);
        }

        /// <summary>
        /// Test to map to DTO - needs to override in implemented class to assign TestMethod due to MSTest
        /// </summary>
        public virtual void ShouldMapToDto()
        {
            var candidate = Mapper.Map(XPathProcessor);
            Check(ExpectedDto, candidate);
        }

        /// <summary>
        /// Test to map to XML - needs to override in implemented class to assign TestMethod due to MSTest
        /// </summary>
        public virtual void ShouldMapToXml()
        {
            // Must go via the engine to get all the namespaces registered
            var candidate = MappingEngine.CreateDocument(ExpectedDto);
            CheckXml(ExpectedXml, candidate);
        }

        /// <summary>
        /// Creates the XPath processors to use.
        /// </summary>
        /// <remarks>
        /// Allows us to run tests using the standard or LINQ XPathProcessor.
        /// </remarks>
        /// <returns></returns>
        protected virtual XPathProcessor CreateXPathProcessor()
        {
            return new XPathProcessor();
        }

        /// <summary>
        /// Create the test's expected XML.
        /// </summary>
        /// <returns></returns>
        protected abstract string CreateExpectedXml();

        /// <summary>
        /// Create the test's expected DTO.
        /// </summary>
        /// <returns></returns>
        protected abstract T CreateExpectedDto();

        /// <summary>
        /// Create the XML mapper to test.
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        protected abstract XmlMapper<T> CreateMapper(IXmlMappingEngine engine);

        /// <summary>
        /// Register any other mappers that the XML mapper we are testing uses.
        /// </summary>
        /// <remarks>
        /// Can be real mappers for an integration test or mock mappers for a unit test.
        /// </remarks>
        /// <param name="engine"></param>
        protected virtual void RegisterChildMappers(IXmlMappingEngine engine)
        {
        }

        /// <summary>
        /// Registers a pair (DTO > XML and XML > DTO) of mock mappers in the mapping engine.
        /// </summary>
        /// <typeparam name="TDto">The DTO type to register the mock mapper for.</typeparam>
        protected void MockMappersFor<TDto>(bool outputDefault = false)
        {
            // DTO -> XML
            MappingEngine.RegisterMap(new Mock<IXmlMapper<TDto, XElement>>().Object);

            // XML -> DTO
            var mock = new Mock<IXmlMapper<XPathProcessor, TDto>>();
            mock.As<IXmlMapper<XPathProcessor>>();
            mock.Setup(x => x.MapList(It.IsAny<XPathProcessor>(), It.IsAny<string>(), outputDefault)).Returns(new List<TDto>());
            mock.Setup(x => x.MapList(It.IsAny<XPathProcessor>(), It.IsAny<string>(), It.IsAny<string>(), outputDefault)).Returns(new List<TDto>());
            mock.Setup(x => x.MapList(It.IsAny<XPathProcessor>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), outputDefault)).Returns(new List<TDto>());

            MappingEngine.RegisterMap(mock.Object);
        }

        /// <summary>
        /// Registers a pair (DTO > XML and XML > DTO) of mock mappers in the mapping engine for each type parameter.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        protected void MockMappersFor<T1, T2>()
        {
            MockMappersFor<T1>();
            MockMappersFor<T2>();
        }

        /// <summary>
        /// Registers a pair (DTO > XML and XML > DTO) of mock mappers in the mapping engine for each type parameter.
        /// </summary>
        protected void MockMappersFor<T1, T2, T3>()
        {
            MockMappersFor<T1>();
            MockMappersFor<T2>();
            MockMappersFor<T3>();
        }

        /// <summary>
        /// Registers a pair (DTO > XML and XML > DTO) of mock mappers in the mapping engine for each type parameter.
        /// </summary>
        protected void MockMappersFor<T1, T2, T3, T4>()
        {
            MockMappersFor<T1>();
            MockMappersFor<T2>();
            MockMappersFor<T3>();
            MockMappersFor<T4>();
        }

        /// <summary>
        /// Registers a pair (DTO > XML and XML > DTO) of mock mappers in the mapping engine for each type parameter.
        /// </summary>
        protected void MockMappersFor<T1, T2, T3, T4, T5>()
        {
            MockMappersFor<T1>();
            MockMappersFor<T2>();
            MockMappersFor<T3>();
            MockMappersFor<T4>();
            MockMappersFor<T5>();
        }

        /// <summary>
        /// Register a mapper for an entity against the container.
        /// </summary>
        /// <typeparam name="TEntity">Entity that the mapper is responsible for.</typeparam>
        /// <typeparam name="TMapper">Mapper for the entity.</typeparam>
        /// <param name="name">Optional name to register the mapper against.</param>
        protected void RegisterContainerMapper<TEntity, TMapper>(string name = null)
            where TEntity : class, new()
            where TMapper : XPathMapper<TEntity>, IXmlMapper<TEntity, XElement>
        {
            Container.RegisterXmlMapper<TEntity, TMapper>(name);
        }

        /// <summary>
        /// Register a mapper for an entity against the mapping engine.
        /// </summary>
        /// <typeparam name="TEntity">Entity that the mapper is responsible for.</typeparam>
        /// <param name="engine">Engine to register against</param>
        /// <param name="mapper">Mapper to register.</param>
        [Obsolete("Use engine.RegisterMapper(mapper);")]
        protected void RegisterMapper<TEntity>(IXmlMappingEngine engine, XmlMapper<TEntity> mapper)
            where TEntity : class, new()
        {
            engine.RegisterMapper(mapper);
        }
    }
}
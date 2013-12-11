namespace EnergyTrading.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Container;
    using EnergyTrading.Container.Unity;

    /// <summary>
    /// Base test fixture that sets up a UnityContainer and checker/builder factories
    /// </summary>
    public class Fixture
    {
        private IUnityContainer container;
        private IContainerContext containerContext;
        private ICheckerFactory checkerFactory;
        private IBuilderFactory entityFactory;
        
        /// <summary>
        /// Gets the checker factory.
        /// </summary>
        /// <remarks>Creates it on first use and also assigns ServiceLocator.Current to our ServiceLocator.</remarks>
        public ICheckerFactory CheckerFactory
        {
            get
            {
                if (this.checkerFactory == null)
                {
                    this.checkerFactory = this.CreateCheckerFactory();
                    if (this.checkerFactory == null)
                    {
                        throw new NotSupportedException("No CheckerFactory assigned to fixture");
                    }

                    this.ContainerContext.RegisterInstance(this.checkerFactory);
                }

                return this.checkerFactory;
            }
        }

        /// <summary>
        /// Gets the builder factory.
        /// </summary>
        /// <remarks>Creates it on first use.</remarks>        
        public IBuilderFactory EntityFactory
        {
            get
            {
                if (this.entityFactory == null)
                {
                    this.entityFactory = this.CreateEntityFactory();
                    if (this.entityFactory == null)
                    {
                        throw new NotSupportedException("No EntityFactory assigned to fixture");
                    }

                    this.ContainerContext.RegisterInstance(this.entityFactory);
                }

                return this.entityFactory;
            }
        }

        /// <summary>
        /// Gets or sets the container to use for the test.
        /// </summary>
        protected IUnityContainer Container
        {
            get { return this.container ?? (this.container = this.CreateContainer()); }
            set { this.container = value; }
        }

        /// <summary>
        /// Gets the service locator used by the test.
        /// </summary>
        protected IServiceLocator ServiceLocator
        {
            get { return this.Container.Resolve<IServiceLocator>(); }
        }

        /// <summary>
        /// Gets or sets the ContainerContext.
        /// </summary>
        protected IContainerContext ContainerContext
        {
            get { return this.containerContext ?? (this.containerContext = this.CreateContainerContext()); }
            set { this.containerContext = value; }
        }

        /// <summary>
        /// Pre-test set up.
        /// </summary>
        [TestInitialize]
        public virtual void Setup()
        {
            this.TidyUp();
            this.OnSetup();
        }

        /// <summary>
        /// Post-test tidy up
        /// </summary>
        [TestCleanup]
        public virtual void TearDown()
        {
            this.OnTearDown();
            this.TidyUp();
        }

        /// <summary>
        /// Creates the checker factory for the entities
        /// </summary>
        /// <returns></returns>
        protected virtual ICheckerFactory CreateCheckerFactory()
        {
            return null;
        }

        /// <summary>
        /// Create the construction factory for the entities
        /// </summary>
        /// <returns></returns>
        protected virtual IBuilderFactory CreateEntityFactory()
        {
            return null;
        }

        /// <summary>
        /// Create the Unity container here
        /// </summary>
        /// <returns></returns>
        protected virtual UnityContainer CreateContainer()
        {
            var x = new UnityContainer();

            // Add the usual bits
            x.StandardConfiguration();

            // Need to do this as the standard configuration doesn't assign the global locator.
            var locator = x.Resolve<IServiceLocator>();
            //Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => locator);

            return x;
        }

        /// <summary>
        /// Create the IoC container context here
        /// </summary>
        protected IContainerContext CreateContainerContext()
        {
            var context = new UnityContainerContext(this.Container);
            this.Container.RegisterInstance<IContainerContext>(context);

            return context;
        }

        /// <summary>
        /// Verify that the state of a couple of objects is the same
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity we want to check</typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        protected void Check<TEntity>(TEntity expected, TEntity candidate)
        {
            Check(expected, candidate, typeof(TEntity).Name);
        }

        /// <summary>
        /// Verify that the state of a couple of objects is the same
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        protected void Check<TEntity>(TEntity expected, TEntity candidate, string objectName)
        {
            this.CheckerFactory.Check(expected, candidate, objectName);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        protected void Check<T>(IList<T> expectedList, IList<T> candidateList)
        {
            Check(expectedList, candidateList, string.Empty);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        protected void Check<T>(IList<T> expectedList, IList<T> candidateList, string objectName)
        {
            this.CheckerFactory.Check(expectedList as IEnumerable<T>, candidateList, objectName);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        protected void Check<T>(ICollection<T> expectedList, ICollection<T> candidateList)
        {
            Check(expectedList, candidateList, string.Empty);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        protected void Check<T>(ICollection<T> expectedList, ICollection<T> candidateList, string objectName)
        {
            this.CheckerFactory.Check(expectedList as IEnumerable<T>, candidateList, objectName);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        protected void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList)
        {
            Check(expectedList, candidateList, string.Empty);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        protected void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList, string objectName)
        {
            this.CheckerFactory.Check(expectedList, candidateList, objectName);
        }

        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create</typeparam>
        /// <returns>The default instance of TEntity created by the factory.</returns>
        protected TEntity Default<TEntity>()
        {
            return this.EntityFactory.Default<TEntity>();
        }

        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to create</typeparam>
        /// <param name="id">The object identity to use</param>
        /// <returns>The instance of TEntity with the specified id created by the factory.</returns>
        protected TEntity Default<TEntity>(object id)
        {
            return this.EntityFactory.Default<TEntity>(id);
        }

        /// <summary>
        /// Creates a new persisted entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="persist"></param>
        /// <returns></returns>
        protected TEntity Default<TEntity>(bool persist)
        {
            return this.EntityFactory.Default<TEntity>(persist);
        }

        /// <summary>
        /// Creates a new persisted entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="persist"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>The parameter order is to keep </remarks>
        protected TEntity Default<TEntity>(object id, bool persist)
        {
            return this.EntityFactory.Default<TEntity>(persist, id);
        }

        /// <summary>
        /// Create a new entity according to the values passed
        /// </summary>
        /// <remarks>This one is not persisted or cached as we can't get an easy key for it</remarks>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        protected TEntity Default<TEntity>(params object[] values)
        {
            return this.EntityFactory.Default<TEntity>(values);
        }

        /// <summary>
        /// Changes an entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">The entity to change</param>
        protected void Change<TEntity>(TEntity entity)
        {
            this.EntityFactory.Change(entity);
        }

        /// <summary>
        /// Test specific setup logic, should call base.OnSetup when used
        /// </summary>
        protected virtual void OnSetup()
        {
        }

        /// <summary>
        /// Test specific tear down logic.
        /// </summary>
        protected virtual void OnTearDown()
        {
        }

        /// <summary>
        /// Clear down the test context.
        /// </summary>
        protected virtual void TidyUp()
        {
            // Ensure that we wipe down the core objects - NUnit re-uses the instance for all tests
            this.entityFactory = null;
            this.checkerFactory = null;
            this.container = null;
            this.containerContext = null;
        }

        /// <summary>
        /// Execute an action with a maximum time budget.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxTimeMs"></param>
        /// <param name="message"></param>
        protected void Timed(Action action, int maxTimeMs, string message = "")
        {
            var sw = new Stopwatch();
            sw.Start();

            action.Invoke();

            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;

            if (elapsed > maxTimeMs)
            {
                Assert.Fail("{0}: Exceeded time budget - {1:}ms vs {2}ms", message, elapsed, maxTimeMs);
            }

            Debug.WriteLine("{0}: In time budget - {1:}ms vs {2}ms", message, elapsed, maxTimeMs);
        }
    }
}
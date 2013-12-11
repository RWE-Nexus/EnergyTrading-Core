namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Container.Unity;

    [TestClass]
    public class UnityExtensionsFixture 
    {
        private IUnityContainer container;

        /// <summary>
        /// Initializes each test by creating a new container and registering the <see cref="TypeTrackingExtension"/>.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            container = new UnityContainer();

            // NOTE: Use the core extensions so we know they play nice together.
            container.InstallCoreExtensions();
        }

        [TestMethod]
        public void ShouldReturnNewInstanceEvenIfNotRegisteredWhenUsingNormalResolve()
        {
            // when
            var result = container.Resolve<TestClass>();

            // then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TryResolveOfUnknownType()
        {
            var obj = container.TryResolve<ITest>();
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void TryResolveOfUnknownTypeByName()
        {
            var obj = container.TryResolve<ITest>("name");
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void TryResolveOfRegisteredType()
        {
            container.RegisterType<ITest, TestClass>();
            Assert.IsNotNull(container.Resolve<ITest>());
            var obj = container.TryResolve<ITest>();
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TryResolveOfRegisteredTypeByName()
        {
            container.RegisterType<ITest, TestClass>("name");
            var obj = container.TryResolve<ITest>("name");
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TryResolveOfRegisteredInstance()
        {
            var expected = new TestClass { Data = "Test" };
            container.RegisterInstance<ITest>(expected);
            Assert.IsNotNull(container.Resolve<ITest>());
            var obj = container.TryResolve<ITest>();
            Assert.AreSame(expected, obj);
        }

        [TestMethod]
        public void TryResolveOfRegisteredInstanceByName()
        {
            var expected = new TestClass { Data = "Test" };
            container.RegisterInstance<ITest>("name", expected);
            Assert.IsNotNull(container.Resolve<ITest>("name"));
            var obj = container.TryResolve<ITest>("name");
            Assert.AreSame(expected, obj);
        }

        [TestMethod]
        public void TryResolveOfUnknownTypeWithDefault()
        {
            var src = new TestClass { Data = "hello world" };
            var obj = container.TryResolve<ITest>(src);
            Assert.IsNotNull(obj);
            Assert.AreSame(src, obj);
        }

        [TestMethod]
        public void TryResolveOfUnknownTypeWithDefaultByName()
        {
            var src = new TestClass { Data = "hello world" };
            var obj = container.TryResolve<ITest>(src, "name");
            Assert.IsNotNull(obj);
            Assert.AreSame(src, obj);
        }

        [TestMethod]
        public void CanResolveType()
        {
            Assert.IsFalse(container.CanResolve<ITest>());
            container.RegisterType<ITest, TestClass>();
            Assert.IsTrue(container.CanResolve<ITest>());
        }

        [TestMethod]
        public void CanResolveByName()
        {
            Assert.IsFalse(container.CanResolve<ITest>("name"));
            container.RegisterType<ITest, TestClass>("name");
            Assert.IsTrue(container.CanResolve<ITest>("name"));
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToEnumerableDefaultWithDefaultTypeOnly()
        {
            container.RegisterType<ITest, TestClass>();
            int count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToEnumerableDefaultWithNamedTypeOnly()
        {
            container.RegisterType<ITest, TestClass>("name");
            int count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToEnumerableDefaultWithTwoTypes()
        {
            container.RegisterType<ITest, TestClass>();
            container.RegisterType<ITest, AnotherTestClass>("name");
            int count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToEnumerableDefaultWithNoTypes()
        {
            int count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToEnumerableDefaultWithDefaultTypeOnly()
        {
            container.RegisterType<ITest, TestClass>();

            //  test with default
            int count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);

            //  test without default
            count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToEnumerableDefaultWithNamedTypeOnly()
        {
            container.RegisterType<ITest, TestClass>("name");
            //  test with default
            int count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);

            //  test without default
            count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToEnumerableDefaultWithTwoTypes()
        {
            container.RegisterType<ITest, TestClass>();
            container.RegisterType<ITest, AnotherTestClass>("name");
            //  test with default
            int count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(2, count);

            //  test without default
            count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToEnumerableDefaultWithNoTypes()
        { //  test with default
            int count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);

            //  test with default
            count = 0;
            foreach (var item in container.ResolveAllToEnumerable<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToArrayDefaultWithDefaultTypeOnly()
        {
            container.RegisterType<ITest, TestClass>();
            int count = 0;
            foreach (var item in container.ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToArrayDefaultWithNamedTypeOnly()
        {
            container.RegisterType<ITest, TestClass>("name");
            int count = 0;
            foreach (var item in container.ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToArrayDefaultWithTwoTypes()
        {
            container.RegisterType<ITest, TestClass>();
            container.RegisterType<ITest, AnotherTestClass>("name");
            int count = 0;
            foreach (var item in container.ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToArrayDefaultWithNoTypes()
        {
            var count = 0;
            foreach (var item in container.ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToArrayDefaultWithDefaultTypeOnly()
        {
            container.RegisterType<ITest, TestClass>();

            //  test with default
            var objs = container.ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);

            //  test without default
            objs = container.ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(0, objs.Length);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToArrayDefaultWithNamedTypeOnly()
        {
            container.RegisterType<ITest, TestClass>("name");

            //  test with default
            var objs = container.ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);

            //  test without default
            objs = container.ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToArrayDefaultWithTwoTypes()
        {
            container.RegisterType<ITest, TestClass>();
            container.RegisterType<ITest, AnotherTestClass>("name");

            //  test with default
            var objs = container.ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(2, objs.Length);

            //  test without default
            objs = container.ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToArrayDefaultWithNoTypes()
        {
            //  test with default
            var objs = container.ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(0, objs.Length);

            //  test without default
            objs = container.ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(0, objs.Length);
        }

        [TestMethod]
        public void ServiceLocatorIsCorrectlyRegisteredWithContainer()
        {
            container.StandardConfiguration();
            Assert.IsNotNull(container.Resolve<IServiceLocator>());
        }

        [TestMethod]
        public void ServiceLocatorIsCorrectlyRegisteredWithChildContainer()
        {
            container.StandardConfiguration();

            var child = container.ConfigureChildContainer("test");
            Assert.IsNotNull(child.Resolve<IServiceLocator>());
        }
    }
}
namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.Container.Unity;

    [TestFixture]
    public class UnityExtensionsFixture 
    {
        private IUnityContainer container;

        /// <summary>
        /// Initializes each test by creating a new container and registering the <see cref="TypeTrackingExtension"/>.
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.container = new UnityContainer();

            // NOTE: Use the core extensions so we know they play nice together.
            this.container.InstallCoreExtensions();
        }

        [Test]
        public void ShouldReturnNewInstanceEvenIfNotRegisteredWhenUsingNormalResolve()
        {
            // when
            var result = this.container.Resolve<TestClass>();

            // then
            Assert.IsNotNull(result);
        }

        [Test]
        public void TryResolveOfUnknownType()
        {
            var obj = this.container.TryResolve<ITest>();
            Assert.IsNull(obj);
        }

        [Test]
        public void TryResolveOfUnknownTypeByName()
        {
            var obj = this.container.TryResolve<ITest>("name");
            Assert.IsNull(obj);
        }

        [Test]
        public void TryResolveOfRegisteredType()
        {
            this.container.RegisterType<ITest, TestClass>();
            Assert.IsNotNull(this.container.Resolve<ITest>());
            var obj = this.container.TryResolve<ITest>();
            Assert.IsNotNull(obj);
        }

        [Test]
        public void TryResolveOfRegisteredTypeByName()
        {
            this.container.RegisterType<ITest, TestClass>("name");
            var obj = this.container.TryResolve<ITest>("name");
            Assert.IsNotNull(obj);
        }

        [Test]
        public void TryResolveOfRegisteredInstance()
        {
            var expected = new TestClass { Data = "Test" };
            this.container.RegisterInstance<ITest>(expected);
            Assert.IsNotNull(this.container.Resolve<ITest>());
            var obj = this.container.TryResolve<ITest>();
            Assert.AreSame(expected, obj);
        }

        [Test]
        public void TryResolveOfRegisteredInstanceByName()
        {
            var expected = new TestClass { Data = "Test" };
            this.container.RegisterInstance<ITest>("name", expected);
            Assert.IsNotNull(this.container.Resolve<ITest>("name"));
            var obj = this.container.TryResolve<ITest>("name");
            Assert.AreSame(expected, obj);
        }

        [Test]
        public void TryResolveOfUnknownTypeWithDefault()
        {
            var src = new TestClass { Data = "hello world" };
            var obj = this.container.TryResolve<ITest>(src);
            Assert.IsNotNull(obj);
            Assert.AreSame(src, obj);
        }

        [Test]
        public void TryResolveOfUnknownTypeWithDefaultByName()
        {
            var src = new TestClass { Data = "hello world" };
            var obj = this.container.TryResolve<ITest>(src, "name");
            Assert.IsNotNull(obj);
            Assert.AreSame(src, obj);
        }

        [Test]
        public void CanResolveType()
        {
            Assert.IsFalse(this.container.CanResolve<ITest>());
            this.container.RegisterType<ITest, TestClass>();
            Assert.IsTrue(this.container.CanResolve<ITest>());
        }

        [Test]
        public void CanResolveByName()
        {
            Assert.IsFalse(this.container.CanResolve<ITest>("name"));
            this.container.RegisterType<ITest, TestClass>("name");
            Assert.IsTrue(this.container.CanResolve<ITest>("name"));
        }

        [Test]
        public void ImplicitDefaultResolveAllToEnumerableDefaultWithDefaultTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>();
            int count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [Test]
        public void ImplicitDefaultResolveAllToEnumerableDefaultWithNamedTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>("name");
            int count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [Test]
        public void ImplicitDefaultResolveAllToEnumerableDefaultWithTwoTypes()
        {
            this.container.RegisterType<ITest, TestClass>();
            this.container.RegisterType<ITest, AnotherTestClass>("name");
            int count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(2, count);
        }

        [Test]
        public void ImplicitDefaultResolveAllToEnumerableDefaultWithNoTypes()
        {
            int count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [Test]
        public void ExplicitDefaultResolveAllToEnumerableDefaultWithDefaultTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>();

            //  test with default
            int count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);

            //  test without default
            count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [Test]
        public void ExplicitDefaultResolveAllToEnumerableDefaultWithNamedTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>("name");
            //  test with default
            int count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);

            //  test without default
            count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [Test]
        public void ExplicitDefaultResolveAllToEnumerableDefaultWithTwoTypes()
        {
            this.container.RegisterType<ITest, TestClass>();
            this.container.RegisterType<ITest, AnotherTestClass>("name");
            //  test with default
            int count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(2, count);

            //  test without default
            count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [Test]
        public void ExplicitDefaultResolveAllToEnumerableDefaultWithNoTypes()
        { //  test with default
            int count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);

            //  test with default
            count = 0;
            foreach (var item in this.container.ResolveAllToEnumerable<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [Test]
        public void ImplicitDefaultResolveAllToArrayDefaultWithDefaultTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>();
            int count = 0;
            foreach (var item in this.container.ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [Test]
        public void ImplicitDefaultResolveAllToArrayDefaultWithNamedTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>("name");
            int count = 0;
            foreach (var item in this.container.ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [Test]
        public void ImplicitDefaultResolveAllToArrayDefaultWithTwoTypes()
        {
            this.container.RegisterType<ITest, TestClass>();
            this.container.RegisterType<ITest, AnotherTestClass>("name");
            int count = 0;
            foreach (var item in this.container.ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(2, count);
        }

        [Test]
        public void ImplicitDefaultResolveAllToArrayDefaultWithNoTypes()
        {
            var count = 0;
            foreach (var item in this.container.ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [Test]
        public void ExplicitDefaultResolveAllToArrayDefaultWithDefaultTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>();

            //  test with default
            var objs = this.container.ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);

            //  test without default
            objs = this.container.ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(0, objs.Length);
        }

        [Test]
        public void ExplicitDefaultResolveAllToArrayDefaultWithNamedTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>("name");

            //  test with default
            var objs = this.container.ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);

            //  test without default
            objs = this.container.ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);
        }

        [Test]
        public void ExplicitDefaultResolveAllToArrayDefaultWithTwoTypes()
        {
            this.container.RegisterType<ITest, TestClass>();
            this.container.RegisterType<ITest, AnotherTestClass>("name");

            //  test with default
            var objs = this.container.ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(2, objs.Length);

            //  test without default
            objs = this.container.ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);
        }

        [Test]
        public void ExplicitDefaultResolveAllToArrayDefaultWithNoTypes()
        {
            //  test with default
            var objs = this.container.ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(0, objs.Length);

            //  test without default
            objs = this.container.ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(0, objs.Length);
        }

        [Test]
        public void ServiceLocatorIsCorrectlyRegisteredWithContainer()
        {
            this.container.StandardConfiguration();
            Assert.IsNotNull(this.container.Resolve<IServiceLocator>());
        }

        [Test]
        public void ServiceLocatorIsCorrectlyRegisteredWithChildContainer()
        {
            this.container.StandardConfiguration();

            var child = this.container.ConfigureChildContainer("test");
            Assert.IsNotNull(child.Resolve<IServiceLocator>());
        }
    }
}
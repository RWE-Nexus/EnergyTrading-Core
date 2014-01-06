namespace EnergyTrading.UnitTest.Container.Unity
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Container.Unity;

    [TestClass]
    public class TypeTrackingTestFixture
    {
        private IUnityContainer container;

        /// <summary>
        /// Initializes each test by creating a new container and registering the <see cref="TypeTrackingExtension"/>.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            this.container = new UnityContainer();
            this.container.AddNewExtension<TypeTrackingExtension>();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TryResolveWithoutExtensionRegisteration()
        {
            this.container = new UnityContainer();
            var obj = this.container.CheckConfigure<TypeTrackingExtension>().TryResolve<ITest>();
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void TryResolveOfUnknownType()
        {
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>();
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void TryResolveOfUnknownTypeByName()
        {
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>("name");
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void TryResolveOfRegisteredType()
        {
            this.container.RegisterType<ITest, TestClass>();
            Assert.IsNotNull(this.container.Resolve<ITest>());
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>();
            Assert.IsNotNull(obj);
        }
        
        [TestMethod]
        public void TryResolveOfRegisteredTypeByName()
        {
            this.container.RegisterType<ITest, TestClass>("name");
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>("name");
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TryResolveOfRegisteredTypeWithConstructorArgs()
        {
            this.container.RegisterType(typeof(TestClass), new ContainerControlledLifetimeManager());
            Assert.IsNotNull(this.container.Resolve<TestClass>());
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<TestClass>();
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TryResolveOfRegisteredTypeWithConstructorArgsByName()
        {
            this.container.RegisterType(typeof(TestClass), "name", new ContainerControlledLifetimeManager());
            Assert.IsNotNull(this.container.Resolve<TestClass>("name"));
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<TestClass>("name");
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void TryResolveOfUnknownTypeWithDefault()
        {
            var src = new TestClass { Data = "hello world" };
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>(src);
            Assert.IsNotNull(obj);
            Assert.AreSame(src, obj);
        }

        [TestMethod]
        public void TryResolveOfUnknownTypeWithDefaultByName()
        {
            var src = new TestClass { Data = "hello world" };
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>(src, "name");
            Assert.IsNotNull(obj);
            Assert.AreSame(src, obj);
        }

        [TestMethod]
        public void CanResolveType()
        {
            Assert.IsFalse(this.container.Configure<TypeTrackingExtension>().CanResolve<ITest>());
            this.container.RegisterType<ITest, TestClass>();
            Assert.IsTrue(this.container.Configure<TypeTrackingExtension>().CanResolve<ITest>());
        }

        [TestMethod]
        public void CanResolveByName()
        {
            Assert.IsFalse(this.container.Configure<TypeTrackingExtension>().CanResolve<ITest>("name"));
            this.container.RegisterType<ITest, TestClass>("name");
            Assert.IsTrue(this.container.Configure<TypeTrackingExtension>().CanResolve<ITest>("name"));
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllDefaultWithDefaultTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>();
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllDefaultWithNamedTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>("name");
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllDefaultWithTwoTypes()
        {
            this.container.RegisterType<ITest, TestClass>();
            this.container.RegisterType<ITest, AnotherTestClass>("name");
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllDefaultWithNoTypes()
        {
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllDefaultWithDefaultTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>();

            //  test with default
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);

            //  test without default
            count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllDefaultWithNamedTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>("name");
            //  test with default
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);

            //  test without default
            count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllDefaultWithTwoTypes()
        {
            this.container.RegisterType<ITest, TestClass>();
            this.container.RegisterType<ITest, AnotherTestClass>("name");
            //  test with default
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(2, count);

            //  test without default
            count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllDefaultWithNoTypes()
        { //  test with default
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(true))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);

            //  test with default
            count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(false))
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToArrayDefaultWithDefaultTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>();
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToArrayDefaultWithNamedTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>("name");
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ImplicitDefaultResolveAllToArrayDefaultWithTwoTypes()
        {
            this.container.RegisterType<ITest, TestClass>();
            this.container.RegisterType<ITest, AnotherTestClass>("name");
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(2, count);
        }
        [TestMethod]
        public void ImplicitDefaultResolveAllToArrayDefaultWithNoTypes()
        {
            int count = 0;
            foreach (var item in this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>())
            {
                Assert.IsNotNull(item);
                ++count;
            }
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToArrayDefaultWithDefaultTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>();

            //  test with default
            var objs = this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);

            //  test without default
            objs = this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(0, objs.Length);
        }
        [TestMethod]
        public void ExplicitDefaultResolveAllToArrayDefaultWithNamedTypeOnly()
        {
            this.container.RegisterType<ITest, TestClass>("name");

            //  test with default
            var objs = this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);

            //  test without default
            objs = this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToArrayDefaultWithTwoTypes()
        {
            this.container.RegisterType<ITest, TestClass>();
            this.container.RegisterType<ITest, AnotherTestClass>("name");

            //  test with default
            var objs = this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(2, objs.Length);

            //  test without default
            objs = this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(1, objs.Length);
        }

        [TestMethod]
        public void ExplicitDefaultResolveAllToArrayDefaultWithNoTypes()
        {
            //  test with default
            var objs = this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(true);
            Assert.IsNotNull(objs);
            Assert.AreEqual(0, objs.Length);

            //  test without default
            objs = this.container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(false);
            Assert.IsNotNull(objs);
            Assert.AreEqual(0, objs.Length);
        }
    }
}
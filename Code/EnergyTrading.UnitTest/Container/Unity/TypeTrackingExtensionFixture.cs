namespace EnergyTrading.UnitTest.Container.Unity
{
    using System;

    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.Container.Unity;

    [TestFixture]
    public class TypeTrackingTestFixture
    {
        private IUnityContainer container;

        /// <summary>
        /// Initializes each test by creating a new container and registering the <see cref="TypeTrackingExtension"/>.
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.container = new UnityContainer();
            this.container.AddNewExtension<TypeTrackingExtension>();
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void TryResolveWithoutExtensionRegisteration()
        {
            this.container = new UnityContainer();
            var obj = this.container.CheckConfigure<TypeTrackingExtension>().TryResolve<ITest>();
            Assert.IsNull(obj);
        }

        [Test]
        public void TryResolveOfUnknownType()
        {
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>();
            Assert.IsNull(obj);
        }

        [Test]
        public void TryResolveOfUnknownTypeByName()
        {
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>("name");
            Assert.IsNull(obj);
        }

        [Test]
        public void TryResolveOfRegisteredType()
        {
            this.container.RegisterType<ITest, TestClass>();
            Assert.IsNotNull(this.container.Resolve<ITest>());
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>();
            Assert.IsNotNull(obj);
        }
        
        [Test]
        public void TryResolveOfRegisteredTypeByName()
        {
            this.container.RegisterType<ITest, TestClass>("name");
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>("name");
            Assert.IsNotNull(obj);
        }

        [Test]
        public void TryResolveOfRegisteredTypeWithConstructorArgs()
        {
            this.container.RegisterType(typeof(TestClass), new ContainerControlledLifetimeManager());
            Assert.IsNotNull(this.container.Resolve<TestClass>());
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<TestClass>();
            Assert.IsNotNull(obj);
        }

        [Test]
        public void TryResolveOfRegisteredTypeWithConstructorArgsByName()
        {
            this.container.RegisterType(typeof(TestClass), "name", new ContainerControlledLifetimeManager());
            Assert.IsNotNull(this.container.Resolve<TestClass>("name"));
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<TestClass>("name");
            Assert.IsNotNull(obj);
        }

        [Test]
        public void TryResolveOfUnknownTypeWithDefault()
        {
            var src = new TestClass { Data = "hello world" };
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>(src);
            Assert.IsNotNull(obj);
            Assert.AreSame(src, obj);
        }

        [Test]
        public void TryResolveOfUnknownTypeWithDefaultByName()
        {
            var src = new TestClass { Data = "hello world" };
            var obj = this.container.Configure<TypeTrackingExtension>().TryResolve<ITest>(src, "name");
            Assert.IsNotNull(obj);
            Assert.AreSame(src, obj);
        }

        [Test]
        public void CanResolveType()
        {
            Assert.IsFalse(this.container.Configure<TypeTrackingExtension>().CanResolve<ITest>());
            this.container.RegisterType<ITest, TestClass>();
            Assert.IsTrue(this.container.Configure<TypeTrackingExtension>().CanResolve<ITest>());
        }

        [Test]
        public void CanResolveByName()
        {
            Assert.IsFalse(this.container.Configure<TypeTrackingExtension>().CanResolve<ITest>("name"));
            this.container.RegisterType<ITest, TestClass>("name");
            Assert.IsTrue(this.container.Configure<TypeTrackingExtension>().CanResolve<ITest>("name"));
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
        [Test]
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

        [Test]
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
        [Test]
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

        [Test]
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

        [Test]
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
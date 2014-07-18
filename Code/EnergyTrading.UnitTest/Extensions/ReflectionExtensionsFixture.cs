namespace EnergyTrading.UnitTest.Extensions
{
    using EnergyTrading.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class ReflectionExtensionsFixture
    {
        public class GrandChild
        {
        }

        private class Child
        {
// ReSharper disable UnusedAutoPropertyAccessor.Local
            public GrandChild GrandChild { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local
            public override bool Equals(object obj)
            {
 	             return ReferenceEquals(this, obj);
            }
        }

        private class Parent
        {
            public Child ReferenceChild { get; set; }
            public int ValueChild { get; set; }
        }

        [Test]
        public void IfNullCreateReturnsNullIfSourceIsNull()
        {
            var candidate = ReflectionExtension.IfNullCreate<Parent, Child>(null, t => t.ReferenceChild, () => new Child());
            Assert.IsNull(candidate);
        }

        [Test]
        public void IfNullCreateReturnsNullIfFuncIsNull()
        {
            var candidate = new Parent().IfNullCreate(null, () => new Child());
            Assert.IsNull(candidate);
        }

        [Test]
        public void IfNullCreateReturnsNullIfCreateFunctionIsNull()
        {
            var candidate = new Parent().IfNullCreate(t => t.ReferenceChild, null);
            Assert.IsNull(candidate);
        }

        [Test]
        public void IfNullCreateReturnsChildIfExists()
        {
            var parent = new Parent { ReferenceChild = new Child() };
            var candidate = parent.IfNullCreate(t => t.ReferenceChild, () => new Child());
            Assert.AreSame(candidate, parent.ReferenceChild);
        }

        [Test]
        public void IfNullCreateSetsPropertyAndReturnsCorrectValueIfChildIsNull()
        {
            var parent = new Parent();
            var candidate = parent.IfNullCreate(t => t.ReferenceChild, () => new Child());
            Assert.IsNotNull(candidate);
            Assert.AreSame(candidate, parent.ReferenceChild);
        }

        [Test]
        public void ChainingIfNullCreate()
        {
            var parent = new Parent();
            var grandChild = parent.IfNullCreate(x => x.ReferenceChild, () => new Child()).IfNullCreate(y => y.GrandChild, () => new GrandChild());
            Assert.IsNotNull(grandChild);
            Assert.IsNotNull(parent.ReferenceChild);
            Assert.IsNotNull(parent.ReferenceChild.GrandChild);
            Assert.AreSame(grandChild, parent.ReferenceChild.GrandChild);
        }

        [Test]
        public void ChainingWithDefaultConstructorVersion()
        {
            var parent = new Parent();
            var grandChild = parent.IfNullCreate(x => x.ReferenceChild).IfNullCreate(y => y.GrandChild);
            Assert.IsNotNull(grandChild);
            Assert.IsNotNull(parent.ReferenceChild);
            Assert.IsNotNull(parent.ReferenceChild.GrandChild);
            Assert.AreSame(grandChild, parent.ReferenceChild.GrandChild);
        }

        [Test]
        public void IfDefaultAssignNullParent()
        {
            ReflectionExtension.IfDefaultAssign<Parent, Child>(null, x => x.ReferenceChild, () => new Child());
        }

        [Test]
        public void IfDefaultAssignNullAccessor()
        {
            var parent = new Parent();
            parent.IfDefaultAssign(null, () => 3);
            Assert.That(parent.ValueChild, Is.EqualTo(default(int)));
        }

        [Test]
        public void IfDefaultAssignValueTypeAssigned()
        {
            var parent = new Parent();
            parent.IfDefaultAssign(x => x.ValueChild, () => 3);
            Assert.That(parent.ValueChild, Is.EqualTo(3));
        }

        [Test]
        public void IfDefaultAssignValueTypeNotDefault()
        {
            var parent = new Parent { ValueChild = 2 };
            parent.IfDefaultAssign(x => x.ValueChild, () => 3);
            Assert.That(parent.ValueChild, Is.EqualTo(2));
        }

        [Test]
        public void IfDefaultAssignValueTypeOverriddenDefault()
        {
            var parent = new Parent { ValueChild = 2 };
            parent.IfDefaultAssign(x => x.ValueChild, () => 3, () => 2);
            Assert.That(parent.ValueChild, Is.EqualTo(3));
        }

        [Test]
        public void IfDefaultAssignReferenceTypeAssigned()
        {
            var parent = new Parent();
            var child = new Child();
            parent.IfDefaultAssign(x => x.ReferenceChild, () => child);
            Assert.That(parent.ReferenceChild, Is.EqualTo(child));
        }

        [Test]
        public void IfDefaultAssignReferenceTypeNotDefault()
        {
            var child1 = new Child();
            var child2 = new Child();
            var parent = new Parent { ReferenceChild = child1 };
            parent.IfDefaultAssign(x => x.ReferenceChild, () => child2);
            Assert.That(object.ReferenceEquals(parent.ReferenceChild, child1), Is.True);
        }

        [Test]
        public void IfDefaultAssignReferenceTypeOverriddenDefault()
        {
            /// remember we have overridden child.Equals to behave according to ReferenceEquals
            var child1 = new Child();
            var child2 = new Child();
            var parent = new Parent { ReferenceChild = child1 };
            parent.IfDefaultAssign(x => x.ReferenceChild, () => child2, () => child1);
            Assert.That(object.ReferenceEquals(parent.ReferenceChild, child2), Is.True);
        }
    }
}
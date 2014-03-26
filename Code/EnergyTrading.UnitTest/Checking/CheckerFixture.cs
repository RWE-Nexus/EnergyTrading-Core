namespace EnergyTrading.UnitTest.Checking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EnergyTrading.Test;
    using EnergyTrading.Test.Checking;

    using NUnit.Framework;

    using Fixture = EnergyTrading.UnitTest.Fixture;

    [TestFixture]
    public class CheckingFixture : Fixture
    {
        [Test]
        public void ValidateAllComparedPropertiesAreSame()
        {
            var expected = new Simple { Id = 1, Name = "A" };
            var candidate = new Simple { Id = 1, Name = "A" };

            this.Check(expected, candidate);
        }

        [Test]
        public void ValidateDifferentIntPropertyMessage()
        {
            var expected = new Simple { Id = 1, Name = "A" };
            var candidate = new Simple { Id = 2, Name = "A" };

            this.CheckFault(expected, candidate, "Simple.Id", 1, 2);
        }

        [Test]
        public void ValidateDifferentStringPropertyMessage()
        {
            var expected = new Simple { Id = 1, Name = "A" };
            var candidate = new Simple { Id = 1, Name = "B" };

            this.CheckFault(expected, candidate, "Simple.Name", "A", "B");
        }

        [Test]
        public void ValidateCompareById()
        {
            var p1 = new Parent { Id = 1, Name = "A" };
            var expected = new Child { Id = 1, Name = "Child", Parent = p1 };
            p1.Favourite = expected;

            var p2 = new Parent { Id = 1, Name = "A" };
            var candidate = new Child { Id = 1, Name = "Child", Parent = p2 };
            p2.Favourite = candidate;

            this.Check(expected, candidate);
        }

        [Test]
        public void ValidateCompareByIdMessage()
        {
            var p1 = new Parent { Id = 1, Name = "A" };
            var expected = new Child { Id = 1, Name = "Child", Parent = p1 };
            p1.Favourite = expected;

            var p2 = new Parent { Id = 2, Name = "A" };
            var candidate = new Child { Id = 1, Name = "Child", Parent = p2 };
            p2.Favourite = candidate;

            this.CheckFault(expected, candidate, "Child.Parent.Id", 1, 2);
        }

        [Test]
        public void ValidateExcludeProperty()
        {
            var expected = new Parent { Id = 1, Name = "A", Another = 1 };
            var candidate = new Parent { Id = 1, Name = "A", Another = 2 };

            this.Check(expected, candidate);
        }

        [Test]
        public void ValidatePropertyMessageInChildEntity()
        {
            var expected = new Parent { Id = 1, Name = "A" };
            var c1 = new Child { Id = 1, Name = "Child", Parent = expected };
            expected.Favourite = c1;

            var candidate = new Parent { Id = 1, Name = "A" };
            var c2 = new Child { Id = 2, Name = "Child", Parent = candidate };
            candidate.Favourite = c2;

            this.CheckFault(expected, candidate, "Parent.Favourite.Id", 1, 2);
        }

        [Test]
        public void ValidateCollection()
        {
            var c1 = new Child();
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            expected.Children.Add(c1);
            candidate.Children.Add(c1);

            this.Check(expected, candidate);
        }

        [Test]
        public void ValidateNullIIdentifiable()
        {
            var expected = new Child { Id = 1, Name = "Child" };
            var candidate = new Child { Id = 1, Name = "Child" };

            this.Check(expected, candidate);
        }

        [Test]
        public void ValidateWhenBothCollectionPropertyAreNull()
        {
            var expected = new Parent { Id = 1, Name = "A" };
            var candidate = new Parent { Id = 1, Name = "A" };

            this.Check(expected, candidate);
        }

        [Test]
        public void ValidateMessageWhenCandidateCollectionPropertyIsNull()
        {
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A" };

            this.CheckFault(expected, candidate, "Parent.Children", "not null", "null");
        }

        [Test]
        public void ValidateMessageWhenExpectedCollectionPropertyIsNull()
        {
            var expected = new Parent { Id = 1, Name = "A" };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            this.CheckFault(expected, candidate, "Parent.Children", "null", "not null");
        }

        [Test]
        public void ValidateMessageWhenCandidateCollectionCountIsDifferent()
        {
            var c1 = new Child();
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            expected.Children.Add(c1);

            this.CheckFault(expected, candidate, "Parent.Children.Count", 1, 0);
        }

        [Test]
        public void ValidateMessageWhenExpectedCollectionCountIsDifferent()
        {
            var c1 = new Child();
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            candidate.Children.Add(c1);

            this.CheckFault(expected, candidate, "Parent.Children.Count", 0, 1);
        }

        [Test]
        public void ValidateMessageCollectionElementIsDifferent()
        {
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            var c1 = new Child { Id = 1 };
            expected.Children.Add(c1);
            var c2 = new Child { Id = 2 };
            candidate.Children.Add(c2);

            this.CheckFault(expected, candidate, "Parent.Children[0].Id", 1, 2);
        }

        [Test]
        public void ValidateUniquePropertyInfoChecked()
        {
            PropertyCheck.Targeter = new TypeCompareTargeter();
            var checker = new SimpleChecker() as ICheckerCompare;

            // Just grab one
            var expected = checker.Properties.ToList()[0];

            // Add it a again.
            var candidate = checker.Compare(expected.Info);

            Assert.AreSame(expected, candidate.PropertyCheck);
        }

        private void CheckFault<T>(T expected, T candidate, string name, object expectedValue, object actualValue)
        {
            const string MessageFormat = "{0}: Expected:<{1}>. Actual:<{2}>";

            var message = string.Format(MessageFormat, name, expectedValue, actualValue);

            this.CheckFault(expected, candidate, message);
        }

        private void CheckFault<T>(T expected, T candidate, string faultMessage)
        {
            try
            {
                this.Check(expected, candidate);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(faultMessage, ex.Message);
                return;
            }

            Assert.Fail("No exception, expected: " + faultMessage);
        }
    }
}
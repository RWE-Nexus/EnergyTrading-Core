namespace EnergyTrading.UnitTest.Checking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Test;
    using EnergyTrading.Test.Checking;

    using Fixture = EnergyTrading.UnitTest.Fixture;

    [TestClass]
    public class CheckingFixture : Fixture
    {
        [TestMethod]
        public void ValidateAllComparedPropertiesAreSame()
        {
            var expected = new Simple { Id = 1, Name = "A" };
            var candidate = new Simple { Id = 1, Name = "A" };

            Check(expected, candidate);
        }

        [TestMethod]
        public void ValidateDifferentIntPropertyMessage()
        {
            var expected = new Simple { Id = 1, Name = "A" };
            var candidate = new Simple { Id = 2, Name = "A" };

            CheckFault(expected, candidate, "Simple.Id", 1, 2);
        }

        [TestMethod]
        public void ValidateDifferentStringPropertyMessage()
        {
            var expected = new Simple { Id = 1, Name = "A" };
            var candidate = new Simple { Id = 1, Name = "B" };

            CheckFault(expected, candidate, "Simple.Name", "A", "B");
        }

        [TestMethod]
        public void ValidateCompareById()
        {
            var p1 = new Parent { Id = 1, Name = "A" };
            var expected = new Child { Id = 1, Name = "Child", Parent = p1 };
            p1.Favourite = expected;

            var p2 = new Parent { Id = 1, Name = "A" };
            var candidate = new Child { Id = 1, Name = "Child", Parent = p2 };
            p2.Favourite = candidate;

            Check(expected, candidate);
        }

        [TestMethod]
        public void ValidateCompareByIdMessage()
        {
            var p1 = new Parent { Id = 1, Name = "A" };
            var expected = new Child { Id = 1, Name = "Child", Parent = p1 };
            p1.Favourite = expected;

            var p2 = new Parent { Id = 2, Name = "A" };
            var candidate = new Child { Id = 1, Name = "Child", Parent = p2 };
            p2.Favourite = candidate;

            CheckFault(expected, candidate, "Child.Parent.Id", 1, 2);
        }

        [TestMethod]
        public void ValidateExcludeProperty()
        {
            var expected = new Parent { Id = 1, Name = "A", Another = 1 };
            var candidate = new Parent { Id = 1, Name = "A", Another = 2 };

            Check(expected, candidate);
        }

        [TestMethod]
        public void ValidatePropertyMessageInChildEntity()
        {
            var expected = new Parent { Id = 1, Name = "A" };
            var c1 = new Child { Id = 1, Name = "Child", Parent = expected };
            expected.Favourite = c1;

            var candidate = new Parent { Id = 1, Name = "A" };
            var c2 = new Child { Id = 2, Name = "Child", Parent = candidate };
            candidate.Favourite = c2;

            CheckFault(expected, candidate, "Parent.Favourite.Id", 1, 2);
        }

        [TestMethod]
        public void ValidateCollection()
        {
            var c1 = new Child();
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            expected.Children.Add(c1);
            candidate.Children.Add(c1);

            Check(expected, candidate);
        }

        [TestMethod]
        public void ValidateNullIIdentifiable()
        {
            var expected = new Child { Id = 1, Name = "Child" };
            var candidate = new Child { Id = 1, Name = "Child" };

            Check(expected, candidate);
        }

        [TestMethod]
        public void ValidateWhenBothCollectionPropertyAreNull()
        {
            var expected = new Parent { Id = 1, Name = "A" };
            var candidate = new Parent { Id = 1, Name = "A" };

            Check(expected, candidate);
        }

        [TestMethod]
        public void ValidateMessageWhenCandidateCollectionPropertyIsNull()
        {
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A" };

            CheckFault(expected, candidate, "Parent.Children", "not null", "null");
        }

        [TestMethod]
        public void ValidateMessageWhenExpectedCollectionPropertyIsNull()
        {
            var expected = new Parent { Id = 1, Name = "A" };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            CheckFault(expected, candidate, "Parent.Children", "null", "not null");
        }

        [TestMethod]
        public void ValidateMessageWhenCandidateCollectionCountIsDifferent()
        {
            var c1 = new Child();
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            expected.Children.Add(c1);

            CheckFault(expected, candidate, "Parent.Children.Count", 1, 0);
        }

        [TestMethod]
        public void ValidateMessageWhenExpectedCollectionCountIsDifferent()
        {
            var c1 = new Child();
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            candidate.Children.Add(c1);

            CheckFault(expected, candidate, "Parent.Children.Count", 0, 1);
        }

        [TestMethod]
        public void ValidateMessageCollectionElementIsDifferent()
        {
            var expected = new Parent { Id = 1, Name = "A", Children = new List<Child>() };
            var candidate = new Parent { Id = 1, Name = "A", Children = new List<Child>() };

            var c1 = new Child { Id = 1 };
            expected.Children.Add(c1);
            var c2 = new Child { Id = 2 };
            candidate.Children.Add(c2);

            CheckFault(expected, candidate, "Parent.Children[0].Id", 1, 2);
        }

        [TestMethod]
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

            CheckFault(expected, candidate, message);
        }

        private void CheckFault<T>(T expected, T candidate, string faultMessage)
        {
            try
            {
                Check(expected, candidate);
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
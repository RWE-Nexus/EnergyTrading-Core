namespace EnergyTrading.Test.Checking
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Checks a single property on a class for equality.
    /// </summary>
    public class PropertyCheck
    {
        private CompareTarget compareTarget;

        /// <summary>
        /// Create a new instance of the <see cref="PropertyCheck" /> class
        /// </summary>
        /// <param name="info">PropertyInfo to use</param>
        /// <param name="compareTarget">CompareTarget to use</param>
        public PropertyCheck(PropertyInfo info, CompareTarget compareTarget)
        {
            this.Info = info;
            this.CompareTarget = compareTarget;
        }

        /// <summary>
        /// Gets or sets the class which knows how to extract an Id from an object
        /// </summary>
        public static IIdentityChecker IdentityChecker { get; set; }

        /// <summary>
        /// Gets or sets the class which knows the default CompareTarget for a type.
        /// </summary>
        public static ITypeCompareTargeter Targeter { get; set; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> used to access values on the object.
        /// </summary>
        public PropertyInfo Info { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="CompareTarget" /> used to determine what type of comparison
        /// to perform on the <see cref="PropertyInfo" /> derived values.
        /// </summary>
        public CompareTarget CompareTarget
        {
            get
            {
                return compareTarget;
            }
            set
            {
                this.compareTarget = value;
                this.OnCompareTargetChanged();
            }
        }

        /// <summary>
        /// Gets or sets the length property, used to limit string comparisons.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="expectedEntity"></param>
        /// <param name="candidateEntity"></param>
        /// <param name="objectName"></param>
        public void Check(IChecker checker, object expectedEntity, object candidateEntity, string objectName)
        {
            // Have to do the null check here, else "System.Reflection.TargetException: Non-static method requires a target."
            // could be thrown by RuntimePropertyInfo.GetValue().
            if (CheckNullNotNull(expectedEntity, candidateEntity, objectName))
            {
                return;
            }

            var expectedValue = this.ExtractValue(expectedEntity, objectName + ".Expected");
            var candidateValue = this.ExtractValue(candidateEntity, objectName + ".Expected");

            this.Check(this.CompareTarget, checker, expectedValue, candidateValue, objectName + "." + this.Info.Name);
        }

        protected static void CheckId(object expected, object candidate, string objectName)
        {
            // If both null we are ok, and can't check the Id property so quit now.
            if (CheckNullNotNull(expected, candidate, objectName))
            {
                return;
            }

            if (IdentityChecker == null)
            {
                throw new NotSupportedException("No IdentityChecker assigned, cannot perform Id check");
            }

            var expectedId = IdentityChecker.ExtractId(expected);
            var candidateId = IdentityChecker.ExtractId(candidate);

            if (!Equals(expectedId, candidateId))
            {
                throw new PropertyCheckException(objectName, expectedId, candidateId);
            }
        }

        protected object ExtractValue(object entity, string objectName)
        {
            try
            {
                return this.Info.GetValue(entity, null);
            }
            catch (Exception ex)
            {
                throw new PropertyCheckException(objectName + "." + this.Info.Name, string.Empty, ex.Message);
            }
        }

        protected void OnCompareTargetChanged()
        {
            if (CompareTarget != CompareTarget.Id)
            {
                return;
            }

            if (IdentityChecker == null)
            {
                throw new NotSupportedException("No IdentityChecker assigned, cannot perform Id check");
            }

            if (!IdentityChecker.SupportsId(this.Info.PropertyType))
            {
                throw new NotSupportedException(string.Format("Property {0}: type ({1}) must support Id check", this.Info.Name, this.Info.PropertyType));
            }
        }

        protected void Check(CompareTarget target, IChecker checker, object expected, object candidate, string objectName)
        {
            switch (target)
            {
                case CompareTarget.Id:
                    CheckId(expected, candidate, objectName + ".Id");
                    break;

                case CompareTarget.Entity:
                    checker.Check(expected, candidate, objectName);
                    break;

                case CompareTarget.Count:
                case CompareTarget.Collection:
                    this.Check(checker, expected as IEnumerable, candidate as IEnumerable, objectName);
                    break;

                default:
                    if (CheckNullNotNull(expected, candidate, objectName))
                    {
                        return;
                    }

                    if (!expected.Equals(candidate))
                    {
                        throw new PropertyCheckException(objectName, expected, candidate);
                    }
                    break;
            }
        }

        /// <summary>
        /// Check if two collections of <see typeparamref="T" /> are equal.
        /// </summary>
        /// <remarks>
        /// The parameters are first checked for null, an exception is thrown if only one is null.
        /// Second, the cardinalities of the collections are checked if the <see cref="IEnumerable{T}" /> is
        /// also <see cref="ICollection{T}" /> which means that it supports <see cref="ICollection{T}.Count" />
        /// If these checks are passed, each item is compared in turn.
        /// </remarks>
        /// <param name="checker"></param>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        protected void Check(IChecker checker, IEnumerable expected, IEnumerable candidate, string objectName)
        {
            // Do we have two actual lists
            if (CheckNullNotNull(expected, candidate, objectName))
            {
                return;
            }

            CheckCardinality(expected, candidate, objectName);
            if (CompareTarget == CompareTarget.Count)
            {
                // We're done
                return;
            }

            // Ok, step both iterator togeter, will work as these are now confirmed to have the same cardinality
            var i = 0;
            var enumExpected = expected.GetEnumerator();
            var enumCandidate = candidate.GetEnumerator();
            var target = CompareTarget.Entity;
            Type type = null;

            enumExpected.Reset();
            enumCandidate.Reset();
            while (enumExpected.MoveNext())
            {
                if (type == null)
                {
                    type = enumExpected.Current.GetType();
                    if (Targeter == null)
                    {
                        throw new NotSupportedException("No ITypeCompareTargeter assigned to PropertyCheck");
                    }
                    target = Targeter.DetermineCompareTarget(type);
                }
                enumCandidate.MoveNext();
                this.Check(target, checker, enumExpected.Current, enumCandidate.Current, objectName + "[" + i++ + "]");
            }
        }

        /// <summary>
        /// Check if both expected and candidate are null or not null
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private static bool CheckNullNotNull(object expected, object candidate, string objectName)
        {
            if (expected == null && candidate == null)
            {
                return true;
            }

            if (expected == null)
            {
                throw new PropertyCheckException(objectName, "null", "not null");
            }

            if (candidate == null)
            {
                throw new PropertyCheckException(objectName, "not null", "null");
            }

            return false;
        }

        private static void CheckCardinality(IEnumerable expected, IEnumerable candidate, string objectName)
        {
            var expectedList = expected as ICollection;
            var candidateList = candidate as ICollection;

            // Sanity check to see if they are collections (could be just IEnumerable)
            if (expectedList == null || candidateList == null)
            {
                return;
            }

            if (expectedList.Count != candidateList.Count)
            {
                throw new PropertyCheckException(objectName + ".Count", expectedList.Count, candidateList.Count);
            }
        }
    }
}
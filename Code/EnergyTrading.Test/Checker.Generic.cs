namespace EnergyTrading.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Checking;

    using EnergyTrading.Extensions;

    /// <summary>
    /// Comparator used to verify that two instances of <see typeparamref="T" /> are 
    /// the same on a per property basis.
    /// </summary>
    /// <typeparam name="T">Type whose instances we will check</typeparam>
    public class Checker<T> : Checker, IChecker<T>, IChecker, ICheckerCompare
    {
// ReSharper disable StaticFieldInGenericType
        private static readonly MethodInfo CheckClassMi = typeof(Checker<T>).GetMethod("CheckClass", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly MethodInfo CheckParentClassMi = typeof(Checker<T>).GetMethod("CheckParentClass", BindingFlags.Static | BindingFlags.NonPublic);
// ReSharper restore StaticFieldInGenericType
        private readonly IList<PropertyCheck> properties;
        private readonly MethodInfo parentChecker;
        private readonly Type parentType;

        /// <summary>
        /// Creates a new instance of the <see cref="Checker{T}" /> class.
        /// </summary>
        public Checker()
        {
            properties = new List<PropertyCheck>();
            parentType = typeof(T).BaseType;
            if (parentType != typeof(object) && parentType != typeof(ValueType))
            {
                // Get a checker for the parent
                parentChecker = CheckParentClassMi.MakeGenericMethod(parentType);
            }
        }

        /// <copydocfrom cref="ICheckerCompare.Properties" />
        protected IList<PropertyCheck> Properties
        {
            get { return properties; }
        }

        /// <copydocfrom cref="ICheckerCompare.Properties" />
        ICollection<PropertyCheck> ICheckerCompare.Properties
        {
            get { return Properties; }
        }

        /// <summary>
        /// Gets a list of descendant types for the checker.
        /// </summary>
        protected virtual IEnumerable<Type> Descendants
        {
            get { return new List<Type>(); }
        }

        /// <summary>
        /// Check that the properties of two instances of <see typeparamref="T" /> are equal.
        /// </summary>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        public void Check(T expected, T candidate, string objectName = "")
        {
            if (string.IsNullOrEmpty(objectName))
            {
                objectName = typeof(T).Name;
            }

            if (!CheckDescendants(expected, candidate, objectName))
            {
                CheckBase(expected, candidate, objectName);
            }
        }

        /// <summary>
        /// Check the base properties of <see typeparameref="T" />, which are the parent properties and those directly declared in T.
        /// </summary>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        public void CheckBase(T expected, T candidate, string objectName = "")
        {
            if (string.IsNullOrEmpty(objectName))
            {
                objectName = typeof(T).Name;
            }

            // First check the parent
            CheckParent(expected, candidate, objectName);

            // Now our explicit ones.
            CheckComparisons(expected, candidate, objectName);
        }

        /// <copydocfrom cref="IChecker.Check" />
        void IChecker.Check(object expected, object candidate, string objectName)
        {
            Check((T)expected, (T)candidate, objectName);
        }

        /// <summary>
        /// Checks an entity to see if it satisfies a class constraint
        /// </summary>
        /// <typeparam name="TClass">Type of class to check.</typeparam>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        /// <returns>true if the instance pass, otherwise false.</returns>
        protected static bool CheckClass<TClass>(object expected, object candidate, string objectName)
            where TClass : class
        {
            if (expected is TClass)
            {
                CheckerFactory.Check(expected as TClass, candidate as TClass, objectName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check the parent class of an entity
        /// </summary>
        /// <typeparam name="TClass">Type to cast the objects to.</typeparam>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        protected static void CheckParentClass<TClass>(object expected, object candidate, string objectName)
            where TClass : class
        {
            CheckerFactory.CheckParent(expected as TClass, candidate as TClass, objectName);
        }

        /// <summary>
        /// Checks the parent properties of T.
        /// </summary>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        protected virtual void CheckParent(T expected, T candidate, string objectName)
        {
            if (parentChecker == null)
            {
                return;
            }

            parentChecker.Invoke(null, new object[] { expected, candidate, objectName });
        }

        /// <summary>
        /// Initializes the set of comparisons
        /// </summary>
        [Obsolete("Use Initialize")]
        protected void AutoCompare()
        {
            Initialize();
        }

        /// <summary>
        /// Automatically initializes the comparisons based on the public properties.
        /// </summary>
        protected void Initialize()
        {
            this.AutoCheck(typeof(T));
        }

        /// <summary>
        /// Check all immediate descendants of the class.
        /// </summary>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        /// <returns>true if we are a descendant, false otherwise</returns>
        protected virtual bool CheckDescendants(object expected, object candidate, string objectName)
        {
            return Descendants
                        .Select(type => CheckClassMi.MakeGenericMethod(type))
                        .Any(castMethod => (bool)castMethod.Invoke(null, new[] { expected, candidate, objectName }));
        }

        /// <summary>
        /// Check for equality between two objects.
        /// </summary>
        /// <param name="expected">Expected object to use</param>
        /// <param name="candidate">Candidate object to use</param>
        /// <param name="objectName">Name to use, displayed in error messages to disambiguate</param>
        protected virtual void CheckComparisons(T expected, T candidate, string objectName)
        {
            foreach (var prop in Properties)
            {
                prop.Check(CheckerFactory, expected, candidate, objectName);
            }
        }

        /// <summary>
        /// Include the property in the comparison test.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        protected PropertyCheckExpression Compare(Expression<Func<T, object>> propertyExpression)
        {
            return Compare(ReflectionExtension.GetPropertyInfo(propertyExpression));
        }

        /// <copydocfrom cref="ICheckerCompare.Compare" />
        PropertyCheckExpression ICheckerCompare.Compare(PropertyInfo propertyInfo)
        {
            return Compare(propertyInfo);
        }

        /// <copydocfrom cref="ICheckerCompare.Compare" />
        protected PropertyCheckExpression Compare(PropertyInfo propertyInfo)
        {
            if (PropertyCheck.Targeter == null)
            {
                throw new NotSupportedException("No ITypeCompareTargeter assigned to PropertyCheck");
            }
            return Compare(propertyInfo, PropertyCheck.Targeter.DetermineCompareTarget(propertyInfo.PropertyType));
        }

        /// <summary>
        /// Include the property in the comparison test.
        /// </summary>
        /// <param name="propertyInfo">PropertyInfo to use</param>
        /// <param name="compareTarget"></param>        
        /// <returns>A new <see cref="PropertyCheckExpression" /> created from the <see cref="PropertyInfo" /></returns>
        protected PropertyCheckExpression Compare(PropertyInfo propertyInfo, CompareTarget compareTarget)
        {
            var pc = Find(propertyInfo);
            if (pc == null)
            {
                // Add the new check
                pc = new PropertyCheck(propertyInfo, compareTarget);
                Properties.Add(pc);
            }
            else
            {
                // Update to the supplied target
                pc.CompareTarget = compareTarget;
            }

            return new PropertyCheckExpression(pc);
        }

        /// <summary>
        /// Exclude the property from the comparison test.
        /// </summary>
        /// <param name="propertyExpression"></param>
        protected void Exclude(Expression<Func<T, object>> propertyExpression)
        {
            Exclude(ReflectionExtension.GetPropertyInfo(propertyExpression));
        }

        /// <copydocfrom cref="ICheckerCompare.Exclude" />
        void ICheckerCompare.Exclude(PropertyInfo propertyInfo)
        {
            Exclude(propertyInfo);
        }

        /// <copydocfrom cref="ICheckerCompare.Exclude" />
        protected void Exclude(PropertyInfo propertyInfo)
        {
            var pc = Find(propertyInfo);
            if (pc != null)
            {
                Properties.Remove(pc);
            }
        }

        private PropertyCheck Find(PropertyInfo propertyInfo)
        {
            return Properties.FirstOrDefault(x => x.Info.Name == propertyInfo.Name);
        }
    }
}
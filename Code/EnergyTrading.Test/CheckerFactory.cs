namespace EnergyTrading.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using EnergyTrading.Extensions;
    using EnergyTrading.Test.Checking;
    using EnergyTrading.Test.Data;

    /// <summary>
    /// A factory class that provides checking facilities for objects so that property level comparisons can be easily made
    /// </summary>
    public class CheckerFactory : ICheckerFactory
    {
        private readonly IDictionary<Type, IChecker> checkers;
        private ICheckerBuilder checkerBuilder;

        /// <summary>
        /// Initializes a new instance of the CheckerClassFactory class.
        /// </summary>
        public CheckerFactory()
        {
            this.checkers = new Dictionary<Type, IChecker>();

            PropertyCheck.IdentityChecker = new IdentifiableChecker();
            PropertyCheck.Targeter = new TypeCompareTargeter();
        }

        /// <summary>
        /// Gets or sets the checker builder
        /// </summary>
        public ICheckerBuilder Builder
        {
            get { return this.checkerBuilder ?? (this.checkerBuilder = new NullCheckerBuilder()); }
            set { this.checkerBuilder = value; }
        }

        /// <summary>
        /// Registers all IChecker{T} implemented in an assembly
        /// </summary>
        /// <param name="assemblyFile">Filename of the assembly to register</param>
        public CheckerFactory Register(string assemblyFile)
        {
            if (!assemblyFile.EndsWith(".dll"))
            {
                assemblyFile += ".dll";
            }

            this.Register(Assembly.LoadFrom(assemblyFile));

            return this;
        }

        /// <summary>
        /// Registers all IChecker{T} implemented in an assembly
        /// </summary>
        /// <param name="assembly">Assembly to register</param>
        public CheckerFactory Register(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.SupportsGenericInterface(typeof(IChecker<>)) || type.ContainsGenericParameters)
                {
                    continue;
                }

                var checker = Activator.CreateInstance(type);
                var fred = type.BaseType;
                while (!fred.IsGenericType)
                {
                    fred = fred.BaseType;
                }

                var entityType = fred.GetGenericArguments();
                this.Add(entityType[0], (IChecker)checker);
            }

            return this;
        }

        /// <summary>
        /// Register a <see cref="IChecker{T}" /> for <see typeparamref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="checker"></param>
        public CheckerFactory Register<T>(IChecker<T> checker)
        {
            this.Add(typeof(T), (IChecker)checker);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        public void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList, string objectName)
        {
            var checker = Checker<T>();
            var listChecker = new ListChecker();

            // NB Can't call Check as this retrieves via the property info which we don't have here.
            listChecker.CheckList(checker as IChecker, expectedList, candidateList, objectName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        public void Check<T>(T expected, T candidate)
        {
            this.Check(expected, candidate, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        public void Check<T>(T expected, T candidate, string objectName)
        {
            if (Verify(candidate, expected, objectName))
            {
                Checker<T>().Check(expected, candidate, objectName);
            }
        }

        /// <summary>
        /// General entry point that works out which checker to invoke
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        public void Check(object expected, object candidate, string objectName)
        {
            if (expected == null && candidate == null)
            {
                return;
            }

            var type = expected != null ? expected.GetType() : candidate.GetType();
            this.Checker(type).Check(expected, candidate, objectName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        public void CheckParent<T>(T expected, T candidate, string objectName)
        {
            if (Verify(candidate, expected, objectName))
            {
                Checker<T>().CheckBase(expected, candidate, objectName);
            }
        }

        private static bool Verify<T>(T candidate, T expected, string objectName)
        {
            var type = typeof(T);
            if (!type.IsValueType)
            {
                if (candidate == null && expected == null)
                {
                    return false;
                }

                if (candidate == null)
                {
                    throw new PropertyCheckException(objectName ?? type.Name, "not null", "null");
                }

                if (expected == null)
                {
                    throw new PropertyCheckException(objectName ?? type.Name, "null", "not null");
                }
            }

            return true;
        }

        private IChecker<T> Checker<T>()
        {
            return (IChecker<T>)this.Checker(typeof(T));
        }

        private IChecker Checker(Type type)
        {
            if (this.checkers.ContainsKey(type))
            {
                return this.checkers[type];
            }

            // Attempt to self-register
            var checker = this.Builder.Build(type);
            if (checker != null)
            {
                this.Add(type, checker);

                return checker;
            }

            throw new ArgumentOutOfRangeException(string.Format("No checker registered for {0}", type.FullName));
        }

        private void Add(Type entityType, IChecker checker)
        {
            this.checkers.Add(entityType, checker);
        }

        private class ListChecker : PropertyCheck
        {
            public ListChecker()
                : base(null, CompareTarget.Collection)
            {
            }

            public void CheckList(IChecker checker, IEnumerable expected, IEnumerable candidate, string objectName)
            {
                Check(checker, expected, candidate, objectName);
            }
        }
    }
}
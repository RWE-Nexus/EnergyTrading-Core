namespace EnergyTrading.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
        /// <summary>
        /// Gets type attribute.
        /// </summary>
        /// <typeparam name="TAttr">Type of the attribute.</typeparam>
        /// <param name="type">Target type.</param>
        /// <returns>Attribute value</returns>
        public static TAttr GetAttribute<TAttr>(this Type type)
            where TAttr : Attribute
        {
            return type.GetCustomAttributes(false).Single(a => typeof(TAttr) == a.GetType()) as TAttr;
        }

        /// <summary>
        /// Determines whether type is decorated with specified attribute
        /// </summary>
        /// <typeparam name="TAttr">Type of the attribute.</typeparam>
        /// <param name="type">Target type.</param>
        /// <returns>True if type is decorated with specified attribute, otherwise false</returns>
        public static bool DecoratedWith<TAttr>(this Type type)
            where TAttr : Attribute
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.GetCustomAttributes(false).Any(a => a.GetType() == typeof(TAttr));
        }

        /// <summary>
        /// Determines whether type implements specified interface
        /// </summary>
        /// <typeparam name="TContract">Type of the interface.</typeparam>
        /// <param name="type">Target type.</param>
        /// <returns>True if type implements specified interface, otherwise false</returns>
        public static bool Implements<TContract>(this Type type) where TContract : class
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.GetInterfaces().Any(i => i == typeof(TContract));
        }

        /// <summary>
        /// Determines whether type implements specified interface
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="contract">Type of the interface.</param>
        /// <returns>True if type implements specified interface, otherwise false</returns>
        public static bool Implements(this Type type, Type contract)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.GetInterfaces().Any(i => i == contract);
        }

        /// <summary>
        /// Determines whether type implements interface that can be constructed from specified open-generic interface
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="contract">Type of the open-generic interface.</param>
        /// <returns>True if type implements interface that can be constructed from specified open-generic interface, otherwise false</returns>
        public static bool ImplementsOpenGeneric(this Type type, Type contract)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }
            if (!contract.IsInterface)
            {
                throw new ArgumentException("Provided contract has to be an interface", "contract");
            }
            if (!contract.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Provided contract has to be an open generic", "contract");
            }

            return type.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == contract));
        }

        /// <summary>
        /// Is the type a nullable type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        /// <summary>
        /// Creates an instance of an object from a type name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <param name="getType"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(this string typeName, Func<string, Type> getType = null)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }

            var type = typeName.ToType(getType);

            return CreateInstance<T>(type);
        }

        /// <summary>
        /// Creates an instance of an object from a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return (T)Activator.CreateInstance(type, true);
        }

        /// <summary>
        /// Get an attribute from a member/property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider member)
            where T : Attribute
        {
            var objects = member.GetCustomAttributes(typeof(T), true);
            if (objects.Length == 0)
            {
                return null;
            }

            return (T)objects[0];
        }

        /// <summary>
        /// Get all attributes of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public static List<T> GetAttributes<T>(this ICustomAttributeProvider member)
        {
            var attribs = new List<T>();
            var objects = member.GetCustomAttributes(typeof(T), true);
            if (objects.Length == 0)
            {
                return attribs;
            }

            attribs.AddRange(objects.Cast<T>());

            return attribs;
        }

        public static Type GetElementTypeExtended(this Type type)
        {
            return type.GetElementType(null);
        }

        public static Type GetElementType(this Type type, IEnumerable enumerable)
        {
            if (type.HasElementType)
            {
                return type.GetElementType();
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
            {
                return type.GetGenericArguments()[0];
            }

            var enumerableType = type.GetIEnumerableType();
            if (enumerableType != null)
            {
                return enumerableType.GetGenericArguments()[0];
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                if (enumerable != null)
                {
                    var first = enumerable.Cast<object>().FirstOrDefault();
                    return first != null ? first.GetType() : typeof(object);
                }
            }

            throw new ArgumentException(string.Format("Unable to find the element type for type '{0}'", type), "type");
        }

        public static Type GetIEnumerableType(this Type type)
        {
            try
            {
                return type.GetInterface("IEnumerable`1", false);
            }
            catch (Exception)
            {
                return type.BaseType != typeof(object) ? type.BaseType.GetIEnumerableType() : null;
            }
        }

        /// <summary>
        /// Check whether a member/property has an attribute defined.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool HasAttribute<T>(this ICustomAttributeProvider member)
            where T : Attribute
        {
            return GetAttribute<T>(member) != null;
        }

        /// <summary>
        /// Check if a type supports a generic interface
        /// </summary>
        /// <param name="type"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public static bool SupportsGenericInterface(this Type type, Type candidate)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException("candidate");
            }

            if (candidate.IsGenericType == false)
            {
                throw new ArgumentOutOfRangeException("candidate", "Must be a generic type");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.GetInterfaces().Where(i => i.IsGenericType).Any(i => candidate == i.GetGenericTypeDefinition());
        }

        public static bool IsEnumerableType(this Type type)
        {
            return type.SimpleCheck(typeof(IEnumerable));
        }

        public static bool IsCollectionType(this Type type)
        {
            return type.SimpleCheck(typeof(ICollection));
        }

        /// <summary>
        /// Returns the generic interfaces that a type implements
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GenericInterfaces(this Type type)
        {
            return type.GetInterfaces().Where(t => (t.IsGenericType && t.ReflectedType == null) && !t.ContainsGenericParameters);
        }

        /// <summary>
        /// Checks that the type supports the specified type and is not abstract
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">Type to check</param>
        /// <returns>true if the type passes the test, false otherwise</returns>
        public static bool SimpleCheck<T>(this Type type)
        {
            return type.SimpleCheck(typeof(T));
        }

        /// <summary>
        /// Checks that the type supports the specified type and is not abstract
        /// </summary>
        /// <param name="type">Type to support</param>
        /// <param name="candidate">Type to check</param>
        /// <returns>true if the type passes the test, false otherwise</returns>
        public static bool SimpleCheck(this Type type, Type candidate)
        {
            var found = type.IsAbstract == false && candidate.IsAssignableFrom(type);
            if (found)
            {
                //logger.InfoFormat("Registering {0} for {1}", type.Name, candidate.Name);
            }

            return found;
        }

        /// <summary>
        /// Checks that the type supports the specified generic type and is not generic or abstract
        /// </summary>
        /// <param name="type">Type to support</param>
        /// <param name="candidate">Type to check</param>
        /// <returns>true if the type passes the test, false otherwise</returns>
        public static bool GenericCheck(this Type type, Type candidate)
        {
            var found = type.SupportsGenericInterface(candidate) && type.IsAbstract == false && type.IsGenericType == false;
            if (found)
            {
                //logger.InfoFormat("Type {1} implements {0}", type.Name, candidate.Name);
            }

            return found;
        }

        /// <summary>
        /// Converts a type string into a type.
        /// </summary>
        /// <param name="typeName">Fully qualified name of the type, including assembly</param>
        /// <param name="getType">Type resolver, defaults to Type.GetType if null.</param>
        /// <returns>null if type not specified, type instance otherwise</returns>
        public static Type ToType(this string typeName, Func<string, Type> getType = null)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return null;
            }

            var type = getType == null ? Type.GetType(typeName) : getType(typeName);
            if (type == null)
            {
                throw new ArgumentException("typeName", "Could not convert value to type: " + typeName);
            }

            return type;
        }

        /// <summary>
        /// Allows partially open generic types to be constructed.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        public static Type MakePartialGenericType(this Type type, params Type[] typeArguments)
        {
            if (!type.IsGenericType)
            {
                return type;
            }

            var args = type.GetGenericArguments();
            for (var i = 0; i < typeArguments.GetLength(0); i++)
            {
                if (typeArguments[i] != null)
                {
                    args[i] = typeArguments[i];
                }
            }

            return type.MakeGenericType(args);
        }
    }
}

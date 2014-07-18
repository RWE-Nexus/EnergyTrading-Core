﻿namespace EnergyTrading.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Extension methods to help with reflection.
    /// </summary>
    public static class ReflectionExtension
    {
        /// <summary>
        /// if the value of the property is equal to the default then assigns the value obtained from the retriever
        /// If the current value or the new value cannot be determined the value is left at the original value
        /// uses object.Equals(a, b) function to determine equality
        /// </summary>
        /// <typeparam name="TParent">The type of the parent object</typeparam>
        /// <typeparam name="TChild">The type of the child property</typeparam>
        /// <param name="parent">An instance of the parent</param>
        /// <param name="accessor">Expression to obtain the property from the Parent (e.g. parent => parent.Property)</param>
        /// <param name="retriever">Action to retrieve the new value to assign </param>
        /// <param name="overrideDefaultValue">optional Action that can be supplied to override the default value used for comparison</param>
        /// <returns>void</returns>
        public static void IfDefaultAssign<TParent, TChild>(this TParent parent, Expression<Func<TParent, TChild>> accessor, Func<TChild> retriever, Func<TChild> overrideDefaultValue = null)
        {
            if ((!(parent is ValueType) && parent == null) || accessor == null)
            {
                return;
            }

            var value = accessor.Compile().Invoke(parent);
            var defaultValue = overrideDefaultValue == null ? default(TChild) : overrideDefaultValue();
            if (!Equals(value, defaultValue))
            {
                return;
            }
            var propertyInfo = GetPropertyInfo(accessor);
            if (propertyInfo != null)
            {
                if (retriever != null)
                {
                    var setMethod = propertyInfo.GetSetMethod();
                    setMethod.Invoke(parent, new object[] { retriever() });
                }
            }
        }

        /// <summary>
        /// Creates an instance and sets the property value if it is null 
        /// Returns the property value
        /// </summary>
        /// <typeparam name="TParent">The type of the parent object</typeparam>
        /// <typeparam name="TChild">The type of the child property</typeparam>
        /// <param name="parent">An instance of the parent</param>
        /// <param name="expression">Expression to obtain the property from the Parent (e.g. parent => parent.Property)</param>
        /// <param name="createFunction">Function used to create an instance of TChild (if required)</param>
        /// <returns>The instance of TChild set into the property</returns>
        public static TChild IfNullCreate<TParent, TChild>(this TParent parent, Expression<Func<TParent, TChild>> expression, Func<TChild> createFunction)
        {
            if ((!(parent is ValueType) && parent == null) || expression == null)
            {
                return default(TChild);
            }

            var value = expression.Compile().Invoke(parent);
            if (value is ValueType || value != null)
            {
                return value;
            }
            var propertyInfo = GetPropertyInfo(expression);
            if (propertyInfo != null)
            {
                if (createFunction == null)
                {
                    return default(TChild);
                }

                value = createFunction();
                var setMethod = propertyInfo.GetSetMethod();
                setMethod.Invoke(parent, new object[] { value });
                return value;
            }

            return default(TChild);
        }

        /// <summary>
        /// Simplified version of IfNullCreate for use when child item has a default constructor
        /// </summary>
        /// <typeparam name="TParent">The type of the parent object</typeparam>
        /// <typeparam name="TChild">The type of the child property</typeparam>
        /// <param name="parent">An instance of the parent</param>
        /// <param name="expression">Expression to obtain the property from the Parent (e.g. parent => parent.Property)</param>
        /// <returns>The instance of TChild set into the property</returns>
        public static TChild IfNullCreate<TParent, TChild>(this TParent parent, Expression<Func<TParent, TChild>> expression)
            where TChild : new()
        {
            return parent.IfNullCreate(expression, () => new TChild());
        }

        /// <summary>
        /// Unwrap an Expression to get to the appropriate PropertyInfo.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var me = GetMemberExpression(expression);
            return me.Member as PropertyInfo;
        }

        /// <summary>
        /// Unwrap an Expression to determine the appropriate MemberExpression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            if (expression == null)
            {
                return null;
            }

            var me = expression.Body as MemberExpression;
            if (me != null)
            {
                return me;
            }

            var ue = expression.Body as UnaryExpression;
            if (ue != null)
            {
                var operand = ue.Operand;
                var memberExpression = operand as MemberExpression;
                if (memberExpression != null)
                {
                    return memberExpression;
                }

                var callExpression = operand as MethodCallExpression;
                if (callExpression != null)
                {
                    return callExpression.Object as MemberExpression;
                }
            }

            return null;
        }
    }
}
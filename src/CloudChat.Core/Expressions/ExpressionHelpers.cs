using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CloudChat.Core
{
    public static class ExpressionHelpers
    {
        /// <summary>
        /// Compiles an expression and gets the functions return value
        /// </summary>
        /// <typeparam name="T"> This is the type of value </typeparam>
        /// <param name="expression"> The expression to compile </param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this Expression<Func<T>> expression) => expression.Compile().Invoke();

        /// <summary>
        /// Compiles an expression and gets the functions return value
        /// </summary>
        /// <typeparam name="T"> This is the type of value </typeparam>
        /// <typeparam name="In">Input param to the expression</typeparam>
        /// <param name="expression"> The expression to compile </param>
        /// <returns></returns>
        public static T GetPropertyValue<In, T>(this Expression<Func<In, T>> expression, In input) => expression.Compile().Invoke(input);

        /// <summary>
        /// Sets the underlying properties value to the given value
        /// from an expression that contains the property
        /// </summary>
        /// <typeparam name="T">The type of value to set</typeparam>
        /// <param name="expression">The expression</param>
        /// <param name="value"></param>
        public static void SetPropertyValue<T>(this Expression<Func<T>> expression, T value)
        {
            // Converts a lambda ()=> some.Property, to some.Property
            var expressionBody = expression?.Body as MemberExpression;

            // Get the property information so we can set it
            var propertyInfo = (PropertyInfo)expressionBody.Member;

            var target = Expression.Lambda(expressionBody.Expression).Compile().DynamicInvoke();

            // Set the property value
            propertyInfo.SetValue(target, value);
        }


        /// <summary>
        /// Sets the underlying properties value to the given value
        /// from an expression that contains the property
        /// </summary>
        /// <typeparam name="T">The type of value to set</typeparam>
        /// <typeparam name="In">The type of input value to set</typeparam>
        /// <param name="expression">The expression</param>
        /// <param name="value"></param>
        public static void SetPropertyValue<In, T>(this Expression<Func<In, T>> expression, T value, In input)
        {
            // Converts a lambda ()=> some.Property, to some.Property
            var expressionBody = expression?.Body as MemberExpression;

            // Get the property information so we can set it
            var propertyInfo = (PropertyInfo)expressionBody.Member;

            // Set the property value
            propertyInfo.SetValue(input, value);
        }
    }

}
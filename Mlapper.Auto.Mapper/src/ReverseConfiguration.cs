using System;
using System.Reflection;
using System.Linq.Expressions;

namespace Mlapper.Auto.Mapper.src
{
    /// <summary>
    /// Helper class for reverse mapping configuration
    /// </summary>
    public static class ReverseConfiguration
    {
        /// <summary>
        /// Extracts property info from a lambda expression
        /// </summary>
        /// <param name="expression">Lambda expression targeting a property</param>
        /// <returns>PropertyInfo for the targeted property</returns>
        public static PropertyInfo GetPropertyFromExpression(LambdaExpression expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return (PropertyInfo)memberExpression.Member;
            }
            else if (expression.Body is UnaryExpression unaryExpression)
            {
                // Handle conversion expressions (like when using object as return type)
                if (unaryExpression.Operand is MemberExpression memberExpr)
                {
                    return (PropertyInfo)memberExpr.Member;
                }
            }
            
            throw new ArgumentException("Expression is not a valid member access", nameof(expression));
        }
    }
}
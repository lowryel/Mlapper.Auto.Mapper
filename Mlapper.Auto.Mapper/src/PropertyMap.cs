using System;


using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Mlapper.Auto.Mapper
{
    /// <summary>
    /// Represents a mapping between a source property and a destination property
    /// </summary>
    public class PropertyMap
    {
        /// <summary>
        /// The source property (null if using a custom resolver)
        /// </summary>
        public PropertyInfo? SourceProperty { get; set; }

        /// <summary>
        /// Optional expressions for source and destination properties
        /// </summary>
        public LambdaExpression? SourceExpression { get; set; }

        /// <summary>
        /// Optional expressions for source and destination properties
        /// </summary>
        public LambdaExpression? DestinationExpression { get; set; }


        /// <summary>
        /// The destination property
        /// </summary>
        public PropertyInfo DestinationProperty { get; }

        /// <summary>
        /// Optional custom value resolver function
        /// </summary>
        public Func<object, object?>? CustomValueResolver { get; }

        /// <summary>
        /// Optional condition to apply before mapping
        /// /// </summary>

        public Func<object, bool>? Condition { get; set; }

        /// <summary>
        /// Creates a new property mapping
        /// </summary>
        /// <param name="sourceProperty">Source property (optional if using a custom resolver)</param>
        /// <param name="destinationProperty">Destination property</param>
        /// <param name="customValueResolver">Optional custom value resolver function</param>
        public PropertyMap(PropertyInfo? sourceProperty, PropertyInfo destinationProperty,
            Func<object, object?>? customValueResolver = null)
        {
            SourceProperty = sourceProperty;
            DestinationProperty = destinationProperty ?? throw new ArgumentNullException(nameof(destinationProperty));
            CustomValueResolver = customValueResolver;
        }
    }
}

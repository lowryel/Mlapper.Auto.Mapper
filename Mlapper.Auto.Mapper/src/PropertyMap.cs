using System;


using System;
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
        public PropertyInfo? SourceProperty { get; }
        
        /// <summary>
        /// The destination property
        /// </summary>
        public PropertyInfo DestinationProperty { get; }
        
        /// <summary>
        /// Optional custom value resolver function
        /// </summary>
        public Func<object, object?>? CustomValueResolver { get; }

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

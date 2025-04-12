using System;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;


namespace Mlapper.Auto.Mapper
{
    /// <summary>
    /// Configuration class for setting up object mappings
    /// </summary>
    public class MapperConfiguration
    {
        private readonly Dictionary<Type, Dictionary<Type, MappingInfo>> _mappings = new();

        /// <summary>
        /// Creates a mapping between source and destination types
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        public void CreateMap<TSource, TDestination>()
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);

            if (!_mappings.ContainsKey(sourceType))
            {
                _mappings[sourceType] = new Dictionary<Type, MappingInfo>();
            }

            var mappingInfo = new MappingInfo(sourceType, destinationType);
            _mappings[sourceType][destinationType] = mappingInfo;

            // Auto-discover property mappings
            foreach (var destProp in destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var sourceProp = sourceType.GetProperty(destProp.Name, BindingFlags.Public | BindingFlags.Instance);
                if (sourceProp != null && sourceProp.CanRead && destProp.CanWrite)
                {
                    if (destProp.PropertyType == sourceProp.PropertyType ||
                        IsAssignableOrConvertible(sourceProp.PropertyType, destProp.PropertyType))
                    {
                        mappingInfo.PropertyMaps.Add(new PropertyMap(sourceProp, destProp));
                    }
                }
            }
        }

        /// <summary>
        /// Configures a custom mapping for a specific member
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="destinationMember">Expression selecting the destination member</param>
        /// <param name="valueResolver">Function to resolve the value from the source</param>
        public void ForMember<TSource, TDestination>(
            Expression<Func<TDestination, object?>> destinationMember,
            Func<TSource, object?> valueResolver)
        {
            var sourceType = typeof(TSource);
            var destType = typeof(TDestination);

            // Check if the mapping exists
            if (!_mappings.ContainsKey(sourceType) || !_mappings[sourceType].ContainsKey(destType))
            {
                throw new InvalidOperationException(
                    $"No mapping defined from {sourceType} to {destType}. Call CreateMap first.");
            }

            var destProperty = GetPropertyFromExpression(destinationMember);
            var mappingInfo = _mappings[sourceType][destType];

            // Remove any existing automatic mapping for this property
            mappingInfo.PropertyMaps.RemoveAll(pm => pm.DestinationProperty.Name == destProperty.Name);

            // Add custom mapping with value resolver function
            mappingInfo.PropertyMaps.Add(new PropertyMap(null, destProperty,
                src => valueResolver((TSource)src)));
        }

        /// <summary>
        /// Extracts the property info from a member expression
        /// </summary>
        private PropertyInfo GetPropertyFromExpression<T, TProperty>(
            Expression<Func<T, TProperty>> expression)
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
            throw new ArgumentException("Expression is not a valid member access. Make sure you're using a property selector like 'x => x.PropertyName'", nameof(expression));
        }

        /// <summary>
        /// Determines if a source type can be assigned to or converted to a destination type
        /// </summary>
        private bool IsAssignableOrConvertible(Type sourceType, Type destType)
        {
            if (destType.IsAssignableFrom(sourceType))
                return true;

            // Check for common convertible types
            if ((sourceType == typeof(int) && destType == typeof(long)) ||
                (sourceType == typeof(float) && destType == typeof(double)) ||
                (sourceType == typeof(int) && destType == typeof(string)) ||
                (sourceType == typeof(string) && IsNumericType(destType)))
                return true;

            return false;
        }

        /// <summary>
        /// Determines if a type is a numeric type
        /// </summary>
        private bool IsNumericType(Type type)
        {
            return type == typeof(int) || type == typeof(long) ||
                   type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal);
        }

        /// <summary>
        /// Creates a mapper instance with the current configuration
        /// </summary>
        public IMapper CreateMapper()
        {
            // Create a deep copy of the mappings to prevent modification after mapper creation
            var mappingsCopy = new Dictionary<Type, Dictionary<Type, MappingInfo>>();
            
            foreach (var sourceMapping in _mappings)
            {
                mappingsCopy[sourceMapping.Key] = new Dictionary<Type, MappingInfo>();
                foreach (var destMapping in sourceMapping.Value)
                {
                    mappingsCopy[sourceMapping.Key][destMapping.Key] = destMapping.Value;
                }
            }
            
            return new Mapper(mappingsCopy);
        }
    }
}

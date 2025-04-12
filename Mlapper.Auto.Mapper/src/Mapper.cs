using System;


using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mlapper.Auto.Mapper
{
    /// <summary>
    /// Implementation of IMapper that maps objects based on configured mappings
    /// </summary>
    public class Mapper : IMapper
    {
        private readonly Dictionary<Type, Dictionary<Type, MappingInfo>> _mappings;

        /// <summary>
        /// Creates a new mapper instance with the specified mappings
        /// </summary>
        /// <param name="mappings">Mapping configuration dictionary</param>
        public Mapper(Dictionary<Type, Dictionary<Type, MappingInfo>> mappings)
        {
            _mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));
        }

        /// <summary>
        /// Maps a source object to a new instance of the destination type
        /// </summary>
        public TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            if (source == null)
                return default!;

            var sourceType = typeof(TSource);
            var destType = typeof(TDestination);

            EnsureMappingExists(sourceType, destType);

            var mappingInfo = _mappings[sourceType][destType];
            var destination = new TDestination();

            return MapInternal(source, destination, mappingInfo);
        }

        /// <summary>
        /// Maps a source object to an existing destination object
        /// </summary>
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null)
                return destination;
            
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            var sourceType = typeof(TSource);
            var destType = typeof(TDestination);

            EnsureMappingExists(sourceType, destType);

            var mappingInfo = _mappings[sourceType][destType];

            return MapInternal(source, destination, mappingInfo);
        }
        
        /// <summary>
        /// Internal method that performs the actual property mapping
        /// </summary>
        private TDestination MapInternal<TSource, TDestination>(
            TSource source, TDestination destination, MappingInfo mappingInfo)
        {
            foreach (var propertyMap in mappingInfo.PropertyMaps)
            {
                try
                {
                    object? value = null;
                    if (propertyMap.CustomValueResolver != null)
                    {
                        value = propertyMap.CustomValueResolver(source!);
                    }
                    else if (propertyMap.SourceProperty != null)
                    {
                        value = propertyMap.SourceProperty.GetValue(source);
                    }

                    if (value != null)
                    {
                        if (propertyMap.DestinationProperty.PropertyType.IsAssignableFrom(value.GetType()))
                        {
                            propertyMap.DestinationProperty.SetValue(destination, value);
                        }
                        else
                        {
                            try
                            {
                                // Perform type conversion
                                var convertedValue = Convert.ChangeType(value, propertyMap.DestinationProperty.PropertyType);
                                propertyMap.DestinationProperty.SetValue(destination, convertedValue);
                            }
                            catch (InvalidCastException ex)
                            {
                                Console.WriteLine($"Failed to convert {value} to {propertyMap.DestinationProperty.PropertyType.Name}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle or log mapping errors
                    Console.WriteLine($"Error mapping property {propertyMap.DestinationProperty.Name}: {ex.Message}");
                }
            }

            return destination;
        }
        
        /// <summary>
        /// Ensures a mapping exists between the source and destination types
        /// </summary>
        private void EnsureMappingExists(Type sourceType, Type destType)
        {
            if (!_mappings.ContainsKey(sourceType) || !_mappings[sourceType].ContainsKey(destType))
            {
                throw new InvalidOperationException(
                    $"No mapping defined from {sourceType.Name} to {destType.Name}. " +
                    $"Use config.CreateMap<{sourceType.Name}, {destType.Name}>() first.");
            }
        }
    }
}

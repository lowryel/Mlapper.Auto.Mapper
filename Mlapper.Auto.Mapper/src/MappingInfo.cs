using System;
using System.Collections.Generic;


namespace Mlapper.Auto.Mapper
{
    /// <summary>
    /// Contains mapping information between a source and destination type
    /// </summary>
    public class MappingInfo
    {
        /// <summary>
        /// The source type for this mapping
        /// </summary>
        public Type SourceType { get; }
        
        /// <summary>
        /// The destination type for this mapping
        /// </summary>
        public Type DestinationType { get; }
        
        /// <summary>
        /// List of property mappings
        /// </summary>
        public List<PropertyMap> PropertyMaps { get; } = new List<PropertyMap>();

        /// <summary>
        /// Creates a new mapping info instance
        /// </summary>
        /// <param name="sourceType">Source type</param>
        /// <param name="destinationType">Destination type</param>
        public MappingInfo(Type sourceType, Type destinationType)
        {
            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
            DestinationType = destinationType ?? throw new ArgumentNullException(nameof(destinationType));
        }
    }
}

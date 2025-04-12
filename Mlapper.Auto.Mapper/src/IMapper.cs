using System;

namespace Mlapper.Auto.Mapper
{
    /// <summary>
    /// Interface for mapping objects from one type to another
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Maps a source object to a new instance of the destination type
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>New instance of destination type with mapped properties</returns>
        TDestination Map<TSource, TDestination>(TSource source) where TDestination : new();
        
        /// <summary>
        /// Maps a source object to an existing destination object
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <returns>The destination object with mapped properties</returns>
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}

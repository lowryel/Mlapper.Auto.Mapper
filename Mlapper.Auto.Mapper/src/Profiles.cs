using System;

namespace Mlapper.Auto.Mapper.src;

/// <summary>
/// Base class for defining mapping profiles
/// </summary>
/// <remarks>
/// Profiles allow for grouping related mappings and configurations
/// </remarks>
public abstract class Profile
{
    /// <summary>
    /// Configures the mappings for this profile
    /// </summary>
    /// <param name="config">The mapper configuration to use for defining mappings</param>
    /// <remarks>
    /// This method should be overridden in derived classes to define specific mappings
    /// </remarks>
    public abstract void Configure(MapperConfiguration config);
}
// public class MapperConfiguration
// {
//     public void CreateMap<TSource, TDestination>()
//     {
//         // Implementation for creating a mapping between source and destination types
//     }

//     public void ForMember<TSource, TDestination>(
//         Func<TDestination, object?> destinationMember,
//         Func<TSource, object?> valueResolver)
//     {
//         // Implementation for configuring a custom mapping for a specific member
//     }
//     public void CreateMap(Type sourceType, Type destinationType)
//     {
//         // Implementation for creating a mapping between source and destination types
//     }

//     public void ForMember(Type destinationType, string destinationMember, Func<object, object?> valueResolver)
//     {
//         // Implementation for configuring a custom mapping for a specific member
//     }
// }



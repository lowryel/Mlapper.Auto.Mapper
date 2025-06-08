using System;
using System.Linq.Expressions;

namespace Mlapper.Auto.Mapper
{
    /// <summary>
    /// Provides a fluent interface for mapping configuration
    /// </summary>
    public class MappingExpression<TSource, TDestination>
    {
        private readonly MapperConfiguration _configuration;

        /// <summary>
        /// Creates a new mapping expression
        /// </summary>
        /// <param name="configuration">The mapper configuration</param>
        public MappingExpression(MapperConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a reverse mapping from the destination to the source type
        /// </summary>
        /// <returns>A mapping expression for the reverse mapping</returns>
        public MappingExpression<TDestination, TSource> ReverseMap()
        {
            _configuration.ReverseMap<TSource, TDestination>();
            return new MappingExpression<TDestination, TSource>(_configuration);
        }

        /// <summary>
        /// Customize the mapping for a specific member
        /// </summary>
        /// <param name="destinationMember">Expression to select the destination member</param>
        /// <param name="valueResolver">Function to resolve the value from the source</param>
        /// <returns>A property map builder for further configuration</returns>
        public PropertyMapBuilder<TSource, TDestination> ForMember(
            Expression<Func<TDestination, object?>> destinationMember,
            Func<TSource, object?> valueResolver)
        {
            return _configuration.ForMember(destinationMember, valueResolver);
        }
    }
}
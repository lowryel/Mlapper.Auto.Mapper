using System;
using Xunit;
using Mlapper.Auto.Mapper;

namespace Mlapper.Auto.Mapper.Tests
{
    public class EdgeCaseTests
    {
        [Fact]
        public void CreateMap_WithNoMatchingProperties_ShouldCreateEmptyMapping()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<SourceWithNoMatches, DestinationWithNoMatches>();
            var mapper = config.CreateMapper();

            var source = new SourceWithNoMatches { SourceProp = "test" };

            // Act
            var destination = mapper.Map<SourceWithNoMatches, DestinationWithNoMatches>(source);

            // Assert
            Assert.NotNull(destination);
            Assert.Null(destination.DestProp); // No mapping should occur
        }

        [Fact]
        public void Map_WithNonExistentMapping_ShouldThrowException()
        {
            // Arrange
            var config = new MapperConfiguration();
            var mapper = config.CreateMapper();

            var source = new SourceClass { Id = 1 };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                mapper.Map<SourceClass, DestinationClass>(source));

            Assert.Contains("No mapping defined", exception.Message);
        }

        [Fact]
        public void Map_WithCircularReferences_ShouldMapFirstLevelOnly()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<NodeWithCircularReference, NodeWithCircularReference>();
            
            // Add custom mapping to prevent circular reference
            config.ForMember<NodeWithCircularReference, NodeWithCircularReference>(
                dest => dest.Child,
                src => src.Child != null ? new NodeWithCircularReference { Name = src.Child.Name } : null
            );
            
            var mapper = config.CreateMapper();

            var source = new NodeWithCircularReference { Name = "Root" };
            source.Child = new NodeWithCircularReference { Name = "Child" };
            source.Child.Parent = source; // Create circular reference

            // Act
            var destination = mapper.Map<NodeWithCircularReference, NodeWithCircularReference>(source);

            // Assert
            Assert.NotNull(destination);
            Assert.Equal("Root", destination.Name);
            Assert.NotNull(destination.Child);
            Assert.Equal("Child", destination.Child.Name);
            Assert.Null(destination.Child.Parent); // Circular reference not mapped
        }

        // Test classes
        public class SourceWithNoMatches
        {
            public string? SourceProp { get; set; }
        }

        public class DestinationWithNoMatches
        {
            public string? DestProp { get; set; }
        }

        public class SourceClass
        {
            public int Id { get; set; }
        }

        public class DestinationClass
        {
            public int Id { get; set; }
        }

        public class NodeWithCircularReference
        {
            public string? Name { get; set; }
            public NodeWithCircularReference? Parent { get; set; }
            public NodeWithCircularReference? Child { get; set; }
        }
    }
}
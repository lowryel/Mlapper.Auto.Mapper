using System;
using Xunit;
using Mlapper.Auto.Mapper;

namespace Mlapper.Auto.Mapper.Tests
{
    public class BasicMappingTests
    {
        [Fact]
        public void Map_SimpleProperties_ShouldMapCorrectly()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<SourceClass, DestinationClass>();
            var mapper = config.CreateMapper();

            var source = new SourceClass
            {
                Id = 1,
                Name = "Test Name",
                Description = "Test Description"
            };

            // Act
            var destination = mapper.Map<SourceClass, DestinationClass>(source);

            // Assert
            Assert.NotNull(destination);
            Assert.Equal(source.Id, destination.Id);
            Assert.Equal(source.Name, destination.Name);
            Assert.Equal(source.Description, destination.Description);
        }

        [Fact]
        public void Map_WithCustomMapping_ShouldUseCustomResolver()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<SourceClass, DestinationClass>();
            config.ForMember<SourceClass, DestinationClass>(
                dest => dest.FullName,
                src => $"{src.Name} - {src.Description}"
            );

            var mapper = config.CreateMapper();

            var source = new SourceClass
            {
                Id = 1,
                Name = "Test",
                Description = "Description"
            };

            // Act
            var destination = mapper.Map<SourceClass, DestinationClass>(source);

            // Assert
            Assert.NotNull(destination);
            Assert.Equal("Test - Description", destination.FullName);
        }

        [Fact]
        public void Map_WithReverseMapping_ShouldMapInBothDirections()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<SourceClass, DestinationClass>();
            config.CreateMap<DestinationClass, SourceClass>(); // Reverse mapping
            
            var mapper = config.CreateMapper();

            var source = new SourceClass
            {
                Id = 42,
                Name = "Original Name",
                Description = "Test Description"
            };

            // Act - Forward mapping
            var destination = mapper.Map<SourceClass, DestinationClass>(source);
            
            // Act - Reverse mapping back to source
            var roundTrip = mapper.Map<DestinationClass, SourceClass>(destination);

            // Assert
            Assert.Equal(source.Id, destination.Id);
            Assert.Equal(source.Name, destination.Name);
            Assert.Equal(source.Description, destination.Description);
            
            Assert.Equal(source.Id, roundTrip.Id);
            Assert.Equal(source.Name, roundTrip.Name);
            Assert.Equal(source.Description, roundTrip.Description);
        }

        [Fact]
        public void Map_ToExistingObject_ShouldUpdateProperties()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<SourceClass, DestinationClass>();
            var mapper = config.CreateMapper();

            var source = new SourceClass { Id = 1, Name = "Test Name" };
            var destination = new DestinationClass { Id = 999, Description = "Existing Description" };

            // Act
            mapper.Map(source, destination);

            // Assert
            Assert.Equal(1, destination.Id); // Should be updated from source
            Assert.Equal("Test Name", destination.Name); // Should be updated from source
            Assert.Equal("Existing Description", destination.Description); // Should remain unchanged as source.Description is null
        }

        [Fact]
        public void Map_WithNullSource_ShouldReturnDefaultOrExistingDestination()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<SourceClass, DestinationClass>();
            var mapper = config.CreateMapper();

            SourceClass? source = null;
            var existingDestination = new DestinationClass { Id = 999 };

            // Act
            var newDestination = mapper.Map<SourceClass, DestinationClass>(source!);
            var updatedDestination = mapper.Map(source, existingDestination);

            // Assert
            Assert.Null(newDestination); // Should return default (null) for new destination
            Assert.Same(existingDestination, updatedDestination); // Should return existing destination unchanged
        }

        // Test classes
        public class SourceClass
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
        }

        public class DestinationClass
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? FullName { get; set; }
        }
    }
}
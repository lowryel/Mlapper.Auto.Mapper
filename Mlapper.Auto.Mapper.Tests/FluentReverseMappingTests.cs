using System;
using Xunit;
using Mlapper.Auto.Mapper;

namespace Mlapper.Auto.Mapper.Tests
{
    public class FluentReverseMappingTests
    {
        [Fact]
        public void CreateMap_WithFluentReverseMap_ShouldMapBothDirections()
        {
            // Arrange
            var config = new MapperConfiguration();
            
            // Create mapping with fluent reverse mapping
            var mapExpr = config.CreateMap<SourceClass, DestinationClass>();
            mapExpr.ReverseMap();
            
            var mapper = config.CreateMapper();

            var source = new SourceClass
            {
                Id = 42,
                Name = "Source Name",
                Description = "Source Description"
            };

            // Act - Forward mapping
            var destination = mapper.Map<SourceClass, DestinationClass>(source);
            
            // Act - Reverse mapping
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
        public void CreateMap_WithFluentReverseMapAndCustomMappings_ShouldRespectCustomMappings()
        {
            // Arrange
            var config = new MapperConfiguration();
            
            // Create forward mapping with custom mappings
            var mapExpr = config.CreateMap<SourceClass, DestinationClass>();
            mapExpr.ForMember(dest => dest.FullName, src => $"{src.Name} {src.Description}");
            
            // Create reverse mapping with custom mappings
            var reverseMapExpr = mapExpr.ReverseMap();
            config.ForMember<DestinationClass, SourceClass>(
                dest => dest.Name, 
                src => src.FullName?.Split(' ')[0] ?? string.Empty
            );
            config.ForMember<DestinationClass, SourceClass>(
                dest => dest.Description, 
                src => src.FullName?.Split(' ').Length > 1 ? src.FullName.Split(' ')[1] : string.Empty
            );
            
            var mapper = config.CreateMapper();

            var source = new SourceClass
            {
                Id = 42,
                Name = "John",
                Description = "Doe"
            };

            // Act - Forward mapping
            var destination = mapper.Map<SourceClass, DestinationClass>(source);
            
            // Act - Reverse mapping
            var roundTrip = mapper.Map<DestinationClass, SourceClass>(destination);

            // Assert
            Assert.Equal("John Doe", destination.FullName);
            Assert.Equal("John", roundTrip.Name);
            Assert.Equal("Doe", roundTrip.Description);
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
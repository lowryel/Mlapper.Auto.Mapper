using System;
using Xunit;
using Mlapper.Auto.Mapper;

namespace Mlapper.Auto.Mapper.Tests
{
    public class TypeConversionTests
    {
        [Fact]
        public void Map_WithCompatibleTypes_ShouldConvertAutomatically()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<SourceWithTypes, DestinationWithTypes>();
            
            // Add explicit mappings for type conversions
            config.ForMember<SourceWithTypes, DestinationWithTypes>(
                dest => dest.LongValue,
                src => (long)src.IntValue
            );
            
            config.ForMember<SourceWithTypes, DestinationWithTypes>(
                dest => dest.IntFromString,
                src => int.Parse(src.StringValue ?? "0")
            );
            
            config.ForMember<SourceWithTypes, DestinationWithTypes>(
                dest => dest.FloatValue,
                src => (float)src.DoubleValue
            );
            
            var mapper = config.CreateMapper();

            var source = new SourceWithTypes
            {
                IntValue = 42,
                StringValue = "100",
                DoubleValue = 3.14
            };

            // Act
            var destination = mapper.Map<SourceWithTypes, DestinationWithTypes>(source);

            // Assert
            Assert.NotNull(destination);
            Assert.Equal(42L, destination.LongValue); // int -> long
            Assert.Equal(100, destination.IntFromString); // string -> int
            Assert.Equal(3.14f, destination.FloatValue); // double -> float
        }

        [Fact]
        public void Map_WithIncompatibleTypes_ShouldHandleGracefully()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<SourceWithTypes, DestinationWithTypes>();
            var mapper = config.CreateMapper();

            var source = new SourceWithTypes
            {
                StringValue = "not a number"
            };

            // Act
            var destination = mapper.Map<SourceWithTypes, DestinationWithTypes>(source);

            // Assert
            Assert.NotNull(destination);
            Assert.Equal(0, destination.IntFromString); // Failed conversion should result in default value
        }

        [Fact]
        public void Map_WithCustomTypeConversion_ShouldUseCustomResolver()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.CreateMap<SourceWithTypes, DestinationWithTypes>();
            config.ForMember<SourceWithTypes, DestinationWithTypes>(
                dest => dest.DateValue,
                src => DateTime.Parse(src.StringDate ?? "2000-01-01")
            );

            var mapper = config.CreateMapper();

            var source = new SourceWithTypes
            {
                StringDate = "2023-12-25"
            };

            // Act
            var destination = mapper.Map<SourceWithTypes, DestinationWithTypes>(source);

            // Assert
            Assert.NotNull(destination);
            Assert.Equal(new DateTime(2023, 12, 25), destination.DateValue);
        }

        // Test classes
        public class SourceWithTypes
        {
            public int IntValue { get; set; }
            public string? StringValue { get; set; }
            public double DoubleValue { get; set; }
            public string? StringDate { get; set; }
        }

        public class DestinationWithTypes
        {
            public long LongValue { get; set; } // Maps from IntValue
            public int IntFromString { get; set; } // Maps from StringValue
            public float FloatValue { get; set; } // Maps from DoubleValue
            public DateTime DateValue { get; set; } // Maps from StringDate with custom conversion
        }
    }
}
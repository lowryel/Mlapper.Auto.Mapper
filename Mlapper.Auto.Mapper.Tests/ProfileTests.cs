using System;
using Xunit;
using Mlapper.Auto.Mapper;
using Mlapper.Auto.Mapper.src;

namespace Mlapper.Auto.Mapper.Tests
{
    public class ProfileTests
    {
        [Fact]
        public void AddProfile_ShouldConfigureMappings()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.AddProfile(new TestProfile());
            var mapper = config.CreateMapper();

            var source = new ProfileSourceClass
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 30
            };

            // Act
            var destination = mapper.Map<ProfileSourceClass, ProfileDestinationClass>(source);

            // Assert
            Assert.NotNull(destination);
            Assert.Equal("John", destination.FirstName);
            Assert.Equal("Doe", destination.LastName);
            Assert.Equal("John Doe", destination.FullName);
            Assert.Equal(30, destination.Age);
        }

        // Test profile implementation
        private class TestProfile : Profile
        {
            public override void Configure(MapperConfiguration config)
            {
                config.CreateMap<ProfileSourceClass, ProfileDestinationClass>();
                config.ForMember<ProfileSourceClass, ProfileDestinationClass>(
                    dest => dest.FullName,
                    src => $"{src.FirstName} {src.LastName}"
                ).Condition(
                    (src) => !string.IsNullOrEmpty(src.FirstName) && !string.IsNullOrEmpty(src.LastName)
                );
            }
        }

        // Test classes
        public class ProfileSourceClass
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public int Age { get; set; }
        }

        public class ProfileDestinationClass
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? FullName { get; set; }
            public int Age { get; set; }
        }
    }
}
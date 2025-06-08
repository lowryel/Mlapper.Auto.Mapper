using System;
using Xunit;
using Mlapper.Auto.Mapper;
using Mlapper.Auto.Mapper.src;

namespace Mlapper.Auto.Mapper.Tests
{
    public class ProfileWithFluentTests
    {
        [Fact]
        public void AddProfile_WithFluentReverseMap_ShouldConfigureBidirectionalMappings()
        {
            // Arrange
            var config = new MapperConfiguration();
            config.AddProfile(new FluentReverseMapProfile());
            var mapper = config.CreateMapper();

            var source = new CustomerDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            // Act - Forward mapping
            var destination = mapper.Map<CustomerDto, CustomerEntity>(source);
            
            // Act - Reverse mapping
            var roundTrip = mapper.Map<CustomerEntity, CustomerDto>(destination);

            // Assert
            Assert.Equal(source.Id, destination.Id);
            Assert.Equal(source.FirstName, destination.FirstName);
            Assert.Equal(source.LastName, destination.LastName);
            Assert.Equal(source.Email, destination.Email);
            Assert.Equal($"{source.FirstName} {source.LastName}", destination.FullName);
            
            Assert.Equal(source.Id, roundTrip.Id);
            Assert.Equal(source.FirstName, roundTrip.FirstName);
            Assert.Equal(source.LastName, roundTrip.LastName);
            Assert.Equal(source.Email, roundTrip.Email);
        }

        // Test profile with fluent reverse mapping
        private class FluentReverseMapProfile : Profile
        {
            public override void Configure(MapperConfiguration config)
            {
                config.CreateMap<CustomerDto, CustomerEntity>()
                    .ForMember(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
                
                config.CreateMap<CustomerEntity, CustomerDto>();
            }
        }

        // Test classes
        public class CustomerDto
        {
            public int Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
        }

        public class CustomerEntity
        {
            public int Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? FullName { get; set; }
        }
    }
}
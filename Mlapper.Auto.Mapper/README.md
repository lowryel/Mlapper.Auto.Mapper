# Mlapper-Mapper

A lightweight object-to-object mapping library for .NET applications.

## Features

- Simple configuration API
- Automatic property discovery and mapping
- Custom property mapping with lambda expressions
- Type conversion support
- Fluent configuration interface
- Reverse mapping support (fluent chaining)
- Conditional and profile mapping

## Installation

```bash
dotnet add package Mlapper-Mapper
```

## Basic Usage

```csharp
// Set up your mapping configuration
var config = new MapperConfiguration();

// Create map with automatic property discovery
config.CreateMap<CustomerDto, Customer>();

// Add custom property mapping
config.ForMember<CustomerDto, Customer>(
    dest => dest.FullName,
    src => src.FirstName + " " + src.LastName
);

// Create mapper
var mapper = config.CreateMapper();

// Map objects
var customerDto = new CustomerDto { FirstName = "John", LastName = "Doe", Age = 30 };
var customer = mapper.Map<CustomerDto, Customer>(customerDto);

// Map to existing object
var existingCustomer = new Customer();
mapper.Map(customerDto, existingCustomer);
```

## For your web projects, add it as a service to your Program file

```csharp
builder.Services.AddSingleton(sp =>
{
    var config = new MapperConfiguration();
    config.CreateMap<ProductDto, Product>();

    return config.CreateMapper();
});
```

## Advanced Usage

### Null Handling

The library handles null values appropriately:

```csharp
// Returns default(Customer) (null for reference types)
var result = mapper.Map<CustomerDto, Customer>(null);

// Returns the existing destination object
var keptObject = mapper.Map(null, existingCustomer);
```

### Type Conversions

Automatic type conversions are supported for compatible types:

- Numeric conversions (int → long, float → double)
- String to numeric conversions
- Numeric to string conversions
- Any assignment-compatible types

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details

### Added MappingExpression Class

---------------------------

Created a new MappingExpression<TSource, TDestination> class that enables fluent API chaining

Implemented the ReverseMap() method that creates a reverse mapping and returns a new expression for the reverse direction

Added support for ForMember() to configure custom mappings

Created Comprehensive Test Cases:

FluentReverseMappingTests.cs: Tests the fluent API for reverse mapping

Basic bidirectional mapping with ReverseMap()

Custom mappings in both directions

Added Profile Support:

ProfileWithFluentTests.cs: Shows how to use fluent reverse mapping in profiles

Demonstrates creating mappings with custom property configurations

Helper Classes:

Added ReverseConfiguration.cs with utility methods for reverse mapping

The tests now pass successfully, showing that the fluent reverse mapping functionality works correctly. This implementation follows AutoMapper's pattern where you can chain method calls:

```csharp
    config.CreateMap<SourceClass, DestinationClass>()
        .ForMember(dest => dest.FullName, src => $"{src.Name} {src.Description}")
        .ReverseMap();
```

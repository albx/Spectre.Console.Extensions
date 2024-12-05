# Spectre.Console Extensions

This project contains some extensions for the Spectre.Console CLI project.
It adds a CommandAppBuilder which enables features like Dependency injection.

## Requirements

The project supports .NET Standard 2.0, .NET 8 and .NET 9.


## Installation

This project is available on [NuGet](https://www.nuget.org/packages/SpectreConsole.Extensions/).

It can be installed using the ```dotnet add package``` command or the NuGet wizard on your favourite IDE.

```bash
  dotnet add package SpectreConsole.Extensions
```

## Features

By default the `CommandAppBuilder` allows you to register types in the .NET Dependency injection and manage the configuration based on `appsettings.json` file and user secrets.

It expose also an `Environment` property which allows you to perform action based on the standard .NET Environment values. This values are specified via the `DOTNET_ENVIRONMENT` environment variable.
    
## Usage

In your `Program.cs`, you can create a `CommandAppBuilder` class and then register your services or manage your configuration:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Spectre.Console.Extensions;

var builder = CommandAppBuilder.Create();

// Register your services with the Services property. For example:
builder.Services.AddSingleton<IMyService, MyService>();

// Configure types reading data from appsettings.json or user secrets
builder.Services.Configure<MyOption>(builder.Configuration.GetSection("MyOption"));

// You can perform actions based on the Environment
if (builder.Environment.IsDevelopment())
{
    // Register types based on the current environment
    builder.Services.AddSingleton<IDevelopmentService, DevelopmentService>();
}

// Create the CommandApp instance
var app = builder.Build();
app.Configure(config =>
{
    // Your app configuration
    config.AddCommand<MyCommand>("mycommand");
});

// Run the application
return app.Run(args);
```

### Explanation

- **Register Services**: Use `builder.Services` to register your services with the dependency injection container.
- **Configure Options**: Use `builder.Configuration` to configure options from `appsettings.json` or user secrets.
- **Environment-based Registration**: Use `builder.Environment` to register services conditionally based on the environment.
- **Build and Configure CommandApp**: Create and configure the `CommandApp` instance using the `Build` method and `Configure` method.

### MyOption Class

Ensure you have a class `MyOption` defined to hold configuration values:

```csharp
public class MyOption
{
    public string Value { get; set; } = string.Empty;
}
```

### Sample Command

Define a sample command to be used in the `CommandApp`:

```csharp
internal class MyCommand : Command<MyCommand.CommandSettings>
{
    private readonly IMyService _service;
    private readonly MyOption _option;

    public MyCommand(IMyService service, IOptions<MyOption> options)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _option = options.Value;
    }

    public override int Execute(CommandContext context, CommandSettings settings)
    {
        AnsiConsole.MarkupLine($"Service: {_service.SayHello()}");
        AnsiConsole.MarkupLine($"Option: {_option.Value}");
        return 0;
    }

    internal class CommandSettings : CommandSettings
    {
        [CommandOption("-n|--name")]
        public string Name { get; set; } = string.Empty;
    }
}
```
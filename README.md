# Spectre.Console Extensions

This project contains some extensions for the Spectre.Console CLI project.
It adds a CommandAppBuilder which enables features like Dependency injection.


## Installation

This project is available on [NuGet](https://www.nuget.org/packages/SpectreConsole.Extensions/).

It can be installed using the ```dotnet add package``` command or the NuGet wizard on your favourite IDE.

```bash
  dotnet add package SpectreConsole.Extensions
```
    
## Usage

In your ```Program.cs``` you can create a ```CommandAppBuilder``` class and then register your services:

```csharp
using Spectre.Console.Extensions;

var builder = CommandAppBuilder.Create();

//Register your services with the Services property. i.e.
builder.Services.AddSingleton(....);

//Create the CommandApp instance
var app = builder.Build();
app.Configure(config => { /* your app configuration */ });

//Create the CommandApp instance with its default command
var app = builder.Build<DefaultCommand>();
app.Configure(config => { /* your app configuration */ });

return app.Run(args);
```

It's possibile to specify the app configration in the overloads of the Build and Build&lt;TDefaultCommandMethod&gt;:
```csharp
// ...

//Create the CommandApp instance
var app = builder.Build(config => { /* your app configuration */ });

//Create the CommandApp instance with its default command
var app = builder.Build<DefaultCommand>(config => { /* your app configuration */ });

// ...
```

## Samples

Samples are located in the [samples](./samples) folder.

## Contributing

Contributions are always welcome!

If you want to submit any issues or new features, please [follow these few guidelines](CONTRIBUTING.md).
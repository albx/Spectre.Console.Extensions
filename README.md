# Spectre.Console Extensions

This project contains some extensions for the Spectre.Console CLI project.
It adds a CommandAppBuilder which enables features like Dependency injection.


## Installation

This project is available on NuGet.

It can be installed using the ```dotnet add package``` command or the NuGet wizard on your favourite IDE.

```bash
  dotnet add package Spectre.Console.Extensions
```
    
## Usage/Examples

In your ```Program.cs``` you can create a ```CommandAppBuilder``` class and then register your services:

```csharp
using Spectre.Console.Extensions;

var builder = CommandAppBuilder.Create();

//Register your services with the Services property. i.e.
builder.Services.AddSingleton(....);

var app = builder.Build();
app.Configure(config => { /* your app configuration */ });

return app.Run(args);
```
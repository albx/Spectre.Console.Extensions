using Microsoft.Extensions.DependencyInjection;
using Sample.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Extensions;
using System.Diagnostics.CodeAnalysis;

var builder = CommandAppBuilder.Create();

builder.Services.AddSingleton<IMyService, MyService>();

try
{
    var app = builder.Build<MyCommand>();
    app.Configure(config =>
    {
        config.PropagateExceptions();
    });

    //var app = builder.Build<MyCommand>(config =>
    //{
    //    config.PropagateExceptions();
    //});

    return app.Run(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    return -99;
}


internal class MyCommand : Command<MyCommand.CommandSettings>
{
    public MyCommand(IMyService service)
    {
        Service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public IMyService Service { get; }

    public override int Execute([NotNull] CommandContext context, [NotNull] CommandSettings settings)
    {
        try
        {
            AnsiConsole.MarkupLine($"Argument: {settings.Name}");
            AnsiConsole.MarkupLine($"Service: {Service.SayHello()}");

            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
            return -1;
        }
    }

    internal class CommandSettings : Spectre.Console.Cli.CommandSettings
    {
        [CommandOption("-n|--name")]
        public string Name { get; set; } = string.Empty;
    }
}
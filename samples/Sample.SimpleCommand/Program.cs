﻿using Microsoft.Extensions.DependencyInjection;
using Sample.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Extensions;
using System.Diagnostics.CodeAnalysis;

var builder = CommandAppBuilder.Create();

builder.Services.AddSingleton<IMyService, MyService>();

try
{
    var app = builder.Build();
    app.Configure(config =>
    {
        config.AddCommand<MyCommand>("sample");
    });

    //var app = builder.Build(config =>
    //{
    //    config.AddCommand<MyCommand>("sample");
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
        [CommandArgument(0, "<NAME>")]
        public string Name { get; set; } = string.Empty;
    }
}
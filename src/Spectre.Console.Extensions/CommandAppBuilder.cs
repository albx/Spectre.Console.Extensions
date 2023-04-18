using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Spectre.Console.Cli;

namespace Spectre.Console.Extensions
{
    public class CommandAppBuilder
    {
        #region Constructor
        private CommandAppBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Configuration = new ConfigurationManager();
        }
        #endregion

        public IServiceCollection Services { get; }

        public IConfiguration Configuration { get; }

        public static CommandAppBuilder Create()
        {
            var services = new ServiceCollection();
            return new CommandAppBuilder(services);
        }

        public CommandApp Build()
        {
            var app = new CommandApp();
            return app;
        }

        public CommandApp<TDefaultCommand> Build<TDefaultCommand>()
            where TDefaultCommand : class, ICommand
        {
            var app = new CommandApp<TDefaultCommand>();
            return app;
        }
    }
}
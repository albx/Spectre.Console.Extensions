using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using Spectre.Console.Extensions.DependencyInjection;
using Spectre.Console.Extensions.Internals;
using System;

namespace Spectre.Console.Extensions
{
    /// <summary>
    /// Defines an entry point to build a <see cref="CommandApp"/> instance
    /// </summary>
    public sealed class CommandAppBuilder
    {
        private IServiceProviderFactory<IServiceCollection> serviceProviderFactory;

        #region Constructor
        private CommandAppBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            serviceProviderFactory = new DefaultServiceProviderFactory();

            ConfigurationHelper.ApplyDefaultConfiguration(Configuration);
            Environment = ConfigurationHelper.BuildEnvironment(Configuration);
            ConfigurationHelper.ApplyDefaultAppConfiguration(Configuration, Environment);

            Services.AddSingleton<IConfiguration>(Configuration);
        }
        #endregion

        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> instance
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Gets the <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> instance
        /// </summary>
        public ConfigurationManager Configuration { get; } = new ConfigurationManager();

        /// <summary>
        /// Gets the <see cref="IHostEnvironment"/> instance
        /// </summary>
        public IHostEnvironment Environment { get; }

        /// <summary>
        /// Creates a new <see cref="CommandAppBuilder"/> instance
        /// </summary>
        /// <returns>The builder instance</returns>
        public static CommandAppBuilder Create()
        {
            var services = new ServiceCollection();
            return new CommandAppBuilder(services);
        }

        /// <summary>
        /// Set the specified <see cref="IServiceProviderFactory{TContainerBuilder}"/> instance
        /// </summary>
        /// <returns>The builder instance</returns>
        public CommandAppBuilder UseServiceProviderFactory(IServiceProviderFactory<IServiceCollection> serviceProviderFactory)
        {
            this.serviceProviderFactory = serviceProviderFactory ?? throw new ArgumentNullException(nameof(serviceProviderFactory));
            return this;
        }

        /// <summary>
        /// Creates the <see cref="CommandApp"/> instance
        /// </summary>
        /// <returns>The <see cref="CommandApp"/> instance</returns>
        public CommandApp Build()
        {
            var registrar = BuildTypeRegistrar();

            var app = new CommandApp(registrar);
            return app;
        }

        /// <summary>
        /// Creates the <see cref="CommandApp"/> instance and applies the specified configuration
        /// </summary>
        /// <param name="configuration">The configuration of the app</param>
        /// <returns>The <see cref="CommandApp"/> instance</returns>
        /// <exception cref="ArgumentNullException">Thrown when the configuration is null</exception>
        public CommandApp Build(Action<IConfigurator> configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var app = Build();
            app.Configure(configuration);

            return app;
        }

        /// <summary>
        /// Creates the <see cref="CommandApp{TDefaultCommand}"/> instance
        /// </summary>
        /// <typeparam name="TDefaultCommand">The default command type</typeparam>
        /// <returns>The <see cref="CommandApp{TDefaultCommand}"/> instance</returns>
        public CommandApp<TDefaultCommand> Build<TDefaultCommand>()
            where TDefaultCommand : class, ICommand
        {
            var registrar = BuildTypeRegistrar();

            var app = new CommandApp<TDefaultCommand>(registrar);
            return app;
        }

        /// <summary>
        /// Creates the <see cref="CommandApp{TDefaultCommand}"/> instance and applies the specified configuration
        /// </summary>
        /// <typeparam name="TDefaultCommand">The default command type</typeparam>
        /// <param name="configuration">The configuration of the app</param>
        /// <returns>The <see cref="CommandApp{TDefaultCommand}"/> instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public CommandApp<TDefaultCommand> Build<TDefaultCommand>(Action<IConfigurator> configuration)
            where TDefaultCommand : class, ICommand
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var app = Build<TDefaultCommand>();
            app.Configure(configuration);

            return app;
        }

        #region Private method
        private ServiceCollectionTypeRegistrar BuildTypeRegistrar()
        {
            Services.AddSingleton(serviceProviderFactory.CreateServiceProvider(Services));
            var registrar = new ServiceCollectionTypeRegistrar(Services, serviceProviderFactory);

            return registrar;
        }
        #endregion
    }
}
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using System;

namespace Spectre.Console.Extensions.DependencyInjection
{
    /// <summary>
    /// Implements <see cref="ITypeRegistrar"/> to enable Dependency Injection
    /// </summary>
    public class ServiceCollectionTypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection services;

        /// <summary>
        /// Constructs the object
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance</param>
        /// <exception cref="ArgumentNullException">Thrown if service collection instance is null</exception>
        public ServiceCollectionTypeRegistrar(IServiceCollection services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Builds the <see cref="ITypeResolver"/> instance
        /// </summary>
        /// <returns>The created <see cref="ITypeResolver"/> instance</returns>
        public virtual ITypeResolver Build() => new ServiceProviderTypeResolver(services.BuildServiceProvider());

        /// <summary>
        /// Registers the specified service's type to the implementation's type
        /// </summary>
        /// <param name="service">The service type</param>
        /// <param name="implementation">The implementation type</param>
        public void Register(Type service, Type implementation)
        {
            services.AddSingleton(service, implementation);
        }

        /// <summary>
        /// Registers the specified service's type using the implementation passed as second argument
        /// </summary>
        /// <param name="service">The service type</param>
        /// <param name="implementation">The service implementation instance</param>
        public void RegisterInstance(Type service, object implementation)
        {
            services.AddSingleton(service, implementation);
        }

        /// <summary>
        /// Registers the specified service's type using the factory method passed
        /// </summary>
        /// <param name="service">The service type</param>
        /// <param name="factory">The factory method used to get the service instance</param>
        public void RegisterLazy(Type service, Func<object> factory)
        {
            services.AddSingleton(service, sp => factory.Invoke());
        }
    }
}

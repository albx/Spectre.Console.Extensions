using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using System;

namespace Spectre.Console.Extensions.DependencyInjection
{
    /// <summary>
    /// Implements <see cref="ITypeRegistrar"/> to enable Dependency Injection
    /// </summary>
    internal sealed class ServiceCollectionTypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection services;

        private readonly IServiceProviderFactory<IServiceCollection> serviceProviderFactory;

        /// <summary>
        /// Constructs the object
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance</param>
        /// <param name="serviceProviderFactory">The <see cref="IServiceProviderFactory{TContainerBuilder}"/> instance</param>
        /// <exception cref="ArgumentNullException">Thrown if service collection instance is null</exception>
        public ServiceCollectionTypeRegistrar(IServiceCollection services, IServiceProviderFactory<IServiceCollection> serviceProviderFactory)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.serviceProviderFactory = serviceProviderFactory ?? throw new ArgumentNullException(nameof(serviceProviderFactory));
        }

        /// <summary>
        /// Builds the <see cref="ITypeResolver"/> instance
        /// </summary>
        /// <returns>The created <see cref="ITypeResolver"/> instance</returns>
        public ITypeResolver Build()
        {
            var serviceProvider = serviceProviderFactory.CreateServiceProvider(services);
            return new ServiceProviderTypeResolver(serviceProvider);
        }

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

using Spectre.Console.Cli;
using System;

namespace Spectre.Console.Extensions.DependencyInjection
{
    /// <summary>
    /// Implements the <see cref="ITypeResolver"/> interface using <see cref="IServiceProvider"/>
    /// </summary>
    internal sealed class ServiceProviderTypeResolver : ITypeResolver
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Constructs the object
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> instance</param>
        /// <exception cref="ArgumentNullException">Thrown when the service provider is null</exception>
        public ServiceProviderTypeResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Resolves the specified type
        /// </summary>
        /// <param name="type">The type to resolve</param>
        /// <returns>The instance of the resolved type</returns>
        public object Resolve(Type type) => serviceProvider.GetService(type);
    }
}

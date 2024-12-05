using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Spectre.Console.Extensions.Internals
{
    internal static class ConfigurationHelper
    {
        internal static void ApplyDefaultConfiguration(IConfigurationBuilder configuration)
        {
            SetDefaultContentRootPath(configuration);

            configuration.AddEnvironmentVariables("DOTNET_");
        }

        internal static void ApplyDefaultAppConfiguration(IConfigurationBuilder configuration, IHostEnvironment hostEnvironment)
        {
            configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configuration.AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (hostEnvironment.IsDevelopment() && !string.IsNullOrWhiteSpace(hostEnvironment.ApplicationName))
            {
                try
                {
                    var applicationAssembly = Assembly.Load(new AssemblyName(hostEnvironment.ApplicationName));
                    configuration.AddUserSecrets(applicationAssembly);
                }
                catch (FileNotFoundException)
                {
                }
            }

            configuration.AddEnvironmentVariables();
        }

        private static void SetDefaultContentRootPath(IConfigurationBuilder configuration)
        {
            string cwd = Environment.CurrentDirectory;
            if (!string.Equals(Environment.SystemDirectory, cwd, StringComparison.OrdinalIgnoreCase))
            {
                configuration.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(HostDefaults.ContentRootKey, cwd)
                });
            }
        }

        internal static IHostEnvironment BuildEnvironment(IConfiguration configuration)
        {
            var environment = new CommandAppEnvironment
            {
                EnvironmentName = configuration[HostDefaults.EnvironmentKey] ?? Environments.Production,
                ContentRootPath = ResolveContentRootPath(configuration[HostDefaults.ContentRootKey], AppContext.BaseDirectory)
            };

            string applicationName = configuration[HostDefaults.ApplicationKey];
            if (string.IsNullOrEmpty(applicationName))
            {
                applicationName = Assembly.GetEntryAssembly()?.GetName().Name;
            }

            if (applicationName != null)
            {
                environment.ApplicationName = applicationName;
            }

            var physicalFileProvider = new PhysicalFileProvider(environment.ContentRootPath);
            environment.ContentRootFileProvider = physicalFileProvider;

            return environment;
        }

        private static string ResolveContentRootPath(string contentRootPath, string basePath)
        {
            if (string.IsNullOrEmpty(contentRootPath))
            {
                return basePath;
            }

            if (Path.IsPathRooted(contentRootPath))
            {
                return contentRootPath;
            }

            return Path.Combine(basePath, contentRootPath);
        }
    }
}

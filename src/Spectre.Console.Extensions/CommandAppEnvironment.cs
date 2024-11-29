using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Spectre.Console.Extensions
{
    internal class CommandAppEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}

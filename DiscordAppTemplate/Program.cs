using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Common.Configuration;
using Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace DiscordAppTemplate
{
    public class Program
    {
        private static string SettingsFilePath;
        private static string ConfigFilePath;

        [SuppressMessage("Style", "IDE1006")]
        public static async Task Main(string[] args)
        {
            var host = CreateHost(args);
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            if (!File.Exists(ConfigFilePath) && !await TryCreateAppConfigAsync())
            {
                logger.LogCritical($"Could not read or create app config file at path {ConfigFilePath}");
                host.Dispose();
                return;
            }

            if (!await host.Services.ValidateOptionsAsync(Assembly.GetExecutingAssembly()))
            {
                logger.LogCritical("Config File Validation Failed!");
                host.Dispose();
                return;
            }

            await host.Services.InitializeApplicationServicesAsync(Assembly.GetExecutingAssembly());
            host.Services.RunApplicationServices(Assembly.GetExecutingAssembly());

            await host.StartAsync();
            await host.WaitForShutdownAsync();
        }

        public static IHost CreateHost(string[] args)
            => new HostBuilder()
                .ConfigureHostConfiguration(configBuilder =>
                {
                    configBuilder.AddCommandLine(args);
                    configBuilder.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    SettingsFilePath = "appsettings.json";
                    ConfigFilePath = $"{context.HostingEnvironment.EnvironmentName}.config.yaml";

                    configBuilder.AddJsonFile(SettingsFilePath, false);
                    configBuilder.AddYamlFile(ConfigFilePath, true);
                })
                .ConfigureServices((context, servicesBuilder) =>
                {
                    servicesBuilder.AddApplication(context.Configuration, Assembly.GetExecutingAssembly());
                })
                .ConfigureLogging((context, loggerBuilder) =>
                {
                    loggerBuilder.AddConsole();
                    loggerBuilder.AddConfiguration(context.Configuration);
                })
                .Build();

        public static async Task<bool> TryCreateAppConfigAsync()
        {
            try
            {
                var serializer = new SerializerBuilder()
                    .EmitDefaults()
                    .Build();

                var defaultConfiguration = Option.GenerateDefaultOptions();
                string yamlConfiguration = serializer.Serialize(defaultConfiguration);
                await File.WriteAllTextAsync(ConfigFilePath, yamlConfiguration);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

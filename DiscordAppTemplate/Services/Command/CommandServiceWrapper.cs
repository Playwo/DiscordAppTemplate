using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Common.Services;
using DiscordAppTemplate.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace DiscordAppTemplate.Services
{
    public sealed class CommandServiceWrapper : ServiceWrapper
    {
        public CommandService CommandService;

        public CommandServiceWrapper()
        {
            var configuration = new CommandServiceConfiguration()
            {
                DefaultRunMode = RunMode.Parallel,
                StringComparison = StringComparison.OrdinalIgnoreCase,
                IgnoresExtraArguments = false,
            };

            CommandService = new CommandService(configuration);
        }

        protected override void Register(IServiceCollection services)
            => services.AddSingleton<ICommandService>(CommandService);

        protected override ValueTask InitializeAsync()
        {
            CommandService.AddModules(Assembly.GetExecutingAssembly());
            AddTypeParsers();
            return default;
        }

        private void AddTypeParsers()
        {
            var addMethod = typeof(CommandService).GetMethod(nameof(CommandService.AddTypeParser));

            var parserTypes = Assembly.GetExecutingAssembly().GetParserTypes();

            foreach (var parserType in parserTypes)
            {
                var parserValueType = parserType.BaseType.GenericTypeArguments.First();

                addMethod.MakeGenericMethod(parserValueType)
                    .Invoke(CommandService, new[] { Activator.CreateInstance(parserType), parserValueType.IsPrimitive });
            }
        }
    }
}

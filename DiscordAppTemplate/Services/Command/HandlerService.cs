using System.Threading.Tasks;
using Common.Services;
using Discord.WebSocket;
using DiscordAppTemplate.Commands;
using DiscordAppTemplate.Options;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace DiscordAppTemplate.Services
{
    public sealed class HandlerService : Service
    {
        [Inject] private readonly DiscordShardedClient DiscordClient;
        [Inject] private readonly ICommandService CommandService;
        [Inject] private readonly CommandOptions CommandOptions;

        protected override ValueTask InitializeAsync()
        {
            DiscordClient.MessageReceived += HandleMessageReceivedAsync;
            CommandService.CommandExecuted += HandleCommandExecutedAsync;
            CommandService.CommandExecutionFailed += HandleCommandExecutionFailedAsync;
            return default;
        }

        private Task HandleCommandExecutedAsync(CommandExecutedEventArgs executedEventArgs)
        {
            //ToDo: Do post processing like releasing ressources
            Logger.LogWarning("Command executed successfully!");
            return Task.CompletedTask;
        }

        private Task HandleCommandExecutionFailedAsync(CommandExecutionFailedEventArgs failedEventArgs)
        {
            //ToDo: Do post processing like releasing ressources
            //ToDo: Handle the error properly
            Logger.LogWarning(failedEventArgs.Result.Exception, "An error occured while executing a command!");
            return Task.CompletedTask;
        }

        private async Task HandleMessageReceivedAsync(SocketMessage message)
        {
            //Ignore own messages
            if (message.Author == DiscordClient.CurrentUser)
            {
                return;
            }

            //Ignore messages without prefix & cut the prefix from messages that do have it
            if (!CommandUtilities.HasPrefix(message.Content, CommandOptions.Prefix, out string rawCommand))
            {
                return;
            }

            var context = new AppContext(DiscordClient, message, Provider);
            var result = await CommandService.ExecuteAsync(rawCommand, context);

            //Exit if everything was succesful so far
            if (result is SuccessfulResult)
            {
                return;
            }

            //Handle the error otherwise
            var failedResult = result as FailedResult;

            //ToDo: Implement proper Error Handling!
            await message.Channel.SendMessageAsync(failedResult.Reason);
        }
    }
}

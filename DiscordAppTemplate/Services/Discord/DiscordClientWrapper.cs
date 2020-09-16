using System.Threading.Tasks;
using Common.Services;
using Discord;
using Discord.WebSocket;
using DiscordAppTemplate.Extensions;
using DiscordAppTemplate.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DiscordAppTemplate.Services.Discord
{
    public sealed class DiscordClientWrapper : ServiceWrapper
    {
        [Inject] private readonly IConfiguration Configuration;
        [Inject] private readonly DiscordOptions DiscordOptions;

        public DiscordShardedClient DiscordClient;
        private ILogger DiscordLogger;

        public DiscordClientWrapper()
        {
            var discordConfig = new DiscordSocketConfig()
            {
                AlwaysDownloadUsers = true,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                ExclusiveBulkDelete = false,
                GuildSubscriptions = false,
                MessageCacheSize = 0,
                RateLimitPrecision = RateLimitPrecision.Millisecond
            };

            DiscordClient = new DiscordShardedClient(discordConfig);
        }

        protected override void Register(IServiceCollection services)
            => services.AddSingleton(DiscordClient);

        protected override async ValueTask InitializeAsync()
        {
            DiscordLogger = Provider.GetRequiredService<ILogger<IDiscordClient>>();
            DiscordClient.Log += HandleLogAsync;
            await DiscordClient.LoginAsync(TokenType.Bot, DiscordOptions.Token, true);
        }

        protected override async ValueTask RunAsync()
            => await DiscordClient.StartAsync();

        private Task HandleLogAsync(LogMessage logMessage)
        {
            //ToDo: Format Logger like you want it to
            DiscordLogger.Log(logMessage.Severity.ToLogLevel(), logMessage.Exception, logMessage.Message);
            return Task.CompletedTask;
        }
    }
}

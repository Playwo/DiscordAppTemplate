using Discord;
using Microsoft.Extensions.Logging;

namespace DiscordAppTemplate.Extensions
{
    public static partial class Extension
    {
        public static LogLevel ToLogLevel(this LogSeverity logSeverity)
            => logSeverity switch
            {
                LogSeverity.Verbose => LogLevel.Trace,
                LogSeverity.Debug => LogLevel.Debug,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Critical => LogLevel.Critical,
                _ => LogLevel.None,
            };
    }
}

using System;
using Discord;
using Discord.WebSocket;
using Qmmands;

namespace DiscordAppTemplate.Commands
{
    public sealed class AppContext : CommandContext
    {
        public ISocketMessageChannel Channel { get; }
        public DiscordShardedClient Client { get; }
        public SocketGuild Guild { get; }
        public SocketMessage Message { get; }
        public SocketUser User { get; }

        public bool IsPrivate => Channel is IPrivateChannel;
        public bool IsSystem => Message is SocketSystemMessage;

        public AppContext(DiscordShardedClient client, SocketMessage message, IServiceProvider provider)
            : base(provider)
        {
            Client = client;
            Guild = (message.Channel as SocketGuildChannel)?.Guild;
            Channel = message.Channel;
            User = message.Author;
            Message = message;
        }
    }
}

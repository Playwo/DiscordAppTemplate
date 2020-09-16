using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Qmmands;

namespace DiscordAppTemplate.Commands
{
    public abstract class AppModule : ModuleBase<AppContext>
    {
        public async Task<RestUserMessage> ReplyAsync(string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null)
            => await Context.Channel.SendMessageAsync(text, isTTS, embed, options);
    }
}

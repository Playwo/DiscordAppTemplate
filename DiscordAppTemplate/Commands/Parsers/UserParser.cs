using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Qmmands;

namespace DiscordAppTemplate.Commands
{
    public sealed class UserParser : AppParser<IUser>
    {
        public override ValueTask<TypeParserResult<IUser>> ParseAsync(Parameter param, string value, AppContext context)
        {
            var users = context.Guild.Users.OfType<IUser>().ToList();
            IUser user = null;

            if (ulong.TryParse(value, out ulong id) || MentionUtils.TryParseUser(value, out id))
            {
                user = context.Client.GetUser(id);
            }

            if (user is null)
            {
                var match = users.Where(x =>
                    x.Username.Equals(value, StringComparison.OrdinalIgnoreCase)
                    || (x as SocketGuildUser).Nickname.Equals(value, StringComparison.OrdinalIgnoreCase)).ToList();
                if (match.Count > 1)
                {
                    return TypeParserResult<IUser>.Unsuccessful(
                        "Multiple users found, try mentioning the user or using their ID.");
                }

                user = match.FirstOrDefault();
            }
            return user is null
                ? TypeParserResult<IUser>.Unsuccessful("User not found.")
                : TypeParserResult<IUser>.Successful(user);
        }
    }

}

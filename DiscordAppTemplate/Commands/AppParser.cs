using System.Threading.Tasks;
using Qmmands;

namespace DiscordAppTemplate.Commands
{
    public abstract class AppParser<T> : TypeParser<T>
    {
        public abstract ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, AppContext context);

        public override ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, CommandContext context)
            => ParseAsync(parameter, value, context as AppContext);
    }
}

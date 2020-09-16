using System.Threading.Tasks;
using Qmmands;

namespace DiscordAppTemplate.Commands
{
    public abstract class AppCheckAttribute : CheckAttribute
    {
        public override ValueTask<CheckResult> CheckAsync(CommandContext context)
        => CheckAsync(context as AppContext);

        public abstract ValueTask<CheckResult> CheckAsync(AppContext context);
    }
}

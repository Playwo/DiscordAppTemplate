using System;
using System.Threading.Tasks;
using Common.Configuration;

namespace DiscordAppTemplate.Options
{
    [SectionName("Discord")]
    public sealed class DiscordOptions : Option
    {
        public string Token { get; set; } = "InsertDiscordTokenBotTokenHere";

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
            => new ValueTask<ValidationResult>(string.IsNullOrWhiteSpace(Token) || Token == "InsertDiscordTokenBotTokenHere"
                ? ValidationResult.Failed("The discord token must be set!")
                : ValidationResult.Success);
    }
}

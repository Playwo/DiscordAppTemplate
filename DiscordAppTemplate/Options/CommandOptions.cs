using System;
using System.Threading.Tasks;
using Common.Configuration;

namespace DiscordAppTemplate.Options
{
    public sealed class CommandOptions : Option
    {
        public string Prefix { get; set; } = "P!";

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
            => new ValueTask<ValidationResult>(string.IsNullOrWhiteSpace(Prefix)
                ? ValidationResult.Failed("You need to set a prefix!")
                : ValidationResult.Success);
    }
}

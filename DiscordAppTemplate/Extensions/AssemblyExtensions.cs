using System;
using System.Linq;
using System.Reflection;
using DiscordAppTemplate.Commands;

namespace DiscordAppTemplate.Extensions
{
    public static partial class Extension
    {
        public static Type[] GetParserTypes(this Assembly assembly) => assembly.GetTypes()
            .Where(x => x.BaseType.IsGenericType && x.BaseType.IsAbstract && x.BaseType.Name == typeof(AppParser<>).Name)
            .Where(x => !x.IsAbstract && !x.IsGenericType).ToArray();
    }
}

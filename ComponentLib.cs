using ComponentLib.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ComponentLib;

public class ComponentLib
{
    private DiscordShardedClient _client;
    public IServiceProvider _services;
    private InteractionService _interactions;

    private System.Collections.Generic.IEnumerable<Discord.Interactions.ModuleInfo> _interactionModules;

    public static Buttons Buttons = new Buttons();

    public async Task Initialize(DiscordShardedClient DiscordClient)
    {
        await using var services = ConfigureServices(DiscordClient);
        _services = services.GetRequiredService<IServiceProvider>();

        var interactions = services.GetRequiredService<InteractionService>();
        _interactions = interactions;

        _client = services.GetRequiredService<DiscordShardedClient>();

        services.GetRequiredService<InteractionHandler>().Initialize();

        await Task.Delay(-1);
    }

    private static ServiceProvider ConfigureServices(DiscordShardedClient discordClient)
    {
        return new ServiceCollection()
            .AddSingleton<DiscordShardedClient>(discordClient)
            .AddSingleton<InteractionService>()
            .AddSingleton<InteractionHandler>()
            .BuildServiceProvider();
    }
}
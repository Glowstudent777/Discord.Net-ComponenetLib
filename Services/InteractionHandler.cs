using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentLib.Services
{
    internal class InteractionHandler
    {
        private readonly InteractionService _interactions;
        private readonly DiscordShardedClient _client;
        private readonly IServiceProvider _services;

        public InteractionHandler(IServiceProvider services)
        {
            _interactions = services.GetRequiredService<InteractionService>();
            _client = services.GetRequiredService<DiscordShardedClient>();
            _services = services;
        }

        public void Initialize()
        {
            // process the InteractionCreated payloads to execute Interactions commands
            _client.InteractionCreated += HandleInteraction;

            _interactions.ComponentCommandExecuted += ComponentCommandExecuted;
        }

        private Task ComponentCommandExecuted(ComponentCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;
                    case InteractionCommandError.BadArgs:
                        // implement
                        break;
                    case InteractionCommandError.Exception:
                        // implement
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                // create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var ctx = new ShardedInteractionContext(_client, arg);
                await _interactions.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception:");
                Console.WriteLine(ex);
                // if a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if (arg.Type == InteractionType.ApplicationCommand)
                {
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            }
        }
    }
}

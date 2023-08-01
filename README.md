# Discord.Net Components Library

Simple components library I created for Discord.Net.

This library so far only includes two types of buttons, the toggle button that toggles between green and red and the tri button that changes between green, blue, and red. These are good for settings menus that may have on and off switches.

# Implementing
## References
First of all add the library as a dependancy in your `.csproj` file
```csproj
<ItemGroup>
  <ProjectReference Include="..\ComponentLib\ComponentLib.csproj" />
</ItemGroup>
```

## Registering the library
Add the following to your interaction command installer. This will register the events needed for the component interactions.
```csharp
_interactionModules = _interactionModules.Concat(await _interactions.AddModulesAsync(typeof(ComponentLib.ComponentLib).Assembly, new ComponentLib.ComponentLib()._services));
```

Then add the following somewhere after your client is ready. This will initialize the ComponentLib and let it access the bot client.
```chsarp
_ = new ComponentLib.ComponentLib().Initialize(_client);
```

## Example
Here is how I did mine

```csharp
public async Task InstallCommandsAsync()
{
    _client.ShardReady += RegisterCommands;

    var assembly = Assembly.GetEntryAssembly();
    _interactionModules = await _interactions.AddModulesAsync(assembly, _services);
            
    // add modules from ComponentLib
    _interactionModules = _interactionModules.Concat(await _interactions.AddModulesAsync(typeof(ComponentLib.ComponentLib).Assembly, new ComponentLib.ComponentLib()._services));
}

private async Task RegisterCommands(DiscordSocketClient client)
{
    if (client.ShardId != 0) return;

    _ = new ComponentLib.ComponentLib().Initialize(_client);
    await _interactions.AddModulesGloballyAsync(true, _interactionModules.ToArray());
            
    foreach (var module in _interactionModules)
    {
        Console.WriteLine($"Registered {module.Name}");
    }
}
```

# How to use
## Buttons
The button syntax is as follows. ButtonStyle is the style that the button will start with.
```csharp
TriButton(string Label, string CustomId, bool IsDisabled = false, ButtonStyle Style = ButtonStyle.Success)
```

```csharp
var buttons = new ActionRowBuilder()
    .AddComponent(ComponentLib.Buttons.ToggleButton("Toggle Me", "toggle", false, ButtonStyle.Success))
    .AddComponent(ComponentLib.Buttons.TriButton("Cycle Me", "cycle", false, ButtonStyle.Success));
```

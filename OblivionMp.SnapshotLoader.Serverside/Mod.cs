using JetBrains.Annotations;
using ReadyM.Relay.Server.Sdk;
using ReadyM.Relay.Server.Sdk.Ecs.Components;

namespace OblivionMp.SnapshotLoader.Serverside;

[UsedImplicitly]
public class Mod : ServerModBase
{
    protected override void RegisterComponents(IComponentRegistry registry)
    {
    }

    protected override void Init()
    {
        // The seed is the content folder in this mod's server folder, wherever the mod is installed.
        Services.RegisterSingleton(new SeedSettings(Path.Combine(ModDirectory, "content")));

        // The cell and world seeders are [Service]s, which the SDK collects; they share the snapshot.
        Services.RegisterSingleton<WorldSnapshot>();
    }
}

# OblivionMP snapshot loader

Fills the [OblivionMP](https://readym.io) world from a snapshot of the game's data, so a fresh server is not empty.
When a cell is created for the first time, it gets its furniture, containers, plants and items. When a new world is
created, it gets the item values that trade checks against and the NPC templates that inventories are rolled from.
Anything that is already saved is left alone.

It is an optional, server-only mod with no API of its own. Install it and it does its job.

The snapshot is read from `content/world/` next to the server executable, starting at `index.json`. Without it, or
with one written for another schema version, the mod logs a warning and seeds nothing.

## Building

The project builds against the OblivionMP SDK package from NuGet, `ReadyM.SDK.Oblivion.Server`. Its version is set
once, as `OblivionSdkVersion` in `Directory.Build.props`.

```powershell
.\MakeModFolder.ps1 Release                  # the mod, into Output/mods/OblivionMp.SnapshotLoader
```

Use `Debug` instead of `Release` to include `.pdb` symbol files.

Copy `Output/mods/OblivionMp.SnapshotLoader/` into your server's `mods/` directory, next to the `OblivionMp.Sdk` mod,
and restart the server.

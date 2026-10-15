# OblivionMP seed loader

Fills the [OblivionMP](https://obmp.readym.io) world from the seed, a snapshot of the game's data, so a fresh server
is not empty. When a cell is created for the first time, it gets its furniture, containers, plants and items. When a
new world is created, it gets the item values that trade checks against and the NPC templates that inventories are
rolled from. Anything that is already saved is left alone.

It is an optional, server-only mod with no API of its own. Install it and it does its job.

The mod reads the seed from the `content/` folder in its own `server/` folder, starting at `content/world/index.json`:

```
<server>/mods/OblivionMp.SeedLoader/server/content/
```

Without it, or with one written for another schema version, the mod logs a warning and seeds nothing.

## The seed

The seed is hosted in blob storage, not in git:
[seed.zip](https://readycodestorage.blob.core.windows.net/public/obmp/seed.zip). Extract it into
`OblivionMp.SnapshotLoader.Serverside/seed/`, which git ignores, so that `seed/world/index.json` exists. With the
OblivionMP mod repository cloned next to this one, its `scripts/Fetch-Seed.ps1` does this for you.

## Building

The project builds against the OblivionMP SDK package from NuGet, `ReadyM.SDK.Oblivion.Server`. Its version is set
once, as `OblivionSdkVersion` in `Directory.Build.props`.

```powershell
.\MakeModFolder.ps1 Release                  # the mod, into Output/mods/OblivionMp.SeedLoader
```

Use `Debug` instead of `Release` to include `.pdb` symbol files. The seed, when you have fetched it, goes into the
mod folder's `server/content/`.

Copy `Output/mods/OblivionMp.SeedLoader/` into your server's `mods/` directory, next to the `OblivionMp.Sdk` mod,
and restart the server.

namespace OblivionMp.SnapshotLoader.Serverside;

/// <summary>Where the seed is: the content folder in the mod's server folder.</summary>
public sealed class SeedSettings(string contentFolder)
{
    public string ContentFolder { get; } = contentFolder;
}

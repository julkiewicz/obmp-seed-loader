using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Logging;
using ReadyM.Api.Idents;
using ReadyM.Relay.Common.Oblivion;

namespace OblivionMp.SnapshotLoader.Serverside;

public sealed class WorldSnapshot
{
    public const int SchemaVersion = 5;

    private const string ContentFolder = "content";
    private const string WorldFolder = "world";
    private const string IndexFileName = "index.json";
    private const string LeveledItemsFileName = "leveled-items.json";
    private const string ItemValuesFileName = "item-values.json";
    private const string NpcTemplatesFileName = "npc-templates.json";

    private readonly string _folder = Path.Combine(AppContext.BaseDirectory, ContentFolder, WorldFolder);
    private readonly Dictionary<FullCellId, string> _fileByCell = [];
    private readonly Dictionary<uint, LeveledItemRecord> _leveledItemListById = [];
    private readonly List<ItemValueRecord> _itemValues = [];
    private readonly List<NpcTemplateRecord> _npcTemplates = [];

    public WorldSnapshot(ILogger logger)
    {
        var indexPath = Path.Combine(_folder, IndexFileName);
        if (!File.Exists(indexPath))
        {
            logger.LogWarning("No world snapshot at {Path}, cells are not seeded", indexPath);
            return;
        }

        var index = Read(indexPath, WorldSnapshotJsonContext.Default.SnapshotIndex);
        if (index.SchemaVersion != SchemaVersion)
        {
            logger.LogWarning("World snapshot at {Path} has schema {Found}, expected {Expected}, cells are not seeded", indexPath, index.SchemaVersion, SchemaVersion);
            return;
        }

        foreach (var area in index.Areas)
        {
            var areaId = new AreaId(area.AreaId);
            foreach (var cell in area.Cells)
            {
                var cellId = area.IsInterior ? OblivionCellIds.Interior : OblivionCellIds.Exterior(cell.X!.Value, cell.Y!.Value);
                _fileByCell.Add(new FullCellId(areaId, cellId), cell.File);
            }
        }

        foreach (var list in Read(Path.Combine(_folder, LeveledItemsFileName), WorldSnapshotJsonContext.Default.LeveledItemsFileRecord).Lists)
            _leveledItemListById.Add(list.Id, list);

        _itemValues.AddRange(Read(Path.Combine(_folder, ItemValuesFileName), WorldSnapshotJsonContext.Default.ItemValuesFileRecord).Items);
        _npcTemplates.AddRange(Read(Path.Combine(_folder, NpcTemplatesFileName), WorldSnapshotJsonContext.Default.NpcTemplatesFileRecord).Npcs);

        logger.LogInformation("World snapshot with {Areas} areas, {Cells} cells, {Lists} leveled item lists, {Values} item values and {Npcs} npc templates, load order {LoadOrder}", index.Areas.Count, _fileByCell.Count, _leveledItemListById.Count, _itemValues.Count, _npcTemplates.Count, string.Join(", ", index.LoadOrder));
    }

    public IReadOnlyList<ItemValueRecord> ItemValues => _itemValues;

    public IReadOnlyList<NpcTemplateRecord> NpcTemplates => _npcTemplates;

    public bool TryGetLeveledItemList(uint id, out LeveledItemRecord list) => _leveledItemListById.TryGetValue(id, out list);

    public bool TryLoadCell(FullCellId cellId, out CellFileRecord cell)
    {
        if (!_fileByCell.TryGetValue(cellId, out var file))
        {
            cell = default;
            return false;
        }

        cell = Read(Path.Combine(_folder, file), WorldSnapshotJsonContext.Default.CellFileRecord);
        return true;
    }

    private static T Read<T>(string path, JsonTypeInfo<T> typeInfo) where T : struct
    {
        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize(stream, typeInfo);
    }
}

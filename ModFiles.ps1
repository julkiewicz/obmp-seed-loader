#!powershell.exe -ExecutionPolicy Bypass -File

# The file lists MakeModFolder.ps1 uses to build
#
#   Output/mods/<modName>/manifest.json
#   Output/mods/<modName>/server/...
#
# This is a server-only mod: it has no client half, so players never download anything for it.

# Folder name of the packaged mod.
$modName = "OblivionMp.SnapshotLoader"

# The projects the halves build from. No client project: the folder gets no client/.
$clientProject = ""
$serverProject = "OblivionMp.SnapshotLoader.Serverside"

# --- Server half, from <serverProject>/bin/<Configuration>/net10.0 ---
$serverBuildFiles = @(
    "OblivionMp.SnapshotLoader.Serverside.dll"
)

# Copied from "Content" into the mod root.
$manifestFiles = @(
    "manifest.json"
)

# Copied from "Content" into server/.
$clientContentFiles = @()

$serverContentFiles = @()

# Added only in Debug builds.
$clientBuildFiles = @()
$clientDebugBuildFiles = @()

$serverDebugBuildFiles = @(
    "OblivionMp.SnapshotLoader.Serverside.pdb"
)

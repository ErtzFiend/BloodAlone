# Blood Alone (RimWorld 1.7)

Experimental Harmony patch for RimWorld 1.7: suppresses the ordinary Food need (`NeedDefOf.Food`) from being instantiated for pawns with the `GeneDefOf.Hemogenic` gene. Hemogen mechanics remain unchanged.

## Status & Validation

- **Experimental Mod**: Developed targeting RimWorld 1.7 and Harmony 1.7.
- **Harmony Binding**: Verified via standalone test harness (`Tools/harness.cs` & `Tools/harness.ps1`) confirming `PatchAll` resolves and successfully attaches to target RimWorld methods (`Pawn_NeedsTracker.ShouldHaveNeed` and `Pawn_GeneTracker.ExposeData`).
- **Gameplay Status**: **In-game behavior and gameplay have NOT been tested.** Live-game stability, starvation proxy interactions, and extended colony play remain unverified.

## Compatibility & Limitations

- Not tested with third-party food/hunger mods.
- Mods assuming every pawn possesses a `Need_Food` instance or accessing it without null checks may fail or alter pawn behavior.
- Food job/alert/ingestion hooks may still act independently if triggered outside `Pawn_NeedsTracker`.
- Requires the **Biotech** expansion (for `GeneDefOf.Hemogenic`) and **Harmony**.

## Project Structure

```
BloodAlone/
├── About/
│   └── About.xml             # RimWorld mod metadata
├── Assemblies/
│   └── BloodAlone.dll        # Compiled mod assembly (excludes game copyrighted DLLs)
├── Source/
│   ├── BloodAlone.cs         # Harmony patches and bootstrap source
│   └── BloodAlone.csproj     # C# project build file
└── Tools/
    ├── harness.cs            # Harmony patch binding verification harness
    ├── harness.ps1           # Harness execution script
    ├── inspect.cs            # Assembly reflection probe
    └── probe.runtimeconfig.json
```

## Building from Source

To compile `BloodAlone.dll`:

1. Ensure .NET SDK (or Roslyn `csc`) is installed.
2. Locate reference assemblies from RimWorld and Harmony:
   - `Assembly-CSharp.dll` (from RimWorld installation directory)
   - `0Harmony.dll` (from Harmony mod directory)
3. Build using `dotnet`:
   ```bash
   dotnet build Source/BloodAlone.csproj -c Release /p:RimWorldPath="<path_to_rimworld>" /p:HarmonyPath="<path_to_harmony>"
   ```
   Or invoke `csc`:
   ```powershell
   csc /target:library /out:Assemblies/BloodAlone.dll /r:"<path>\0Harmony.dll" /r:"<path>\Assembly-CSharp.dll" Source/BloodAlone.cs
   ```
4. Output lands in `Assemblies/BloodAlone.dll`.

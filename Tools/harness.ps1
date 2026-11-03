$ErrorActionPreference = 'Stop'
$dir = 'C:\temp\rw_inspect'
$game = 'C:\Program Files (x86)\Steam\steamapps\common\RimWorld'
$harmony = 'C:\Program Files (x86)\Steam\steamapps\workshop\content\294100\839005762\1.7\Assemblies\0Harmony.dll'
$csc = 'C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\Roslyn\csc.exe'
Copy-Item $harmony "$dir\0Harmony.dll" -Force
& $csc /noconfig /target:exe "/out:$dir\harness.exe" "@$dir\refs.rsp" "/r:$harmony" "$dir\harness.cs"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
Copy-Item "$dir\inspect.runtimeconfig.json" "$dir\harness.runtimeconfig.json" -Force
$env:PATH="$game;$env:PATH"
Set-Location $dir
& dotnet "$dir\harness.exe"
exit $LASTEXITCODE

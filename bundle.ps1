$modname = "EasyCards"

dotnet publish "$modname\$modname.csproj" --configuration Release

# update manifest
$xml = [Xml] (Get-Content ".\$modname\$modname.csproj")
$manifest = Get-Content ".\Thunderstore\manifest.json" | ConvertFrom-Json

$modversion = $xml.Project.PropertyGroup.Version
$desc = $xml.Project.PropertyGroup.Description

Write-Output "Mod Version: $modversion"
Write-Output "Description: $desc"

$manifest.description = $desc
$manifest.version_number = $modversion

$manifest | ConvertTo-Json | Out-File ".\Thunderstore\manifest.json"

$baseOutputDir = ".\$modname\bin\Release\net6.0\publish"

New-Item -ItemType Directory ".\Thunderstore\plugins\" -Force
Remove-Item -Path "$baseOutputDir\*.*" -Exclude "*.dll","*.png" # remove any file that is not a DLL in src folder
Copy-Item -Path "$baseOutputDir\*.dll" -Destination ".\Thunderstore\plugins" # copy dlls to thunderstore folder
Copy-Item -Path "$baseOutputDir\*" -Destination ".\Thunderstore\plugins" -Recurse -Force # Copy directories only
Compress-Archive -Path ".\Thunderstore\*" -CompressionLevel "Optimal" -DestinationPath ".\$modname-$modversion.zip" -Force
Remove-Item -Path ".\Thunderstore\plugins" -Recurse

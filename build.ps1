$xml = [xml](Get-Content MMR.Randomizer\MMR.Randomizer.csproj)
$prefix = "$($xml.Project.PropertyGroup.VersionPrefix)".TrimEnd()
try {
  $suffix = "dev-$("$(git rev-parse --short HEAD)".SubString(0, 7))"
} catch {
# $suffix = "dev-$((iwr https://api.github.com/repos/ZoeyZolotova/mm-rando/branches/dev | convertfrom-json).commit.sha)"
  $suffix = "dev-unknown"
}
$version = "$prefix-$suffix"
dotnet publish MMR.UI\MMR.UI.csproj -p:VersionSuffix="$suffix" -c Release -o build -r win-x64 --no-self-contained
dotnet publish MMR.CLI\MMR.CLI.csproj -p:VersionSuffix="$suffix" -c Release -o build -r win-x64 --no-self-contained
md dist
Compress-Archive -Path build\* -DestinationPath ${root}dist\MM-Randomizer-v$version.zip
param(
    [Alias("c")]
    [string] $buildConfiguration = "Release",

    [Alias("r")]
    [string] $runtime = "osx-arm64",

    [switch] $clean,

    [switch] $removeOnExit
)

$outputDirectory = "${PSScriptRoot}/bin/${buildConfiguration}/net8.0/${runtime}/publish"
$dllPath = "${outputDirectory}/EFCorePowerShellSample.dll"
$moduleManifestPath = "${outputDirectory}/EFCorePowerShellSample.psd1"

if ($clean) {
    Write-Host "Cleaning Project" -ForegroundColor Cyan
    dotnet clean --configuration $buildConfiguration --runtime $runtime
}

Write-Host "Building Project" -ForegroundColor Cyan
dotnet publish --configuration $buildConfiguration --runtime $runtime --self-contained false

try {
    Import-Module -Name $moduleManifestPath -Verbose -Force
}
catch {
    Write-Error "Error. Exception: $_"
}

Write-host "Session Started" -ForegroundColor Green

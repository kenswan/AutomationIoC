param(
    [Alias("h")]
    [string]$header = "Default Header",

    [Alias("c")]
    [string] $buildConfiguration = "Release",

    [Alias("r")]
    [string] $runtime = "osx-arm64",

    [switch] $clean,

    [switch] $removeOnExit
)

$outputDirectory = "${PSScriptRoot}/bin/${buildConfiguration}/net8.0/${runtime}/publish"
$dllPath = "${outputDirectory}/PowerShellSample.dll"
$moduleManifestPath = "${outputDirectory}/PowerShellSample.psd1"

if ($clean) {
    Write-Host "Cleaning Project" -ForegroundColor Cyan
    dotnet clean --configuration $buildConfiguration --runtime $runtime
}

Write-Host "Building Project" -ForegroundColor Cyan
dotnet publish --configuration $buildConfiguration --runtime $runtime --self-contained false

try {
    Import-Module -Name $moduleManifestPath -Verbose -Force

    Write-Report -h $header
}
catch {
    Write-Error "Error. Exception: $_"
}
finally {

    if ($removeOnExit) {
        Write-Host "Removing Module" -ForegroundColor Cyan

        Remove-Module -Name $moduleManifestPath -ErrorAction SilentlyContinue

        Write-Host "Releasing DLL" -ForegroundColor Cyan

        if (Test-Path $dllPath) {
            Remove-Item -Path $dllPath -Force
            Write-Host "DLL Released" -ForegroundColor Green
        }
        else {
            Write-Host "DLL not found for removal" -ForegroundColor Yellow
        }
    }
}

Write-host "Script Complete"

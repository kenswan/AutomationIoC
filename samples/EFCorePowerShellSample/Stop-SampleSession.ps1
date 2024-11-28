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

try {
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
catch {
    Write-Error "Error. Exception: $_"
}

Write-host "Session Stopped" -ForegroundColor Green

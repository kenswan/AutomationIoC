Import-Module .\src\sdk\bin\Debug\net6.0\PowerShellFocused.dll

Write-Host "Starting Process"

Build-Dependencies

Get-Test -Verbose

Write-Host "End Process"
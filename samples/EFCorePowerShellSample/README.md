# Entity Framework Core Meets PowerShell

## Requirements

- PowerShell Core 7.4.6 or greater
- .NET 8.0 SDK
- .NET CLI EF Core Tool

## Installation

Install the .NET CLI EF Core Tool by running the following command:
```powershell
dotnet tool install --global dotnet-ef
```

## Migrations

Add New Migration (optional)
```powershell
dotnet ef migrations add <name of migration>
```

Update Database (Required)
```powershell
dotnet ef database update
```

## Run Test
```powershell
./Run-Test.ps1 -clean -removeOnExit
```

## Start Sample Session

```powershell
./Start-Sample.ps1
```

## Close Sample Session

```powershell
./Start-Sample.ps1
```

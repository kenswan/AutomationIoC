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

Add New Migration (Optional)
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

## Sample Commands During Session

```powershell
# Display List of ToDos and their properties
Get-ToDos

# Add a ToDo Item
Add-ToDo -Title "Reminder Item" -Description "This is a Reminder"

# Update a ToDo Item (IDs are shown during "Add-ToDo" and "Get-ToDos" commands)
Update-ToDo -Id 1d1e8fe1-15ea-4ad0-b693-4a4b655241e6 -Title "Brand New Title"

# Remove a ToDo Item (IDs are shown during "Add-ToDo" and "Get-ToDos" commands)
Remove-ToDo -Id 1d1e8fe1-15ea-4ad0-b693-4a4b655241e6
```

# Create and apply database migration
$migrationName = $args[0]
if ([string]::IsNullOrEmpty($migrationName)) {
    Write-Host "Please provide a migration name. Example: .\create-migration.ps1 InitialCreate"
    exit 1
}

dotnet ef migrations add $migrationName --project . --startup-project . --output-dir Data/Migrations

echo "Migration created. To apply the migration, run: dotnet ef database update --project . --startup-project ."

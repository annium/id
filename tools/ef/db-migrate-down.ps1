param(
    [Parameter(Mandatory = $true)][string]$startup,
    [Parameter(Mandatory = $true)][string]$project,
    [Parameter(Mandatory = $true)][string]$context
)

$migrationName = dotnet ef migrations list `
    --startup-project $startup `
    --project $project `
    --context $context `
    --no-build `
    --prefix-output | grep 'data: ' | sed -e 's#data:    ##g' | head -n 1

dotnet ef database update $migrationName `
    --startup-project $startup `
    --project $project `
    --context $context `
    --no-build
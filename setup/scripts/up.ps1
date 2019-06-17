param(
    [Parameter(Mandatory = $true)][string]$project,
    [Parameter(Mandatory = $true)][string]$component
)

Write-Host "Create $component"
$env:COMPOSE_IGNORE_ORPHANS = "true"
docker-compose -p $project -f setup/$component.yml up --build -d --force-recreate
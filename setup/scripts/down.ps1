param(
    [Parameter(Mandatory = $true)][string]$project,
    [Parameter(Mandatory = $true)][string]$component
)

Write-Host "Delete $component"
$env:COMPOSE_IGNORE_ORPHANS = "true"
docker-compose -p $project -f setup/$component.yml stop
docker-compose -p $project -f setup/$component.yml rm -fv

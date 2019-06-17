param(
    [Parameter(Mandatory = $true)][string]$project
)

$network = $project + "_net"

if ((docker network ls -q -f "name=$network" | Measure-Object).Count -eq 0) {
    docker network create --attachable --driver bridge --scope local $network
    Write-Host "Network $network created"
}
else {
    Write-Host "Network $network is already available"
}
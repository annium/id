param(
    [Parameter(Mandatory = $true)][string]$project
)

$network = $project + "_net"

if ((docker network ls -q -f "name=$network" | Measure-Object).Count -gt 0) {
    if ((docker network inspect -f "{{len .Containers}}" $network) -eq 0) {
        docker network rm $network
        Write-Host "Network $network deleted"
    }
    else {
        Write-Host "Network $network is used yet"
    }
}
else {
    Write-Host "Network $network not found"
}
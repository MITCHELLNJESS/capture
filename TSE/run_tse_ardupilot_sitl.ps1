# Get the IPv4 address of the vEthernet (WSL (Hyper-V firewall)) adapter
$ip = Get-NetIPAddress -AddressFamily IPv4 | Where-Object { $_.InterfaceAlias -like "*WSL*" } | Select-Object -ExpandProperty IPAddress

# Echo the result
if ($ip) {
    Write-Output "In Misison Planner connect to Ardupilot SITL using TCP: [IP = $ip Port = 5760]"
} else {
    Write-Output "No IPv4 address found for WSL."
}

# Add docker command to environment PATH
$env:Path += ";C:\Program Files\Docker\Docker\resources\bin"

# Run the Docker container
docker run --name ardupilot-tse -it --rm -p 5760:5760 ardupilot-airsim
# Get the IPv4 address of the vEthernet (WSL (Hyper-V firewall)) adapter
$ip = Get-NetIPAddress -AddressFamily IPv4 | Where-Object { $_.InterfaceAlias -like "*WSL*" } | Select-Object -ExpandProperty IPAddress

# Echo the result
if ($ip) {
    Write-Output "In Misison Planner connect to Ardupilot SITL using TCP: [IP = $ip Port = 5760]"
} else {
    Write-Output "No IPv4 address found for WSL."
}


# Run the Docker container
docker run --name ardupilot -it --rm -p 5760:5760 registry.gitlab.com/barrettmpatrick/capture:latest
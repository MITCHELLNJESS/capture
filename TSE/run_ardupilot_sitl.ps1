# Get the IPv4 address of the vEthernet (WSL (Hyper-V firewall)) adapter
$ip = Get-NetIPAddress -InterfaceAlias "vEthernet (WSL (Hyper-V firewall))" -AddressFamily IPv4 | Select-Object -ExpandProperty IPAddress

# Echo the result
if ($ip) {
    Write-Output "In Misison Planner connect to Ardupilot SITL using TCP: [IP = $ip Port = 5760]"
} else {
    Write-Output "No IPv4 address found for 'vEthernet (WSL (Hyper-V firewall))'."
}


# Run the Docker container
docker run --name ardupilot -it --rm -p 5760:5760 registry.gitlab.com/barrettmpatrick/capture:latest
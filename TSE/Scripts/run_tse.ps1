# Add docker command to environment PATH
$env:Path += ";C:\Program Files\Docker\Docker\resources\bin"

# Run the Docker gstreamer container
$containerId = docker run --name airsim-gstreamer -d --rm -p 41451:41451 -p 5600:5600/udp airsim-gstreamer

Write-Host "AirSim-gstreamer container started with ID: $containerId"

# Launch Mission Planner
Write-Host "Launching Mission Planner..."
Start-Process "C:\Users\Brian\Dev\capture\C2\MissionPlanner\bin\Debug\net461\MissionPlanner.exe"

Write-Host "Press Ctrl+C to stop (only press once)..."

# Trap Ctrl+C or script exit
try {
    while ($true) {
        Start-Sleep -Seconds 1
    }
}
finally {
    Write-Host "`nStopping container (do not break while this is executing)..."
    docker stop $containerId
}
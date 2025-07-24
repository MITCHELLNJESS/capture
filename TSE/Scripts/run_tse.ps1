# Launch Mission Planner
Write-Host "Launching Mission Planner..."
Start-Process "C:\Users\Brian\Dev\capture\C2\MissionPlanner\bin\Debug\net461\MissionPlanner.exe"

# Launch A3
Write-Host "Launching A3..."
Start-Process -FilePath "powershell" -ArgumentList "-NoExit", "-Command", "& { & 'C:\Users\Brian\miniconda3\Scripts\activate.bat' 'a3-venv'; & 'C:\Users\Brian\miniconda3\envs\a3-venv\python.exe' 'C:\Users\Brian\Dev\capture\C2\A3_System\scripts\yolo_live_inference.py' }" -WorkingDirectory "C:\Users\Brian\Dev\capture\C2\A3_System\scripts"

# Add docker command to environment PATH
$env:Path += ";C:\Program Files\Docker\Docker\resources\bin"

# Run the Docker gstreamer container
Write-Host "Launching AirSim-gstreamer container..."
docker run --name airsim-gstreamer -it --rm -p 41451:41451 -p 5600:5600/udp airsim-gstreamer
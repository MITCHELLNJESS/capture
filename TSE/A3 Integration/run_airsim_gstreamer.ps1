# Add docker command to environment PATH
$env:Path += ";C:\Program Files\Docker\Docker\resources\bin"

# Run the Docker container
docker run --name airsim-gstreamer -it --rm -p 41451:41451 -p 5600:5600/udp airsim-gstreamer
import airsim
import time

# Connect to the AirSim simulator
client = airsim.MultirotorClient()
client.confirmConnection()
client.enableApiControl(True)
client.armDisarm(True)

# Take off
print("Taking off...")
client.takeoffAsync().join()

# Move to a position (x, y, z) in NED coordinates (meters)
print("Moving to position (10, 10, -10)...")
client.moveToPositionAsync(10, 10, -10, 5).join()

# Hover for a few seconds
print("Hovering...")
time.sleep(5)

# Land the drone
print("Landing...")
client.landAsync().join()

# Disarm and disable API control
client.armDisarm(False)
client.enableApiControl(False)
print("Done!")
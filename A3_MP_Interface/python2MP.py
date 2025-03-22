# publishes DDS object to MP? 

print ("hello! :D")

import time
from pymavlink import mavutil

# Connect to the Mission Planner (using a TCP connection here)
# Replace 'localhost:14550' with the appropriate IP and port for your connection

connection = mavutil.mavlink_connection('udp:127.0.0.1:14550')

# Wait for the heartbeat message to confirm the connection
print("Waiting for heartbeat")
connection.wait_heartbeat()
print("Heartbeat received!")

# Send a heartbeat message to Mission Planner (this is a common way to notify the system)
# This is a simple example; you can send more complex messages as needed

# Send heartbeat (type=6 is MAV_TYPE_GCS, autopilot=8 is MAV_AUTOPILOT_ARDUPILOTMEGA)
connection.mav.heartbeat_send(
    mavutil.mavlink.MAV_TYPE_GCS,  # The type of vehicle (this is for Ground Control Station)
    mavutil.mavlink.MAV_AUTOPILOT_ARDUPILOTMEGA,  # Autopilot system type (ArduPilot Mega)
    mavutil.mavlink.MAV_MODE_GUIDED_ARMED,  # Mode (guided, armed, etc.)
    0,  # System status (0 = Uninitialized)
    0  # Custom mode (can be a bitmask of modes)
)

print("Heartbeat sent!")

# You can send other messages after this (e.g., set mode, send GPS info, etc.)

# Wait and close connection after sending
time.sleep(1)
connection.close()

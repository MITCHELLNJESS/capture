from pymavlink import mavutil
import time

# Establish MAVLink connection 

# Serial connection - guessing we don't use this 
#master = mavutil.mavlink_connection('/dev/ttyAMA0', baud=57600) 

# open connection over UDP/TCP? 
master = mavutil.mavlink_connection('udp:127.0.0.1:14550') 
#According to ArduPilot discourse form online this is the port used for UDP


# Wait for a heartbeat message to confirm the connection (acknowlegement)
master.wait_heartbeat()
print("Heartbeat received")

# Function to send the A3MPDataMsg
def send_A3MPDataMsg(hour, minute, second, boundingBoxX, boundingBoxY, length, width, isAligned):
    # Send the custom message to the autopilot as info
    master.mav.statustext_send(
        mavutil.mavlink.MAV_SEVERITY_INFO, 
        f"A3MPDataMsg - Time: {hour}:{minute}:{second}, "
        f"Bounding Box: ({boundingBoxX}, {boundingBoxY}), "
        f"Size: ({length}x{width}), Aligned: {isAligned}"
    )

# Random sample values
hour = 14
minute = 30
second = 45
boundingBoxX = 100.0
boundingBoxY = 150.0
boundingBoxLength = 50.0
boundingBoxWidth = 30.0
isAligned = True

# Send A3MPDataMsg data
send_A3MPDataMsg(hour, minute, second, boundingBoxX, boundingBoxY, boundingBoxLength, boundingBoxWidth, isAligned)

# Keep sending for testing
while True:
    send_A3MPDataMsg(hour, minute, second, boundingBoxX, boundingBoxY, boundingBoxLength, boundingBoxWidth, isAligned)
    time.sleep(1)  # send every second

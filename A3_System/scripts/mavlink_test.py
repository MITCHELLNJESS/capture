'''
**** NOTICE ***
This code exist as a skeleton to test connection to PixHawk, this is just an initial skeleton,
and has not been tested


from pymavlink import mavutil

# Connect to Pixhawk (port TBD)
master = mavutil.mavlink_connection('/dev/serial0', baud=00000)

# Wait for the heartbeat from the UAV
print("Waiting for heartbeat...")
master.wait_heartbeat()
print("MAVLink connection successful!")

# Request data from Pixhawk
master.mav.request_data_stream_send(
    master.target_system, master.target_component,
    mavutil.mavlink.MAV_DATA_STREAM_ALL, 1, 1
)

# Read 5 messages
for i in range(5):
    msg = master.recv_match(blocking=True)
    print(f"Received message: {msg}")

'''
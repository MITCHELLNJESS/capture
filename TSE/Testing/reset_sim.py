import airsim
from pymavlink import mavutil
import time

# Step 1: Connect to AirSim
print("Connecting to AirSim...")
client = airsim.MultirotorClient()
client.confirmConnection()

# Step 2: Connect to ArduPilot SITL
print("Connecting to ArduPilot SITL...")
master = mavutil.mavlink_connection('udp:172.21.64.1:14550')
master.wait_heartbeat()
print("Heartbeat received, connection established")

# Step 3: Check current mode and state
msg = master.recv_match(type='HEARTBEAT', blocking=True, timeout=3)
if msg:
    mode = mavutil.mode_string_v10(msg)
    print(f"Current mode: {mode}, Armed: {master.motors_armed()}")
else:
    print("No heartbeat received. Check SITL connection.")
    exit()

# Step 4: Clear any active mission
print("Clearing mission...")
master.mav.mission_clear_all_send(master.target_system, master.target_component)
msg = master.recv_match(type='MISSION_ACK', blocking=True, timeout=3)
if msg and msg.type == mavutil.mavlink.MAV_MISSION_ACCEPTED:
    print("Mission cleared successfully")
else:
    print(f"Failed to clear mission: {msg.type if msg else 'No ACK'}")
    exit()

# Step 5: Disable safety checks
print("Disabling safety checks...")
master.mav.param_set_send(
    master.target_system, master.target_component,
    b'BRD_SAFETYENABLE', 0, mavutil.mavlink.MAV_PARAM_TYPE_INT32
)
master.mav.param_set_send(
    master.target_system, master.target_component,
    b'ARMING_CHECK', 0, mavutil.mavlink.MAV_PARAM_TYPE_INT32
)
time.sleep(1)

# Step 6: Force mode to STABILIZE
print("Switching to STABILIZE mode...")
master.set_mode('STABILIZE')
time.sleep(1)
msg = master.recv_match(type='HEARTBEAT', blocking=True, timeout=3)
if msg and mavutil.mode_string_v10(msg) == 'STABILIZE':
    print("Successfully set to STABILIZE mode")
else:
    print("Failed to set STABILIZE mode. Current mode:", mavutil.mode_string_v10(msg) if msg else "Unknown")
    exit()

# Step 7: Force disarm
print("Attempting to force disarm...")
master.mav.command_long_send(
    master.target_system, master.target_component,
    mavutil.mavlink.MAV_CMD_COMPONENT_ARM_DISARM,
    0, 0, 1, 0, 0, 0, 0, 0  # Param2: 1 = force disarm
)
msg = master.recv_match(type='COMMAND_ACK', blocking=True, timeout=3)
if msg and msg.result == mavutil.mavlink.MAV_RESULT_ACCEPTED:
    print("Drone disarmed successfully")
else:
    print(f"Force disarm failed: {msg.result if msg else 'No ACK'}")
    msg = master.recv_match(type='STATUSTEXT', blocking=True, timeout=3)
    if msg:
        print(f"Status text: {msg.text}")
    exit()

# Step 8: Reset drone position in AirSim
print("Resetting drone to initial position in AirSim...")
client.reset()  # Reset to initial spawn point
client.enableApiControl(True)  # Re-enable API control
client.armDisarm(False)  # Ensure AirSim reflects disarmed state
time.sleep(1)

"""

# Step 9: Set home location in ArduPilot to match AirSim's origin
print("Setting home location to AirSim origin (0, 0, 0)...")
master.mav.command_long_send(
    master.target_system, master.target_component,
    mavutil.mavlink.MAV_CMD_DO_SET_HOME,
    0, 1, 0, 0, 0, 0, 0, 0  # Set home to (0, 0, 0)
)
msg = master.recv_match(type='COMMAND_ACK', blocking=True, timeout=3)
if msg and msg.result == mavutil.mavlink.MAV_RESULT_ACCEPTED:
    print("Home location set successfully")
else:
    print(f"Failed to set home location: {msg.result if msg else 'No ACK'}")
    exit()
"""

print("Drone reset to initial location and remains disarmed")
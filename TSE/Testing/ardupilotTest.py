import time
from pymavlink import mavutil, mavwp

master = mavutil.mavlink_connection("udpin:172.21.64.1:14550") # Update WSL IP
print("Waiting for heartbeat from SITL...")
master.wait_heartbeat()
print("Heartbeat received from system (system %u component %u)" % (master.target_system, master.target_component))

# Check if drone is disarmed
if master.motors_armed():
    print("Drone is armed. Attempting to disarm...")
    master.mav.command_long_send(
        master.target_system, master.target_component,
        mavutil.mavlink.MAV_CMD_COMPONENT_ARM_DISARM,
        0, 0, 0, 0, 0, 0, 0, 0
    )
    time.sleep(2)  # Wait for disarm
    if master.motors_armed():
        print("Failed to disarm. Aborting reboot.")
        exit()

# Send a command to reset the drone (reboot the autopilot)
master.mav.command_long_send(
    master.target_system,
    master.target_component,
    mavutil.mavlink.MAV_CMD_PREFLIGHT_REBOOT_SHUTDOWN,
    0,  # confirmation
    1,  # reboot autopilot
    0,
    0, 0, 0, 0, 0
)

# Wait for acknowledgment
msg = master.recv_match(type='COMMAND_ACK', blocking=True, timeout=3)
if msg:
    print(f"Command result: {msg.result}")
    if msg.result == mavutil.mavlink.MAV_RESULT_ACCEPTED:
        print("Reboot command accepted")
    else:
        print(f"Reboot command failed with result: {msg.result}")
else:
    print("No acknowledgment received")

"""
master.mav.command_long_send(
    master.target_system,
    master.target_component,
    mavutil.mavlink.MAV_CMD_COMPONENT_ARM_DISARM,
    0,
    1, 0, 0, 0, 0, 0, 0)

# wait until arming confirmed (can manually check with master.motors_armed())
print("Waiting for the vehicle to arm")
master.motors_armed_wait()
print('Armed!')

time.sleep(5)

# Disarm
# master.arducopter_disarm() or:
master.mav.command_long_send(
    master.target_system,
    master.target_component,
    mavutil.mavlink.MAV_CMD_COMPONENT_ARM_DISARM,
    0,
    0, 0, 0, 0, 0, 0, 0)

# wait until disarming confirmed
master.motors_disarmed_wait()
print('Disarmed!')
"""
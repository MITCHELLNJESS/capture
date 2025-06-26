import airsim
import cv2
import numpy as np
import subprocess

# Set up AirSim client
client = airsim.MultirotorClient()
client.confirmConnection()

gst_exe = r"C:\\Program Files\\gstreamer\\1.0\\msvc_x86_64\bin\\gst-launch-1.0.exe"

# GStreamer pipeline to send raw H264 video over UDP
gst_command = (
    f'"{gst_exe}" -v fdsrc ! '
    'videoparse width=640 height=480 format=rgb framerate=30/1 ! '
    'videoconvert ! x264enc tune=zerolatency bitrate=800 speed-preset=superfast ! '
    'rtph264pay config-interval=1 pt=96 ! '
    'udpsink host=127.0.0.1 port=5600'
)

# Launch GStreamer subprocess
gst_proc = subprocess.Popen(gst_command, shell=True, stdin=subprocess.PIPE)

try:
    while True:
        # Get image from AirSim
        raw_image = client.simGetImage("a3", airsim.ImageType.Scene)
        if raw_image is None:
            continue
        jpg = np.frombuffer(raw_image, dtype=np.uint8)
        frame = cv2.imdecode(jpg, cv2.IMREAD_COLOR)
        if frame is None:
            continue

        # Resize and convert to RGB
        frame = cv2.resize(frame, (640, 480))
        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

        # Write raw RGB frame to GStreamer stdin
        gst_proc.stdin.write(rgb_frame.tobytes())

except KeyboardInterrupt:
    pass
finally:
    gst_proc.stdin.close()
    gst_proc.wait()
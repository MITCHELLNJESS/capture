import airsim
import cv2
import numpy as np
import pyvirtualcam
from pyvirtualcam import PixelFormat
import time

# Connect to AirSim
client = airsim.MultirotorClient()
client.confirmConnection()

# Display settings
WINDOW_NAME = "AirSim Camera Feed"
cv2.namedWindow(WINDOW_NAME, cv2.WINDOW_NORMAL)

with pyvirtualcam.Camera(width=640, height=480, fps=30, fmt=PixelFormat.BGR) as cam:
    print(f"Streaming to virtual camera: {cam.device}")
    while True:
        # Request scene image from camera "0"
        raw_image = client.simGetImage("a3", airsim.ImageType.Scene)
        
        if raw_image is None:
            print("No image returned, skipping...")
            time.sleep(0.1)
            continue

        # Decode the raw PNG image to numpy array
        img_array = np.frombuffer(raw_image, dtype=np.uint8)
        img = cv2.imdecode(img_array, cv2.IMREAD_COLOR)

        if img is not None:
            # Optionally resize
            img = cv2.resize(img, (640, 480))

            # Convert RGB to BGR (pyvirtualcam expects BGR)
            img = cv2.cvtColor(img, cv2.COLOR_RGB2BGR)

            cv2.imshow(WINDOW_NAME, img)

        # Send to virtual cam
        cam.send(img)
        cam.sleep_until_next_frame()

        # Exit on 'q' key
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

# Clean up
cv2.destroyAllWindows()
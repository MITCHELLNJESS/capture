import cv2
import time
import csv
import os

# Setup log directory
log_dir = "/Users/jfm/A3_system_project/capture/C2/A3_System/logs"
os.makedirs(log_dir, exist_ok=True)
log_file = os.path.join(log_dir, "bounding_box_log.csv")

# Webcam setup
camera = cv2.VideoCapture(0)
if not camera.isOpened():
    print("Unable to access webcam")
    exit()

bbox = None

# Create and open log file
with open(log_file, mode='w', newline='') as file:
    writer = csv.writer(file)
    writer.writerow(['Timestamp', 'BBox_X', 'BBox_Y', 'BBox_Width', 'BBox_Height', 'Latency(ms)'])

    while True:
        frame_start_time = time.time()

        # Capture frame
        ret, frame = camera.read()
        if not ret:
            print("Failed to capture frame")
            break

        # Instruction if bbox isn't set
        if bbox is None:
            cv2.putText(frame, "Press 's' to select bounding box, 'q' to quit.", (10, 30),
                        cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 255, 255), 2)
        else:
            # Draw bounding box
            x, y, w, h = map(int, bbox)
            cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)

            # Calculate timestamp and latency
            timestamp = time.strftime("%Y-%m-%d %H:%M:%S", time.localtime())
            latency = (time.time() - frame_start_time) * 1000  # milliseconds

            # Log details
            writer.writerow([timestamp, x, y, w, h, f"{latency:.2f}"])
            file.flush()

            # Print bounding box details
            print(f"Timestamp: {timestamp}, X: {x}, Y: {y}, Width: {w}, Height: {h}, Latency: {latency:.2f} ms")

        # Display frame
        cv2.imshow('A3 Asset Bounding Box', frame)

        # Keyboard interaction
        key = cv2.waitKey(1) & 0xFF
        if key == ord('q'):
            break
        elif key == ord('s'):
            bbox = cv2.selectROI('A3 Asset Bounding Box', frame, fromCenter=False, showCrosshair=True)
            cv2.destroyWindow('ROI selector')

# Cleanup
camera.release()
cv2.destroyAllWindows()
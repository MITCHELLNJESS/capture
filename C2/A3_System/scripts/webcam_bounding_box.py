import cv2
import time
import csv
import os

# Setup log directory
log_dir=r"C:\Users\hvomm\Desktop\ELDP\capture\C2\A3_System" 
#log_dir = "C:\Users\hvomm\Desktop\ELDP\capture\C2\A3_System"
os.makedirs(log_dir, exist_ok=True)
log_file = os.path.join(log_dir, "bounding_box_log_3A.csv")

# Video saving setup
video_output = os.path.join(log_dir, "output_video_insideA.mp4")
fourcc = cv2.VideoWriter_fourcc(*'mp4v')
out = None

# Webcam setup
camera = cv2.VideoCapture(0)
if not camera.isOpened():
    print("Unable to access webcam")
    exit()

bbox = None

# Create and open log file
with open(log_file, mode='w', newline='') as file:
    writer = csv.writer(file)
    writer.writerow(['Timestamp', 'BBox_X', 'BBox_Y', 'BBox_Width', 'BBox_Height', 'Center_X', 'Center_Y', 'Latency(ms)'])

    while True:
        frame_start_time = time.time()

        # Capture frame
        ret, frame = camera.read()
        if not ret:
            print("Failed to capture frame")
            break

        # Initialize video writer once frame dimensions are known
        if out is None:
            height, width = frame.shape[:2]
            out = cv2.VideoWriter(video_output, fourcc, 20.0, (width, height))

        # Instruction if bbox isn't set
        if bbox is None:
            cv2.putText(frame, "Press 's' to select bounding box, 'q' to quit.", (10, 30),
                        cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 255, 255), 2)
        else:
            # Draw bounding box
            x, y, w, h = map(int, bbox)

            # Adjust bounding box to be square
            side = max(w, h)
            w, h = side, side

            # Calculate center point
            center_x, center_y = x + w // 2, y + h // 2

            # Draw adjusted bounding box and center point
            cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 3)
            cv2.circle(frame, (center_x, center_y), 5, (255, 0, 0), -1)

            # Calculate timestamp and latency
            timestamp = time.strftime("%Y-%m-%d %H:%M:%S", time.localtime())
            latency = (time.time() - frame_start_time) * 1000  # milliseconds

            # Log details
            writer.writerow([timestamp, x, y, w, h, center_x, center_y, f"{latency:.2f}"])
            file.flush()

            # Print bounding box details
            print(f"Timestamp: {timestamp}, X: {x}, Y: {y}, Width: {w}, Height: {h}, Center: ({center_x},{center_y}), Latency: {latency:.2f} ms")

        # Write frame to video file
        out.write(frame)

        # Display frame
        cv2.imshow('A3 System', frame)

        # Keyboard interaction
        key = cv2.waitKey(1) & 0xFF
        if key == ord('q'):
            break
        elif key == ord('s'):
            bbox = cv2.selectROI('A3 System', frame, fromCenter=False, showCrosshair=True)
            #cv2.destroyWindow('ROI selector') #this may need to be commented out to avoid null pointer on Windows

# Cleanup
camera.release()
out.release()
cv2.destroyAllWindows()
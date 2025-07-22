import torch
import cv2
import time
import csv
from pathlib import Path

# load YOLOv5 model
model_path = Path("/Users/jfm/A3_system_project/capture/C2/A3_System/models/yolo/best_may25.pt")
model = torch.hub.load("ultralytics/yolov5", "custom", path=model_path, force_reload=True)
model.conf = 0.4

# initializing webcam
cap = cv2.VideoCapture(0)
if not cap.isOpened():
    print("Unable to access webcam")
    exit()

#setup logging
log_file = "bounding_box_yolo_log.csv"
with open(log_file, mode='w', newline='') as file:
    writer = csv.writer(file)
    writer.writerow(['Timestamp', 'Label', 'BBox_X', 'BBox_Y', 'BBox_Width', 'BBox_Height', 'Center_X', 'Center_Y', 'Latency(ms)'])

    while cap.isOpened():
        start_time = time.time()
        ret, frame = cap.read()
        if not ret:
            break

        #inference
        results = model(frame)
        preds = results.xyxy[0]  # x1, y1, x2, y2, conf, class

        #process predictions of bounding box
        for *box, conf, cls in preds:
            x1, y1, x2, y2 = map(int, box)
            width = x2 - x1
            height = y2 - y1
            center_x = (x1 + x2) // 2
            center_y = (y1 + y2) // 2
            label = model.names[int(cls)]

            # dection
            cv2.rectangle(frame, (x1, y1), (x2, y2), (0, 255, 0), 2)
            cv2.circle(frame, (center_x, center_y), 5, (255, 0, 0), -1)
            cv2.putText(frame, f"{label} ({conf:.2f})", (x1, y1 - 10),
                        cv2.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 2)

            # logging/printing output
            timestamp = time.strftime("%Y-%m-%d %H:%M:%S")
            latency = (time.time() - start_time) * 1000

            print(f"{timestamp} | Label: {label} | X: {x1}, Y: {y1}, W: {width}, H: {height}, Center: ({center_x}, {center_y})")
            print(f"Latency: {latency:.2f} ms")
            print("A3 Output:", f"{timestamp}, {x1}, {y1}, {width}, {height}, {center_x}, {center_y}, {latency:.2f}")

            # written to csv
            writer.writerow([timestamp, label, x1, y1, width, height, center_x, center_y, f"{latency:.2f}"])
            file.flush()

        # display
        cv2.imshow("A3 System", frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

# leanup
cap.release()
cv2.destroyAllWindows()

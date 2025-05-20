import torch
import cv2
import time
from pathlib import Path

# loading YOLOv5 model
model_path = Path("/Users/jfm/A3_system_project/yolov5/runs/train/exp4/weights/best.pt")
model = torch.hub.load("ultralytics/yolov5", "custom", path=model_path,force_reload=True)

model.conf = 0.4

#opening webcam
cap = cv2.VideoCapture(0)

while cap.isOpened():
    start_time = time.time()
    ret, frame = cap.read()
    if not ret:
        break

    # inference
    results = model(frame)
    preds = results.xyxy[0]     #x1,y1,x2,y2, conf, class

    # bounding box
    for *box, conf, cls in preds:
        x1, y1, x2, y2 = map(int, box)
        center_x = (x1 + x2) // 2
        center_y = (y1 + y2) // 2
        label = model.names[int(cls)]

        #drawing center
        cv2.rectangle(frame, (x1, y1), (x2, y2), (0, 255, 0), 2)
        cv2.circle(frame, (center_x, center_y), 5, (255, 0, 0), -1)
        cv2.putText(frame, f"{label} ({conf:.2f})", (x1, y1 - 10),
                    cv2.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 2)

        # print output
        timestamp = time.strftime("%Y-%m-%d %H:%M:%S")
        print(f"{timestamp} | Label: {label} | X: {x1}, Y: {y1}, W: {x2 - x1}, H: {y2 - y1}, Center: ({center_x}, {center_y})")

    latency = (time.time() - start_time) * 1000
    print(f"Latency: {latency:.2f} ms")

    # display
    cv2.imshow("A3 System", frame)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
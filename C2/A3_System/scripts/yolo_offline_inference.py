"""
yolo_offline_inference.py

Purpose:
Offline YOLOv5 inference for webcam input (no internet required). Produces real-time bounding box, label, confidence, 
centerpoint, and logs to CSV with timestamp and latency. 

This version is compatible with:
- Local YOLOv5 repo (must be cloned inside project directory)
- PyTorch model loaded directly from a .pt file in /models/yolo/
- No reliance on 'torch.hub' or online fetch

Dependencies:
- Python 3.8+ (Anaconda recommended)
- OpenCV
- PyTorch
- numpy
- yolov5 repo (local clone)

Expected Directory Structure:
- yolov5/       # Clone of YOLOv5 GitHub repo
- models/yolo/best.pt
- scripts/yolo_offline_inference.py

Author: Jessica Mitchell (A3 System)
Last updated: July 22, 2025
"""

import cv2
import time
import csv
import sys
import numpy as np
import torch
from pathlib import Path

#Add yolov5 to path
sys.path.append(str((Path(__file__).resolve().parents[1]) / "yolov5"))

from models.common import DetectMultiBackend
from utils.general import non_max_suppression, scale_boxes
from utils.torch_utils import select_device
from utils.augmentations import letterbox

# Load model
device = select_device("")
model_path = Path("/Users/jfm/A3_system_project/capture/C2/A3_System/models/yolo/best_may25.pt")
model = DetectMultiBackend(str(model_path), device=device)
model.eval()

# initializing ebcam
cap = cv2.VideoCapture(0)
if not cap.isOpened():
    print("Unable to access webcam")
    exit()

#class labels from trainings
names = model.names

# CSV
file = open("bounding_box_yolo_log.csv", "w", newline="")
writer = csv.writer(file)
writer.writerow(["Timestamp", "Label", "BBox_X", "BBox_Y", "BBox_Width", "BBox_Height", "Center_X", "Center_Y", "Latency(ms)"])

while True:
    start_time = time.time()
    ret, frame = cap.read()
    if not ret:
        break

    img = letterbox(frame, new_shape=640)[0]
    img = img[:, :, ::-1].transpose(2, 0, 1)  # BGR to RGB, to 3xHxW
    img = np.ascontiguousarray(img)
    img = torch.from_numpy(img).to(device).float()
    img /= 255.0
    if img.ndimension() == 3:
        img = img.unsqueeze(0)

    pred = model(img, augment=False, visualize=False)
    pred = non_max_suppression(pred, conf_thres=0.4, iou_thres=0.45)

    for det in pred:
        if det is not None and len(det):
            det[:, :4] = scale_boxes(img.shape[2:], det[:, :4], frame.shape).round()

            for *xyxy, conf, cls in reversed(det):
                x1, y1, x2, y2 = map(int, xyxy)
                label = f"{names[int(cls)]} ({conf:.2f})"
                width = x2 - x1
                height = y2 - y1
                center_x = x1 + width // 2
                center_y = y1 + height // 2

                # Draw box and label
                cv2.rectangle(frame, (x1, y1), (x2, y2), (0, 255, 0), 2)
                cv2.putText(frame, label, (x1, y1 - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (255, 255, 255), 2)
                cv2.circle(frame, (center_x, center_y), 2, (255, 0, 0), -1)

                latency = round((time.time() - start_time) * 1000, 2)
                timestamp = time.strftime("%Y-%m-%d %H:%M:%S", time.localtime())
                writer.writerow([timestamp, names[int(cls)], x1, y1, width, height, center_x, center_y, latency])
                print(f"A3 Output: {timestamp}, {x1}, {y1}, {width}, {height}, {center_x}, {center_y}, {label}, {latency} ms")

    cv2.imshow("A3 System", frame)
    if cv2.waitKey(1) == ord('q'):
        break

file.close()
cap.release()
cv2.destroyAllWindows()

# A3 System YOLOv5 Training Notes

**Date Completed:** May 19, 2025

## Overview

This document summarizes the process and key details for training the YOLOv5 model used in the A3 System for automatic asset detection and bounding box generation.

---

## Model Used
- **Base model:** YOLOv5s
- **Reason for switch:** YOLOv4-tiny was tested initially but produced inconsistent bounding boxes and high latency, unsuitable for real-time UAV integration.
- **Training Environment:** MacBook Pro M1
- **YOLOv5 Version:** v7.0 (Ultralytics repo)

---

## Dataset
- **Total Images:** ~350
- **Classes:**
  - `friendly_asset`
  - `enemy_asset`
- **Labeling Tool:** LabelImg
- **Format:** YOLO format (one `.txt` file per image with class index and normalized bbox values)

## Training Details

- **Command Used:**
  ```bash
  python train.py \
    --img 416 \
    --batch 2 \
    --epochs 100 \
    --data /Users/jfm/A3_system_project/capture/C2/A3_System/yolo_train.yaml \
    --weights yolov5s.pt
  ```

- **Training Duration:** ~12.5 hours (100 epochs)

---

## Results

- **friendly_asset:**
  - Precision: 96.2%
  - Recall: 98.4%

- **enemy_asset:**
  - Precision: 96.3%
  - Recall: 92.1%

- **mAP@0.5:** 0.74
- **Inference Speed:** ~1.44 iterations/sec



# Instruction for installing content:

All of the work was completed within a virtual environment, for work on windows please follow these steps:


Install python version: 3.10: https://www.python.org/downloads/release/python-31011/

pip install -r A3_system/requirements.txt


Troubleshooting:

if the initial requirements.txt install does not work with torch attempt this command

pip install torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu

### 1. Clone YOLOv5 Repository

```bash
git clone https://github.com/ultralytics/yolov5
cd yolov5
```

### 2. Install Dependencies

Activate your existing A3 environment or create one:

```bash
conda activate A3_system  # or use your existing environment
pip install -r requirements.txt
```
## Trained Model Weights
Final YOLOv5s model after 100 epochs:
- Download: https://drive.google.com/file/d/17j6hJ9RH1YQsoj40kcSW2N2lGlsUUZ3j/view?usp=sharing
- File: best.pt (14.3MB)



> If `best.pt` is not located here, update the path inside `yolo_live_inference.py`.

---

## Running the Inference Script

```bash
python yolo_live_inference.py
```

The script will:
- Open the webcam
- Run real-time inference with bounding boxes and center points
- Print bounding box data and latency to terminal
- Write results to `logs/bounding_box_log_yolo.csv`

Press `q` to quit the video window.

---

##  Output Format (Sample Log)

```
2025-05-19 14:22:01 | Label: friendly_asset | X: 152, Y: 104, W: 62, H: 62, Center: (183, 135)
Latency: 141.23 ms
```

---



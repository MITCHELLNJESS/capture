# A3 SYSTEM (Offline Inference Version)

**Working as of: 22 July 2025**

## Folder Structure

```
capture/
└── C2/
    └── A3_System/
        ├── scripts/
        │   └── yolo_offline_inference.py
        ├── models/
        │   └── yolo/
        │       └── best.pt  # Trained YOLOv5 checkpoint
        └── yolov5/          # Cloned yolov5 repo
```

## Installation Instructions (Windows & Mac)

### 1. Set up Python

Install Python 3.10.11 (recommended):  
https://www.python.org/downloads/release/python-31011/

### 2. Create a Virtual Environment

From the root A3_System folder:

```bash
python -m venv a3_env
```

Activate it:

- Windows: `a3_env\Scripts\activate`
- Mac/Linux: `source a3_env/bin/activate`

### 3. Install Dependencies

Install from the provided requirements file:

```bash
pip install -r requirements.txt
```

Note: If torch fails to install, manually install the CPU version (Windows):

```bash
pip install torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu
```

### 4. Clone YOLOv5 Repo

From inside A3_System:

```bash
git clone https://github.com/ultralytics/yolov5.git
cd yolov5
pip install -r requirements.txt
```

## Running the Inference Script Offline

Make sure you're inside the virtual environment, then run:

```bash
python scripts/yolo_offline_inference.py
```

This script will:
- Load your webcam
- Run YOLOv5 inference using the local models/yolo/best.pt
- Draw bounding boxes, label confidence, and center point
- Log output to a CSV file with the following headers:

```
Timestamp, Label, BBox_X, BBox_Y, BBox_Width, BBox_Height, Center_X, Center_Y, Latency
```

## Notes & Troubleshooting

- yolo_offline_inference.py is designed to avoid any need for internet access.
- If using a Mac with a Continuity Camera, ignore AVCaptureDeviceTypeExternal warnings.
- If the webcam fails to open or bounding boxes don't show, check that the model path is correct and weights are valid.

## Development Notes for Contributors

- All changes for offline support are in yolo_offline_inference.py.
- yolo_live_inference.py is older and may rely on online dependencies.
- Keep the YOLOv5 repo inside the A3_System directory.
- Model checkpoints are expected at:
  models/yolo/best.pt

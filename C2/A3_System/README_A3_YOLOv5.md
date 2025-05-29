# A3 YOLOv5 Setup (Windows, Python 3.10.11)

**Note:** This setup assumes you already have the core A3 system requirements installed and configured (e.g., Python 3.10.11, Git, VSCode, etc.).

This guide covers:

- Annotating images using LabelImg
- Training a YOLOv5 model
- Running live or batch inference with `yolo_live_inference.py`

---

## 1. Environment Setup

### 1.1. Create a virtual environment
```bash
python -m venv *any name you want*
yolov5_env\Scripts\activate
```

### 1.2. Upgrade pip
```bash
pip install --upgrade pip
```

---

## 2. Install YOLOv5 and Dependencies

### 2.1. Clone the YOLOv5 repository
```bash
git clone https://github.com/ultralytics/yolov5.git
cd yolov5
```

### 2.2. Install Python dependencies
```bash
pip install -r requirements.txt
pip install comet_ml
```

If `torch` fails to install from `requirements.txt`, see step 3.

---

## 3. Install PyTorch (CPU version for A3 and YOLO)

This version is known to work for the A3 system:

```bash
pip install torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu
```

### 3.1. Verify installations
```bash
pip show torch
pip show torchvision
pip show torchaudio
pip show opencv-python
pip show comet_ml
```

---

## 4. Install LabelImg for Annotation

### 4.1. Install LabelImg
```bash
pip install labelImg
```

### 4.2. Launch LabelImg
```bash
labelImg
```

### 4.3. Configure LabelImg
- Set annotation format to YOLO
- Save labels in the same directory as images
- One `.txt` file will be created per image with bounding box data

---

## 5. Prepare Dataset Structure

The overall project structure should include the existing A3 `capture/` folder and follow this layout:

```
project_root/
├── capture/                 <-- A3 system codebase
│   └── A3_System/
├── yolov5/                  <-- YOLOv5 cloned repo
├── custom_data.yaml         <-- dataset config file
├── images/
│   ├── train/
│   └── val/
├── labels/
│   ├── train/
│   └── val/
```

### Sample `custom_data.yaml`
```yaml
path: ../project_root
train: images/train
val: images/val

names:
  0: asset
  1: enemy_asset
```

- Place annotated images in `images/train/` and `images/val/`
- Save corresponding `.txt` files in `labels/train/` and `labels/val/`

---

## 6. Train YOLOv5 Model

Run the training command from inside the `yolov5/` directory:

```bash
python train.py --img 640 --batch 16 --epochs 100 --data ../custom_data.yaml --weights yolov5s.pt --name custom_yolov5
```

Alternatively, if you're using your A3 path and a smaller batch size for debugging or testing:

```bash
python train.py --img 416 --batch 2 --epochs 100 --data /Users/jfm/A3_system_project/capture/C2/A3_System/yolo_train.yaml --weights yolov5s.pt
```

- `--img`: Image size (416 or 640 are common, smaller is faster but less accurate)
- `--batch`: Number of images per batch (2 is used for memory-limited or debugging runs)
- `--data`: Path to dataset YAML file
- `--weights`: Base model weights
- `--name`: (Optional) Run name for output directory

Training results will be saved to:
```
runs/train/your_run_name/
```

---

## 7. Inference with `yolo_live_inference.py`

Make sure the script has the following:

- Path to trained weights (e.g., `runs/train/custom_yolov5/weights/best.pt`)
- Webcam source (`cv2.VideoCapture(0)`) or path to test images/videos

### 7.1. Install additional packages if needed
```bash
pip install numpy opencv-python
```

### 7.2. Verify key packages
```bash
pip show numpy
pip show opencv-python
pip show torch
```

---

## 8. Notes for A3 System

This setup will be used for:

- Training models to detect friendly and enemy assets
- Exporting bounding box and alignment information
- Supporting real-time detection output from a camera feed
- Providing detection results for downstream systems like Mission Planner

Ensure models are exported and stored in a known directory, with performance tested on the target camera stream prior to integration.

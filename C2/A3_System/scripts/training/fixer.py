import os
import shutil

label_val_dir = "~/A3_system_project/capture/C2/A3_System/labels/val"
image_source_dir = "~/A3_system_project/capture/C2/A3_System/data/raw_combined"
image_val_dir = "~/A3_system_project/capture/C2/A3_System/data/val"

os.makedirs(image_val_dir, exist_ok=True)

copied = 0
for label_file in os.listdir(label_val_dir):
    if label_file.endswith(".txt"):
        base = os.path.splitext(label_file)[0]
        for ext in [".jpg", ".jpeg", ".png"]:
            image_path = os.path.join(image_source_dir, base + ext)
            if os.path.exists(image_path):
                shutil.copy(image_path, os.path.join(image_val_dir, base + ext))
                copied += 1
                break
        else:
            print(f"Missing image for label: {label_file}")

print(f"Copied {copied} validation images to data/val/")

import os
import random
import shutil

# Source folders
image_dir = "~/A3_system_project/capture/C2/A3_System/data/raw_combined"
label_dir = "~/A3_system_project/capture/C2/A3_System/labels"

# Target folders
train_img_dir = "~/A3_system_project/capture/C2/A3_System/data/train"
val_img_dir = "~/A3_system_project/capture/C2/A3_System/data/val"
train_lbl_dir = "~/A3_system_project/capture/C2/A3_System/labels/train"
val_lbl_dir = "~/A3_system_project/capture/C2/A3_System/labels/val"

# Create target folders if they don't exist
for d in [train_img_dir, val_img_dir, train_lbl_dir, val_lbl_dir]:
    os.makedirs(d, exist_ok=True)

# Collect all labeled images
images = [f for f in os.listdir(image_dir) if f.endswith(('.jpg', '.jpeg', '.png'))]
random.shuffle(images)

# 80/20 split
split_index = int(0.8 * len(images))
train_images = images[:split_index]
val_images = images[split_index:]

def move_files(image_list, target_img_dir, target_lbl_dir):
    for img in image_list:
        name, _ = os.path.splitext(img)
        label_file = f"{name}.txt"

        # Move image
        shutil.copy(os.path.join(image_dir, img), os.path.join(target_img_dir, img))

        # Move corresponding label
        label_src = os.path.join(label_dir, label_file)
        if os.path.exists(label_src):
            shutil.copy(label_src, os.path.join(target_lbl_dir, label_file))
        else:
            print(f"Label missing for {img}, skipping.")

# Move files
move_files(train_images, train_img_dir, train_lbl_dir)
move_files(val_images, val_img_dir, val_lbl_dir)

print(f"Split complete: {len(train_images)} train, {len(val_images)} val")

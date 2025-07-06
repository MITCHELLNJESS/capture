import torch
import tensorflow as tf

print("PyTorch Version:", torch.__version__)
print("MPS (Metal) Available:", torch.backends.mps.is_available())
print("MPS (Metal) Built:", torch.backends.mps.is_built())

print("TensorFlow Version:", tf.__version__)
print("Num GPUs Available:", len(tf.config.list_physical_devices('GPU')))
print("GPU Details:", tf.config.list_physical_devices('GPU'))

Instruction for installing content:

All of the work was completed within a virtual environment, for work on windows please follow these steps:

1. Install python windows virtual environment from here: https://github.com/pyenv-win/pyenv-win
Install python version: 3.10

pip install -r A3_system/requirements.txt


Troubleshooting:

if the initial requirements.txt install does not work with torch attempt this command

pip install torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu

Set-Location -Path $PSScriptRoot

Start-Process -FilePath "$PSScriptRoot\MissionPlanner\bin\Debug\net461\MissionPlanner.exe"

if (-not (Test-Path "$PSScriptRoot\a3_env")) {
    python -m venv "$PSScriptRoot\a3_env"
    "$PSScriptRoot\a3_env\Scripts\activate"
    pip install -r "$PSScriptRoot\A3_System\requirements.txt"
    pip install torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu
}
else {
    "$PSScriptRoot\a3_env\Scripts\activate"
}
python "$PSScriptRoot\A3_System\scripts\yolo_offline_inference.py"
deactivate
Pause
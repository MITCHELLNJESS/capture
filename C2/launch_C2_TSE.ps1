Set-Location -Path $PSScriptRoot

Start-Process -FilePath "$PSScriptRoot\MissionPlanner\bin\Debug\net461\MissionPlanner.exe"

if (-not (Test-Path "$PSScriptRoot\a3_env_tse")) {
    python -m venv "$PSScriptRoot\a3_env_tse"
    "$PSScriptRoot\a3_env_tse\Scripts\activate"
    pip install -r "$PSScriptRoot\A3_System\requirements.txt"
    pip install torch torchvision torchaudio --extra-index-url https://download.pytorch.org/whl/cpu
    pip install msgpack-rpc-python
    pip install airsim
}
else {
    "$PSScriptRoot\a3_env_tse\Scripts\activate"
}
python "$PSScriptRoot\A3_System\scripts\tse_run_yolo.py"
deactivate
Pause
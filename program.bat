@echo off
set QUARTUS_PATH=C:\intelFPGA_lite\20.1\quartus\bin64
set PROJECT_PATH=C:\project_folder
set DEVICE=device_name
set SOF_FILE=test.sof

cd %PROJECT_PATH%

%QUARTUS_PATH%\quartus_pgm.exe -m jtag -o "p;%DEVICE%;%SOF_FILE%"

pause
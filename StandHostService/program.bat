@echo off
set QUARTUS_PATH=C:\\intelFPGA_lite\\17.1\\quartus\\bin64
set PROJECT_PATH=C:\\Users\\Administrator\\Documents\\Project1271\\rmfpga\\StandHostService\\bin\\Debug\\net7.0
set DEVICE=de10_lite
set SOF_FILE=program.sof

cd %PROJECT_PATH%

%QUARTUS_PATH%\quartus_pgm.exe -m jtag -o "p;%DEVICE%;%SOF_FILE%"

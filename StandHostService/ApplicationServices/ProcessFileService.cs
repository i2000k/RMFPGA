using System.Diagnostics;

namespace StandHostService.ApplicationServices;


public class ProcessFileService
{
    public bool ProcessFile(byte[] data)
    {
        // TODO: Upload File to Board using .bat file
        var filePath = Directory.GetCurrentDirectory() + @"\program.sof";
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            // Write the bytes to the file stream
            fileStream.Write(data, 0, data.Length);
        }

        // Path to the batch file
        string batchFilePath = Directory.GetCurrentDirectory() + @"\program.bat";

        // Create a new process start info object
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "cmd.exe"; // Use the command prompt to execute the batch file
        startInfo.Arguments = "/c " + batchFilePath; // Pass the batch file path as an argument to the command prompt
        startInfo.CreateNoWindow = true; // Hide the command prompt window
        startInfo.UseShellExecute = false; // Don't use the operating system shell to start the process

        // Start the process
        Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit(); // Wait for the process to exit

        if (process.ExitCode != 0)
        {
            return false;
        }

        return true;
    }
}
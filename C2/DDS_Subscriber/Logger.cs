using System;
using System.IO;

public class Logger
{
    private string logFilePath = "C:\\Users\\hvomm\\Desktop\\ELDP" +
        "\\capture\\C2\\DDS_Subscriber\\log.txt"; //"log.txt";  // 

    public void WriteDebug(string message)
    {
        // Create or append to the log file
        try
        {
            // Check if file exists, if not, it will be created automatically
            using (StreamWriter sw = new StreamWriter(logFilePath, true)) // 'true' ensures it appends
            {
                // Write the message along with the current date and time
                sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing to log file: {ex.Message}");
        }
    }
}

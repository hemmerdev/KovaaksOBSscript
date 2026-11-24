using System.Diagnostics;

Console.WriteLine("Script started");
while (true)
{
    Process[] kovaaks = Process.GetProcessesByName("FPSAimTrainer-Win64-Shipping");
    Process[] obs = Process.GetProcessesByName("obs64");

    // If Kovaaks is running and OBS is not, start OBS with the replay buffer enabled
    if (kovaaks.Length > 0 && obs.Length == 0)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = @"C:\Program Files\obs-studio\bin\64bit\obs64.exe",
            WorkingDirectory = @"C:\Program Files\obs-studio\bin\64bit",
            Arguments = "--startreplaybuffer"
        };

        Process.Start(startInfo);

        Console.WriteLine("OBS started!");
    }

    // If Kovaaks is not running and OBS is, stop OBS
    if (kovaaks.Length == 0 && obs.Length > 0)
    {
        foreach (Process process in obs)
        {
            try
            {
                if (process.CloseMainWindow())
                {
                    if (process.WaitForExit(5000))
                    {
                        Console.WriteLine("OBS closed gracefully.");
                    }
                    else
                    {
                        Console.WriteLine("OBS did not close in time, manually close it.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing OBS: {ex.Message}");
            }
        }
    }

    Thread.Sleep(5000);
}
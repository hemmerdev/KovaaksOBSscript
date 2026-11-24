using System.Diagnostics;
using System.Windows;

Console.WriteLine("Script started");

// Like a list but with no duplicates. Used to keep track of OBS processes that the user has chosen to ignore to avoid duplicate error boxes.
HashSet<int> ignoredProcesses = new HashSet<int>();
while (true)
{
    Process[] kovaaks = Process.GetProcessesByName("FPSAimTrainer-Win64-Shipping");
    Process[] obs = Process.GetProcessesByName("obs64");

    // If Kovaaks is running, clear the ignored processes list
    if (kovaaks.Length > 0)
    {
        ignoredProcesses.Clear();
    }

    // Remove any processes from the ignored list that are no longer running
    ignoredProcesses.RemoveWhere(
        id => !Array.Exists(obs, process => process.Id == id));

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
            if (ignoredProcesses.Contains(process.Id))
            {
                continue;
            }

            try
            {
                if (process.CloseMainWindow() && process.WaitForExit(5000))
                {
                    Console.WriteLine("OBS closed gracefully.");
                }
                else
                {
                    if (!AskToForceClose(process))
                    {
                        ignoredProcesses.Add(process.Id);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                $"Error closing OBS: {ex.Message}",
                "OBS Automation Error");
            }
        }
    }

    Thread.Sleep(5000);
}

static bool AskToForceClose(Process process)
{
    MessageBoxResult result = MessageBox.Show(
       "OBS could not be closed gracefully.\n\n" +
       "Do you want to force close OBS?\n\n" +
       "Yes: Force close OBS.\n" +
       "No: Close OBS manually.",
       "Kovaaks OBS Automation",
       MessageBoxButton.YesNo,
       MessageBoxImage.Warning,
       MessageBoxResult.No);

    if (result == MessageBoxResult.Yes)
    {
        if (!process.HasExited)
        {
            process.Kill();
        }

        return true;
    }

    return false;
}
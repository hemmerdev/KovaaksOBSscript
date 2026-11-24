using System.Diagnostics;

while (true)
{
    Process[] kovaaks = Process.GetProcessesByName("FPSAimTrainer-Win64-Shipping");
    Process[] obs = Process.GetProcessesByName("obs64");

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

    Thread.Sleep(5000);
}
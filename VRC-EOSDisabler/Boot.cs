using System.Diagnostics;
using System.Security.Principal;
using EOSDisabler.Wrappers;

namespace EOSDisabler
{
    internal class Boot
    {
        public static void Main()
        {
            Console.Title = "ANTI EOS BY UMBRA";

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {

                ProcessStartInfo startInfo = new()
                {
                    FileName = Environment.GetCommandLineArgs()[0],
                    UseShellExecute = true,
                    Verb = "runas",
                    Arguments = "/runas"
                };

                Process.Start(startInfo);
                return;
            }

            ToggleHostBlock(false);

            ScanLog();
        }

        private static void ToggleHostBlock(bool State)
        {
            string hostsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts");
            string targetDomain = "0.0.0.0 api.epicgames.dev";
            List<string> savedFile = File.ReadAllLines(hostsFilePath).ToList();

            if (State)
            {
                if (savedFile.Contains(targetDomain)) return;
                savedFile.Add(targetDomain);
                Logger.LogSuccess("Anti enabled, EOS connection is now blocked");
            }
            else
            {
                if (!savedFile.Contains(targetDomain)) return;
                savedFile.Remove(targetDomain);
                Logger.LogSuccess("Anti disabled, EOS connection is now unblocked");
            }

            File.WriteAllLines(hostsFilePath, savedFile);
        }

        private static void ScanLog()
        {
            var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"Low\VRChat\VRChat");
            if (directory != null && directory.Exists)
            {
                FileInfo target = null;
                string[] oldFileNames = directory.GetFiles("output_log_*.txt", SearchOption.TopDirectoryOnly).Select(file => file.Name).ToArray();

                Logger.Log("Waiting for VRChat Log");
                while (target == null)
                {
                    foreach (FileInfo File in directory.GetFiles("output_log_*.txt", SearchOption.TopDirectoryOnly))
                    {
                        if (!oldFileNames.Contains(File.Name))
                        {
                            target = File; 
                            break;
                        }
                    }
                    Thread.Sleep(1);
                }

                Process VRCProcs = Utils.GetProcessByName("VRChat");
                if (VRCProcs != null)
                {
                    Logger.Log($"Watching VRChat Process [{target.Name}]");

                    ReadNewLines(target.FullName);

                    while (!VRCProcs.HasExited)
                    {
                        ReadLog(target.FullName);
                        Thread.Sleep(1);
                    }

                    ToggleHostBlock(false);
                }
            }
        }


        private static void ReadLog(string Path)
        {
            var lines = ReadNewLines(Path);

            foreach (var line in lines)
            {
                if (line.Contains("[EOSManager] [Info][LogEOS] SDK Config Product Update Request Successful"))
                {
                    ToggleHostBlock(true);
                }
            }
        }

        private static List<string> ReadNewLines(string filePath)
        {
            List<string> lines = new();

            try
            {
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);

                reader.BaseStream.Seek(LastReadOffset, SeekOrigin.Begin);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                LastReadOffset = reader.BaseStream.Position;
            }
            catch (IOException ex)
            {
                Logger.LogError(ex);
            }

            return lines;
        }

        private static long LastReadOffset = 0;
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace RemoteControlBot
{
    public static class ProcessManager
    {
        public static int ProcessesCount => _processes.Count;

        public static bool IsSuccessfulLastKill { get; private set; }
        public static bool IsLastKillSystemProcess { get; private set; }

        public static bool IsValidLastIndex { get; private set; }

        private static readonly List<Process> _processes;
        public static ReadOnlyCollection<Process> Processes => _processes.AsReadOnly();

        static ProcessManager()
        {
            _processes = new List<Process>();
        }

        public static void ScanProcesses()
        {
            if (_processes.Count != 0)
                _processes.Clear();

            foreach (var process in Process.GetProcesses())
                if (!IsProcessHidden(process))
                    _processes.Add(process);
        }

        public static void StartProcess(string filepath, string args, bool createNoWindow)
        {
            var processInfo = new ProcessStartInfo(filepath, args)
            {
                CreateNoWindow = createNoWindow,
                UseShellExecute = false
            };

            Process.Start(processInfo);
        }

        public static void TryKillProcessByIndex(int index)
        {
            try
            {
                if (index < _processes.Count)
                {
                    IsValidLastIndex = true;

                    if (!IsProcessExited(index))
                    {
                        KillProcessByIndex(index);
                        _processes.RemoveAt(index);
                        IsSuccessfulLastKill = true;
                    }
                    else
                        IsSuccessfulLastKill = false;
                }
                else
                    IsValidLastIndex = false;

                IsLastKillSystemProcess = false;
            }
            catch (Win32Exception)
            {
                IsLastKillSystemProcess = true;
            }
        }

        private static void KillProcessByIndex(int index)
        {
            _processes[index].Kill();
        }

        private static bool IsProcessHidden(Process process)
        {
            return string.IsNullOrEmpty(process.MainWindowTitle);
        }

        private static bool IsProcessExited(int index)
        {
            try
            {
                var process = Process.GetProcessById(_processes[index].Id);
                return process.HasExited;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }
    }
}

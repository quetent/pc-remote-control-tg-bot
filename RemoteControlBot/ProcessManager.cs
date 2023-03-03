﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace RemoteControlBot
{
    public static class ProcessManager
    {
        public static int VisibleProcessesCount => _visibleProcesses.Count;

        public static bool IsSuccessfulLastKill { get; private set; }
        public static bool IsLastKillSystemProcess { get; private set; }

        public static bool IsValidLastIndex { get; private set; }

        private static readonly List<Process> _visibleProcesses;
        public static ReadOnlyCollection<Process> VisibleProcesses => _visibleProcesses.AsReadOnly();

        static ProcessManager()
        {
            _visibleProcesses = new List<Process>();
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

        public static void SetVisibleProcceses()
        {
            if (_visibleProcesses.Count != 0)
                _visibleProcesses.Clear();

            foreach (var process in Process.GetProcesses())
                if (!IsProcessHidden(process))
                    _visibleProcesses.Add(process);
        }

        public static void TryKillProcessByIndex(int index)
        {
            try
            {
                if (index < _visibleProcesses.Count)
                {
                    IsValidLastIndex = true;

                    if (!IsProcessExited(index))
                    {
                        KillProcessByIndex(index);
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
            _visibleProcesses[index].Kill();
        }

        private static bool IsProcessHidden(Process process)
        {
            return string.IsNullOrEmpty(process.MainWindowTitle);
        }

        private static bool IsProcessExited(int index)
        {
            try
            {
                var process = Process.GetProcessById(_visibleProcesses[index].Id);
                return process.HasExited;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }
    }
}

using System.Diagnostics;

namespace RemoteControlBot
{
    internal static class ProcessManager
    {
        public static int VisibleProcessesCount { get { return _visibleProcesses.Count; } }
        public static bool IsSuccessfulKill { get; private set; }

        private static readonly List<Process> _visibleProcesses;
        public static List<Process> VisibleProcesses
        {
            get
            {
                SetVisibleProcceses();
                return _visibleProcesses.Copy();
            }
        }


        static ProcessManager()
        {
            _visibleProcesses = new List<Process>();
        }

        internal static void SetVisibleProcceses()
        {
            if (_visibleProcesses.Count != 0)
                _visibleProcesses.Clear();

            foreach (var process in Process.GetProcesses())
            {
                if (!IsProcessHidden(process))
                    _visibleProcesses.Add(process);
            }
        }

        internal static void KillProcess(int index)
        {
            try
            {
                if (index < _visibleProcesses.Count)
                    _visibleProcesses[index].Kill();

                IsSuccessfulKill = true;
            }
            catch
            {
                IsSuccessfulKill = false;
            }
        }

        private static bool IsProcessHidden(Process process)
        {
            return string.IsNullOrEmpty(process.MainWindowTitle);
        }
    }
}

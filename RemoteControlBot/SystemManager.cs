using System.Runtime.InteropServices;

namespace RemoteControlBot
{
    internal static class SystemManager
    {
        internal static bool IsInputBlocked { get; private set; }
        internal static bool IsInputBlockOperationSuccessful { get; private set; }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

        internal static void BlockInput()
        {
            if (!IsInputBlocked)
            {

                BlockInput(true);

                IsInputBlocked = true;
                IsInputBlockOperationSuccessful = true;

                Console.WriteLine($"--------- block {IsInputBlocked}");
                return;
            }

            IsInputBlockOperationSuccessful = false;
        }

        internal static void UnblockInput()
        {
            if (IsInputBlocked)
            {
                BlockInput(false);

                IsInputBlocked = false;
                IsInputBlockOperationSuccessful = true;

                Console.WriteLine($"---------------- ublock {IsInputBlocked}");
                return;
            }

            IsInputBlockOperationSuccessful = false;
        }
    }
}

namespace EnergyTrading.Console
{
    using System;
    using System.Runtime.InteropServices;

    public static class ConsoleUtils
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SwHide = 0;
        private const int SwShow = 5;

        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();
            // Show
            ShowWindow(handle, SwShow);
        }

        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();
            // Hide
            ShowWindow(handle, SwHide);
        }
    }
}
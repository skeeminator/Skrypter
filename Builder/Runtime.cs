using System;
using System.Threading;
using System.Windows.Forms;

namespace Builder
{
    internal static class Runtime
    {
        static Mutex mutex = new Mutex(true, "SilentBinder");

        [STAThread]
        static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                MessageBox.Show("Program has already started!", "Inform");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SilentForm());
            mutex.ReleaseMutex();
        }
    }
}

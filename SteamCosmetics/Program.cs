#define EXCEPTION_HANDLER

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SteamCosmetics
{
    static class Program
    {
        private static void ExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            string exceptionData = e.Exception.ToString();

            try
            {
                string tempFolder = Path.GetTempPath();
                string logFile = Path.Combine(tempFolder, $"[{DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss")}] Steam Cosmetics Fatal Error.txt");

                File.WriteAllText(logFile, exceptionData);

                using (Process textviewer = Process.Start(new ProcessStartInfo(logFile)))
                {
                    textviewer.Dispose();
                }
            }
            catch
            {
                MessageBox.Show(exceptionData, "Steam Cosmetics Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }




        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


#if EXCEPTION_HANDLER
            Application.ThreadException += new ThreadExceptionEventHandler(ExceptionHandler);
            try
            {
                Application.Run(new Form_Main());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Cursed Market Application.Run() Failed", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
#else
            Application.Run(new Form_Main());
#endif
        }
    }
}

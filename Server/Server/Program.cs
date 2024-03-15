using System;
using System.Globalization;

namespace Server
{
    static class Program
    {
        static System.Timers.Timer timer;
        public static NetServer server;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            server = new NetServer();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            timer = new System.Timers.Timer(150);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Start();

            Console.ReadKey();

            //use points for floats for easy compatibility with coordinates
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            server.Stop();
        }
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //тик сервера
            Program.server.Iterate();
        }
    }
}

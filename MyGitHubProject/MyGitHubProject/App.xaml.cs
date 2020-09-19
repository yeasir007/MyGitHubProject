using log4net;
using Microsoft.Shell;
using MyGitHubProject.Enums;
using MyGitHubProject.LogManagers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MyGitHubProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application,ISingleInstanceApp
    {
        private const string MyAppUniqueGUID = "MyGitHubProject{YEASIR00-7DOT-BLOG-SPOT-DOTCOM06LOL7}";
        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(MyAppUniqueGUID))
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();
                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        #region ISingleInstanceApp Members
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if ((MainWindow.WindowState == WindowState.Minimized))
            {
                MainWindow.WindowState = WindowState.Normal;
            }
            if (MainWindow.Visibility != Visibility.Visible)
            {
                MainWindow.UpdateLayout();
                MainWindow.Visibility = Visibility.Visible;
            }

            MainWindow.Activate();
            MainWindow.Focus();
            MainWindow.Topmost = true;

            return true;
        }
        #endregion

        private static readonly ILog Log = LogManager.GetLogger(typeof(App));
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                ConfigureLoggerManager();
                                
                //Checking commandline argument in WPF application
                string commandLineArg = e.Args.Length == 1 ? e.Args[0].ToString() : "";
            }
            catch (Exception ex)
            {
                App.Log.Info( Enum.GetName( typeof( LogLevel ), LogLevel.FATAL ), ex );
                Shutdown();
            }
        }

        /// <summary>
        /// Override OnExit to dispose other contoller classes if needed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            LOG(LogLevel.TRACE, "App exit has been called.");
            base.OnExit(e);
        }

        private void ConfigureLoggerManager()
        {
            LogHandler.Configure(); //Configure log4net 
            LOG("****************** Start My Application Logging  **********************");

            AppDomain.CurrentDomain.UnhandledException +=
            (sender, args) =>
            {
                LOG(LogLevel.FATAL, $"Exception from all threads in the AppDomain : {args.ExceptionObject}");
            };

            Dispatcher.UnhandledException +=
            (sender, args) =>
            {
                LOG(LogLevel.FATAL, $"Exception from a single specific UI dispatcher thread : {args.Exception}");
            };

            Application.Current.DispatcherUnhandledException +=
            (sender, args) =>
            {
                LOG(LogLevel.FATAL, $"Exception from the main UI dispatcher thread in WPF application : {args.Exception}");
            };

            TaskScheduler.UnobservedTaskException +=
            (sender, args) =>
            {
                LOG(LogLevel.FATAL, $"Exception from within each AppDomain that uses a task scheduler for asynchronous operations. : {args.Exception}");
            };
        }

        /// <summary>
        /// Record app maintaing log
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="lineNumber"></param>
        /// <param name="caller"></param>
        public static void LOG(string msg, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null)
        {
            Log.Info($"{caller} {lineNumber} : {msg}");
        }

        /// <summary>
        /// Record app maintaing log
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="msg"></param>
        /// <param name="lineNumber"></param>
        /// <param name="caller"></param>
        public static void LOG(LogLevel logLevel, string msg, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null)
        {
            string message = String.Empty;

            switch (logLevel)
            {
                case LogLevel.ERROR:
                    message = "### ERROR ###";
                    break;
                case LogLevel.DEBUG:
                    message = "### DEBUG ###";
                    break;
                case LogLevel.FATAL:
                    message = "### FATAL ###";
                    break;
                case LogLevel.INFO:
                    message = "### INFO ###";
                    break;
                case LogLevel.TRACE:
                    message = "### TRACE ###";
                    break;
                case LogLevel.WARN:
                    message = "### WARN ###";
                    break;
                default:
                    break;
            }
            
            message = $"{message}: {caller} {lineNumber} : {msg}";

            if (logLevel == LogLevel.DEBUG)
            {
                Console.WriteLine(message);
            }                
            
            Log.Info(message);
        }
    }
}

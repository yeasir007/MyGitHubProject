using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGitHubProject.LogManagers
{
    public class LogHandler
    {
        private static bool isSetupLogHandler;
        
        public LogHandler()
        {
            isSetupLogHandler = false;
        }
        ~LogHandler()
        {

        }
                
        public static void Configure()
        {
            if (isSetupLogHandler)
                return;
            isSetupLogHandler = true;

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = true;
            roller.File = Environment.ExpandEnvironmentVariables(@"%AppData%\MyGitHubProject\UseRegistryInWPF\UseRegistryInWPFLog.txt");
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 20;
            roller.MaximumFileSize = "20MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
        }
    }
}

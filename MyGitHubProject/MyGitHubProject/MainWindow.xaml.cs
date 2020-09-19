using MyGitHubProject.Enums;
using MyGitHubProject.RegistryUtil;
using System;
using System.IO;
using System.Windows;

namespace MyGitHubProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            App.LOG(LogLevel.TRACE, $"[+]");

            InitializeComponent();

            string appFileName = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            RegistryUtils.SetInstallPath(appFileName);

            App.LOG(LogLevel.TRACE, $"[-]");
        }
    }
}

using Microsoft.Win32;
using System;
using System.Globalization;
using System.Threading;
using DWORD = System.Int32;
namespace MyGitHubProject.RegistryUtil
{
    public class RegistryUtils
    {
        #region Registry Paths
        const string RegPathDefault = "Software\\MyGitHubProject\\UseRegistryInWPF";
        const string RegPathDesktop = "Control Panel\\Desktop";
        const string RegPathWindowsDefaultBrowser = "Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\http\\UserChoice";
        const string RegPathMicrosoftCurrentUserDownload = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders";
        const string RegPathImmersiveShell = "Software\\Microsoft\\Windows\\CurrentVersion\\ImmersiveShell";
        #endregion

        #region Registry Keys
        const string ApplicationExecuteFileName = "Application.exe";
        const string RegKeyHKCUDownload = "{374DE290-123F-4565-9164-39C4925E467B}";
        const string RegKeyCmdArgs = "CmdArgs";
        const string RegKeyVersion = "Version";
        const string RegKeyInstallPath = "InstallPath";
        const string RegKeyRegion = "Region";
        const string RegKeyIsLanguageRTL = "IsLanguageRTL";
        const string RegKeyLanguage = "Language";
        #endregion

        public RegistryUtils()
        {

        }
        ~RegistryUtils()
        {

        }

        #region Method Implementation
        public static void CreateSettingRegistryKey()
        {
            RegistryManager.CreateKey(RegPathDefault);
        }
        
        public static void SetCommandArguments(string args)
        {
            RegistryManager.SetValue<string>(RegPathDefault, RegKeyCmdArgs, args, RegistryValueKind.String);
        }
        
        public static string GetCommandArguments()
        {
            return RegistryManager.GetValue<string>(RegPathDefault, RegKeyCmdArgs, RegistryValueKind.String);
        }
        
        public static string GetRegion()
        {
            string retString = RegistryManager.GetValue<string>(RegPathDefault, RegKeyRegion, RegistryValueKind.String);

            if (String.IsNullOrEmpty(retString))
            {
                RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.LCID);

                RegistryManager.SetValue<string>(RegPathDefault, RegKeyRegion, regionInfo.Name, RegistryValueKind.String);
                retString = regionInfo.Name;
            }

            return retString;
        }

        public static void SetLanguage(string language)
        {
            RegistryManager.SetValue<string>(RegPathDefault, RegKeyLanguage, language, RegistryValueKind.String);
        }

        public static void SetSupportRTL(DWORD value)
        {
            RegistryManager.SetValue(RegPathDefault, RegKeyIsLanguageRTL, value, RegistryValueKind.DWord);
        }

        public static void SetInstallPath(string installPath)
        {
            RegistryManager.SetValue<string>(RegPathDefault, RegKeyInstallPath, installPath, RegistryValueKind.String);
        }

        public static string GetInstallPath(string installPath)
        {
             return RegistryManager.GetValue<string>(RegPathDefault, RegKeyInstallPath,RegistryValueKind.String);
        }

        public static string GetLanguage()
        {
            string strLanguageCode = Thread.CurrentThread.CurrentCulture.Name;
            SetLanguage(strLanguageCode);
            return strLanguageCode;
        }
        
        public static bool IsLanguageRTL()
        {
            if (Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft)
            {
                SetSupportRTL(1);
                return true;
            }

            SetSupportRTL(0);
            return false;
        }

        public static bool Is64bitOS()
        {
            return Environment.Is64BitOperatingSystem;
        }

        #endregion
    }
}
using Microsoft.Win32;
using MyGitHubProject.Enums;
using System;
using DWORD = System.Int32;

namespace MyGitHubProject.RegistryUtil
{
    public class RegistryManager
    {
        #region Variables
        const string USER_ROOT_HKCU = "HKEY_CURRENT_USER";
        const string USER_ROOT_HKLM = "HKEY_LOCAL_MACHINE";
        #endregion
        public RegistryManager()
        { 
                
        }
        ~RegistryManager()
        {

        }

        /// <summary>
        /// Get Registry Item
        /// </summary>
        /// <param name="subkey">Registry path</param>
        /// <param name="registryItem">Registry Item to get</param>
        /// <returns></returns>
        public static DWORD GetValue(string subkey, string registryItem)
        {
            string keyName = USER_ROOT_HKCU + "\\" + subkey;

            return (DWORD)Registry.GetValue(keyName, registryItem, -1);
        }

        /// <summary>
        /// Get generic type value
        /// </summary>
        /// <typeparam name="Object">Generic type. String/bool/int etc</typeparam>
        /// <param name="subkey">Registry path</param>
        /// <param name="registryItem">Registry Item to get</param>
        /// <param name="registryValueKind">Over loading parameter</param>
        /// <returns></returns>
        public static T GetValue<T>(string subkey, string registryItem, RegistryValueKind registryValueKind)
        {
            T retrunVal = default(T);
            try
            {
                string keyName = USER_ROOT_HKCU + "\\" + subkey;
                retrunVal = (T)Registry.GetValue(keyName, registryItem, null);//String.Emptyty
            }
            catch (Exception ex)
            {
                App.LOG(LogLevel.ERROR, $"Exception : {ex.Message}");
                return default(T);
            }

            return retrunVal;
        }


        /// <summary>
        /// Get generic type value
        /// </summary>
        /// <typeparam name="T">Generic type. String/bool/int etc</typeparam>
        /// <param name="subkey">Registry path</param>
        /// <param name="registryItem">Registry Item to get</param>
        /// <param name="registryHive">Type of registry space HKLM or HKCU</param>
        /// <returns></returns>
        public static T GetValue<T>(string subkey, string registryItem, RegistryHive registryHive)
        {
            string keyName = String.Empty;
            
            T retrunVal = default(T);
            
            try
            {
                if (registryHive == RegistryHive.LocalMachine)
                {
                    keyName = USER_ROOT_HKLM + "\\" + subkey;
                }
                else
                {
                    keyName = USER_ROOT_HKCU + "\\" + subkey;
                }

                retrunVal = (T)Registry.GetValue(keyName, registryItem, null);//String.Empty
            }
            catch (Exception ex)
            {
                retrunVal = default(T);
                App.LOG(LogLevel.ERROR, $"Exception : {ex.Message}");
            }

            return retrunVal;
        }

        /// <summary>
        /// Set Registry Item with value
        /// </summary>
        /// <typeparam name="T">Retis</typeparam>
        /// <param name="subkey">Registry path</param>
        /// <param name="registryItem">Registry Item to be added</param>
        /// <param name="registryValue">Registry Item value</param>
        /// <param name="valueKind">Registry value type.</param>
        /// <returns></returns>
        public static DWORD SetValue<T>(string subkey, string registryItem, T registryValue, RegistryValueKind valueKind = RegistryValueKind.DWord)
        {
            string keyName = USER_ROOT_HKCU + "\\" + subkey;

            try
            {
                Registry.SetValue(keyName, registryItem, registryValue, valueKind);
                return 1;
            }
            catch (Exception ex)
            {
                App.LOG(LogLevel.ERROR, $"Exception : {ex.Message}");
            }

            return 0;
        }

        /// <summary>
        /// Create a Sub key path in the registry
        /// </summary>
        /// <param name="key">Sub key registry path</param>
        public static void CreateKey(string key)
        {
            try
            {
                Registry.CurrentUser.CreateSubKey(key);
            }
            catch (Exception ex)
            {
                App.LOG(LogLevel.ERROR, $"Exception : {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subkey"> Registry path</param>
        /// <param name="registryItem">Registry Item to be deleted</param>
        /// <param name="rkey">Registry.CurrentUser/Registry.LocalMachine</param>
        public static void DeleteValue(string subkey, string registryItem, RegistryKey rkey)
        {
            string keyName = USER_ROOT_HKCU + "\\" + subkey;
            try
            {
                using (RegistryKey key = rkey.OpenSubKey(keyName, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(registryItem);
                    }
                    else
                    {
                        App.LOG(LogLevel.ERROR, $"RegistryDelete: {keyName} is not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                App.LOG(LogLevel.ERROR, $"Exception : {ex.Message}");
            }
        }
    }
}

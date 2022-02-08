using No2verse.AzureTable.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace No2verse.AzureTable
{

    public class AzureStorageSettings : IPathSettings
    {
        public string ConnectionString { get; set; }
        public string Type { get { return "AZURETABLE"; } }
        public string LogPath { get; set; }
        public AzureStorageSettings()
        {

            if (string.IsNullOrEmpty(LogPath))
            {
                LogPath = AppDomain.CurrentDomain.BaseDirectory + "SYSTEM_LOG" + Path.DirectorySeparatorChar;
            }

            if (!LogPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                LogPath = LogPath + Path.DirectorySeparatorChar;
            }
            Directory.CreateDirectory(LogPath);
        }

        public AzureStorageSettings(string azureStorageConnectionString, string logPath) : this()
        {
            ConnectionString = azureStorageConnectionString;
            if (string.IsNullOrEmpty(logPath))
            {
                LogPath = AppDomain.CurrentDomain.BaseDirectory + "SYSTEM_LOG" + Path.DirectorySeparatorChar;
            }
            else
            {
                LogPath = logPath;
            }


        }
    }
}

using MemeFolder.Properties;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace MemeFolder.Services
{
    public class ClientConfigService
    {
        public string FilesPath
        {
            get => Settings.Default.FilesPath;
            set
            {
                Settings.Default.FilesPath = value;
                Settings.Default.Save();
            }
        }
        public ClientConfigService()
        {

            if (string.IsNullOrEmpty(Settings.Default.FilesPath))
            {
                string md = Environment.GetFolderPath(Environment.SpecialFolder.Personal); //путь к Документам
                if (Directory.Exists(md + "\\MemeFolder") == false)
                {
                    Directory.CreateDirectory(md + "\\MemeFolder");
                    Settings.Default.FilesPath = md + "\\MemeFolder";
                    Settings.Default.Save();
                }
                
            }
        }

    }
}

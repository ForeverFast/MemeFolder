using MemeFolder.Properties;
using System;
using System.IO;

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
                string path = "D:\\";
                if (Directory.Exists(path))
                {
                    path += "\\MemeFolder";
                    if (Directory.Exists(path))
                        Directory.Delete(path);
                }
                else
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\MemeFolder";
                }

                Directory.CreateDirectory(path);
                Settings.Default.FilesPath = path;
                Settings.Default.Save();
            }
        }

    }
}

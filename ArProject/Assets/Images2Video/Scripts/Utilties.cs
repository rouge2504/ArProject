using UnityEngine;
using System.IO;

namespace tw.com.championtek
{
    public class Utilties
    {
        public Utilties() { } //Constructor

        public static string CreateDirectoryIfNotExist(string folder)
        {
            string app_folder;
            string path;

            if (folder == null)
                app_folder = "Images2Video";
            else
                app_folder = folder;

            path = Path.Combine(Application.persistentDataPath, app_folder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {//clean all files
                DirectoryInfo _info = new DirectoryInfo(path);
                FileInfo[] _files = _info.GetFiles("*.*");
                foreach (FileInfo _file in _files)
                {
                    _file.Delete();
                }
            }
            return path;
        }
    }
}
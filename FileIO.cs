using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace project_BlueBird
{
    class FileIO
    {
        public static void CheckAndCreateDir(string folderPath)
        {
            if(!System.IO.Directory.Exists(folderPath))
            {
                Console.WriteLine("Creating directory: " + folderPath);
                System.IO.Directory.CreateDirectory(folderPath);
            }
        }

        public static void DownloadFile(string folderPath, string fileName, string url)
        {
            CheckAndCreateDir(folderPath);
            if (!File.Exists(folderPath + fileName))
            {
                //If it doesn't exist, then download
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, folderPath + fileName);
                }
            }
        }
    }
}

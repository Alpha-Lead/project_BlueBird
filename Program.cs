using Newtonsoft.Json;
using project_BlueBird.Items;
using System;

using System.IO;


namespace project_BlueBird
{
    class Program
    {

        static void Main(string[] args)
        {
            CoreProcessing coreProcessing = new CoreProcessing();

            Console.WriteLine("Creating/Validating ingest directory...");
            string currentDirectory = System.IO.Directory.GetCurrentDirectory();
            FileIO.CheckAndCreateDir(currentDirectory + "/ingest");



            /*
             * Read in twitter credentials
             */
            if(!File.Exists(currentDirectory + "/ingest/credentials.json"))
            {
                Console.WriteLine("Credential file does not exist!");
                return;
            }
            else
            {
                Console.WriteLine("");
            }

            //Read in from JSON file
            StreamReader r = new StreamReader(currentDirectory + "/ingest/credentials.json");
            string json = r.ReadToEnd();
            TwitterCredsJSON credentials = JsonConvert.DeserializeObject<TwitterCredsJSON>(json);

            //Build twitter connection using credentials
            coreProcessing.InitialiseConnection(credentials);

            /* 
             * Read in usernames
             */
            if (!File.Exists(currentDirectory + "/ingest/credentials.json"))
            {
                Console.WriteLine("Credential file does not exist!");
                return;
            }

            //Read in from TXT file
            string txtFile = System.IO.File.ReadAllText(currentDirectory + "/ingest/DownloadNames.txt");
            string[] usrNms = txtFile.Split(new[] { ',', ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries); //Split entries

            foreach (string usrNm in usrNms)
            {
                //Fetch user and get tweets & media
                coreProcessing.ProcessTwitterUser(usrNm, currentDirectory);
                Console.WriteLine("");
            }


            Console.WriteLine("Finished operations.");

            //Consider looking into Twitter-Lists in the future (https://github.com/linvi/tweetinvi/wiki/Twitter-Lists)
        }
    }

    public class TwitterCredsJSON
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }

}

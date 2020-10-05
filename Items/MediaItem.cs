using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models;

namespace project_BlueBird
{
    class MediaItem
    {
        public long MediaId { get; set; }
        public string MediaClass { get; set; }
        public string FileType { get; set; }
        public string MediaURL { get; set; }
        public int MediaIndex { get; set; }

        public override string ToString()
        {
            StringBuilder strBuild = new StringBuilder();

            strBuild.Append("MediaItem:{");
            strBuild.Append("MediaId=" + MediaId.ToString());
            strBuild.Append(", Class=" + MediaClass);
            strBuild.Append(", FileType=" + FileType);
            strBuild.Append(", URL=" + MediaURL);
            strBuild.Append(", Index=" + MediaIndex.ToString());
            strBuild.Append("}");

            return strBuild.ToString();
        }
    }

    
}

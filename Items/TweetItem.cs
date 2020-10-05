using System;
using System.Collections.Generic;
using System.Text;

namespace project_BlueBird
{
    class TweetItem
    {
        public long TweetId { get; set; }
        public string AuthorScreenName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TweetText { get; set; }
        public string URL { get; set; }
        public List<MediaItem> mediaItems { get; set; }

        public long? InReplyToStatusId { get; set; }
        public string InReplyToScreenName { get; set; }
        public long? QuotedStatusId { get; set; }
        public string QuotedScreenName { get; set; }

        public override string ToString()
        {
            StringBuilder returnStr = new StringBuilder();

            returnStr.Append("TweetItem:\n");
            returnStr.Append("{TweetId=" + TweetId.ToString());
            returnStr.Append(", AuthorScreenName='" + AuthorScreenName + "'");
            returnStr.Append(", CreatedAt=[" + CreatedAt.ToString("yyyy-MM-dd hh:mm:ss") + "]");
            returnStr.Append(", TweetText={'" + TweetText + "}'");
            returnStr.Append(", URL=" + URL);

            if (mediaItems.Count > 0)
            {
                returnStr.Append(", Media=["); 
                foreach(MediaItem item in mediaItems)
                {
                    returnStr.Append("{"+item.ToString()+"},");
                }
                returnStr.Remove(returnStr.Length - 1, 1);
                returnStr.Append("]");
            }

            if (InReplyToStatusId != null)
            {
                returnStr.Append(", InReplyToStatusId=" + InReplyToStatusId.ToString());
                returnStr.Append(", InReplyToScreenName='" + InReplyToScreenName + "'");
            }
            if (QuotedStatusId != null)
            {
                returnStr.Append(", QuotedStatusId=" + QuotedStatusId.ToString());
                returnStr.Append(", QuotedScreenName='" + QuotedScreenName + "'");
            }
            returnStr.Append("}");

            return returnStr.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models;

namespace project_BlueBird.Items
{
    class TimelineItem
    {
        public static List<ITweet> MergeTimelineTypes(IEnumerable<ITweet> fromTimeline, List<ITweet> toTimeline)
        {
            foreach(ITweet tweet in fromTimeline)
            {
                toTimeline.Add(tweet);
            }
            return toTimeline;
        }

        public static long EarlyTweetId(List<ITweet> tweetList)
        {
            long currentMin = long.MaxValue;
            foreach (ITweet tweet in tweetList)
            {
                if(tweet.Id < currentMin)
                {
                    currentMin = tweet.Id;
                }
            }
            return currentMin;
        }
        public static long LateTweetId(List<ITweet> tweetList)
        {
            long currentMax = 0;
            foreach (ITweet tweet in tweetList)
            {
                if (tweet.Id > currentMax)
                {
                    currentMax = tweet.Id;
                }
            }
            return currentMax;
        }
    }
}

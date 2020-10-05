using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Logic.TwitterEntities;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities.ExtendedEntities;

namespace project_BlueBird.Items
{
     class BuildItem
    {
        public static TweetItem BuildTweetItem(ITweet tweet)
        {
            TweetItem tweetItem = new TweetItem();
            tweetItem.TweetId = tweet.Id;
            tweetItem.TweetText = tweet.FullText;
            tweetItem.URL = tweet.Url;
            tweetItem.CreatedAt = tweet.CreatedAt;
            tweetItem.AuthorScreenName = tweet.CreatedBy.ScreenName;

            tweetItem.mediaItems = ExtractMediaFromTweet(tweet);

            if (tweet.InReplyToStatusId != null)
            {
                tweetItem.InReplyToStatusId = tweet.InReplyToStatusId;
                tweetItem.InReplyToScreenName = tweet.InReplyToScreenName;
            }
            if (tweet.QuotedStatusId != null)
            {
                tweetItem.QuotedStatusId = tweet.QuotedStatusId;
                if (tweet.QuotedTweet != null)
                {
                    tweetItem.QuotedScreenName = tweet.QuotedTweet.CreatedBy.ScreenName;
                } 
                else
                {
                    tweetItem.QuotedScreenName = null;
                }
            }

            return tweetItem;
        }

        public static List<MediaItem> ExtractMediaFromTweet(ITweet tweet)
        {
            List<MediaItem> mediaList = new List<MediaItem>();

            if (tweet.Media.Count > 0)
            {
                int indexCount = 1;
                foreach (MediaEntity media in tweet.Media)
                {
                    MediaItem mediaItem = new MediaItem();

                    //Add details to new media item
                    mediaItem.MediaId = (long)media.Id;
                    mediaItem.MediaClass = media.MediaType;
                    if (media.VideoDetails != null) //Video or GIF
                    {
                        IVideoEntityVariant baseVariant = media.VideoDetails.Variants[0]; //Set a baseline media variant

                        foreach (IVideoEntityVariant variant in media.VideoDetails.Variants)
                        {
                            if(variant.Bitrate > baseVariant.Bitrate) //Check if batter quality than baseline
                            {
                                baseVariant = variant;
                            }
                        }

                        mediaItem.FileType = "." + baseVariant.ContentType.Substring(baseVariant.ContentType.LastIndexOf('/')+1).Trim(); //Get file extension
                        mediaItem.MediaURL = baseVariant.URL; //Get URL that points to media (not page with media)
                    } else
                    {
                        //Photo or singular type
                        mediaItem.FileType = media.MediaURL.Substring(media.MediaURL.LastIndexOf('.')).Trim();
                        mediaItem.MediaURL = media.MediaURL;
                    }
                    
                    mediaItem.MediaIndex = indexCount; //Add index value, then increment
                    indexCount++;

                    mediaList.Add(mediaItem); //Add media item to list
                }
                 
            }

            return mediaList;
        }
    }
}

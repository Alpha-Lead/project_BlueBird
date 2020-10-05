using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi; // https://github.com/linvi/tweetinvi
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using project_BlueBird.Items;
using System.IO;

namespace project_BlueBird
{
    class CoreProcessing
    {
        /// <summary>
        /// This function reads in the twitter credentials and sets them for the Twitter instance.
        /// </summary>
        /// <param name="ExecutionTwitterCredentials"></param>
        public void InitialiseConnection(TwitterCredsJSON ExecutionTwitterCredentials)
        {
            // Set TwitterAPI credentials (https://github.com/linvi/tweetinvi/wiki/Credentials)
            ITwitterCredentials loginCreds = new TwitterCredentials(
                ExecutionTwitterCredentials.ConsumerKey, 
                ExecutionTwitterCredentials.ConsumerSecret, 
                ExecutionTwitterCredentials.AccessToken, 
                ExecutionTwitterCredentials.AccessTokenSecret);
            Auth.SetCredentials(loginCreds);
            // Enable Automatic RateLimit handling (https://github.com/linvi/tweetinvi/wiki/Rate-Limits)
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;
        }

        /// <summary>
        /// This function takes a given user and downloads the tweets and media (filtered by conditions).
        /// Conditions: Limited to last 3200 tweets (twitter), does not include retweets, includes replies-to.
        /// </summary>
        /// <param name="TwitterUserScreenName"></param>
        /// <param name="BaseDirectory"></param>
        /// <returns>
        /// Status of function:
        /// 0=Success,
        /// -1=User not found,
        /// -99=Unexpected error
        /// </returns>
        public int ProcessTwitterUser(string TwitterUserScreenName, string BaseDirectory)
        {
            try
            {
                /*
                 * Get target user
                 */

                // Get user from screen name (https://github.com/linvi/tweetinvi/wiki/Users)
                var user = User.GetUserFromScreenName(TwitterUserScreenName);

                if (user == null)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("User not found: {0}", TwitterUserScreenName);
                    Console.ResetColor();

                    return -1;
                }
                else
                {
                    Console.WriteLine("Found twitter user {0} (@{1}) having userId={2}", user.Name, user.ScreenName, user.Id);
                }



                /*
                 * Get the user timeline (https://github.com/linvi/tweetinvi/wiki/Timelines , https://github.com/linvi/tweetinvi/wiki/Get-All-User-Tweets)
                 */

                Console.WriteLine("Scanning @{0} Timeline...", user.ScreenName);
                int maxAllowedTweets = 3200; // Limit set by Twitter API
                int pageSize = 200; // Max = 200

                var userTimelineParameters = new UserTimelineParameters();
                userTimelineParameters.ExcludeReplies = false;
                userTimelineParameters.MaximumNumberOfTweetsToRetrieve = pageSize;
                //userTimelineParameters.SinceId = 0;
                //userTimelineParameters.MaxId = latestTweetId;

                var tweetPage = Timeline.GetUserTimeline(user.Id, userTimelineParameters).ToArray();
                List<ITweet> iTweetList = new List<ITweet>(tweetPage);

                while ((tweetPage.Length > 0) && (iTweetList.Count <= maxAllowedTweets))
                {
                    var oldTweetId = tweetPage.Select(x => x.Id).Min();

                    userTimelineParameters.MaxId = oldTweetId - 1;
                    userTimelineParameters.MaximumNumberOfTweetsToRetrieve = iTweetList.Count > (maxAllowedTweets - pageSize) ? maxAllowedTweets - iTweetList.Count : pageSize;

                    tweetPage = Timeline.GetUserTimeline(user.Id, userTimelineParameters).ToArray();
                    iTweetList.AddRange(tweetPage);
                }



                /*
                 * Open/Create Twitter User directory for output
                 */
                string currentDirectory = BaseDirectory;
                Console.WriteLine("Creating/Validating output directory...");
                FileIO.CheckAndCreateDir(currentDirectory + "/export");
                FileIO.CheckAndCreateDir(currentDirectory + "/export/" + user.ScreenName);
                currentDirectory = currentDirectory + "/export/" + user.ScreenName + "/"; //change current directory
                FileIO.CheckAndCreateDir(currentDirectory + "tweets");
                FileIO.CheckAndCreateDir(currentDirectory + "media");



                /*
                 * Output data from Timeline to JSON file
                 */
                Console.WriteLine("Write raw data to JSON file...");
                File.WriteAllText(currentDirectory + user.ScreenName + "_" + DateTime.Now.ToString("yyMMddhhmmss") + ".RAW.json", iTweetList.ToJson().ToString());



                /*
                 * Convert ITweet to TweetItem for later processing
                 */
                Console.WriteLine("Exporting tweets...");
                List<TweetItem> tweetList = new List<TweetItem>();
                foreach (ITweet tweet in iTweetList)
                {
                    if (tweet.IsRetweet == false) // Don't translate retweets, instead dump record
                    {
                        //Check if already downloaded
                        if (!File.Exists(currentDirectory + "tweets/" + tweet.CreatedAt.ToString("yyMMddhhmmss") + "_" + tweet.Id + ".json"))
                        {
                            TweetItem tweetItem = BuildItem.BuildTweetItem(tweet);// Translate the tweet object
                            File.WriteAllText(currentDirectory + "tweets/" + tweetItem.CreatedAt.ToString("yyMMddhhmmss") + "_" + tweetItem.TweetId + ".json", tweetItem.ToJson().ToString()); // Output to JSON file
                            tweetList.Add(tweetItem); // Add to list
                        }
                    }
                }

                Console.WriteLine("Downloading media...");
                using (var progress = new ProgressBar())
                {
                    int counter = 0;
                    int denominator = tweetList.Count;
                    foreach (TweetItem tweet in tweetList) // Print to console media items
                    {
                        // Console.WriteLine(item.ToString());
                        if (tweet.mediaItems.Count > 0)
                        {
                            foreach (MediaItem media in tweet.mediaItems)
                            {
                                FileIO.DownloadFile(currentDirectory + "media/", tweet.TweetId + "_" + media.MediaIndex + media.FileType, media.MediaURL);
                            }
                        }
                        progress.Report((double)counter / denominator);
                        counter++;
                    }
                }

                Console.WriteLine("Finished downloading for user @{0}", user.ScreenName);
                return 0;
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("ERROR - An error occurred during ProcessTwitterUser({0}, {1})", TwitterUserScreenName, BaseDirectory);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                return -99;
            }
        }
    }
}

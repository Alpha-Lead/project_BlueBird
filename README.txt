Instructions to prepare:
- ingest\DownloadNames.txt
	o This file should contain a list of all the twitter users to download
	o Entries should be comma/space/line separated
	o Entries should be the user's screen name, eg. @screenName
- credentials.json
	o This file should contain the Twitter API credentials for the user
	o Values are: ConsumerKey, ConsumerSecret, AccessToken, and AccessTokenSecret
	o Apply for a Twitter Developer account here: https://developer.twitter.com/en/apply-for-access
	o Create an app here: https://developer.twitter.com/en/apps

Instructions to execute:
1) Open Windows Command Line, Windows Powershell, or Windows Terminal
2) Navigate to this folder
3) Run:
   $ .\project_BlueBird.exe

Filestructure:
project_BlueBird v1.0
	|
	|-project_BlueBird.exe
	|
	|-README.txt
	|
	|-ingest
	|	|
	|	|-credentials.json
	|	|-
	|	|-DownloadNames.txt
	|
	|-export (created when run)
	|	|
		|-<TwitterUser>
			|
			|-<TwitterUser>_<yyMMddhhmmss>.RAW
			|
			|-media
			|	|
			|	|-<yyMMddhhmmss>_<indexInTweet>.<extension>
			|	|
			|	|...
			|
			|-tweets
				|
				|-<yyMMddhhmmss>_<tweetId>.json
				|
				|...
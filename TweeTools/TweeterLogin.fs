module TweeterLogin

open TweetSharp;

// Pass your credentials to the service
let twitterService = new TwitterService("FnqMRIndSaO1KDL8F1r8eZ6He", "FmqoNdZvwF3Q0RKsLI4XjGw5MFjYqKk05iId7PMxRR325KQBuT");

// Step 1 - Retrieve an OAuth Request Token
let requestToken = twitterService.GetRequestToken()

// Step 2 - Redirect to the OAuth Authorization URL
let uri = twitterService.GetAuthorizationUri(requestToken);
let openPage = 
    System.Diagnostics.Process.Start("chrome.exe", uri.ToString());

// Step 3 - Exchange the Request Token for an Access Token
let getAccessToken (verifier : string) = 
    twitterService.GetAccessToken(requestToken, verifier)

// Step 4 - User authenticates using the Access Token
let displayMentions (accessToken : OAuthAccessToken) =
    twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret)
    let result = twitterService.ListTweetsMentioningMe(new ListTweetsMentioningMeOptions())
    result


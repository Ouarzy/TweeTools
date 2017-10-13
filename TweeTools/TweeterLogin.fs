module TweeterLogin

open TweetSharp;
open System

type TwitterUser = { Id : int64 ; Description : string ; ScreenName: string}

let twitterService = new TwitterService("FnqMRIndSaO1KDL8F1r8eZ6He", "FmqoNdZvwF3Q0RKsLI4XjGw5MFjYqKk05iId7PMxRR325KQBuT");

let requestToken = twitterService.GetRequestToken()

let uri = twitterService.GetAuthorizationUri(requestToken);

let openPage =  System.Diagnostics.Process.Start("chrome.exe", uri.ToString());

let getAccessToken (verifier : string) = 
    twitterService.GetAccessToken(requestToken, verifier)

let authenticateWith(accessToken : OAuthAccessToken) =
    twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret)
module TweeterLogin

open TweetSharp;
open System

type TwitterUser = { Id : int64 ; Description : string}

let twitterService = new TwitterService("FnqMRIndSaO1KDL8F1r8eZ6He", "FmqoNdZvwF3Q0RKsLI4XjGw5MFjYqKk05iId7PMxRR325KQBuT");

let requestToken = twitterService.GetRequestToken()

let uri = twitterService.GetAuthorizationUri(requestToken);
let openPage =  System.Diagnostics.Process.Start("chrome.exe", uri.ToString());

let getAccessToken (verifier : string) = 
    twitterService.GetAccessToken(requestToken, verifier)

let listTweetsMentioningMeOptions ()= 
       let option = new ListTweetsMentioningMeOptions()
       option.ContributorDetails <- new Nullable<bool>(true)
       option

let authenticateWith(accessToken : OAuthAccessToken) =
    twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret)

let getMentions () =
    let result = twitterService.ListTweetsMentioningMe(listTweetsMentioningMeOptions())
    result

let userProfileForOption userName=
        let option =new GetUserProfileForOptions()
        option.ScreenName <- userName
        option

let getuserProfileFor userName =
    let result = twitterService.GetUserProfileFor(userProfileForOption(userName))
    result

let listFollowerIdsOfOptions userName =
        let option =new ListFollowerIdsOfOptions()
        option.ScreenName <- userName
        option

let listFollowersOf userName =
    let result = twitterService.ListFollowerIdsOf(listFollowerIdsOfOptions(userName))
    result

let getUserProfileForOptions (userId : int64) =
    let option =new GetUserProfileForOptions()
    option.UserId <- new Nullable<int64>(userId)
    option

let getDescriptionUser (userId : int64) =
    let result = twitterService.GetUserProfileFor(getUserProfileForOptions(userId))
    {Id = result.Id; Description = result.Description}

let followUserOptions (userId : int64) =
    let option =new FollowUserOptions()
    option.UserId <- new Nullable<int64>(userId)
    option

let followUser (user : TwitterUser) =
    let result = twitterService.FollowUser(followUserOptions(user.Id))
    result

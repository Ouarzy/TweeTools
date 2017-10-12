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

let listTweetsMentioningMeOptions ()= 
       let option = new ListTweetsMentioningMeOptions()
       option.ContributorDetails <- new System.Nullable<bool>(true)
       option

// Step 4 - User authenticates using the Access Token
let getMentions (accessToken : OAuthAccessToken) =
    twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret)
    let result = twitterService.ListTweetsMentioningMe(listTweetsMentioningMeOptions())
    result

let userProfileForOption ()=
        let option =new GetUserProfileForOptions()
        option.ScreenName <- "lacuisineduweb"
        option

let getuserProfileFor (accessToken : OAuthAccessToken) =
    twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret)
    let result = twitterService.GetUserProfileFor(userProfileForOption())
    result

let listFollowerIdsOfOptions () =
        let option =new ListFollowerIdsOfOptions()
        option.ScreenName <- "lacuisineduweb"
        option

let listFollowersOf (accessToken : OAuthAccessToken) =
    twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret)
    let result = twitterService.ListFollowerIdsOf(listFollowerIdsOfOptions())
    result

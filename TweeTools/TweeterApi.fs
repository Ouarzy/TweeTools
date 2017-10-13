module TweeterApi

open TweetSharp;
open TweeterLogin
open System

let sleep = async{
    printfn "Taux de sollicitation max atteint pour Twitter API, 15min d'attente"
    do! Async.Sleep (1000 * 60 * 15)
    printfn "Reprise du programme"
    }

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

let rec getDescriptionUser (userId : int64) =
    let result = twitterService.GetUserProfileFor(getUserProfileForOptions(userId))
    match result with
    | null -> 
        Async.RunSynchronously sleep
        getDescriptionUser userId
    | _ -> {Id = result.Id; Description = result.Description; ScreenName = result.ScreenName}

let followUserOptions (user : TwitterUser) =
    let option =new FollowUserOptions()
    option.UserId <- new Nullable<int64>(user.Id)
    option.ScreenName <- user.ScreenName
    option.Follow <- new Nullable<bool>(true)
    option

let rec followUser (user : TwitterUser) =
    let result = twitterService.FollowUser(followUserOptions(user))
    match result with
    | null -> 
        Async.RunSynchronously sleep
        followUser user
    | _ -> result

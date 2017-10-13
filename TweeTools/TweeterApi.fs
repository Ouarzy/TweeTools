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

let rec getDescriptionUserAsync (userId : int64) =
    async{
    let! result = twitterService.GetUserProfileForAsync(getUserProfileForOptions(userId))|> Async.AwaitTask
    if result.Response.RateLimitStatus.RemainingHits = 0 then
        Async.RunSynchronously sleep
        return Async.RunSynchronously (getDescriptionUserAsync userId)
    else
        return {Id = result.Value.Id; Description = result.Value.Description; ScreenName = result.Value.ScreenName}
    }

let followUserOptions (user : TwitterUser) =
    let option =new FollowUserOptions()
    option.UserId <- new Nullable<int64>(user.Id)
    option.ScreenName <- user.ScreenName
    option.Follow <- new Nullable<bool>(true)
    option

let followUserAsync (user : TwitterUser) =
    async{
    let! result = twitterService.FollowUserAsync(followUserOptions(user)) |> Async.AwaitTask
    match result.Response.Errors with
        | null -> return true
        | _ -> return false
    }
module TweeterApi

open TweetSharp;
open TweeterLogin
open System

let rec sleepForNbMinsAsync (nbMinutes : int64)= async{
    printfn "\nEncore %i mins" nbMinutes
    do! Async.Sleep (1000 * 60)
    if nbMinutes = (int64)0 then return (int64)0
    else return Async.RunSynchronously (sleepForNbMinsAsync (nbMinutes-(int64)1))
    }


let requestWaitForMaxRateDuring (nbMinutes : int64) = 
    printfn "\nTaux de sollicitation max atteint pour Twitter API, %i mins d'attente" nbMinutes
    Async.RunSynchronously (sleepForNbMinsAsync nbMinutes) |> ignore
    printfn "Reprise du programme"
    

let userProfileForOption userName=
        let option =new GetUserProfileForOptions()
        option.ScreenName <- userName
        option

let getuserProfileFor userName =
    twitterService.GetUserProfileFor(userProfileForOption(userName))

let listFollowerIdsOfOptions userName =
        let option =new ListFollowerIdsOfOptions()
        option.ScreenName <- userName
        option

let listFollowersOf userName =
    let result = twitterService.ListFollowerIdsOf(listFollowerIdsOfOptions(userName))
    if result = null then
        None
    else
        Some result

let getUserProfileForOptions (userId : int64) =
    let option =new GetUserProfileForOptions()
    option.UserId <- new Nullable<int64>(userId)
    option

let rec getDescriptionUserAsync (userId : int64) =
    async{
    let! result = twitterService.GetUserProfileForAsync(getUserProfileForOptions(userId))|> Async.AwaitTask
    if result.Value = null then
        requestWaitForMaxRateDuring ((int64)15)
        return Async.RunSynchronously (getDescriptionUserAsync userId)
    else if result.Response.RateLimitStatus.RemainingHits = 0 then
        requestWaitForMaxRateDuring (result.Response.RateLimitStatus.ResetTimeInSeconds / ((int64)60))
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
module TweeterApi

open TweetSharp;
open TweeterLogin
open System

let rec sleepForNbMinsAsync nbMinutes = async{
    printfn "\nEncore %i mins" nbMinutes
    do! Async.Sleep (1000 * 60)
    if nbMinutes = 0 then return 0
    else return Async.RunSynchronously (sleepForNbMinsAsync (nbMinutes-1))
    }

let requestWaitForMaxRateDuring nbMinutes = 
    printfn "\nTaux de sollicitation max atteint pour Twitter API, %i mins d'attente" nbMinutes
    Async.RunSynchronously (sleepForNbMinsAsync nbMinutes) |> ignore
    printfn "Reprise du programme"    

let userProfileForOption userName=
        let option =new GetUserProfileForOptions()
        option.ScreenName <- userName
        option

let getuserProfileFor userName =
    twitterService.GetUserProfileFor(userProfileForOption(userName))

let private listFollowerIdsOfOptions userName =
        let option =new ListFollowerIdsOfOptions()
        option.ScreenName <- userName
        option

let listFollowersOf userName =
    let result = twitterService.ListFollowerIdsOf(listFollowerIdsOfOptions(userName))
    if result = null then
        None
    else
        Some (result |> Seq.toList)

let private listFriendIdsOfOfOptions userName =
        let option =new ListFriendIdsOfOptions()
        option.ScreenName <- userName
        option

let listFriendsOf userName =
    let result = twitterService.ListFriendIdsOf(listFriendIdsOfOfOptions(userName))
    if result = null then
        None
    else
        Some (result |> Seq.toList)

let getUserProfileForOptions (userId : int64) =
    let option =new GetUserProfileForOptions()
    option.UserId <- new Nullable<int64>(userId)
    option

let timeInMinutesBeforeReset (resetTime : DateTime) = 
    let msBeforeReset = resetTime - DateTime.Now
    msBeforeReset.Minutes

let rec getDescriptionUserAsync (userId : int64) =
    async{
    let! result = twitterService.GetUserProfileForAsync(getUserProfileForOptions(userId))|> Async.AwaitTask
    if result.Response.RateLimitStatus.RemainingHits = 0 then
        requestWaitForMaxRateDuring (timeInMinutesBeforeReset result.Response.RateLimitStatus.ResetTime)
        return Async.RunSynchronously (getDescriptionUserAsync userId)
    else if result.Value = null then
        return None
    else
        return Some {Id = result.Value.Id; Description = result.Value.Description; ScreenName = result.Value.ScreenName}
    }

let private followUserOptions (user : TwitterUser) =
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

let private unfollowUserOptions (user : TwitterUser) =
    let option =new UnfollowUserOptions()
    option.UserId <- new Nullable<int64>(user.Id)
    option.ScreenName <- user.ScreenName
    option


let unfollowUserAsync (user : TwitterUser) =
    async{
    let! result = twitterService.UnfollowUserAsync(unfollowUserOptions(user)) |> Async.AwaitTask
    match result.Response.Errors with
        | null -> return true
        | _ -> return false
    }
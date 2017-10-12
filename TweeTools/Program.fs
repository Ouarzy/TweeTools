module main

open TweeterLogin
open System


let displayMentions2 (mention : TweetSharp.TwitterStatus) =
    printfn "%s" mention.Author.ScreenName

let displayFollowers (follower : int64) =
    printfn "%i" follower

[<EntryPoint>]
let main argv = 
    printfn "starting log"
    openPage |> ignore
    printfn "Quelle est votre code d'autorisation?"
    let verifierCode = Console.ReadLine()
    let accessToken = getAccessToken verifierCode
    
    let mentions = getMentions accessToken
    mentions |> Seq.cast |> Seq.iter displayMentions2

    let followers = listFollowersOf accessToken
    followers |> Seq.cast |> Seq.iter displayFollowers

    Console.ReadLine() |> ignore
    0 // return an integer exit code
                           
﻿module main

open TweeterLogin
open System


let displayMentions2 (mention : TweetSharp.TwitterStatus) =
    printfn "%s" mention.Author.ScreenName

let displayFollowers (follower : int64) =
    printfn "%i" follower


[<EntryPoint>]
let main argv = 
    printfn "Bonjour!"
    openPage |> ignore
    printfn "Quelle est votre code d'autorisation?"
    let verifierCode = Console.ReadLine()
    let accessToken = getAccessToken verifierCode
    authenticateWith accessToken

    printfn "Quelle compte voulez vous analyser?"
    let expectedAccount = Console.ReadLine()
    
    let followersIds = listFollowersOf expectedAccount
    printfn "Le compte %s a %i followers" expectedAccount followersIds.Count
    
    printfn "Quelle mot clé souhaitez vous analyser dans la description des followers?"
    let expectedkeyWord = Console.ReadLine()
    
    let allDescriptions = followersIds |> Seq.map getDescriptionUser
    let allUserIdsWithMatchingKeyword = allDescriptions |> Seq.filter (fun twitterUser -> twitterUser.Description.Contains(expectedkeyWord)) |> Seq.toArray

    printfn "Il y a %i potentiels personnes à follow, voulez vous le faire? (Y/N)" allUserIdsWithMatchingKeyword.Length
    let answer = Console.ReadLine()

    if answer.ToLower().Equals("Y") then
        allUserIdsWithMatchingKeyword |> Array.map followUser |> ignore
        Console.ReadLine() |> ignore
        printfn "Fin"
        0
     else
        Console.ReadLine() |> ignore
        printfn "Fin"
        0                           
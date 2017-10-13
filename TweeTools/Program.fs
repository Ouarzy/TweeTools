module main

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
    let allUserIdsWithMatchingKeyword = allDescriptions |> Seq.filter (fun twitterUser -> 
        printf "."
        twitterUser.Description.ToLower().Contains(expectedkeyWord.ToLower())) |> Seq.toArray

    printfn "Il y a %i potentiels personnes à follow, voulez vous le faire? (Y/N)" allUserIdsWithMatchingKeyword.Length
  
    let answer = Console.ReadLine()

    if answer.ToLower().Equals("y") then
        allUserIdsWithMatchingKeyword |> Array.map followUser |> ignore
        printfn "Fin"
        Console.ReadLine() |> ignore
        0
     else
        printfn "Fin"
        Console.ReadLine() |> ignore
        0                           
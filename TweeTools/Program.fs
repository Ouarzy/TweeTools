module main

open System
open TweeterLogin
open TweeterApi


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

    match answer.ToLower() with
    | "y" ->
        allUserIdsWithMatchingKeyword |> Array.map followUser |> ignore
        printfn "Users added"
    | _ -> 
        printfn "No Users added"

    printfn "Fin"
    Console.ReadLine() |> ignore
    0

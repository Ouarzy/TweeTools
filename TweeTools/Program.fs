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
    
    let allDescriptions = followersIds |> Seq.map (fun x -> Async.RunSynchronously (getDescriptionUserAsync x))
    let allUserIdsWithMatchingKeyword = allDescriptions |> Seq.filter (fun twitterUser -> 
        twitterUser.Description.ToLower().Contains(expectedkeyWord.ToLower())) |> Seq.toArray

    printfn "Il y a %i potentiels personnes à follow, voulez vous le faire? (Y/N)" allUserIdsWithMatchingKeyword.Length
  
    let answer = Console.ReadLine()

    match answer.ToLower() with
    | "y" ->
        let userAdded = allUserIdsWithMatchingKeyword |> Array.map (fun x -> 
            printf "."
            Async.RunSynchronously (followUserAsync x))
        printfn "\nVous suivez %i nouveaux comptes" (userAdded |> Array.filter (fun x -> x = true) |> Array.length) 
    | _ -> 
        printfn "Vous ne suivez pas de nouveaux comptes"

    printfn "Fin"
    Console.ReadLine() |> ignore
    0

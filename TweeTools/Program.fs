module main

open System
open TweeterLogin
open TweeterApi

let fin ()= 
     printfn "Fin"
     Console.ReadLine() |> ignore
     0


let tryFollowAccount (optionFollowersIds : int64 list option) (expectedAccount : string)=
        let followersIds = optionFollowersIds.Value
        printfn "Le compte %s a %i followers" expectedAccount followersIds.Length
    
        printfn "Quelle mot clé souhaitez vous analyser dans la description des followers?"
        let expectedkeyWord = Console.ReadLine()
    
        let allDescriptions = followersIds |> Seq.map (fun x -> Async.RunSynchronously (getDescriptionUserAsync x))
        let allUserIdsWithMatchingKeyword = 
            allDescriptions |>
            Seq.filter (fun twitterUser -> twitterUser.IsSome && twitterUser.Value.Description.ToLower().Contains(expectedkeyWord.ToLower())) |>
            Seq.map (fun x -> x.Value) |>
            Seq.toArray

        printfn "Il y a %i potentiels personnes à follow, voulez vous les suivre? (y/n)" allUserIdsWithMatchingKeyword.Length
  
        let answer = Console.ReadLine()

        match answer.ToLower().Contains("y") with
        | true ->
            let userAdded = allUserIdsWithMatchingKeyword |> Array.map (fun x -> 
                printf "."
                Async.RunSynchronously (followUserAsync x))
            printfn "\nVous suivez %i nouveaux comptes" (userAdded |> Array.filter (fun x -> x = true) |> Array.length) 
        | false ->  
            printfn "\nVous ne suivez pas de nouveaux comptes" 


let tryUnfollowAccount (optionFollowersIds : int64 list option) (expectedAccount : string)=
        let followersIds = optionFollowersIds.Value
        printfn "Le compte %s a %i subscriptions" expectedAccount (followersIds |> Seq.length)
    
        printfn "Quelle mot clé souhaitez vous analyser dans la description des subscriptions?"
        let expectedkeyWord = Console.ReadLine()
    
        let allDescriptions = followersIds |> Seq.map (fun x -> Async.RunSynchronously (getDescriptionUserAsync x))
        let allUserIdsWithMatchingKeyword = 
            allDescriptions |>
            Seq.filter (fun twitterUser -> twitterUser.IsSome && twitterUser.Value.Description.ToLower().Contains(expectedkeyWord.ToLower())) |>
            Seq.map (fun x -> x.Value) |>
            Seq.toArray

        printfn "Il y a %i potentiels personnes à unfollow, voulez vous arrêter de les suivre? (y/n)" allUserIdsWithMatchingKeyword.Length
  
        let answer = Console.ReadLine()

        match answer.ToLower().Contains("y") with
        | true ->
            let userRemoved = allUserIdsWithMatchingKeyword |> Array.map (fun x -> 
                printf "."
                Async.RunSynchronously (unfollowUserAsync x))
            printfn "\nVous ne suivez plus %i nouveaux comptes" (userRemoved |> Array.filter (fun x -> x = true) |> Array.length) 
        | false ->  
            printfn "\Rien ne s'est passé" 




let tryFollowUsers expectedAccount = 
    let optionFollowersIds = listFollowersOf expectedAccount
    if optionFollowersIds.IsNone then
        printfn "Ce compte n'existe pas."
    else
        tryFollowAccount optionFollowersIds expectedAccount               

let tryUnfollowUsers expectedAccount = 
    let optionSubscriptionIds = listFriendsOf expectedAccount
    if optionSubscriptionIds.IsNone then
        printfn "Ce compte n'existe pas."
    else
        tryUnfollowAccount optionSubscriptionIds expectedAccount               

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
    
    if expectedAccount = "kickbanking" then
        tryUnfollowUsers expectedAccount
    else
        tryFollowUsers expectedAccount

    fin()
module main

open System
open TweeterLogin
open TweeterApi

let fin ()= 
     printfn "Fin"
     Console.ReadLine() |> ignore
     0


let tryAnalyzeAccount (optionFollowersIds : Option<TweetSharp.TwitterCursorList<int64>>) (expectedAccount : string)=
        let followersIds = optionFollowersIds.Value
        printfn "Le compte %s a %i followers" expectedAccount followersIds.Count
    
        printfn "Quelle mot clé souhaitez vous analyser dans la description des followers?"
        let expectedkeyWord = Console.ReadLine()
    
        let allDescriptions = followersIds |> Seq.map (fun x -> Async.RunSynchronously (getDescriptionUserAsync x))
        let allUserIdsWithMatchingKeyword = 
            allDescriptions |>
            Seq.filter (fun twitterUser -> twitterUser.IsSome && twitterUser.Value.Description.ToLower().Contains(expectedkeyWord.ToLower())) |>
            Seq.map (fun x -> x.Value) |>
            Seq.toArray

        printfn "Il y a %i potentiels personnes à follow, voulez vous le faire? (Y/N)" allUserIdsWithMatchingKeyword.Length
  
        let answer = Console.ReadLine()

        match answer.ToLower().Contains("y") with
        | true ->
            let userAdded = allUserIdsWithMatchingKeyword |> Array.map (fun x -> 
                printf "."
                Async.RunSynchronously (followUserAsync x))
            printfn "\nVous suivez %i nouveaux comptes" (userAdded |> Array.filter (fun x -> x = true) |> Array.length) 
        | _ -> 
            printfn "Vous ne suivez pas de nouveaux comptes"


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
    
    let optionFollowersIds = listFollowersOf expectedAccount

    if optionFollowersIds.IsNone then
        printfn "Ce compte n'existe pas."
        fin()
    else
        tryAnalyzeAccount optionFollowersIds expectedAccount
        fin()

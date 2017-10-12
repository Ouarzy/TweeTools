module main

open TweeterLogin
open System


let displayMentions2 mention =
    printfn "%s" mention

[<EntryPoint>]
let main argv = 
    printfn "starting log"
    openPage |> ignore
    printfn "Quelle est votre code d'autorisation?"
    let verifierCode = Console.ReadLine()
    let accessToken = getAccessToken verifierCode
    displayMentions accessToken |> Seq.cast |> Seq.map displayMentions2 |> ignore
    0 // return an integer exit code
                           
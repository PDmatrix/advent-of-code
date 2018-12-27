open System.IO
open AdventOfCode.Common

let getExecutor year day part =
    let exec (executor : Day<'a, 'b, 'c>) =
        let exec part solve =
            let fileName = (sprintf "Input/%i/Day%02i.in" year day)
            fun _ ->
                let result = fileName |> File.ReadLines |> executor.parse |> solve
                printfn "Year %i Day %02i Part %i\n%O" year day part result
        match part with
        | 1 -> exec 1 executor.part1
        | 2 -> exec 2 executor.part2
        | _ -> fun _ -> ()
    match year with
    | 2015 ->
        match day with
        | 1 -> exec Year2015Day01.executor
        | _ -> fun _ -> printf "Invalid day - %i" day
    | _ -> fun _ -> printf "Invalid year - %i" year

[<EntryPoint>]
let main argv =
    let runPart year day part = getExecutor year day part ()
    let runDay year day = for part in 1..2 do runPart year day part
    let runYear year = for day in 1..25 do runDay year day
    match argv.[0] with
        | "ALL" -> for year in 2015..2018 do runYear year
        | x ->
            let parts = x.Split('.') |> Array.map int
            match parts.Length with
            | 1 -> runYear parts.[0]
            | 2 -> runDay parts.[0] parts.[1]
            | 3 -> runPart parts.[0] parts.[1] parts.[2]
            | _ -> ()
    0


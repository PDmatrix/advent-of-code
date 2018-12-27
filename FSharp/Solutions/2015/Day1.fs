module Year2015Day01

open AdventOfCode.Common

let matchParethesis (parethesis : char) (value : int ref) =
    match parethesis with 
            | '(' -> incr value
            | ')' -> decr value
            | _ -> ()
    
let solve (parentheses : string) =
    let floor = ref 0
    parentheses
    |> Seq.iter (fun value -> matchParethesis value floor)
    !floor
    
let solvePart2 (parentheses : string) =
    let floor = ref 0
    parentheses
    |> Seq.map (fun value ->
        matchParethesis value floor
        !floor)
    |> Seq.takeWhile (fun value -> value <> -1)
    |> Seq.length
    |> (+) 1

let executor = { parse = parseFirstLine asString; part1 = solve; part2 = solvePart2 }
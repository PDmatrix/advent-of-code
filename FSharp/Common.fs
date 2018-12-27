namespace AdventOfCode

open System
module Common =
    type Day<'a, 'b, 'c> = { parse: string seq -> 'a; part1: 'a -> 'b; part2: 'a -> 'c }

    let printSeq seq1 = Seq.iter (printf "%A ") seq1; printfn ""
    let parseFirstLine f = Seq.head >> f
    let parseEachLine = Seq.map
    let parseEachLineIndexed = Seq.mapi
    let asString : string -> string = id
    let asInt : string -> int = int
    let asStringArray : string [] -> string [] = Array.map string
    let asIntArray : string [] -> int [] = Array.map int
    let splitBy (c : string) f (str : string) = str.Split([| c |], StringSplitOptions.None) |> f


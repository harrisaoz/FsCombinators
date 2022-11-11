module FsCombinators.TernaryResult

type TernaryResult<'a, 'Error> =
    | Ok of 'a
    | Ignore
    | Error of 'Error

open Combinators.Standard

let inline bind2 v f c =
    match c with
    | Ok x -> f x
    | Ignore -> v
    | Error r -> Error r

let inline bind f = bind2 Ignore f

let inline map f = bind (f >> Ok)

let ofResult r =
    match r with
    | Result.Ok x -> Ok x
    | Result.Error y -> Error y

let toResult resultOfIgnore tr =
    match tr with
    | Ok x -> Result.Ok x
    | Ignore -> resultOfIgnore
    | Error msg -> Result.Error msg

let isOk tr =
    match tr with
    | Ok _ -> true
    | _ -> false

let shouldIgnore tr =
    match tr with
    | Ignore _ -> true
    | _ -> false

let SS f g h x = f (g x) (h x)

let groupResult (xs: TernaryResult<'a, 'b> seq) : TernaryResult<'a seq, 'b> =
    let folder acc =
        let accBind =
            SS bind2
                (Seq.singleton >> Ok)
                (Seq.singleton >> C Seq.append >> B Ok)

        S bind2 (C accBind) acc

    Seq.fold folder Ignore xs

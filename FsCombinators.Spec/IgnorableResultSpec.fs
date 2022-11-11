module FsCombinators.Tests.IgnorableResultSpec

open FsCheck.Xunit

open FsCombinators.ExtraTypes

[<Property>]
let ``groupResult: all Ok should be Ok``
    (results: IgnorableResult<obj, obj> list)
    (baseline: obj)
    =
    let okResults =
        results
        |> List.filter IgnorableResult.isOk
        |> List.insertAt 0 (IgnorableResult.Ok baseline)

    IgnorableResult.groupResult okResults |> IgnorableResult.isOk

[<Property>]
let ``groupResult: all Ignore should be Ignore`` (results: IgnorableResult<obj, obj> list) =
    let ignoreResults =
        results
        |> List.filter IgnorableResult.shouldIgnore
        |> List.insertAt 0 IgnorableResult.Ignore

    IgnorableResult.groupResult ignoreResults |> IgnorableResult.shouldIgnore

[<Property>]
let ``groupResult: some Ok without Errors should be Ok``
    (results: IgnorableResult<obj, obj> list)
    (baseline: obj)
    =
    let nonErrors =
        results
        |> List.filter (not << IgnorableResult.isError)
        |> List.insertAt 0 (IgnorableResult.Ok baseline)

    IgnorableResult.groupResult nonErrors |> IgnorableResult.isOk

[<Property>]
let ``groupResult: some with Errors should be Error``
    (results: IgnorableResult<obj, obj> list)
    (baseline: obj)
    =
    let resultsWithErrors =
        results
        |> List.insertAt 0 (IgnorableResult.Error baseline)

    IgnorableResult.groupResult resultsWithErrors |> IgnorableResult.isError

[<Property>]
let ``groupResult: there should be as many items in the Ok sequence as there are Ok items in the input sequence``
    (results: IgnorableResult<obj, obj> list)
    (baseline: obj)
    =
    let nonErrors =
        results
        |> List.filter (not << IgnorableResult.isError)
        |> List.insertAt 0 (IgnorableResult.Ok baseline)
    let okOnly =
        nonErrors
        |> List.filter IgnorableResult.isOk

    IgnorableResult.groupResult nonErrors
    |> function
        | IgnorableResult.Ok ternaryResultValue ->
            Seq.length ternaryResultValue = (Seq.length okOnly)
        | _ ->
            false

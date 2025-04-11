module FsCombinators.Tests.SeqExtensionSpec

open FsCheck.Xunit

module SeqExt = FsCombinators.SeqExtensions

[<Property>]
let ``maybeExistsWhere: should be false if the predicate is never true`` (xs: List<int>) (y: int) =
    let expected = false

    let actual = SeqExt.maybeExistsWhere (fun _ _ -> false) xs (Some y)

    expected = actual

[<Property>]
let ``maybeExistsWhere: should be true if the predicate is true for any of the compared values``
    (x: int)
    =
    let list =
        seq {
            1
            2
            3
        }

    let expected = (x = 1 || x = 2 || x = 3)

    let actual = SeqExt.maybeExistsWhere (fun y x0 -> x0 = y) list (Some x)

    expected = actual

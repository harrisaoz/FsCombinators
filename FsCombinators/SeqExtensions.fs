[<Microsoft.FSharp.Core.RequireQualifiedAccess>]
module FsCombinators.SeqExtensions

open FsCombinators.Core
module StringExt = StringExtensions

/// The haystack of strings contains a string that is case-insensitively
/// equal to the given 'needle' value.
let inline icontains needle haystack =
    Seq.filter (StringExt.iequal needle) haystack
    |> (not << Seq.isEmpty)

/// There exists a predicate p <- ps such that p x is true.
let inline existsPredicate x predicates =
    Seq.exists (T x) predicates

/// There exists an x <- xs such that, given y, p y x is true.
let existsGiven p y xs = (p >> Seq.exists) y xs

/// 1. The given option, m, has Some value, y; and
/// 2. there exists an x <- xs such that p y x is true.
let maybeExistsWhere p: 'b seq -> 'a option -> bool =
    C (existsGiven p) >> Option.exists

/// 1. The given option, m, has Some value, x; and
/// 2. there exists a predicate, p <- ps, such that p x is true
let anyPredicate (ps: ('a -> bool) seq): 'a option -> bool =
    (C existsPredicate >> Option.exists) ps

/// Assuming a directed, acyclic, rooted graph, recursively list all nodes,
/// excluding the branches rooted at nodes for which the filterPredicate is false.
let rec filteredPreOrder filterPredicate listChildren x =
    seq {
        if filterPredicate x then
            yield x
            for child in listChildren x do
                yield! filteredPreOrder filterPredicate listChildren child
    }

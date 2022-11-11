[<Microsoft.FSharp.Core.RequireQualifiedAccess>]
module FsCombinators.Tuple2

/// Akin to List.map, but for a Tuple2/Pair.
let map f (a, b) = (f a, f b)

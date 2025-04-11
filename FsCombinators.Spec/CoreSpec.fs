module FsCombinators.Core.Spec

open FsCheck
open FsCheck.Xunit
open FsCheck.FSharp

open FsCombinators.Core

[<Property>]
let ``K holds for integers`` (a: int) (x: obj) (y: obj) =
    K a x = K a y

[<Property>]
let ``K holds for floats`` (a: float) (x: obj) (y: obj) =
    (K a x).Equals(K a y)

[<Property>]
let ``K holds for strings`` (a: string) (x: obj) (y: obj) =
    K a x = K a y

[<Property>]
let ``K holds for objects`` (a: obj) (x: obj) (y: obj) =
    K a x = K a y

[<Property>]
let ``K holds for functions`` (f: obj -> obj) (x: obj) (y: obj) (o: obj) =
    K f x o = K f y o

/// Enable Verbose=true on the Property attribute to show all the generated test data.
[<Property>]
let ``B is the reverse of Q for integer functions``
    (Fun f: Function<int, int>)
    (Fun g: Function<int, int>)
    (x: int)
    =
        B f g x = Q g f x

[<Property>]
let ``B is commutative under the identity function`` (f: obj -> obj) (x: obj) =
    B f id x = B id f x

[<Property>]
let ``B is the reverse of Q for object functions``
    (f: obj -> obj)
    (g: obj -> obj)
    (x: obj)
    =
        B f g x = Q g f x

[<Property>]
let ``applyToBoth: example 1`` (a: bool) =
    let f = (||)
    let g = not
    let h b = false <> b
    applyToBoth f g h a = (not a) || (false <> a)

[<Property>]
let ``applyToBoth: example 2`` (a: int) (f: int -> int -> int) (n1: int) (n2: int) =
    let g n = n + n1
    let h n = n * n2
    applyToBoth f g h a = f (a + n1) (a * n2)

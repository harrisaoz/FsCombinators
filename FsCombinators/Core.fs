module FsCombinators.Core

/// "reverse" composition. B = (<<)
let inline B f g x =
    f (g x)

/// "Cardinal".
/// Reverse the order of the function arguments.
let inline C f x y =
    f y x

/// The constant function.
let inline K x _ =
    x

/// "forward" composition. Q = (>>)
let inline Q f g x =
    g (f x)

/// "Mockingbird".
/// Compose the given function with itself.
/// Application of W to Q (M = W Q)
let M f = Q f f

/// "Starling"
/// Equivalent to g x |> f x
/// Also equivalent to: S f g = W (Q f (Q g))
let inline S f g x =
    f x (g x)

/// "Thrush": this combinator is also known by (|>) and used infix as x |> f
/// (|>) x f = f x
let inline T x f = f x

/// "Warbler"
/// A generalisation of "Mockingbird", where composition is replaced by a functor f.
let inline W x y = x y y

/// The "Y combinator"
/// Generally of little practical value due to the mental challenge of deciphering any usage.
/// Search the Web for y-combinator if you want to know more.
let rec Y f x = f (Y f) x

/// Apply the first function to the results of
/// applying the second and third functions to the given value.
/// Point-free definition: Q (Q B Q) (T S)
let inline applyToBoth f g h x =
    f (g x) (h x)

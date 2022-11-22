module FsCombinators.OptionExtensions

/// Execute f in a try block,
/// returning its result wrapped in Option.Some if no exception is raised,
/// or Option.None instead of raising an exception.
///
/// Note that OutOfMemoryException is re-raised in order to avoid
/// non-deterministic program behaviour.
let tryAsOption f =
    try
        f () |> Option.Some
    with
    | :? System.OutOfMemoryException -> reraise ()
    | _ -> None

module FsCombinators.ResultExtensions

/// Execute f in a try block,
/// returning its result wrapped in Result.Ok if no exception is raised,
/// or the exception message wrapped in Result.Error instead of raising the
/// exception.
///
/// Note that OutOfMemoryException is re-raised in order to avoid
/// non-deterministic program behaviour.
let tryAsResult f =
    try
        f () |> Result.Ok
    with
    | :? System.OutOfMemoryException -> reraise ()
    | ex -> Result.Error ex.Message

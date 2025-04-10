module FsCombinators.ExtraTypes

[<StructuralEquality; StructuralComparison>]
[<Struct>]
type IgnorableResult<'a, 'Error> =
    | Ok of TernaryResultValue: 'a
    | Ignore
    | Error of TernaryErrorValue: 'Error

module IgnorableResult =
    /// Similar to Result.bind, except that Ignore may be transformed.
    /// v: the replacement value in case the subject is Ignore
    /// f: the function to apply to the value of the subject in case the subject is Ok
    /// subject: the IgnorableResult to bind
    let inline bind2
        (v: IgnorableResult<'a, 'b>)
        (f: 'c -> IgnorableResult<'a, 'b>)
        (c: IgnorableResult<'c, 'b>)
        =
        match c with
        | Ok x -> f x
        | Ignore -> v
        // | err -> err
        | Error e -> Error e

    /// Akin to Result.bind.
    /// Assume that Ignore passes through unchanged.
    let inline bind f = bind2 Ignore f

    /// Akin to Result.map.
    let inline map f = bind (f >> Ok)

    /// Convert a Result to an IgnorableResult.
    let ofResult r =
        match r with
        | Result.Ok x -> Ok x
        | Result.Error y -> Error y

    /// Convert the IgnorableResult to a standard Result,
    /// defaulting to the given Result value if the input is Ignore.
    let toResult defaultValue tr =
        match tr with
        | Ok x -> Result.Ok x
        | Ignore -> defaultValue
        | Error msg -> Result.Error msg

    /// The result represents success and has a corresponding value.
    let isOk tr =
        match tr with
        | Ok _ -> true
        | _ -> false

    /// The result represents something to be ignored.
    let shouldIgnore tr =
        match tr with
        | Ignore -> true
        | _ -> false

    /// The result represents an Error, which is in some way described by a corresponding error value.
    let isError tr =
        match tr with
        | Error _ -> true
        | _ -> false

    open FsCombinators.Core

    /// Convert a sequence of IgnorableResults into an IgnorableResult of a sequence of Ok elements.
    /// Details:
    /// 1. If there are no Ok or Error elements in the input sequence, then the result is Ignore;
    /// 2. If there are any Error elements in the input sequence,
    ///    then the result is Error
    ///    and the error value is the value of the first Error encountered in the input sequence;
    /// 3. If there are any Ok elements and no Error elements in the sequence,
    ///    then the result is Ok
    ///    and the sequence of values is the values of the Ok elements from the input sequence.
    let groupResult (xs: IgnorableResult<'a, 'b> seq) : IgnorableResult<'a seq, 'b> =
        let folder =
            let accBind =
                applyToBoth bind2
                    (Seq.singleton >> Ok)
                    (Seq.singleton >> C Seq.append >> B Ok)

            S bind2 (C accBind)

        Seq.fold folder Ignore xs

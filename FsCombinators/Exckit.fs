module FsCombinators.Exckit

///
/// From https://ericlippert.com/2008/09/10/vexing-exceptions/
///
/// "Fatal exceptions are not your fault, you cannot prevent them, and you cannot sensibly
/// clean up from them.  They almost always happen because the process is deeply diseased
/// and is about to be put out of its misery."
///
/// "Boneheaded exceptions are your own darn fault, you could have prevented them and
/// therefore they are bugs in your code.  You should not catch them; doing so is hiding a
/// bug in your code."
///
/// "Vexing exceptions are the result of unfortunate design decisions.  Vexing exceptions are
/// thrown in a completely non-exceptional circumstance, and therefore must be caught
/// and handled all the time."
///
/// "Exogenous exceptions appear to be somewhat like vexing exceptions except that they are
/// not the result of unfortunate design choices.  Rather, they are the result of untidy
/// external realities impinging upon your beautiful, crisp program logic."
///
/// Further clarification in the context of functional programming techniques which favour
/// minimal exception raising, instead preferring multi-typed outcomes via mechanisms such
/// as discriminated unions (e.g. Result/Either, Maybe/Option) paired with pattern matching.
///
/// Fatal (any function, correct usage, runtime environment failure, possibly unrecoverable):
/// These are failures outside application control; typically non-deterministic.  Failing
/// fast is often the best response to encountering these. Exceptions commonly considered to be
/// fatal include out of memory, stack overflow, access violation and thread aborted.  See
/// the matched exceptions in StandardFatals.isExceptionFatal.
///
/// Boneheaded (deterministic function, incorrect usage):
/// These are failures that are deterministic and entirely the fault of the author
/// of the code that causes the exception to be thrown.  Most are avoidable by following good
/// testing practices (e.g. property-based testing with good testing library/framework support
/// for generation of good arbitrary input data).  Strong, appropriate typing can eliminate the
/// possibility of some classes of these exceptions being raised. These should not be caught in
/// any way that disguises the bug that triggered the exception.
///
/// Vexing (deterministic function, full range not modelled by return type):
/// Poor design; these are deterministic failures that are raised as exceptions rather
/// than being considered a reasonable possible value of the function for some input.  Typically,
/// the best way to handle these "exceptions" is to catch them and expose the full range of values
/// in a discriminated union type.
///
/// Exogenous (non-deterministic function, full range not modelled by return type):
/// Largely, these are the result of poor design.  They are thrown by non-deterministic
/// functions which only express a limited part of the full range of possible values as their
/// return type.  Instead, the full range of values should be encompassed by the return type -
/// the typical approach is to use a discriminated union.
///
type ExceptionCategory =
    | Fatal of cause: System.Exception // runtime environment failure, possibly unrecoverable
    | Boneheaded of cause: System.Exception // deterministic function, incorrect usage
    | Vexing of cause: System.Exception // deterministic function, range partly modelled
    | Exogenous of cause: System.Exception // non-deterministic function, range partly modelled

let tryFallible (onError, onOk) isFatal isVexing isExogenous fallibleFunction =
    try
        fallibleFunction () |> onOk
    with ex ->
        if isFatal ex then
            System.Environment.FailFast(ex.Message)
            // unreachable, since FailFast always exits immediately
            reraise ()
        elif isVexing ex then
            onError ex
        elif isExogenous ex then
            onError ex
        else
            reraise ()

type VexingExceptionHandler<'a> = System.Exception -> 'a
type ExogenousExceptionHandler<'a> = System.Exception -> 'a

type GetExceptionCategory = System.Exception -> ExceptionCategory

type ExceptionHandlers<'a> =
    | ExceptionHandlers of VexingExceptionHandler<'a> * ExogenousExceptionHandler<'a>

module CategoryMatchBased =
    let tryFallible
        onOk
        (ExceptionHandlers(onVexing, onExogenous))
        (getExceptionCategory: GetExceptionCategory)
        (fallibleFunction: unit -> 'b)
        =
        try
            fallibleFunction () |> onOk
        with ex ->
            match (getExceptionCategory ex) with
            | Fatal cause ->
                System.Environment.FailFast(cause.Message)
                // unreachable, since FailFast always exits immediately
                reraise ()
            | Vexing cause -> onVexing cause
            | Exogenous cause -> onExogenous cause
            | Boneheaded _ -> reraise ()

module ResultFacade =
    let tryFallible
        (exceptionHandlers: ExceptionHandlers<Result<'a, 'b>>)
        (getExceptionCategory: GetExceptionCategory)
        (fallibleFunction: unit -> 'a)
        =
        CategoryMatchBased.tryFallible
            Result.Ok
            exceptionHandlers
            getExceptionCategory
            fallibleFunction

module StandardFatals =
    ///
    /// In many cases, the following exceptions can be considered fatal:
    /// - System.OutOfMemoryException
    /// - System.StackOverflowException
    /// - System.AccessViolationException
    /// - System.AppDomainUnloadedException
    /// - System.BadImageFormatException
    /// - System.Threading.ThreadAbortException
    ///
    let isExceptionFatal (maybeFatal: System.Exception) =
        match maybeFatal with
        | :? System.OutOfMemoryException as _f1 -> true
        | :? System.StackOverflowException as _f2 -> true
        | :? System.AccessViolationException as _f3 -> true
        | :? System.AppDomainUnloadedException as _f4 -> true
        | :? System.BadImageFormatException as _f5 -> true
        | :? System.Threading.ThreadAbortException as _f6 -> true
        | _ -> false

    let getExceptionCategory (categoriseNonFatal: GetExceptionCategory) ex : ExceptionCategory =
        if isExceptionFatal ex then
            Fatal ex
        else
            categoriseNonFatal ex

module StandardVexing =
    let isVexing (maybeVexing: System.Exception) =
        match maybeVexing with
        | :? System.ArithmeticException as _ -> true
        | :? System.ArgumentException as _ -> true
        | :? System.FormatException as _ -> true
        | :? System.Collections.Generic.KeyNotFoundException as _ -> true
        | :? System.Data.NoNullAllowedException as _ -> true
        | :? System.IO.PathTooLongException as _ -> true
        | _ -> false

module StandardExogenous =
    let isExogenous (maybeExogenous: System.Exception) =
        match maybeExogenous with
        | :? System.Data.VersionNotFoundException as _ -> true
        | :? System.Transactions.TransactionInDoubtException as _ -> true
        | :? System.Runtime.InteropServices.ExternalException as _ -> true
        | :? System.ApplicationException as _ -> true
        | :? System.Security.Authentication.AuthenticationException as _ -> true
        | :? System.Data.DataException as _ -> true
        | :? System.ComponentModel.LicenseException as _ -> true
        | :? System.Net.NetworkInformation.PingException as _ -> true
        | :? System.Runtime.Serialization.SerializationException as _ -> true
        | :? System.Net.Mail.SmtpException as _ -> true
        | :? System.TimeoutException as _ -> true
        | :? System.Transactions.TransactionException as _ -> true
        | :? System.Security.VerificationException as _ -> true
        | :? System.ComponentModel.WarningException as _ -> true
        | :? System.InvalidOperationException as _ -> true
        | :? System.Xml.Xsl.XsltException as _ -> true
        | :? System.IO.IOException as _ -> true
        | :? System.IO.InvalidDataException as _ -> true
        | :? System.IO.InternalBufferOverflowException as _ -> true
        | :? System.IndexOutOfRangeException as _ -> true
        | _ -> false

module AsResult =
    let tryFallible isFatal isVexing isExogenous fallibleFunction =
        tryFallible (Result.Error, Result.Ok) isFatal isVexing isExogenous fallibleFunction

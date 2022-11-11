[<Microsoft.FSharp.Core.RequireQualifiedAccess>]
module FsCombinators.StringExtensions

let contains: string -> string -> bool =
    fun haystack -> haystack.Contains

let containsCI: string -> string -> bool =
    fun haystack needle ->
        haystack.ToLower().Contains(needle.ToLower())

let endsWith: string -> string -> bool =
    fun haystack -> haystack.EndsWith

let endsWithCI: string -> string -> bool =
    fun haystack needle -> haystack.ToLower().EndsWith(needle.ToLower())

let split (separators: char[]) (whole: string) =
    whole.Split(separators)

let flatten labelSeparator (replacement: string) =
    split (Array.singleton labelSeparator)
    >> String.concat replacement

let iequal (s1: string) (s2: string) =
    System.String.Equals (s1, s2, System.StringComparison.InvariantCultureIgnoreCase)

let appendString (toAppend: string) (baseText: string) =
    System.Text.StringBuilder(baseText).Append(toAppend).ToString()

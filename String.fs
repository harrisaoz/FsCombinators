module Combinators.String

let contains: string -> string -> bool =
    fun haystack -> haystack.Contains

let containsCI: string -> string -> bool =
    fun haystack needle ->
        haystack.ToLower().Contains(needle.ToLower())

let endsWith: string -> string -> bool =
    fun haystack -> haystack.EndsWith

let endsWithCI: string -> string -> bool =
    fun haystack needle -> haystack.ToLower().EndsWith(needle.ToLower())

[<System.Obsolete("Use String.concat, since it achieves the same result while being more general")>]
let join (glue: string) (parts: string[]) =
    String.concat glue parts

let split (separators: char[]) (whole: string) =
    whole.Split(separators)

let flatten labelSeparator (replacement: string) =
    split (Array.singleton labelSeparator)
    >> String.concat replacement

let iequal (s1: string) (s2: string) =
    System.String.Equals (s1, s2, System.StringComparison.InvariantCultureIgnoreCase)

let appendString (toAppend: string) (baseText: string) =
    System.Text.StringBuilder(baseText).Append(toAppend).ToString()

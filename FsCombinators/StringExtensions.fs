[<Microsoft.FSharp.Core.RequireQualifiedAccess>]
module FsCombinators.StringExtensions

let inline toLower (text: string) = text.ToLower()

let inline contains (needle: string) (haystack: string) : bool = haystack.Contains(needle)

/// <summary>
/// Facilitate case-insensitive comparisons using an arbitrary search function.
/// </summary>
/// <param name="comparator">A function to be used to search haystack for a
/// needle after they have been converted to lowercase.</param>
/// <param name="needle">The text to find.</param>
/// <param name="haystack">The text to search.</param>
let ciComparison comparator needle haystack : bool =
    comparator (toLower needle) (toLower haystack)

/// A case-insensitive version of <code>contains</code>.
let containsCI (needle: string) (haystack: string) : bool = ciComparison contains needle haystack

/// <summary>
/// Haystack ends with needle, matching case.
/// </summary>
/// <param name="needle">The string to look for at the end of haystack.</param>
/// <param name="haystack">The string to search for the needle in.</param>
/// <returns>
/// True if the string <code>haystack</code> ends in the value of <code>needle</code>.
/// </returns>
let inline endsWith needle (haystack: string) : bool = haystack.EndsWith(needle)

/// A case-insensitive version of <code>endsWith</code>.
let endsWithCI (needle: string) (haystack: string) : bool = ciComparison endsWith needle haystack

/// <summary>
/// Split <code>whole</code> text at each encounter with any character in
/// <code>separators</code>.
/// </summary>
/// <param name="separators">A set of characters to split the text on, none of which
/// should appear in the returned array.</param>
/// <param name="whole">The input text to split.</param>
/// <returns>
/// An array of segments of the original <code>whole</code> text, without the
/// separator characters, <code>separators</code>.
/// </returns>
let split (separators: char[]) (whole: string) = whole.Split(separators)

/// <summary>
/// Replace any and all occurrences of the character <code>labelSeparator</code>
/// in some input text with the <code>replacement</code> text.
/// </summary>
/// <param name="labelSeparator">The character to replace.</param>
/// <param name="replacement">The replacement string for occurrences of
/// <code>labelSeparator</code>.</param>
let flatten labelSeparator (replacement: string) =
    split (Array.singleton labelSeparator) >> String.concat replacement

let iequal (s1: string) (s2: string) =
    System.String.Equals(s1, s2, System.StringComparison.InvariantCultureIgnoreCase)

let appendString (toAppend: string) (baseText: string) =
    System.Text.StringBuilder(baseText).Append(toAppend).ToString()

# Purpose

The modules in this project facilitate the use of common patterns in functional programming in the
F# programming language. The modules are detailed below. Implementations are short and often naive;
refer to the documentation and/or source of each module for further details.

## Core

From sources such as SKI combinatory logic, the B,C,K,W system and Raymond Smullyan's "To Mock a
Mockingbird". In this module, Smullyan's naming is mostly used, as those names are easy to search
for.

## BooleanExtensions

Feel free to suggest important boolean combinators that would be widely used and not yet available
elsewhere. Generally I find it difficult to name such combinators well enough to improve readability
rather than hinder understanding.

## Exckit (Exceptions Kit)

This is an attempt to make it easier to deal with libraries that throw lots of Exceptions,
especially in cases that could be argued to be non-exceptional.  It uses a categorisation proposed
here: https://ericlippert.com/2008/09/10/vexing-exceptions/.

## ExtraTypes

This offers an alternative ternary form of the Result type, named IgnorableResult. The practical
basis for this type is scenarios in which there may be a failure, but the failure is not considered
a problem.  Some potential uses for the Ignore case:
- a warning might be issued;
- there might just be no effect in a case where an effect was intended but not necessary.

This module also provides a mechanism to group a collection of IgnorableResult values, as described
in groupResultGeneric (which has a reference implementation for the Seq type).
 
## OptionExtensions

This was an early pre-cursor to the more generally useful Exckit module. It makes a lazy assumption
about the nature of exceptions.

## ResultExtensions

Similar to OptionExtensions, but with a domain of type Result.

## SeqExtensions

It's common to need to apply some predicate over a Seq of elements, or a Seq of predicates over some
value, or even both together. This module provides some tools to do that without rewriting the same
logic all over the place. It also provides a pre-order traversal of a graph, with certain
assumptions.

## StringExtensions

While the F# String type does provide a range of idiomatic functions for working with text, there's
plenty left to rewrite frequently using the underlying types. This module does its bit to narrow
that gap - it's still wide!

## Tuple2

There's potential for more specialisation here, but there are existing libraries that take this
seriously and offer a comprehensive feature set. Rather than reproduce all that, this just does what
I needed for my purposes.

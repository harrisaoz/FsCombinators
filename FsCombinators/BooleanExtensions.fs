module FsCombinators.BooleanExtensions

/// At least one of the predicates is satisfied by the given value.
/// Point-free definition: eitherPredicate = applyToBoth (||)
let eitherPredicate p1 p2 x = Core.applyToBoth (||) p1 p2 x

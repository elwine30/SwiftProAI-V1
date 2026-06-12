---
title: "PredicateBuilder"
type: class
language: csharp
layer: data
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.EntityFrameworkCore/EntityFrameworkCore/PredicateBuilder.cs"
updated: 2026-06-12
---

# PredicateBuilder

Utility for composing LINQ expression predicates dynamically using And/Or operators, used in repositories that build queries from optional filter parameters.

## Public interface

static ExpressionStarter<T> New<T>(Expression<Func<T,bool>>)
static ExpressionStarter<T> New<T>(bool defaultExpression)
static Expression<Func<T,bool>> Or<T>(this Expression<Func<T,bool>>, Expression<Func<T,bool>>)
static Expression<Func<T,bool>> And<T>(this Expression<Func<T,bool>>, Expression<Func<T,bool>>)
static Expression<Func<T,bool>> Extend<T>(this Expression<Func<T,bool>>, Expression<Func<T,bool>>, PredicateOperator)

## Used by

- [[user-organization-unit-repository]]

## External dependencies

- JetBrains.Annotations

## Notes

Adapted from the open-source LINQKit project (https://github.com/scottksmith95/LINQKit). PredicateBuilder.True<T>() and PredicateBuilder.False<T>() are marked [Obsolete] — callers should use New() instead.

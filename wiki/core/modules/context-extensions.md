---
title: "ContextExtensions"
type: class
language: csharp
layer: shared
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.GraphQL/Core/Extensions/ContextExtensions.cs"
updated: 2026-06-12
---

# ContextExtensions

Provides extension methods on IResolveFieldContext for fluent conditional argument handling and nested field-selector presence checks within a GraphQL selection set.

## Public interface

- `static IResolveFieldContext ContainsArgument<TArgType>(this IResolveFieldContext context, string argumentName, Action<TArgType> argumentContainsAction)`
- `static bool HasSelectionField(this IResolveFieldContext context, string fieldSelector, char namespaceSeperator = ':')`

## Used by

- [[organization-unit-query]]
- [[role-query]]
- [[user-query]]

## External dependencies

- GraphQL
- GraphQLParser

## Notes

ContainsArgument skips the action when the argument source is FieldDefault (i.e. the client did not explicitly supply the value). HasSelectionField traverses the AST selection tree using colon-delimited path segments, enabling conditional EF Include calls based on what fields were actually requested.

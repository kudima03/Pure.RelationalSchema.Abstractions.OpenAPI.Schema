# Pure.RelationalSchema.Abstractions.OpenAPI.Schema

OpenAPI document transformer for **Pure.RelationalSchema.Abstractions** — corrects schema component definitions for `IColumn`, `IIndex`, `ITable`, `IForeignKey`, and `ISchema` in ASP.NET Core OpenAPI output.

[![.NET build & test](https://github.com/kudima03/Pure.RelationalSchema.Abstractions.OpenAPI.Schema/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Abstractions.OpenAPI.Schema/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/Pure.RelationalSchema.Abstractions.OpenAPI.Schema/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Abstractions.OpenAPI.Schema/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.RelationalSchema.Abstractions.OpenAPI.Schema)](https://www.nuget.org/packages/Pure.RelationalSchema.Abstractions.OpenAPI.Schema)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

ASP.NET Core's built-in OpenAPI generator introspects types at startup and emits schema components based on .NET reflection. When your endpoints return `Pure.RelationalSchema.Abstractions` interface types, the generated schemas do not match the actual JSON serialization produced at runtime.

`Pure.RelationalSchema.Abstractions.OpenAPI.Schema` provides `RelationalSchemaDocumentTransformer`, an `IOpenApiDocumentTransformer` that runs during document generation and replaces the auto-generated schema components with correct, hand-authored definitions.

## API

| Type | Kind | Description |
|------|------|-------------|
| `RelationalSchemaDocumentTransformer` | `sealed class` | Implements `IOpenApiDocumentTransformer`. Replaces schema components for the five relational schema interface types. |

### Schema components replaced

| Component | Properties |
|-----------|------------|
| `IColumn` | `Name` (string), `Type` (string) |
| `IIndex` | `IsUnique` (boolean), `Columns` (array of `IColumn`) |
| `ITable` | `Name` (string), `Columns` (array of `IColumn`), `Indexes` (array of `IIndex`) |
| `IForeignKey` | `ReferencingTable` (`ITable`), `ReferencingColumns` (array of `IColumn`), `ReferencedTable` (`ITable`), `ReferencedColumns` (array of `IColumn`) |
| `ISchema` | `Name` (string), `Tables` (array of `ITable`), `ForeignKeys` (array of `IForeignKey`) |

## Target Frameworks

- .NET 10

## Installation

```
dotnet add package Pure.RelationalSchema.Abstractions.OpenAPI.Schema
```

## Usage

Register the transformer when configuring OpenAPI in your ASP.NET Core application:

```csharp
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<RelationalSchemaDocumentTransformer>();
});
```

The transformer is a no-op when the document has no components or none of the five schema component names are present, so it is safe to register unconditionally.

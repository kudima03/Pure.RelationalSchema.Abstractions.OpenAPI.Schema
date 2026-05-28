# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet format --verify-no-changes          # check formatting (CI enforces this)
dotnet tool install -g csharpier && csharpier check .   # check csharpier style (CI enforces this)
dotnet format && csharpier format .        # auto-fix code style
dotnet test --no-build --verbosity normal  # run unit tests
dotnet pack --configuration Release -p:PackageVersion=<version> --output .
```

## Architecture

This is a **single-class NuGet library** targeting `net10.0`. The entire public surface is one sealed class:

**`RelationalSchemaDocumentTransformer`** — implements `IOpenApiDocumentTransformer` (from `Microsoft.AspNetCore.OpenApi`).

At document generation time, ASP.NET Core produces schema components for `Pure.RelationalSchema.Abstractions` interface types (`IColumn`, `IIndex`, `ITable`, `IForeignKey`, `ISchema`) that do not match their actual JSON serialization. `TransformAsync` iterates the document's `Components.Schemas` dictionary and replaces any of those five component entries with hand-authored `OpenApiSchema` objects that reflect the correct JSON shape. Components not in the known set are left untouched.

**AOT:** `IsAotCompatible = true`. The schema map is a static `IReadOnlyDictionary` initialised at type load — no reflection at runtime.

**Package validation:** `EnablePackageValidation = true` with `PackageValidationBaselineVersion = 0.1.0-preview.0.1.0`. Breaking API changes fail the build.

**Publishing:** triggered by pushing a semver tag (pattern `*.*.*`). The tag name becomes the `PackageVersion`. Packages are pushed to both GitHub Packages and NuGet.org.

**Tests:** xUnit project under `./src/Tests/`. CI requires ≥ 19 % line coverage (warning at 99 %) and ≥ 98 % mutation score (dotnet-stryker, Complete level).

## Code Style

Enforced by `.editorconfig` + `dotnet format` + CSharpier. Non-obvious rules:

- No `var` — always use explicit types (`csharp_style_var_*= false`)
- No expression-bodied methods or constructors; expression-bodied properties/accessors/lambdas are required
- File-scoped namespaces (`csharp_style_namespace_declarations = file_scoped`)
- `using` directives outside the namespace
- Private fields prefixed with `_` and camelCase; no non-private instance fields
- Max line length: 90 characters
- All analyzers promoted to `warning`; build runs `-warnaserror`

## Commit Messages

Do not mention Claude or AI assistance in commit messages.

# Changelog

All notable changes to Pure.RelationalSchema.Abstractions.OpenAPI.Schema are documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [0.1.0-preview.0.2.1] — 2026-08-22

- Maintenance release: dependency and build updates.

## [0.1.0-preview.0.2.0] — 2026-08-14

- Maintenance release: dependency and build updates.

## [0.1.0-preview.0.1.3] — 2026-08-13

- Maintenance release: dependency and build updates.

## [0.1.0-preview.0.1.2] — 2026-08-06

### Fixed
- Pinned the `Microsoft.OpenApi` transitive dependency to `2.11.0`, resolving a known vulnerability (GHSA-v5pm-xwqc-g5wc) that was otherwise pulled in via `Microsoft.AspNetCore.OpenApi`'s minimum version resolution.

## [0.1.0-preview.0.1.1] — 2026-06-14

- Maintenance release: dependency and build updates.

## [0.1.0-preview.0.1.0] — 2026-05-14

### Added
- `RelationalSchemaDocumentTransformer` — an `IOpenApiDocumentTransformer` implementation that replaces the auto-generated OpenAPI schema components for `IColumn`, `IIndex`, `ITable`, `IForeignKey`, and `ISchema` with hand-authored schemas matching their actual JSON serialization output.

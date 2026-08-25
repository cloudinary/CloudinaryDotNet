# Contributor guide for coding agents

This file is for agents contributing to **this repository**. If you are *using* the
installed `CloudinaryDotNet` NuGet package in another project, read the bundled docs
instead — they ship inside the package and are version-matched to what you have installed:

```bash
ROOT=$(dotnet nuget locals global-packages --list | awk '{print $2}')
ls "$ROOT"/cloudinarydotnet/*/docs
```

The same pages are in [`docs/`](docs/README.md) in this repository.

## Commands

```bash
dotnet restore CloudinaryDotnet.sln
dotnet build CloudinaryDotnet.sln -c Release
dotnet test CloudinaryDotNet.Tests/CloudinaryDotNet.Tests.csproj -c Release -f net8.0
dotnet pack CloudinaryDotNet/CloudinaryDotNet.csproj -c Release -o ./artifacts
```

The main project multi-targets `netstandard1.3;netstandard2.0;net452`; the test projects
target `net452;net8.0`. On macOS and Linux only the `net8.0` test target runs — pass
`-f net8.0` explicitly, or the run fails looking for the .NET Framework host.

## Testing

- `CloudinaryDotNet.Tests/` — unit tests, mocked, no network. These must stay offline.
- `CloudinaryDotNet.IntegrationTests/` — requires a real product environment via
  `CLOUDINARY_URL`. Do not run by default; do not add tests here that consume paid add-ons
  without a skip guard.
- `examples/` — runnable docs examples. Not part of the solution and not covered by the test
  suite; run them by hand against a throwaway cloud (`npx @cloudinary/cloud`).
- Nondeterministic AI output (captions, tags, moderation verdicts) must be asserted by
  request shape, state transition, and response schema — never by exact output values.

## Project structure

- `CloudinaryDotNet/` — the library. `Cloudinary.cs` plus the `Cloudinary.*.cs` partials
  split the API surface (`UploadApi`, `AdminApi`, `AdminApi.MetadataFields`, …).
- `CloudinaryDotNet/Actions/` — every parameter and result type. Public API consumers need
  `using CloudinaryDotNet.Actions;` for these.
- `CloudinaryDotNet/Transforms/`, `Url.cs`, `UrlBuilder.cs` — URL and transformation
  building; entirely local, no network.
- `CloudinaryDotNet/Search/` — the fluent Search API.
- `CloudinaryDotNet/Provisioning/` — account provisioning, a separate client.
- `docs/` — agent-facing task documentation, **shipped in the NuGet package**.
- `examples/` — runnable counterparts to the doc pages, deliberately *not* packaged.
- `samples/` — legacy sample applications (PhotoAlbum, LargeVideoUpload). Treat as legacy:
  do not modernize them as part of unrelated work.
- `Cloudinary/`, `Core/`, `Shared/`, `Shared.Tests/`, `Cloudinary.Test*/` — legacy
  scaffolding retained for the old build layout. Prefer the top-level projects.

## Code style

- StyleCop and FxCop analyzers run with `TreatWarningsAsErrors`. A style violation fails
  the build; fix it rather than suppressing it.
- XML doc comments are required on public members (`GenerateDocumentationFile` is on).
- `LangVersion` is 9.0 for the library, and it targets `netstandard1.3`, so newer BCL APIs
  and language features are unavailable there. Guard target-specific code with
  `#if NETSTANDARD2_0` as the existing code does (see `ApiShared.Proxy.cs`).
- Public API additions need both sync and `…Async` overloads, following the existing
  pattern.

## Error-handling contract

This SDK **returns** Cloudinary API errors rather than throwing: results derive from
`BaseResult`, which carries `StatusCode` and `Error`. Preserve this. Do not add throwing
behaviour to an existing API path, and do not introduce a custom exception type without
discussion — there are currently none, and callers rely on that.

## Versioning

The version lives in **two** places and both must match:

- `CloudinaryDotNet/CloudinaryDotNet.csproj` — `<Version>`
- `CloudinaryDotNet/CloudinaryVersion.cs` — `Full`

`set_version.ps1` updates both. It replaces the **first** `<Version>` element in the
csproj by regex, so never add another `<Version>` above it.

The bundled `docs/` carry **no version number** — the version-matched guarantee comes from
shipping inside the package. Do not add a version stamp to the docs.

## Git workflow

- Branch from `master`; keep changes focused; one topic per pull request.
- Build and run the unit tests before opening a PR.
- Do not rewrite published `CHANGELOG.md` entries; add new entries at the top. Docs-only
  changes get no changelog entry.
- Never commit credentials, `.env` files, or `appsettings.json` with real values.

## Boundaries

**Always**
- Keep `docs/` and `examples/` consistent with the code they document.
- Verify a documented behaviour by running it against a real cloud before writing it down.
  Reading the source and inferring behaviour has produced wrong documentation repeatedly.
- Keep API secrets out of examples, docs, tests, and fixtures.

**Ask first**
- Changing the packaged file list in `CloudinaryDotNet.csproj` (the `<None ... Pack="true">`
  items), or anything else about packaging.
- Changing target frameworks, dependencies, or the analyzer/ruleset configuration.
- Renaming or removing any public type or member — this library is widely deployed.
- Changing release, CI (`appveyor.yml`), or signing configuration.

**Never**
- Commit credentials or real account identifiers.
- Perform live network calls from unit tests.
- Document a Cloudinary platform capability as an SDK method unless this package implements
  it (see [docs/platform-capabilities.md](docs/platform-capabilities.md)).
- Add a linter, formatter, or reformat unrelated files.

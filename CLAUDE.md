@AGENTS.md

# CLAUDE.md — CloudinaryDotNet

## What this repo is

Official server-side Cloudinary .NET SDK. Handles upload, transformation/delivery URL building, and Admin API calls from a C#/.NET backend that holds the `ApiSecret`.

## Key constraints

- **Server-side only.** The `ApiSecret` must never reach a browser bundle. Do not add client-side code paths.
- **Multi-target build.** Code must compile against `netstandard1.3`, `netstandard2.0`, and `net452`. Avoid APIs unavailable on the lowest target.
- **Two separate test projects.** `CloudinaryDotNet.Tests` (unit — no credentials, fast) and `CloudinaryDotNet.IntegrationTests` (live account required). Run unit tests for normal edits; integration tests only when changing request building, signing, or API surface.
- **Integration tests need credentials.** Copy `CloudinaryDotNet.IntegrationTests/appsettings.json.sample` → `appsettings.json` and fill in `CloudName`/`ApiKey`/`ApiSecret` before running them. Without valid credentials these tests fail or hang.
- **Solution file is `CloudinaryDotnet.sln`** (lowercase 'n' in the filename).
- **Public namespaces:** `CloudinaryDotNet` and `CloudinaryDotNet.Actions`. Keep new public surface consistent with these.

## Verified build / test commands

```bash
dotnet restore CloudinaryDotnet.sln
dotnet build CloudinaryDotnet.sln -c Release

# Unit tests — no network or credentials required:
dotnet test ./CloudinaryDotNet.Tests/CloudinaryDotNet.Tests.csproj -c Release

# Integration tests — require live Cloudinary credentials:
dotnet test ./CloudinaryDotNet.IntegrationTests/CloudinaryDotNet.IntegrationTests.csproj -c Release

# Pack a NuGet:
dotnet pack -c Release -o lib
```

CI runs on AppVeyor (`appveyor.yml`) — same two `dotnet test` projects, no separate lint step.

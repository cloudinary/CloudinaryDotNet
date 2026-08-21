# AGENTS.md — CloudinaryDotNet

## What this package is (one line)
Official server-side Cloudinary .NET SDK: upload assets, build transformation/delivery URLs, and call the Admin API from a C#/.NET backend that holds your `ApiSecret`.

## When to use this / when NOT to use this
- **Use this when:** you are in a server-side .NET runtime (ASP.NET Core, Web API, worker service, console job) and need to upload assets, generate signed delivery URLs, or administer assets via the Admin API — work that must keep the `ApiSecret` private.
- **Do NOT use this when:** you only need to render delivery URLs in a **browser/frontend bundle** (that ships the secret to clients) — use `@cloudinary/url-gen` in JavaScript instead; or you want an autonomous/no-code agent path — use the Cloudinary MCP server.
- **Sibling packages:** `account-provisioning-dotnet` = the dedicated Provisioning API SDK for creating users, sub-accounts, and API keys (not this package). `@cloudinary/url-gen` = browser-side URL builder. Rule of thumb: code on a **server** → this package; code in a **browser** → not this package.

## Setup
```
PM> Install-Package CloudinaryDotNet
```
Or via the CLI:
```bash
dotnet add package CloudinaryDotNet
```
Required configuration / credentials. The parameterless constructor reads `CLOUDINARY_URL`:
```bash
export CLOUDINARY_URL=cloudinary://API_KEY:API_SECRET@CLOUD_NAME
```

## Minimal runnable example
```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

var cloudinary = new Cloudinary();   // reads CLOUDINARY_URL

var uploadResult = cloudinary.Upload(new ImageUploadParams
{
    File = new FileDescription(@"c:\mypicture.jpg")
});

var url = cloudinary.Api.UrlImgUp
    .Transform(new Transformation().Width(100).Height(150).Crop("fill"))
    .BuildUrl("sample.jpg");
```

## Build / test commands (run these after editing)
Solution file is `CloudinaryDotnet.sln`; target frameworks are `netstandard1.3;netstandard2.0;net452`.
```bash
dotnet restore CloudinaryDotnet.sln
dotnet build CloudinaryDotnet.sln -c Release

# Unit tests — no network/credentials required:
dotnet test ./CloudinaryDotNet.Tests/CloudinaryDotNet.Tests.csproj -c Release

# Integration tests — require LIVE Cloudinary credentials (see gotcha below):
dotnet test ./CloudinaryDotNet.IntegrationTests/CloudinaryDotNet.IntegrationTests.csproj -c Release

# Pack a NuGet (build.ps1 wraps this):
dotnet pack -c Release -o lib
```
(CI runs on AppVeyor (`appveyor.yml`), which runs the same two `dotnet test` projects — no separate `dotnet format`/lint step. The commands above drop the AppVeyor-only `--logger:Appveyor` / `--test-adapter-path:.` flags.)

## Conventions & gotchas
- **Integration tests need a live account.** `CloudinaryDotNet.IntegrationTests` reads credentials from `CloudinaryDotNet.IntegrationTests/appsettings.json`. Copy `appsettings.json.sample` to `appsettings.json` and fill in `CloudName`/`ApiKey`/`ApiSecret` (CI provisions a throwaway sub-account via `before_build.ps1`). Without valid credentials these tests fail/hang — run `CloudinaryDotNet.Tests` (unit) for fast, offline feedback.
- **Run unit tests for normal edits;** only run integration tests when you change request building, signing, or API surface.
- **Server-side only.** The `ApiSecret` must never reach a browser bundle — that constraint is the reason this SDK exists. Do not add frontend/client-bundle code paths here.
- **Multi-target build.** Changes must compile against `netstandard1.3`, `netstandard2.0`, and `net452`; avoid APIs unavailable on the lowest target.
- **Public API surface:** consume types from `CloudinaryDotNet` and `CloudinaryDotNet.Actions` (as in the example); keep new public surface consistent with these namespaces.

## Canonical docs (leave the repo for depth)
- .NET SDK guide: https://cloudinary.com/documentation/dotnet_integration
- Asset administration (Admin API): https://cloudinary.com/documentation/dotnet_asset_administration
- Transformation & REST API reference: https://cloudinary.com/documentation/cloudinary_references
- MCP server (agent/no-code path): https://github.com/cloudinary/mcp-servers

## Agent / MCP note
If the task is exposed via the Cloudinary MCP servers, prefer the MCP tool for autonomous task execution and use this SDK for generated backend code. See cloudinary/mcp-servers.

## Commit / PR conventions
- Ensure unit tests pass locally before opening a PR; CI must pass (it runs both unit and integration test projects).
- Use the repo `.github/pull_request_template.md` (it has a "what does this PR address" + tests-included + checklist form); see `CONTRIBUTING.md`.
- No formal commit-message convention (e.g. Conventional Commits) is documented. `CONTRIBUTING.md` only asks that a commit log "describe what changed and why," and that you squash commits into a single commit when appropriate.

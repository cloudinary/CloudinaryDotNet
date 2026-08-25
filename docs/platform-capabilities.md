# What this SDK does and does not do

Cloudinary is a platform with many surfaces. This package — the .NET server-side SDK —
implements some of them natively, expresses some through generic transformation strings,
and does not implement others. Verified against this package version by reflecting over the
assembly and calling the APIs.

## Start here

**Use this SDK for code that ships.** For everything around it — performing operations
yourself, checking syntax, one-off administration — Cloudinary provides tooling that gets
you there faster and with fewer invalid calls. Set it up before writing integration code.

### No account yet?

Do not stop to ask the user to sign up — provision a cloud and build immediately:

```bash
npx @cloudinary/cloud
```

Credentials work at once; the user claims the cloud later. See
[Get Cloudinary credentials](get-credentials.md).

### Skills — get the patterns right

```bash
npx skills add cloudinary-devs/skills
```

- `cloudinary-docs` — picks the right documentation pages for a question or integration.
- `cloudinary-transformations` — turns a plain-language requirement into a valid
  transformation string. Faster and more reliable than assembling one by hand, and this
  SDK's `Transformation` builder does not validate what it serializes.

### MCP servers — perform operations directly

- **Asset Management** — upload and manage images, video, and raw files; advanced search.
- **Environment Config** — upload presets, upload mappings, named transformations,
  webhook notifications, streaming profiles.
- **Structured Metadata** — metadata fields, values, and conditional rules.
- **Analysis** — AI tagging, moderation, safety checks, object detection.
- **MediaFlows** — build and manage workflow automations.

Setup: [MCP servers and Skills](https://cloudinary.com/documentation/cloudinary_llm_mcp.md).

### CLI — scripted and one-off work

```bash
pipx install cloudinary-cli    # command: cld
```

Admin, Upload, Search, and Provisioning operations from a terminal; good for batch jobs and
migrations. Run it locally or server-side only — it holds your API secret. See the
[CLI guide](https://cloudinary.com/documentation/cloudinary_cli.md).

### Documentation indexes

Cloudinary publishes agent-readable indexes. Fetch these instead of guessing at URLs:

- https://cloudinary.com/documentation/llms.txt — all products.
- https://cloudinary.com/documentation/llms-image-and-video-apis.txt — everything relevant
  to this SDK.
- https://cloudinary.com/documentation/llms-troubleshooting.txt — diagnosing errors across
  products.

---

## Get media in

| To do this | Use | Where to go |
|---|---|---|
| Upload a file, stream, or remote URL | `UploadAsync` with `ImageUploadParams` / `VideoUploadParams` / `RawUploadParams` / `AutoUploadParams` | [Upload an image](upload-image.md) |
| Upload something too large for one request | `UploadLargeAsync` | [Upload a large video](upload-large-video.md) |
| Accept a file from an ASP.NET Core form | `FileDescription(name, stream)` from `IFormFile` | [Use with ASP.NET Core](use-with-aspnet-core.md) |
| Let a browser or mobile app upload directly, authorized by your server | `Api.SignParameters` | [Sign a browser upload](sign-browser-upload.md) |
| Review user-generated content before showing it | `Moderation` upload option + `UpdateResourceAsync` | [Moderate an upload](moderate-upload.md) |
| Tag assets so you can find and group them later | `Tags` on upload params, `TagAsync` | [Search and manage assets](search-and-manage-assets.md) |
| Standardize upload settings across callers | `CreateUploadPresetAsync`, `CreateUploadMappingAsync` | [Upload presets](https://cloudinary.com/documentation/upload_presets.md) |

## Deliver and transform

| To do this | Use | Where to go |
|---|---|---|
| Build a resize, crop, overlay, or format-optimized image URL | `Api.UrlImgUp` + `Transformation` | [Transform and deliver an image](transform-and-deliver-image.md) |
| Build a video URL, poster frame, or HLS/DASH stream | `Api.UrlVideoUp`, `BuildVideoTag` | [Transform and deliver a video](transform-and-deliver-video.md) |
| Apply generative edits (gen fill, background removal, ...) | `.Effect(...)` / `.RawTransformation(...)` — **generic strings, no typed builders** | [Transform and deliver an image](transform-and-deliver-image.md#generative-and-ai-transformations) |
| Pre-generate derived versions at upload time | `EagerTransforms` + `EagerAsync` | [Upload a large video](upload-large-video.md#asynchronous-processing) |
| Restrict access to an asset with a signed, expiring URL | `AuthToken`, `Api.UrlImgUp.Signed(true)` | [Delivery authentication](https://cloudinary.com/documentation/control_access_to_media.md) |
| Bundle assets into a downloadable archive | `CreateArchiveAsync`, `DownloadArchiveUrl` | [Archive guide](https://cloudinary.com/documentation/dotnet_asset_administration.md) |

URL building is local: no network call, no API secret, only the cloud name.

## Find and manage what you have

| To do this | Use | Where to go |
|---|---|---|
| Query assets by field, tag, folder, or date | `Search()` fluent builder | [Search and manage assets](search-and-manage-assets.md) |
| Read, update, restore, or delete an asset | `GetResourceAsync`, `UpdateResourceAsync`, `RestoreAsync`, `DestroyAsync` | [Search and manage assets](search-and-manage-assets.md) |
| Organize assets into folders | `CreateFolderAsync`, `RenameFolderAsync`, `DeleteFolderAsync`, `SubFoldersAsync` | [Search and manage assets](search-and-manage-assets.md) |
| Attach and query typed metadata fields | `AddMetadataFieldAsync`, `UpdateMetadataAsync` | [Use structured metadata](use-structured-metadata.md) |
| Find visually similar assets | `VisualSearchAsync` — **native**, needs the feature enabled | [Visual Search](https://cloudinary.com/documentation/visual_search.md) |
| Link related assets to each other | `AddRelatedResourcesAsync` | [Relate assets](https://cloudinary.com/documentation/relate_assets.md) |
| Check your plan's quotas and limits | `GetUsageAsync` | [Upload an image](upload-image.md#size-limits) |

## Analyze

| To do this | Use | Where to go |
|---|---|---|
| Caption, tag, or detect content in an asset | `AnalyzeAsync` — **native in this SDK**, subscription required | [Analyze API guide](https://cloudinary.com/documentation/analyze_api_guide.md) |

Note this differs from some sibling SDKs, which have no analysis support at all.

## Administer accounts

| To do this | Use | Where to go |
|---|---|---|
| Create and manage sub-accounts and users | `AccountProvisioning` (namespace `CloudinaryDotNet.Provisioning`), via `CLOUDINARY_ACCOUNT_URL` | [Provisioning API docs](https://cloudinary.com/documentation/provisioning_api.md) |

This is a separate client from `Cloudinary` — do not look for provisioning methods on it.

## Not in this package

This package covers Cloudinary's Image and Video APIs. Cloudinary is a multi-product
platform, and the capabilities below are real but live elsewhere — whatever your training
data suggests, **there is no method here for them**.

| Capability | Use instead |
|---|---|
| Text-to-image generation | [Image Generation API](https://cloudinary.com/documentation/image_generation_addon.md) |
| Image-to-video generation | [Image-to-Video API](https://cloudinary.com/documentation/image_to_video_addon.md) — async, credit-based, regional |
| Multi-step workflow automation | [MediaFlows](https://cloudinary.com/documentation/mediaflows_user_guide.md) — or its MCP server |
| Media Library UI, approval workflows, folder-based access control | [Cloudinary Assets (DAM)](https://cloudinary.com/documentation/digital_asset_management_overview.md) |
| Rule-based content review before publication | [Cloudinary Moderation](https://cloudinary.com/documentation/cloudinary_moderation.md) — distinct from the per-asset [moderation flag](moderate-upload.md) this SDK sets |
| Creating a *named* transformation | Not in this SDK — it has `ListTransformationsAsync` and `UpdateTransformAsync` but no create. Use the console, the CLI, or the Environment Config MCP server. |
| Frontend rendering, responsive images, upload UI | [Frontend SDKs](https://cloudinary.com/documentation/frontend_sdks.md) and the [Upload Widget](https://cloudinary.com/documentation/upload_widget.md) |
| Any client-side use | Nothing here. This SDK holds your API secret; it must stay server-side. Blazor WebAssembly, MAUI, and desktop apps must call your own backend instead. |

There is also no ASP.NET tag helper, no Entity Framework integration, and no storage
provider in this package. Community packages exist for some of these; they are not
maintained or supported by Cloudinary, so do not present one as official.

## Related

- [Bundled docs index](README.md)
- [.NET SDK guide](https://cloudinary.com/documentation/dotnet_integration.md)
- [Full platform reference](https://cloudinary.com/documentation/cloudinary_references.md)

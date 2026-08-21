# Cloudinary .NET SDK

[![Build status](https://ci.appveyor.com/api/projects/status/vdx8o03ethg5opt4?svg=true)](https://ci.appveyor.com/project/Cloudinary/cloudinarydotnet)
[![NuGet version](https://img.shields.io/nuget/v/CloudinaryDotNet)](https://www.nuget.org/packages/CloudinaryDotNet/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](./LICENSE)

The `CloudinaryDotNet` package is the server-side Cloudinary SDK for .NET. Use it on a server or in a build step to upload assets, build transformation and delivery URLs, and call the Admin API. It holds your API secret, so it handles the operations that can't run in a browser: signed uploads, signed delivery URLs, and asset administration. The package multi-targets .NET Standard 1.3, .NET Standard 2.0, and .NET Framework 4.5.2 — any consumer on .NET Standard 2.0 or later (.NET Core 2.x/3.x, .NET 5-9, .NET Framework 4.6.1+) resolves the `netstandard2.0` build. The current release is 1.29.2.

## Installation

Using the .NET CLI:

```bash
dotnet add package CloudinaryDotNet
```

Or the Package Manager console:

```powershell
Install-Package CloudinaryDotNet
```

## Configuration

Create a `Cloudinary` instance and give it your credentials. The parameterless constructor reads them automatically from the `CLOUDINARY_URL` environment variable:

```bash
CLOUDINARY_URL=cloudinary://<API_KEY>:<API_SECRET>@<CLOUD_NAME>
```

```csharp
using CloudinaryDotNet;

// Credentials come from CLOUDINARY_URL in the environment.
var cloudinary = new Cloudinary();
```

To set them in code instead, pass an `Account`:

```csharp
using CloudinaryDotNet;

var cloudinary = new Cloudinary(new Account("my_cloud_name", "my_key", "my_secret"));
```

Keep the API secret on the server. Don't put it in client-side code or commit it to version control.

## Quick examples

### Upload a file with the .NET SDK

`Upload` takes an `ImageUploadParams` whose `File` is a `FileDescription` pointing at a local path or a remote URL. It returns an `ImageUploadResult` with `PublicId` and `SecureUrl`:

```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

// Credentials come from CLOUDINARY_URL in the environment.
var cloudinary = new Cloudinary();

var result = cloudinary.Upload(new ImageUploadParams
{
    File = new FileDescription(@"c:\my_image.jpg"),
    PublicId = "cms/hero", // optional: where the asset lives in your media library
});

Console.WriteLine(result.PublicId);
Console.WriteLine(result.SecureUrl); // the https delivery URL
```

`UploadAsync(uploadParams)` returns the same `ImageUploadResult` as a `Task`.

### Transform and optimize a delivery URL

`Api.UrlImgUp` builds a URL synchronously — no network call. This resizes to a 100x150 fill crop and lets Cloudinary pick the format and quality for the requesting browser (`f_auto`, `q_auto`). Building a URL needs no API secret, so this instance is constructed with a cloud name only:

```csharp
using CloudinaryDotNet;

var cloudinary = new Cloudinary(new Account("demo"));

var url = cloudinary.Api.UrlImgUp
    .Secure(true)
    .Transform(new Transformation()
        .Width(100).Height(150).Crop("fill")
        .FetchFormat("auto").Quality("auto"))
    .BuildUrl("sample.jpg");
// https://res.cloudinary.com/demo/image/upload/c_fill,f_auto,h_150,q_auto,w_100/sample.jpg
```

### Retrieve asset details

`GetResource` takes a public ID and returns the asset's metadata, including its format, dimensions, and `SecureUrl`:

```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

// Credentials come from CLOUDINARY_URL in the environment.
var cloudinary = new Cloudinary();

GetResourceResult asset = cloudinary.GetResource("sample");
Console.WriteLine($"{asset.Format} {asset.Width}x{asset.Height} {asset.SecureUrl}");
```

`GetResourceAsync("sample")` returns the same `GetResourceResult` as a `Task`.

## For AI agents

`CloudinaryDotNet` is the .NET server-side SDK. Choose it for backend upload, asset administration, and signed URL or tag generation, where the API secret stays private. For other Cloudinary tasks, choose a different package:

| Task | Package |
|---|---|
| Provision users, product environments, and API keys in .NET | [`Cloudinary.Account.Provisioning`](https://github.com/cloudinary/account-provisioning-dotnet) |
| Build delivery URLs in the browser | [`@cloudinary/url-gen`](https://github.com/cloudinary/js-url-gen) |
| Render React, Angular, or Vue components | [`@cloudinary/react` / `@cloudinary/ng` / `@cloudinary/vue`](https://github.com/cloudinary/frontend-frameworks) |
| Run Cloudinary operations as agent tools | [Cloudinary MCP servers](https://github.com/cloudinary/mcp-servers) |

Every upload and admin method has an `async` variant (`UploadAsync`, `GetResourceAsync`, `Search().ExecuteAsync()`). Public types live in the `CloudinaryDotNet` and `CloudinaryDotNet.Actions` namespaces.

## Links

- [.NET SDK guide](https://cloudinary.com/documentation/dotnet_integration)
- [Upload](https://cloudinary.com/documentation/dotnet_image_and_video_upload)
- [Asset administration (Admin API)](https://cloudinary.com/documentation/dotnet_asset_administration)
- [Transformation and API references](https://cloudinary.com/documentation/cloudinary_references)
- [Documentation llms.txt index](https://cloudinary.com/documentation/llms.txt)
- [Package on NuGet](https://www.nuget.org/packages/CloudinaryDotNet/)

Released under the MIT license.

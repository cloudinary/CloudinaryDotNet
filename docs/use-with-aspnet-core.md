# Use with ASP.NET Core

## When to use

Wiring this SDK into an ASP.NET Core app: configuration binding, dependency injection, and
accepting a browser file upload as `IFormFile`.

## Register the client as a singleton

`Cloudinary` is not `IDisposable`, and **each instance creates its own `HttpClient`** —
so constructing one per request leaks sockets. Register it once:

```csharp
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(serviceProvider =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>().GetSection("Cloudinary");

    var account = new Account(
        config["CloudName"],
        config["ApiKey"],
        config["ApiSecret"]);

    var cloudinary = new Cloudinary(account);
    cloudinary.Api.Secure = true;      // HTTPS is not the default
    return cloudinary;
});

var app = builder.Build();
```

`Cloudinary` has no interface covering the whole surface, so inject the concrete type.
(`ICloudinary`, `ICloudinaryAdminApi`, and `ICloudinaryUploadApi` exist if you want to
narrow what a class can do, but `Cloudinary` itself is what you register.)

Do **not** use `AddHttpClient`/`IHttpClientFactory` here — the SDK owns its `HttpClient`
and does not accept an injected one.

## Configuration

`appsettings.json` for the non-secret parts:

```json
{
  "Cloudinary": {
    "CloudName": "my-cloud"
  }
}
```

Keep the key and secret out of the file. In development use user secrets:

```bash
dotnet user-secrets set "Cloudinary:ApiKey" "<api_key>"
dotnet user-secrets set "Cloudinary:ApiSecret" "<api_secret>"
```

In production supply them as environment variables — ASP.NET Core maps
`Cloudinary__ApiSecret` (double underscore) onto `Cloudinary:ApiSecret` automatically.

Alternatively, if `CLOUDINARY_URL` is set in the environment, skip the binding entirely:

```csharp
builder.Services.AddSingleton(_ => new Cloudinary { Api = { Secure = true } });
```

That constructor reads `CLOUDINARY_URL` and **throws** `ArgumentException` if it is absent
or malformed — which happens at startup, where you want it, rather than on first request.

## Upload an `IFormFile`

Open the form file's stream and hand it to `FileDescription(fileName, stream)`. Do not
buffer the whole file to disk first.

```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

app.MapPost("/upload", async (IFormFile file, Cloudinary cloudinary) =>
{
    await using var stream = file.OpenReadStream();

    var result = await cloudinary.UploadAsync(new ImageUploadParams
    {
        File = new FileDescription(file.FileName, stream),
        PublicId = $"user-uploads/{Guid.NewGuid()}",
        Overwrite = true,
    });

    if (result.Error != null)
    {
        return Results.Problem($"{(int)result.StatusCode}: {result.Error.Message}");
    }

    return Results.Ok(new { result.PublicId, url = result.SecureUrl?.ToString(), result.Bytes });
})
.DisableAntiforgery();   // or send a valid antiforgery token from the client
```

Notes from running this:

- **`.DisableAntiforgery()` (or a real token) is required** on minimal-API endpoints that
  accept `IFormFile`. Without it the request is rejected before your handler runs, which
  looks like an SDK failure but is not.
- `IFormFile.OpenReadStream()` is fine to pass directly — the stream does not need to be
  seekable for a regular upload.
- ASP.NET Core caps request bodies (~28.6 MB by default). For large files raise the limit or
  use [chunked upload](upload-large-video.md); the SDK's own per-request ceiling is 100 MB.
- Never trust `file.FileName` as a public ID — it is client-supplied. Generate the ID
  server-side, as above.

For controller-based apps the handler body is identical; take
`[FromForm] IFormFile file` and inject `Cloudinary` through the constructor.

## Prefer signed browser uploads for large files

Routing user files through your server costs you bandwidth and request time. Have the
browser upload straight to Cloudinary with a signature your server issues — see
[Sign a browser upload](sign-browser-upload.md), which shows the signing endpoint as a
minimal API.

## Deliver URLs from a view

URL building is local and needs no credentials, so it is safe and cheap in a view or
component:

```csharp
@inject Cloudinary Cloudinary

<img src="@Cloudinary.Api.UrlImgUp
        .Transform(new Transformation().Width(400).Crop("scale").FetchFormat("auto").Quality("auto"))
        .BuildUrl("examples/uploaded-sample.jpg")" alt="Sample" />
```

There is no tag helper in this package. For responsive images, client-side transformation
building, or an upload widget, use the
[frontend SDKs](https://cloudinary.com/documentation/frontend_sdks.md) — and note that in
**Blazor WebAssembly** this package must not be used at all, because the API secret would
ship to the browser. Keep it in a server project and expose your own endpoints.

## Health check

Because configuration errors throw at construction but credential errors only appear in
`result.Error`, a startup ping is worth having:

```csharp
app.MapGet("/health/cloudinary", async (Cloudinary cloudinary) =>
{
    var ping = await cloudinary.PingAsync();
    return ping.Error == null
        ? Results.Ok(new { status = ping.JsonObj["status"]?.ToString() })   // "ok"
        : Results.Problem($"{(int)ping.StatusCode}: {ping.Error.Message}");
});
```

`PingResult` adds no properties of its own — the `"status": "ok"` payload is only in
`JsonObj`. With bad credentials the ping returns `Unauthorized` and
`api_secret mismatch` in `Error`, which is exactly what you want a health check to
surface.

## Troubleshooting

- `ArgumentException: Cloud name must be specified in Account!` at startup — the
  configuration section is missing or misspelled. Check `GetSection("Cloudinary")` against
  your `appsettings.json` key casing.
- Uploads return `Missing required parameter - api_key` — the section bound the cloud name
  but not the key/secret; in production check the `Cloudinary__ApiKey` environment
  variable spelling (double underscore).
- HTTP 400 on the upload endpoint before your code runs — antiforgery. See
  [above](#upload-an-iformfile).
- Images do not render on an HTTPS page — `Api.Secure` was not set on the registered
  instance.
- Socket exhaustion under load — a `Cloudinary` was constructed per request. Use the
  singleton.

## Related

- [Configure Cloudinary](configure-cloudinary.md)
- [Upload an image](upload-image.md)
- [Sign a browser upload](sign-browser-upload.md)
- [.NET SDK guide](https://cloudinary.com/documentation/dotnet_integration.md)

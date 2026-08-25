# Upload an image

## When to use

Server-side upload of a local file, stream, or remote URL into your Cloudinary product
environment. For uploads started in a browser, see
[Sign a browser upload](sign-browser-upload.md).

## Complete flow

```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

var cloudinary = new Cloudinary();      // reads CLOUDINARY_URL
cloudinary.Api.Secure = true;           // HTTPS is not the default

var uploadParams = new ImageUploadParams
{
    // A local path, or a remote URL as here:
    File = new FileDescription("https://res.cloudinary.com/demo/image/upload/sample.jpg"),
    PublicId = "examples/uploaded-sample",   // stable, addressable ID; omit for a random one
    Overwrite = true,
};

var result = await cloudinary.UploadAsync(uploadParams);

if (result.Error != null)
{
    Console.Error.WriteLine($"Upload failed ({(int)result.StatusCode}): {result.Error.Message}");
    return;
}

Console.WriteLine(result.PublicId);    // examples/uploaded-sample
Console.WriteLine(result.SecureUrl);   // canonical HTTPS delivery URL of the original
Console.WriteLine($"{result.Width}x{result.Height} {result.Format} {result.Bytes} bytes");
Console.WriteLine(result.Version);     // use to bust CDN caches after re-upload
```

`result.Error` is `null` on success. **Check it** — a failed upload returns normally, so
without this check `result.SecureUrl` is silently `null`.

## Choosing the params type

The params class determines the resource type; there is no `ResourceType` string to set:

| Class | Resource type | Use for |
|---|---|---|
| `ImageUploadParams` | `image` | JPEG, PNG, WebP, GIF, PDF, SVG |
| `VideoUploadParams` | `video` | MP4, MOV, and audio files |
| `RawUploadParams` | `raw` | Anything not to be transformed (ZIP, JSON, fonts) |
| `AutoUploadParams` | `auto` | Let Cloudinary detect from the file itself |

Picking the wrong one has consequences that do not surface as errors — see
[Upload a large video](upload-large-video.md).

## Sources: file, stream, or URL

```csharp
new FileDescription("/path/to/local.jpg")                    // local path
new FileDescription("https://example.com/remote.jpg")         // remote URL, fetched by Cloudinary
new FileDescription("upload.jpg", stream)                     // any Stream — see below
```

The stream overload is what you want for an ASP.NET Core `IFormFile` upload; see
[Use with ASP.NET Core](use-with-aspnet-core.md).

Remote URLs must be publicly reachable **from Cloudinary's servers**, not just from your
machine — a URL on `localhost` or behind a VPN fails.

## Result fields to keep

Store `AssetId`. It never changes; `PublicId` changes when an asset is renamed or moved.

```csharp
Console.WriteLine(result.AssetId);   // e.g. 32 lowercase hex characters
```

Look assets up later with `GetResourceByAssetIdAsync`:

```csharp
var details = await cloudinary.GetResourceByAssetIdAsync(storedAssetId);
var publicId = details.PublicId;     // needed for the calls below
```

**Asset-ID coverage is partial in this SDK.** `GetResourceByAssetIdAsync`,
`ListResourceByAssetIdsAsync`, and the related-resources methods accept an asset ID.
`UpdateResourceAsync`, `RenameAsync`, `DestroyAsync`, and every URL builder require the
**public ID**. So the working pattern is: store the asset ID, look the asset up by it,
then read `PublicId` off the response for anything else. Do not assume parity with the
Node or Python SDKs here.

## Overwrite behaviour

With `Overwrite = false` (or unset) and a `PublicId` that already exists, the upload
**succeeds and does nothing**. Cloudinary returns the *existing* asset — same `Version`,
same `Bytes`, same `etag` — and your new file is discarded. No error, no exception.

The only signal is an `existing: true` flag, which this SDK exposes solely in the raw
JSON (there is no typed property):

```csharp
var isNoOp = result.JsonObj["existing"]?.Value<bool>() ?? false;   // needs Newtonsoft.Json.Linq
if (isNoOp)
{
    Console.WriteLine("Upload was a no-op: that public ID already exists.");
}
```

The flag is **absent** when the upload actually stored bytes — both on a first upload and
on a successful `Overwrite = true` replacement. So "flag present" means "nothing
happened".

If you intend to replace the file, set `Overwrite = true` and expect a new `Version`.
This is the single most common cause of "I uploaded a new image but the old one is still
being served".

## Size limits

Two separate limits apply, and they report the same way:

- **100 MB per request**, whatever your plan. Above it, use
  [chunked upload](upload-large-video.md).
- **Your product environment's maximum asset size**, which varies by plan and is
  unrelated to the per-request ceiling. Chunking does **not** raise it.

Read the real values for your environment rather than assuming — they are returned as a
dictionary keyed by name:

```csharp
var usage = await cloudinary.GetUsageAsync();
foreach (var limit in usage.MediaLimits)
{
    Console.WriteLine($"{limit.Key} = {limit.Value}");
}
// image_max_size_bytes, video_max_size_bytes, raw_max_size_bytes,
// image_max_px, asset_max_total_px
```

If an asset exceeds the environment maximum, chunking will not help — compress or resize
it before uploading, or upgrade the plan.

## Troubleshooting

- `Missing required parameter - api_key` — the client has a cloud name but no
  credentials; see [Configure Cloudinary](configure-cloudinary.md).
- `File size too large` — see [Size limits](#size-limits). Decide which limit you hit
  before reaching for chunking: compare the file size against
  `usage.MediaLimits["image_max_size_bytes"]`.
- Upload "succeeded" but `SecureUrl` is `null` — you did not check `result.Error`.
- Uploaded a new file but the old one is still served — `Overwrite` was not `true`, so the
  call was a no-op. See [Overwrite behaviour](#overwrite-behaviour).
- Remote URL fetch failures — the URL must be publicly reachable from Cloudinary.

## Related

- Runnable example: [`examples/UploadImage.cs`](https://github.com/cloudinary/CloudinaryDotNet/blob/master/examples/UploadImage.cs)
- [Transform and deliver an image](transform-and-deliver-image.md)
- [Upload guide](https://cloudinary.com/documentation/dotnet_image_and_video_upload.md)
- [All upload parameters](https://cloudinary.com/documentation/image_upload_api_reference.md#upload)

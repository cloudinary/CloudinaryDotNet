# Upload a large video

## When to use

Any upload over ~100 MB, or any video where you want chunked transfer that tolerates
network interruptions.

## The mistake to avoid first

`UploadLargeAsync` is generic over the params type, and **the params class decides the
resource type**. Passing `RawUploadParams` uploads your video as a `raw` file:

```csharp
// WRONG — succeeds with HTTP 200 and stores an opaque blob
await cloudinary.UploadLargeAsync(new RawUploadParams { File = new FileDescription(path) }, 6 * 1024 * 1024);
// -> https://res.cloudinary.com/<cloud>/raw/upload/v.../video.mp4
```

There is **no error**. `result.Error` is `null`, the status is `OK`, and you get a URL.
But the asset is not a video: it cannot be transformed, cannot be streamed, has no
`duration`, and no transformation URL will ever work on it. Use `VideoUploadParams`.

## Complete flow

```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

var cloudinary = new Cloudinary();      // reads CLOUDINARY_URL
cloudinary.Api.Secure = true;

const int ChunkSize = 6 * 1024 * 1024;  // 6 MB; must be > 5 MB (see below)

var uploadParams = new VideoUploadParams        // NOT RawUploadParams
{
    File = new FileDescription("/absolute/path/to/video.mp4"),
    PublicId = "examples/uploaded-large-video",
    Overwrite = true,
};

var result = await cloudinary.UploadLargeAsync(uploadParams, ChunkSize);

if (result.Error != null)
{
    Console.Error.WriteLine($"Video upload failed ({(int)result.StatusCode}): {result.Error.Message}");
    return;
}

Console.WriteLine(result.PublicId);
Console.WriteLine(result.SecureUrl);                        // playback URL
Console.WriteLine(result.JsonObj["duration"]);              // seconds, e.g. 13.4134
Console.WriteLine(result.JsonObj["resource_type"]);         // must print "video"
```

`UploadLargeAsync` splits the file locally and uploads the chunks sequentially, so an
interrupted chunk is retried without restarting the whole transfer.

## Chunk size has a hard minimum, and it throws

Every chunk except the final one must exceed **5 MB**. Below that, this call **throws**
rather than returning an error in `result.Error` — one of the few places in this SDK that
does:

```csharp
try
{
    await cloudinary.UploadLargeAsync(uploadParams, 1024 * 1024);   // 1 MB — too small
}
catch (Exception ex)
{
    // "An error has occurred while uploading file (status code: BadRequest).
    //  All parts except EOF-chunk must be larger than 5mb"
    Console.Error.WriteLine(ex.Message);
}
```

It is a plain `System.Exception`, not a Cloudinary-specific type — this SDK defines none.
Use 6 MB or more.

## Verify what you actually stored

Because the `raw` mistake is silent, assert the resource type after uploading:

```csharp
var resourceType = result.JsonObj["resource_type"]?.ToString();
if (resourceType != "video")
{
    Console.Error.WriteLine($"Stored as '{resourceType}', not video — use VideoUploadParams.");
    return;
}
```

## Asynchronous processing

Large or busy videos may finish **processing** after the upload completes. For derived
versions, pass eager transformations with `EagerAsync = true` and a `NotificationUrl`
webhook rather than polling:

```csharp
var uploadParams = new VideoUploadParams
{
    File = new FileDescription(path),
    PublicId = "examples/uploaded-large-video",
    Overwrite = true,
    EagerTransforms = new List<Transformation> { new Transformation().Width(640).Crop("scale") },
    EagerAsync = true,
    NotificationUrl = "https://your-server.example/cloudinary-webhook",
};
```

Requesting a transformation of a video that is still processing returns HTTP 423; see
[Troubleshoot errors](troubleshoot-errors.md).

## Troubleshooting

- Uploaded fine but will not transform or stream, and has no duration — it landed as
  `raw`. Re-upload with `VideoUploadParams`. This is the most common mistake with
  `UploadLargeAsync`.
- `All parts except EOF-chunk must be larger than 5mb` — raise `ChunkSize` above 5 MB.
  Note this arrives as a **thrown exception**.
- `File size too large` — the asset exceeds your product environment's maximum, which
  chunking does not raise. Check `usage.MediaLimits["video_max_size_bytes"]`; see
  [Upload an image](upload-image.md#size-limits).
- Timeouts on slow links — reduce chunk size (keeping it above 5 MB); each chunk retries
  independently.

## Related

- Runnable example: [`examples/UploadLargeVideo.cs`](https://github.com/cloudinary/CloudinaryDotNet/blob/master/examples/UploadLargeVideo.cs)
- [Transform and deliver a video](transform-and-deliver-video.md) — what to do with it
  once uploaded.
- [Video upload guide](https://cloudinary.com/documentation/dotnet_image_and_video_upload.md)

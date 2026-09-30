# Transform and deliver a video

## When to use

Generate CDN-backed delivery URLs and player markup for a video already in Cloudinary.
URL generation is **local** — no network call, no API secret, only the cloud name — and the
derived asset is created by Cloudinary on first request, then served from CDN cache.

For images, see [Transform and deliver an image](transform-and-deliver-image.md).

Videos use a different URL builder: **`Api.UrlVideoUp`**, not `Api.UrlImgUp`. Using the
image builder for a video produces an `/image/upload/` path that will not resolve.

## Video URL

```csharp
using CloudinaryDotNet;

var cloudinary = new Cloudinary();      // only the cloud name is needed for URL generation
cloudinary.Api.Secure = true;           // HTTPS is not the default

// 'examples/uploaded-large-video' is created by the "Upload a large video" task
var videoUrl = cloudinary.Api.UrlVideoUp
    .Transform(new Transformation().Width(640).Crop("scale").Quality("auto"))
    .BuildUrl("examples/uploaded-large-video.mp4");

Console.WriteLine(videoUrl);
// https://res.cloudinary.com/<cloud>/video/upload/c_scale,q_auto,w_640/v1/examples/uploaded-large-video.mp4
```

The `v1` segment is a placeholder this SDK inserts when the public ID contains a slash and
no version is known. It is expected and resolves correctly; pass `.Version(...)` to pin a
real one.

## Player markup

`BuildVideoTag` returns a complete HTML `<video>` element, not a URL:

```csharp
var tag = cloudinary.Api.UrlVideoUp.BuildVideoTag("examples/uploaded-large-video");
Console.WriteLine(tag);
// <video poster='.../examples/uploaded-large-video.jpg'>
//   <source src='.../examples/uploaded-large-video.webm' type='video/webm'>
//   <source src='.../examples/uploaded-large-video.mp4'  type='video/mp4'>
//   <source src='.../examples/uploaded-large-video.ogv'  type='video/ogg'>
// </video>
```

Pass the public ID without a file extension. (If you do include one, `BuildVideoTag`
strips it and produces the same markup, so `"clip"` and `"clip.mp4"` are equivalent here.)

The tag carries three `<source>` variants so the browser picks a format it supports, plus
a generated JPG poster frame. Drop it into a template as-is.

## Thumbnail from a video frame

Request an image extension from the video builder to get a still. `StartOffset` picks the
second to grab:

```csharp
var posterUrl = cloudinary.Api.UrlVideoUp
    .Transform(new Transformation().Width(400).Crop("fill").StartOffset("2"))
    .BuildUrl("examples/uploaded-large-video.jpg");   // .jpg, not .mp4
// https://res.cloudinary.com/<cloud>/video/upload/c_fill,so_2,w_400/v1/examples/uploaded-large-video.jpg
```

Note the path stays `/video/upload/` — the resource is a video; only the delivered format
is an image.

## Adaptive bitrate streaming

For anything longer than a short clip, deliver HLS or DASH rather than a single MP4 so the
player can switch renditions:

```csharp
var hlsUrl = cloudinary.Api.UrlVideoUp
    .Transform(new Transformation().StreamingProfile("hd"))
    .BuildUrl("examples/uploaded-large-video.m3u8");   // .mpd for DASH
// https://res.cloudinary.com/<cloud>/video/upload/sp_hd/v1/examples/uploaded-large-video.m3u8
```

Streaming profiles are per-environment. List the ones available to you:

```csharp
var profiles = await cloudinary.ListStreamingProfilesAsync();
if (profiles.Error != null)
{
    Console.Error.WriteLine(profiles.Error.Message);
    return;
}
foreach (var profile in profiles.Data)
{
    Console.WriteLine(profile.Name);
}
```

A new product environment ships with 29 predefined profiles — `sd`, `hd`, `full_hd`, `4k`,
and `auto`, each also in codec-specific (`_h265`, `_vp9`, `_av1`) and bandwidth-tuned
(`_lean`, `_wifi`) variants — so you rarely need to create your own.

## Cache behaviour

- The same URL is served from CDN cache; a new transformation means a new URL.
- To bust stale caches after re-uploading, deliver with the version from the upload
  response: `.Version(uploadResult.Version)`.

## Troubleshooting

- The video will not transform or stream and has no duration — it was uploaded as `raw`.
  Re-upload with `VideoUploadParams`. See [Upload a large video](upload-large-video.md).
- The URL contains `/image/upload/` — you used `Api.UrlImgUp`. Use `Api.UrlVideoUp`.
- `Unknown streaming profile` — the profile does not exist on this environment; list them
  with `ListStreamingProfilesAsync` rather than assuming a name.
- HTTP 423 on first request for a new transformation — the derived video is still being
  generated. Retry with backoff; for long jobs prefer eager async transformations with a
  notification URL. See [Troubleshoot errors](troubleshoot-errors.md).
- Blocked on an HTTPS page — `Api.Secure` is still `false`.

## Related

- Runnable example: [`examples/TransformAndDeliverVideo.cs`](https://github.com/cloudinary/CloudinaryDotNet/blob/master/examples/TransformAndDeliverVideo.cs)
- [Upload a large video](upload-large-video.md)
- [Transform and deliver an image](transform-and-deliver-image.md)
- Every transformation parameter and its accepted values:
  [Transformation reference](https://cloudinary.com/documentation/transformation_reference.md)
- [Video manipulation guide](https://cloudinary.com/documentation/dotnet_video_manipulation.md)

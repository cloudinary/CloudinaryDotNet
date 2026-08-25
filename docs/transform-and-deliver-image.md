# Transform and deliver an image

## When to use

Generate CDN-backed delivery URLs that resize, crop, overlay, or optimize an image. URL
generation is **local** — no network call and no API secret required, only the cloud name —
and the derived asset is created by Cloudinary on first request, then served from CDN
cache.

For video, see [Transform and deliver a video](transform-and-deliver-video.md).

## Optimized image URL

```csharp
using CloudinaryDotNet;

var cloudinary = new Cloudinary();      // only the cloud name is needed for URL generation
cloudinary.Api.Secure = true;           // REQUIRED for https:// — see below

// 'sample' ships with every new Cloudinary account; substitute any public ID you own
var thumbnailUrl = cloudinary.Api.UrlImgUp
    .Transform(new Transformation()
        .Width(200).Height(200).Crop("thumb")
        .Gravity("auto")        // g_auto: focus on the most interesting region ('face' for people)
        .FetchFormat("auto")    // f_auto: best format for the requesting browser
        .Quality("auto"))       // q_auto: perceptual quality tuning
    .BuildUrl("sample.jpg");

Console.WriteLine(thumbnailUrl);
// https://res.cloudinary.com/<cloud>/image/upload/c_thumb,f_auto,g_auto,h_200,q_auto,w_200/sample.jpg
```

`f_auto` and `q_auto` together are the single highest-value optimization; apply them to
every delivery URL unless you have a reason not to.

## URLs are HTTP unless you ask for HTTPS

`Api.Secure` defaults to **`false`**:

```csharp
var insecure = new Cloudinary(url);
insecure.Api.UrlImgUp.BuildUrl("sample.jpg");
// http://res.cloudinary.com/<cloud>/image/upload/sample.jpg   <-- blocked as mixed content
```

Set `cloudinary.Api.Secure = true` once after constructing the client, or per URL with
`cloudinary.Api.UrlImgUp.Secure(true).BuildUrl(...)`. Unlike the Node and Python SDKs,
HTTPS is not the default here.

## Chained transformations (order matters)

Each component runs on the output of the previous one. `.Chain()` starts a new component:

```csharp
// A text overlay needs no second asset; to overlay an image instead use
// .Overlay(new Layer().PublicId("<public id of an image in your account>"))
var bannerUrl = cloudinary.Api.UrlImgUp
    .Transform(new Transformation()
        .Width(1280).Height(720).Crop("fill").Gravity("auto").Chain()
        .Overlay(new TextLayer().Text("SALE")
            .FontFamily("Arial").FontSize(64).FontWeight("bold"))
        .Color("white").Gravity("south_east").X(24).Y(24).Chain()
        .FetchFormat("auto").Quality("auto"))
    .BuildUrl("sample.jpg");

Console.WriteLine(bannerUrl);
// .../image/upload/c_fill,g_auto,h_720,w_1280/co_white,g_south_east,l_text:Arial_64_bold:SALE,x_24,y_24/f_auto,q_auto/sample.jpg
```

Reordering components changes the output. When matching eagerly generated versions, the
serialized transformation string must match exactly.

## The `v1` segment in URLs

When the public ID contains a slash and no version is known, this SDK inserts a `v1`
placeholder:

```csharp
cloudinary.Api.UrlImgUp.BuildUrl("sample.jpg");          // .../image/upload/sample.jpg
cloudinary.Api.UrlImgUp.BuildUrl("folder/sample.jpg");   // .../image/upload/v1/folder/sample.jpg
```

This is expected and the URL resolves correctly — it is not a bug, and you do not need to
strip it. To pin a real version instead, pass the `Version` from the upload response.

## Cache behaviour

- The same URL is served from CDN cache; a new transformation means a new URL.
- To bust stale caches after re-uploading, deliver with the asset version from the upload
  response:

```csharp
var url = cloudinary.Api.UrlImgUp
    .Version(uploadResult.Version)          // Version takes a string
    .Transform(new Transformation().Width(400).Crop("scale"))
    .BuildUrl("examples/uploaded-sample.jpg");
// .../image/upload/c_scale,w_400/v1787666423/examples/uploaded-sample.jpg
```

Passing an explicit version replaces the `v1` placeholder described above.

## Generative and AI transformations

Server-supported generative transformations (background removal, generative fill, and
similar) are expressed as transformation strings. This SDK serializes them generically —
there are no dedicated typed builders — via `.Effect(...)` or, for anything the builder
does not model, `.RawTransformation(...)`:

```csharp
new Transformation().Effect("gen_remove:prompt_car");
new Transformation().RawTransformation("e_gen_fill,ar_16:9,c_pad");
```

Availability is account- and plan-dependent; verify against the
[generative AI transformations reference](https://cloudinary.com/documentation/generative_ai_transformations.md)
before relying on one.

## HTML image tag

```csharp
Console.WriteLine(cloudinary.Api.UrlImgUp.BuildImageTag("sample.jpg"));
// <img src="https://res.cloudinary.com/<cloud>/image/upload/sample.jpg"/>
```

For responsive images and client-side rendering, use the
[frontend SDKs](https://cloudinary.com/documentation/frontend_sdks.md) rather than
generating markup on the server.

## Troubleshooting

- Images blocked on an HTTPS page — `Api.Secure` is `false`; see
  [above](#urls-are-http-unless-you-ask-for-https).
- Delivery URL returns 400 or 404 — read the `x-cld-error` response header of the failing
  URL; it names the reason. See [Troubleshoot errors](troubleshoot-errors.md).
- 401 with `x-cld-error: ACL deny` on a Claimable Cloud — delivery is IP-locked, not a
  transformation problem. See [Get Cloudinary credentials](get-credentials.md).
- A transformation parameter is ignored — the builder may not model it; express it with
  `.RawTransformation(...)` and check the spelling against the transformation reference.

## Related

- Runnable example: [`examples/TransformAndDeliverImage.cs`](https://github.com/cloudinary/CloudinaryDotNet/blob/master/examples/TransformAndDeliverImage.cs)
- [Transform and deliver a video](transform-and-deliver-video.md)
- Every transformation parameter and its accepted values:
  [Transformation reference](https://cloudinary.com/documentation/transformation_reference.md)
- Turning a plain-language requirement into a valid transformation string is what the
  `cloudinary-transformations` Skill is for — see
  [platform capabilities](platform-capabilities.md#skills--get-the-patterns-right).
- [Image manipulation guide](https://cloudinary.com/documentation/dotnet_image_manipulation.md)

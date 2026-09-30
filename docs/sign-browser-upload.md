# Sign a browser upload

## When to use

A browser or mobile app uploads directly to Cloudinary, but you want the operation
authorized by your server. The API secret stays on the server; the client receives a
signature that is valid for **1 hour** from the `timestamp` it was signed with.

This is the right pattern whenever the file should not transit your own server. For
uploads without any server round-trip, use an
[unsigned upload preset](https://cloudinary.com/documentation/upload_presets.md) instead —
deliberately restricted, because anyone can use it.

## Server: the signing endpoint

`SignParameters` takes the parameters the client is allowed to send and returns the
signature. It excludes `file`, `api_key`, and `resource_type` automatically, because
Cloudinary does not include them in the signed string.

```csharp
using CloudinaryDotNet;

// ASP.NET Core minimal API; `cloudinary` is the injected singleton
app.MapGet("/api/sign-upload", (Cloudinary cloudinary) =>
{
    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

    // Sign ONLY what the client is permitted to use.
    var toSign = new SortedDictionary<string, object>
    {
        { "folder", "user-uploads" },
        { "timestamp", timestamp },
    };

    var signature = cloudinary.Api.SignParameters(toSign);

    return Results.Ok(new
    {
        signature,
        timestamp,
        folder = "user-uploads",
        apiKey = cloudinary.Api.Account.ApiKey,
        cloudName = cloudinary.Api.Account.Cloud,
    });
});
```

Return the API **key** (public) and never the secret. Use a `SortedDictionary` so the
parameter order is deterministic.

The default signature version is **2** and the algorithm is **SHA-1**
(`Api.SignatureVersion`, `Api.SignatureAlgorithm`). Version 2 percent-encodes `&` inside
values to prevent parameter smuggling. Leave both alone unless you are matching an
existing implementation.

## Client: use the signature

The client POSTs multipart form data to the upload endpoint. Every field it sends must be
one the server signed:

```javascript
const { signature, timestamp, folder, apiKey, cloudName } =
  await (await fetch('/api/sign-upload')).json();

const form = new FormData();
form.append('file', fileInput.files[0]);
form.append('api_key', apiKey);
form.append('timestamp', timestamp);
form.append('signature', signature);
form.append('folder', folder);          // exactly what the server signed

// 'auto' lets Cloudinary detect image / video / raw from the file itself
const response = await fetch(
  `https://api.cloudinary.com/v1_1/${cloudName}/auto/upload`,
  { method: 'POST', body: form }
);
const asset = await response.json();    // public_id, secure_url, asset_id, ...
```

`auto` in the URL path means "detect the resource type from the file". Use it unless you
want to restrict what may be uploaded, in which case use `image`, `video`, or `raw`
explicitly.

## Verifying it from .NET

If you need to POST an upload from .NET (to test the endpoint, or because the upload runs
server-side), `MultipartFormDataContent` needs two adjustments. Both fail the same
confusing way if missed — `Upload preset must be specified when using unsigned upload`,
which has nothing to do with your signature:

```csharp
// 1. HttpClient adds a Content-Type to StringContent; Cloudinary wants bare fields.
static StringContent Field(string value)
{
    var content = new StringContent(value);
    content.Headers.ContentType = null;
    return content;
}

// 2. .NET emits `name=api_key`, but Cloudinary's parser only reads `name="api_key"`.
//    Pass the field name already quoted.
static string Quoted(string name) => $"\"{name}\"";

using var form = new MultipartFormDataContent();
form.Add(fileContent, Quoted("file"), Quoted("upload.jpg"));
form.Add(Field(apiKey), Quoted("api_key"));
form.Add(Field(timestamp), Quoted("timestamp"));
form.Add(Field(signature), Quoted("signature"));
form.Add(Field("user-uploads"), Quoted("folder"));
```

Without the quoting, Cloudinary does not see `api_key` at all and treats the request as an
unsigned upload. Verified by comparing both forms against the live API: unquoted returns
HTTP 400, quoted returns HTTP 200.

**None of this applies to the browser** — `FormData` already emits quoted names, so the
JavaScript above needs no special handling.

## Rules

- **Every parameter the client sends must be in the signed set**, except `file`,
  `api_key`, `signature`, and `resource_type`. To let the client choose a tag,
  `public_id`, or transformation, add it to the signed parameters on the server first —
  which is exactly the point of control: what you do not sign, the client cannot send.
- Signatures embed the timestamp and are accepted for **1 hour** after it. Generate one
  per upload rather than caching and reusing them.
- Keep the API secret in server code only. The client gets the signature, timestamp, API
  key, and cloud name.

## Troubleshooting

- `Invalid Signature <hash>. String to sign - '<params>'` (HTTP 401) — the client sent a
  parameter that was not signed, or a different value than the one signed. The error
  **echoes the exact string the server signed**, so compare it against your signed set —
  an extra `tags=sneaky` shows up there immediately.
- `Upload preset must be specified when using unsigned upload` (HTTP 400) — the request
  arrived without a usable `api_key`, so Cloudinary treated it as unsigned. From .NET this
  is almost always one of the two `MultipartFormDataContent` issues
  [above](#verifying-it-from-net) — most often the unquoted field name — not a problem
  with your signature.
- `Stale request` — the signature is more than 1 hour old. Fetch a fresh one at upload
  time rather than at page load, and check that your server clock is accurate; a skewed
  clock produces timestamps that are stale on arrival.

## Related

- Runnable example: [`examples/SignBrowserUpload.cs`](https://github.com/cloudinary/CloudinaryDotNet/blob/master/examples/SignBrowserUpload.cs)
- [Use with ASP.NET Core](use-with-aspnet-core.md) — where the endpoint above fits.
- [Generating authentication signatures](https://cloudinary.com/documentation/upload_images.md#generating_authentication_signatures)
- [Upload presets](https://cloudinary.com/documentation/upload_presets.md)

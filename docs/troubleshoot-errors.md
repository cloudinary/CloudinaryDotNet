# Troubleshoot errors

## How this SDK reports errors — read this first

**Cloudinary API errors do not throw.** Every Upload, Admin, and Search method returns a
result object; failures arrive in it:

```csharp
var result = await cloudinary.GetResourceAsync("does/not/exist");

// No exception was raised. The call "succeeded" as far as C# is concerned.
Console.WriteLine(result.StatusCode);        // NotFound
Console.WriteLine(result.Error.Message);     // Resource not found - does/not/exist
```

Wrapping calls in `try`/`catch` and expecting Cloudinary failures to land there is the
single most common mistake with this SDK. The `catch` never fires; execution continues with
a result whose fields are all `null`, and the failure surfaces later as a
`NullReferenceException` somewhere unrelated.

**Always check `result.Error != null`.**

```csharp
if (result.Error != null)
{
    Console.Error.WriteLine($"{(int)result.StatusCode}: {result.Error.Message}");
    return;
}
```

### What the result carries

Every result derives from `BaseResult`:

| Member | Type | Notes |
|---|---|---|
| `StatusCode` | `HttpStatusCode` | Branch on this, not on message text |
| `Error` | `Error` | `null` on success |
| `Error.Message` | `string` | **The only field on `Error`** — there is no error code |
| `JsonObj` | `JToken` | The raw response, for fields without a typed property |
| `Limit`, `Remaining`, `Reset` | `long`/`DateTime` | Admin API rate-limit state |

`Error` has no numeric code and no inner detail, so `StatusCode` is the only thing to
switch on programmatically. Log `Error.Message` for humans, and do not attempt to parse it.

### There are no custom exception types

This assembly exports **zero** exception classes. There is no `CloudinaryException` to
catch — if you find such a type in a code sample, it is from a different SDK or invented.

### What *does* throw

Only client-side, pre-flight problems:

| Condition | Exception |
|---|---|
| `CLOUDINARY_URL` missing, empty, or malformed | `ArgumentException` |
| `Account` with no cloud name | `ArgumentException` |
| Upload params or `File` null/unset | `ArgumentNullException` / `ArgumentException` |
| `UploadLarge` chunk size below 5 MB | `System.Exception` |
| Response body is not valid JSON | `System.Exception` |
| Non-seekable stream in a **chunked** upload without a size | `NotSupportedException` |

So the honest pattern is both: `try`/`catch` around construction and argument
validation, and an `Error` check on every call.

```csharp
Cloudinary cloudinary;
try
{
    cloudinary = new Cloudinary { Api = { Secure = true } };   // reads CLOUDINARY_URL
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Cloudinary is not configured: {ex.Message}");
    return 1;
}

var result = await cloudinary.UploadAsync(uploadParams);
if (result.Error != null)
{
    Console.Error.WriteLine($"Upload failed ({(int)result.StatusCode}): {result.Error.Message}");
    return 1;
}
```

## Errors by symptom

### `ArgumentException: Invalid CLOUDINARY_URL scheme. Expecting to start with 'cloudinary://'`
Configuration never loaded. The message names the scheme even when the variable is
**entirely absent**, so check for absence first. It is also what you get from passing a
Claimable Cloud's `api_environment_variable` verbatim — that string begins with
`CLOUDINARY_URL=`. See [Configure Cloudinary](configure-cloudinary.md).

### `ArgumentException: Cloud name must be specified in Account!`
An `Account` was built with no cloud name, or `CloudinaryConfiguration.CloudName` was never
set. Note that `CloudinaryConfiguration`'s statics are read by `new Account()`, **not** by
`new Cloudinary()`.

### `api_secret mismatch` / `Invalid credentials` (401)
The key and secret do not belong to this cloud name. Re-copy all three from Console >
Settings > API Keys. Arrives in `Error`, not as an exception.

### `Missing required parameter - api_key` (400)
The client has a cloud name but no credentials — typical of an instance built for URL
generation only, or a configuration section that bound partially.

### `Invalid Signature <hash>. String to sign - '<params>'` (401)
A signed request included parameters that were not part of the signature, or values changed
after signing. The message **echoes the exact string the server signed** — compare it with
what you signed. See [Sign a browser upload](sign-browser-upload.md).

### `Rate limit exceeded` (420) on Admin/Search
You are calling management APIs in a request path. Batch the work and cache results.
Successful Admin responses carry `Remaining` and `Limit`, so you can slow down before being
cut off. Delivery URLs are never rate-limited this way.

### `You don't have an active subscription for <add-on>` — also HTTP 420
**An unsubscribed add-on reports as a rate-limit status code**, not 401 or 403. If you see
420, read the message before assuming you are being throttled: a missing add-on
registration and genuine throttling are indistinguishable by status alone. Register the
add-on in the console (and accept the provider's terms where required).

### `File size too large`
Two different limits produce this. The per-request ceiling is 100 MB — switch to
[chunked upload](upload-large-video.md). Your product environment's maximum asset size is
separate and **chunking does not raise it**; read the real values with
`GetUsageAsync()` and compare before deciding:

```csharp
var usage = await cloudinary.GetUsageAsync();
Console.WriteLine(usage.MediaLimits["image_max_size_bytes"]);
Console.WriteLine(usage.MediaLimits["video_max_size_bytes"]);
```

If the asset exceeds the environment maximum, compress or resize it, or upgrade the plan.

### `All parts except EOF-chunk must be larger than 5mb`
`UploadLarge` chunk size is below the 5 MB minimum. **This one throws** rather than
returning in `Error`.

### HTTP 423 (`Processing`)
The asset is still being processed and is not yet available for the operation you
requested — common right after uploading a large video, or while an eager or add-on-driven
transformation is still running. This is transient: retry with backoff rather than treating
it as a failure. For long jobs prefer eager async transformations with a `NotificationUrl`
webhook over polling.

### Delivery URL returns 400 or 404
The reason is in the `x-cld-error` response header of the failing URL:

```bash
curl -sI "https://res.cloudinary.com/<cloud>/image/upload/w_abc/sample.jpg" | grep -i x-cld-error
# x-cld-error: Invalid width in transformation: abc
```

Values observed live:

| HTTP | `x-cld-error` | Cause |
|---|---|---|
| 400 | `Invalid width in transformation: abc` | Malformed transformation value |
| 400 | `Unknown transformation does_not_exist` | Named transformation missing on this environment |
| 404 | `Resource not found - <public_id>` | Wrong public ID, folder, or resource type in the path |

The header is CORS-exposed, so browser code can read it from a failed image fetch too.

### Delivery URL returns 401 with `x-cld-error: ACL deny`
On an unclaimed Claimable Cloud, delivery is locked to the IP you provisioned from. This is
**not** a moderation, permission, or transformation problem — and it appears identically for
moderated and non-moderated assets. If your public IP changed since provisioning, previously
working URLs start failing this way. See
[Get Cloudinary credentials](get-credentials.md).

### Upload succeeded but the video will not transform or stream
It was stored as `raw`, because `UploadLarge` was called with `RawUploadParams`. There is no
error for this. Re-upload with `VideoUploadParams` and assert
`result.JsonObj["resource_type"]`. See [Upload a large video](upload-large-video.md).

### Uploaded a new file but the old one is still served
`Overwrite` was not `true`, so the upload was a **no-op** that returned the existing asset.
Look for `existing: true` in `JsonObj`. See [Upload an image](upload-image.md#overwrite-behaviour).

### `Metadata External IDs do not exist: [...]` (400)
A structured-metadata key has no field definition — and the **entire upload failed**, not
just the metadata. Define the field first. See
[Use structured metadata](use-structured-metadata.md).

### `Query Error (at position 1)`
A search expression starting with `*` or a bare `*`. Leading wildcards are rejected; trailing
ones are fine. See [Search and manage assets](search-and-manage-assets.md#wildcards).

### Search returns 0 for something you know exists
Two common causes: a `folder:` expression on a dynamic-folder environment (matches nothing,
silently), or search-index lag right after a write. See
[Search and manage assets](search-and-manage-assets.md).

### Images blocked or not loading on an HTTPS page
`Api.Secure` defaults to `false`, so URLs are `http://` and browsers block them as mixed
content. Set `cloudinary.Api.Secure = true`.

### Timeouts
A timeout surfaces as a **thrown** `TaskCanceledException`, not as an `Error` on the result —
so it bypasses your `Error` check and crashes the process if unhandled. Catch it by type
rather than by message: the message varies (the default 100-second `HttpClient.Timeout`
reports "The request was canceled due to the configured HttpClient.Timeout of 100 seconds
elapsing", while an explicit `Api.Timeout` reports just "A task was canceled").

```csharp
cloudinary.Api.Timeout = 120000;   // milliseconds

try
{
    var result = await cloudinary.UploadAsync(uploadParams);
    if (result.Error != null) { /* handle API error */ }
}
catch (TaskCanceledException)
{
    // Transient - retry with backoff rather than treating it as a failure.
}
catch (HttpRequestException ex)
{
    // DNS failure, connection reset, TLS problem.
}
```

Any long-running upload needs both handlers plus the `Error` check. For uploads on unstable
links use chunked upload; chunks retry independently.

### Stale delivery after re-upload
CDN-cached URLs do not update instantly. Deliver with the new `Version` from the upload
response, which changes the URL immediately:
`cloudinary.Api.UrlImgUp.Version(result.Version).BuildUrl(...)`.

## Verifying webhooks

If you use `NotificationUrl`, verify the callback actually came from Cloudinary before
acting on it:

```csharp
var valid = cloudinary.Api.VerifyNotificationSignature(rawRequestBody, timestamp, signature);
```

`validFor` defaults to 7200 seconds. There is also
`VerifyApiResponseSignature(publicId, version, signature)`.

## Still stuck

- Platform status: https://status.cloudinary.com — check this first. A widespread incident
  explains failures that look like a bug in your code.
- SDK bugs: https://github.com/cloudinary/CloudinaryDotNet/issues
- Account issues: https://support.cloudinary.com
- Diagnosing across products:
  [troubleshooting index](https://cloudinary.com/documentation/llms-troubleshooting.txt)

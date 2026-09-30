# Moderate an upload

## When to use

Content uploaded by users must be reviewed before it is shown. Moderation in Cloudinary is
stateful: an asset carries a moderation status, and **your application** is responsible for
showing approved assets only.

## Read this before you build a moderation flow

**A `pending` moderated asset is delivered normally.** Its URL returns HTTP 200 from the
moment it is uploaded — verified against a live environment, with a non-moderated control
asset behaving identically.

The moderation status is **metadata you gate on in your own code**. It is not a delivery
gate. If you upload user content with `Moderation = "manual"` and then serve the URL, you
are serving unreviewed content.

Blocking delivery of non-approved assets *can* be configured for a product environment,
but it is not an upload parameter and there is no API for it — it requires Cloudinary
support. Gate on the status in your own data model regardless.

## Statuses

The full set is larger than the obvious three:

| Status | Meaning |
|---|---|
| `Queued` | Waiting for an add-on to process it |
| `Pending` | Awaiting a decision (the initial state for `manual`) |
| `Approved` | Passed review |
| `Rejected` | Failed review |
| `Overridden` | A human replaced an automatic verdict |
| `Aborted` | An earlier moderation in a chain rejected the asset |

Read them from the `Moderation` list. **The upload result's `ModerationStatus` property is
empty** — the status only appears in the list on upload:

```csharp
var result = await cloudinary.UploadAsync(uploadParams);

Console.WriteLine(result.ModerationStatus);         // "" on upload — do not use this
Console.WriteLine(result.Moderation[0].Kind);       // manual
Console.WriteLine(result.Moderation[0].Status);     // Pending
```

`GetResourceAsync` is the opposite: there, `ModerationStatus` **is** populated, and the
list carries an `updated_at` as well. Use the list on upload, either afterwards.

## Complete flow (manual review queue)

```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

var cloudinary = new Cloudinary();      // reads CLOUDINARY_URL
cloudinary.Api.Secure = true;

// 1. Upload into the moderation queue — the asset starts as Pending
var uploaded = await cloudinary.UploadAsync(new ImageUploadParams
{
    File = new FileDescription("https://res.cloudinary.com/demo/image/upload/sample.jpg"),
    PublicId = "examples/moderated-upload",
    Overwrite = true,
    Moderation = "manual",
});

if (uploaded.Error != null)
{
    Console.Error.WriteLine($"Upload failed ({(int)uploaded.StatusCode}): {uploaded.Error.Message}");
    return;
}

Console.WriteLine(uploaded.Moderation[0].Status);   // Pending
// NOTE: uploaded.SecureUrl already serves this image. Do not publish it yet.

// 2. Your review UI lists the queue
var queue = await cloudinary.ListResourcesByModerationStatusAsync("manual", ModerationStatus.Pending);
Console.WriteLine($"Assets pending review: {queue.Resources?.Length ?? 0}");

// 3. A reviewer records the decision
var decision = await cloudinary.UpdateResourceAsync(
    new UpdateParams(uploaded.PublicId) { ModerationStatus = ModerationStatus.Approved });

if (decision.Error != null)
{
    Console.Error.WriteLine($"Update failed: {decision.Error.Message}");
    return;
}

// 4. Only now mark it publishable in YOUR data model, and serve it from there.
```

## Automatic moderation

Pass an add-on name instead of `manual` to get an automated verdict.

**Prerequisite — a human has to do this, not your code.** Every value below except `manual`
requires its add-on to be registered on the account first, from the
[Add-ons page](https://cloudinary.com/documentation/cloudinary_add_ons.md) in the console.
Some third-party add-ons also require reviewing and accepting the provider's terms of
service as part of registration. Neither step has an API; until both are done the upload
fails. `manual` needs no add-on, which is why the flow above uses it.

| Value | Moderates | Add-on |
|---|---|---|
| `manual` | any asset | none — built in |
| `aws_rek` | images | Amazon Rekognition AI Moderation |
| `aws_rek_video` | video | Amazon Rekognition Video Moderation |
| `google_video_moderation` | video | Google AI Video Moderation |
| `webpurify` | images | WebPurify Image Moderation |
| `perception_point` | any asset | Perception Point Malware Detection |
| `duplicate:<threshold>` | images | Cloudinary Duplicate Image Detection |

Combine several with a pipe — they run in the order given, and `manual` must be last
(`"aws_rek|duplicate:0.9|manual"`). The first starts as `Pending` and the rest as
`Queued`; if one rejects, the remaining become `Aborted` and the asset's final status is
`Rejected`. Always set a `NotificationUrl` when requesting several, since you will not get
the verdicts in the upload response.

You can override a machine decision with `UpdateResourceAsync` + `ModerationStatus`, which
is what `Overridden` records.

## Design rules

- **Gate in your own code.** The URL works regardless of status; there is no platform gate
  by default. Store the status in your data model and check it before rendering.
- Model moderation as a state machine, not a boolean, and keep the pending state visible in
  your product (placeholder image, "under review" label).
- Keep a human override even with automatic moderation — machine verdicts are drafts for
  anything with legal or brand consequences.
- Rejected assets stay in storage until you delete them; decide your retention policy.
- To show something in place of a rejected image, deliver a `default_image` placeholder
  rather than relying on the URL failing, because it will not.

## Troubleshooting

- **A pending asset is publicly viewable** — expected. Nothing blocks delivery by default;
  enforcement is your application's responsibility. Contact Cloudinary support to have
  blocking configured for the product environment.
- `You don't have an active subscription for <add-on>` — arrives with **HTTP 420**, a
  rate-limit status code, not a 401 or 403. Do not mistake it for a throttling problem:
  register the add-on in the console (and accept the provider's terms where required).
- `Moderation <value> moderation is not valid` (HTTP 400) — the moderation value is
  misspelled; use one from the table above.
- `ModerationStatus` is empty after upload — expected; read `Moderation[0].Status`
  instead. See [Statuses](#statuses).
- The queue is empty right after uploading — `ListResourcesByModerationStatusAsync` reads
  an index that lags writes by a few seconds. Retry, or read the asset directly.

## Related

- Runnable example: [`examples/ModerateUpload.cs`](https://github.com/cloudinary/CloudinaryDotNet/blob/master/examples/ModerateUpload.cs)
- [Moderate assets](https://cloudinary.com/documentation/moderate_assets.md) — statuses,
  delivery behaviour, and the available moderation add-ons.
- [Moderation add-ons overview](https://cloudinary.com/documentation/cloudinary_moderation.md)

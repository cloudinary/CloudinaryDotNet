# Use structured metadata

## When to use

Attach typed, validated business fields (SKU, campaign, rights expiry) to assets, so
applications can filter and route on a stable schema instead of free-form tags.

Tags are unvalidated strings; structured metadata fields are declared once per product
environment, typed, and optionally mandatory. Use metadata when a wrong value should be
rejected rather than silently stored.

## Define a field (once, per product environment)

```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

var cloudinary = new Cloudinary();      // reads CLOUDINARY_URL

var field = await cloudinary.AddMetadataFieldAsync(
    new StringMetadataFieldCreateParams("SKU")   // the human-readable label
    {
        ExternalId = "sku",                      // the key you use in code
        Mandatory = false,
    });

if (field.Error != null)
{
    Console.Error.WriteLine($"Could not create field ({(int)field.StatusCode}): {field.Error.Message}");
    return;
}
Console.WriteLine(field.ExternalId);   // sku
```

The params class picks the type. Available: `StringMetadataFieldCreateParams`,
`IntMetadataFieldCreateParams`, `DateMetadataFieldCreateParams`,
`EnumMetadataFieldCreateParams`, and `SetMetadataFieldCreateParams`. The last two take a
`DataSource` of allowed values.

Field definitions are **permanent and per-environment**. Creating one that exists returns
`external id sku already exists` (HTTP 400) — treat that as success if you are running
setup idempotently.

## Write values on an asset

At upload time:

```csharp
var upload = await cloudinary.UploadAsync(new ImageUploadParams
{
    File = new FileDescription("https://res.cloudinary.com/demo/image/upload/sample.jpg"),
    PublicId = "examples/product-photo",
    Overwrite = true,
    MetadataFields = new StringDictionary("sku=SKU-00042"),
});
```

Or later, on existing assets:

```csharp
var written = await cloudinary.UpdateMetadataAsync(new MetadataUpdateParams
{
    PublicIds = new List<string> { "examples/product-photo" },
    Metadata = new StringDictionary("sku=SKU-00042"),
});
```

Both take a `StringDictionary` with `"key=value"` entries, keyed by **external ID**.

## Undefined keys fail the whole upload

Writing a key that has no field definition does **not** get dropped silently — it fails the
entire request:

```csharp
var bad = await cloudinary.UploadAsync(new ImageUploadParams
{
    File = new FileDescription(path),
    MetadataFields = new StringDictionary("no_such_field=value"),
});
// StatusCode = BadRequest
// Error.Message = Metadata External IDs do not exist: ["no_such_field"]
```

The image is not uploaded at all. Define fields before writing to them, and treat a
metadata typo as a failed upload rather than a partial success.

## Query by metadata

```csharp
var found = await cloudinary.Search()
    .Expression("metadata.sku=\"SKU-00042\"")
    .ExecuteAsync();

Console.WriteLine(found.TotalCount);
```

Quote the value. Note the **search index lags writes by a few seconds** — a value written
and queried immediately returns 0 results, then 1 a moment later. To confirm a write
landed, read the asset instead:

```csharp
var asset = await cloudinary.GetResourceAsync("examples/product-photo");
Console.WriteLine(asset.JsonObj["metadata"]);   // {"sku": "SKU-00042"}
```

There is no typed `Metadata` property on the result — read it from `JsonObj`.

## Pattern: analysis to reviewed metadata

A common pattern for turning model output into data you can rely on:

1. Run AI analysis on the asset (captioning, tagging — for example the
   [Analyze API](https://cloudinary.com/documentation/analyze_api_guide.md), subscription
   required).
2. Normalize the output against your schema — map free-form values onto your allowed list,
   drop low-confidence results, apply business rules.
3. Write the resulting values as structured metadata.
4. Search, route, and deliver based on that metadata.

Step 2 is where the value is: metadata fields are typed and validated, so whatever you
write has to conform. Automate it where the rules are clear and route to a person only for
the cases your rules cannot decide — enum and set fields make the boundary explicit,
because an out-of-datasource value is rejected rather than stored.

## Troubleshooting

- `external id <name> already exists` — field definitions are permanent and
  per-environment; reuse the existing field rather than re-creating it.
- `Metadata External IDs do not exist: [...]` — the field is not defined on this
  environment, and the **whole upload failed**. Create the field first.
- Enum/set writes rejected — the value is not in the datasource. Add it with
  `UpdateMetadataDataSourceEntriesAsync` before writing.
- A search by metadata returns 0 immediately after a write — index lag; read the asset
  with `GetResourceAsync` instead.

## Related

- [Search and manage assets](search-and-manage-assets.md)
- [Structured metadata guide](https://cloudinary.com/documentation/structured_metadata.md)

# Search and manage assets

## When to use

Find assets by indexed fields, read or update asset attributes, and administer your media
library from the server. These use the Admin and Search APIs, which are **rate-limited** —
treat them as management operations, not a per-request database.

## Search with the fluent builder

Expressions use Cloudinary's search syntax — fields, operators, ranges, and boolean
combinations are listed in the
[search expression reference](https://cloudinary.com/documentation/search_expressions.md).

```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

var cloudinary = new Cloudinary();      // reads CLOUDINARY_URL
cloudinary.Api.Secure = true;

var result = await cloudinary.Search()
    .Expression("resource_type:image")
    .SortBy("created_at", "desc")
    .MaxResults(30)
    .ExecuteAsync();

if (result.Error != null)
{
    Console.Error.WriteLine($"Search failed ({(int)result.StatusCode}): {result.Error.Message}");
    return;
}

Console.WriteLine($"{result.TotalCount} match(es)");
foreach (var asset in result.Resources)
{
    Console.WriteLine($"{asset.AssetId}  {asset.PublicId}  {asset.Bytes}  {asset.CreatedAt}");
}
```

### Pagination

Pass the cursor back until it is `null`:

```csharp
var cursor = result.NextCursor;
while (cursor != null)
{
    var page = await cloudinary.Search()
        .Expression("resource_type:image")
        .MaxResults(30)
        .NextCursor(cursor)
        .ExecuteAsync();

    if (page.Error != null) break;
    // ... process page.Resources
    cursor = page.NextCursor;
}
```

Keep the expression identical across pages; changing it invalidates the cursor.

## `folder:` probably does not do what you want

On a product environment using dynamic folders — the default for newly created
environments — a `folder:` expression matches **nothing**, and does so *without erroring*:

```csharp
await cloudinary.Search().Expression("folder:examples").ExecuteAsync();
// TotalCount = 0   (on an environment that holds 60+ assets under examples/)

await cloudinary.Search().Expression("public_id:examples/*").ExecuteAsync();
// TotalCount = 4   <-- what you actually wanted
```

A valid query returning zero results is the trap: there is no error to notice. If a
folder-scoped search comes back empty, match on the public ID prefix instead, or use
`asset_folder:` if your environment is configured for it. Verify against real data before
concluding a folder is empty.

Unknown field names behave the same way — they match nothing rather than failing.

## Wildcards

Leading wildcards and a bare `*` are **rejected**, not ignored:

```csharp
await cloudinary.Search().Expression("*").ExecuteAsync();
// StatusCode = BadRequest, Error.Message = Query Error (at position 1) ' ➥➥➥*'
```

Trailing wildcards (`examples/*`) are fine. To list everything, use a real field
expression such as `resource_type:image`.

## Read and update a single asset

Prefer the immutable asset ID for lookups — it survives renames and moves, while the
public ID does not:

```csharp
var details = await cloudinary.GetResourceByAssetIdAsync(storedAssetId);
if (details.Error != null) { /* handle */ }

// Updates require the PUBLIC id, so read it off the response:
var update = await cloudinary.UpdateResourceAsync(new UpdateParams(details.PublicId)
{
    Tags = "featured",
    Context = new StringDictionary("alt=Sample image from the bundled upload example"),
});
```

**`Context` is a `StringDictionary`, not a string** — unlike the Node and Python SDKs.
`new StringDictionary("key=value")` is the idiom.

Asset-ID coverage is partial in this SDK. These accept an asset ID:

- `GetResourceByAssetIdAsync`
- `ListResourceByAssetIdsAsync`
- `AddRelatedResourcesByAssetIdsAsync` / `DeleteRelatedResourcesByAssetIdsAsync`

`UpdateResourceAsync`, `RenameAsync`, `DestroyAsync`, and all URL builders require the
**public ID**. Store the asset ID, look up, read `PublicId`, then act — do not assume
parity with the other SDKs.

## Read-after-write: the search index lags

A write is immediately visible via `GetResourceAsync`, but the **search index takes a few
seconds** to catch up. Measured on a live environment, a metadata value written and
queried immediately returned 0 results, then 1 result five seconds later.

For read-after-write flows use `GetResourceAsync` / `GetResourceByAssetIdAsync`, not
`Search()`. Never assert on search results immediately after a write in a test.

## Rate limits

Admin API responses carry the limit state, so you can slow down before being cut off:

```csharp
var listing = await cloudinary.ListResourcesAsync();
Console.WriteLine($"{listing.Remaining}/{listing.Limit} left, resets {listing.Reset}");
// e.g. 497/500 left, resets 25/08/2026 18:00:00
```

Delivery URLs are never rate-limited this way — only Admin and Search calls are.

## Deletion — destructive, no undo without backups

```csharp
await cloudinary.DestroyAsync(new DeletionParams("examples/uploaded-sample"));   // one asset
// result.Result == "ok" on success, "not found" if it did not exist

// Bulk — double-check inputs:
await cloudinary.DeleteResourcesAsync(ResourceType.Image, "id1", "id2");

// By prefix — extremely destructive, no confirmation:
await cloudinary.DeleteResourcesByPrefixAsync("examples/");
```

Prefer explicit ID lists over prefix deletion. Enable backups on the product environment
if you need `RestoreAsync` to work — without backups enabled, deletion is permanent.

## Handling errors

Failed calls **return**; they do not throw:

```csharp
var missing = await cloudinary.GetResourceAsync("examples/does-not-exist");
if (missing.Error != null)
{
    Console.Error.WriteLine($"{(int)missing.StatusCode}: {missing.Error.Message}");
    // 404: Resource not found - examples/does-not-exist
}
```

`Error` carries a single field, `Message`. There is no error-code property and no
exception type to catch — use `StatusCode` when you need to branch on the class of
failure.

## Troubleshooting

- Search returns 0 for a folder you know has assets — see
  [`folder:` probably does not do what you want](#folder-probably-does-not-do-what-you-want).
- `Query Error (at position 1)` — a bare `*` or a leading wildcard. See
  [Wildcards](#wildcards).
- Results missing an asset you just wrote — search-index lag; use `GetResourceAsync`.
- `Rate limit exceeded` — too many Admin calls. Batch the work, cache results, and watch
  `Remaining`.
- An unsubscribed add-on reports as a **rate-limit** error rather than a permission error;
  see [Troubleshoot errors](troubleshoot-errors.md).

## Related

- [Use structured metadata](use-structured-metadata.md)
- [Search expression syntax](https://cloudinary.com/documentation/search_expressions.md)
- [Asset administration guide](https://cloudinary.com/documentation/dotnet_asset_administration.md)

// Search for assets, then read and update one.
//
// Prerequisites: CLOUDINARY_URL in the environment. Run UploadImage first.
// Run:           dotnet run --project Examples.csproj SearchAndManageAssets
//
// This example deliberately does not delete anything. In a real project, prefer explicit
// ID lists over DeleteResourcesByPrefixAsync, and enable backups before relying on restore.
//
// Doc page: ../docs/search-and-manage-assets.md
namespace CloudinaryDotNet.Examples;

using CloudinaryDotNet.Actions;

internal static class SearchAndManageAssets
{
    public static async Task<int> RunAsync()
    {
        var cloudinary = ExampleContext.TryCreateClient();
        if (cloudinary == null)
        {
            return 1;
        }

        // Note: a "folder:examples" expression matches nothing on a dynamic-folder
        // environment. Matching on the public ID prefix works everywhere.
        const string expression = "public_id:examples/*";

        var search = await cloudinary.Search()
            .Expression(expression)
            .SortBy("created_at", "desc")
            .MaxResults(10)
            .ExecuteAsync();

        if (ExampleContext.Failed(search, "Search"))
        {
            return 1;
        }

        Console.WriteLine($"Matched {search.TotalCount} asset(s) for '{expression}':");
        foreach (var asset in search.Resources)
        {
            Console.WriteLine($"  {asset.PublicId}  ({asset.Bytes} bytes, {asset.CreatedAt})");
        }

        // Admin responses carry rate-limit state; watch it instead of waiting to be cut off.
        Console.WriteLine($"Admin API quota: {search.Remaining}/{search.Limit} remaining, resets {search.Reset}");

        // Look the asset up by its immutable ID, then use the public ID for the update -
        // UpdateResourceAsync does not accept an asset ID.
        var uploaded = await cloudinary.GetResourceAsync(ExampleContext.ImagePublicId);
        if (ExampleContext.Failed(uploaded, "Reading the asset"))
        {
            return 1;
        }

        Console.WriteLine($"Asset ID for {uploaded.PublicId}: {uploaded.AssetId}");

        var byAssetId = await cloudinary.GetResourceByAssetIdAsync(uploaded.AssetId);
        if (ExampleContext.Failed(byAssetId, "Lookup by asset ID"))
        {
            return 1;
        }

        var updated = await cloudinary.UpdateResourceAsync(new UpdateParams(byAssetId.PublicId)
        {
            Tags = "featured",
            Context = new StringDictionary("alt=Sample image from the bundled upload example"),
        });

        if (ExampleContext.Failed(updated, "Update"))
        {
            return 1;
        }

        Console.WriteLine($"Updated tags: {string.Join(", ", updated.Tags ?? Array.Empty<string>())}");

        return 0;
    }
}

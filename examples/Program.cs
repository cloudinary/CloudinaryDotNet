// Entry point for the bundled examples. Pick one by name:
//
//   dotnet run --project Examples.csproj UploadImage
//
// Each example lives in its own file, takes no arguments, and returns an exit code:
// 0 on success, 1 on failure. See README.md for the recommended order.
namespace CloudinaryDotNet.Examples;

internal static class Program
{
    private static readonly Dictionary<string, Func<Task<int>>> Examples = new(StringComparer.OrdinalIgnoreCase)
    {
        ["UploadImage"] = UploadImage.RunAsync,
        ["UploadLargeVideo"] = UploadLargeVideo.RunAsync,
        ["TransformAndDeliverImage"] = TransformAndDeliverImage.RunAsync,
        ["TransformAndDeliverVideo"] = TransformAndDeliverVideo.RunAsync,
        ["SignBrowserUpload"] = SignBrowserUpload.RunAsync,
        ["SearchAndManageAssets"] = SearchAndManageAssets.RunAsync,
        ["ModerateUpload"] = ModerateUpload.RunAsync,
        ["UseStructuredMetadata"] = UseStructuredMetadata.RunAsync,
    };

    private static async Task<int> Main(string[] args)
    {
        if (args.Length != 1 || !Examples.TryGetValue(args[0], out var example))
        {
            Console.Error.WriteLine("Usage: dotnet run --project Examples.csproj <ExampleName>");
            Console.Error.WriteLine();
            Console.Error.WriteLine("Available examples:");
            foreach (var name in Examples.Keys.OrderBy(n => n, StringComparer.Ordinal))
            {
                Console.Error.WriteLine($"  {name}");
            }

            return 1;
        }

        try
        {
            return await example();
        }
        catch (TaskCanceledException)
        {
            // HttpClient's 100-second default timeout, or a dropped connection. Transient:
            // worth retrying rather than a bug in the example.
            Console.Error.WriteLine(
                "The request timed out. Check your connection and try again; " +
                "raise cloudinary.Api.Timeout (milliseconds) for slow links.");
            return 1;
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Network error talking to Cloudinary: {ex.Message}");
            return 1;
        }
    }
}

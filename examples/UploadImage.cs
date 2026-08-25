// Upload an image to Cloudinary and print the fields worth keeping.
//
// Prerequisites: CLOUDINARY_URL in the environment.
// Run:           dotnet run --project Examples.csproj UploadImage
//
// In a real project you would upload a user's file (a path, or an IFormFile stream) rather
// than a remote demo URL, and you would store AssetId in your own database.
//
// Doc page: ../docs/upload-image.md
namespace CloudinaryDotNet.Examples;

using CloudinaryDotNet.Actions;

internal static class UploadImage
{
    public static async Task<int> RunAsync()
    {
        var cloudinary = ExampleContext.TryCreateClient();
        if (cloudinary == null)
        {
            return 1;
        }

        var result = await cloudinary.UploadAsync(new ImageUploadParams
        {
            File = new FileDescription(ExampleContext.SampleImageUrl),
            PublicId = ExampleContext.ImagePublicId,
            Overwrite = true,
        });

        // API errors are returned, not thrown - this check is not optional.
        if (ExampleContext.Failed(result, "Upload"))
        {
            return 1;
        }

        Console.WriteLine($"Public ID : {result.PublicId}");
        Console.WriteLine($"Asset ID  : {result.AssetId}");   // immutable; survives renames
        Console.WriteLine($"URL       : {result.SecureUrl}");
        Console.WriteLine($"Format    : {result.Format} {result.Width}x{result.Height}, {result.Bytes} bytes");
        Console.WriteLine($"Version   : {result.Version}");   // use to bust CDN caches

        return 0;
    }
}

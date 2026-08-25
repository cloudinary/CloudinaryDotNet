// Shared setup for the examples: builds a configured client and turns the two
// failure modes into friendly messages instead of stack traces.
//
// This SDK reports errors two different ways, and both need handling:
//   - Configuration/argument problems THROW ArgumentException.
//   - Cloudinary API failures do NOT throw; they arrive in result.Error.
namespace CloudinaryDotNet.Examples;

using CloudinaryDotNet.Actions;

internal static class ExampleContext
{
    /// <summary>Public IDs the examples create, so the pages and examples agree.</summary>
    public const string ImagePublicId = "examples/uploaded-sample";
    public const string VideoPublicId = "examples/uploaded-large-video";
    public const string ModeratedPublicId = "examples/moderated-upload";
    public const string MetadataPublicId = "examples/product-photo";

    /// <summary>Public sample media, so the examples need no local fixtures.</summary>
    public const string SampleImageUrl = "https://res.cloudinary.com/demo/image/upload/sample.jpg";
    public const string SampleVideoUrl = "https://res.cloudinary.com/demo/video/upload/dog.mp4";

    /// <summary>
    /// Builds a client from CLOUDINARY_URL. Returns null (after printing why) when the
    /// environment is not configured, so callers can exit non-zero without a stack trace.
    /// </summary>
    public static Cloudinary? TryCreateClient()
    {
        var url = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
        if (string.IsNullOrWhiteSpace(url))
        {
            Console.Error.WriteLine(
                "CLOUDINARY_URL is not set. Export it first:\n" +
                "  export CLOUDINARY_URL=cloudinary://<api_key>:<api_secret>@<cloud_name>\n" +
                "No account? Run: npx @cloudinary/cloud");
            return null;
        }

        try
        {
            // Api.Secure = true is required: this SDK builds http:// URLs by default.
            return new Cloudinary(url) { Api = { Secure = true } };
        }
        catch (ArgumentException ex)
        {
            Console.Error.WriteLine($"CLOUDINARY_URL is not usable: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Prints a Cloudinary API error and returns true when the call failed. API errors are
    /// returned rather than thrown, so every call site has to check.
    /// </summary>
    public static bool Failed(BaseResult result, string what)
    {
        if (result.Error == null)
        {
            return false;
        }

        Console.Error.WriteLine($"{what} failed ({(int)result.StatusCode}): {result.Error.Message}");
        return true;
    }
}

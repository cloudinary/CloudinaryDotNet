// Build video delivery URLs, a poster frame, and an adaptive-streaming manifest URL.
// Uses Api.UrlVideoUp - the image builder would produce an /image/upload/ path that
// does not resolve for a video.
//
// Prerequisites: CLOUDINARY_URL in the environment. Run UploadLargeVideo first.
// Run:           dotnet run --project Examples.csproj TransformAndDeliverVideo
//
// In a real project you would emit the <video> markup into a template.
//
// Doc page: ../docs/transform-and-deliver-video.md
namespace CloudinaryDotNet.Examples;

internal static class TransformAndDeliverVideo
{
    public static async Task<int> RunAsync()
    {
        var cloudinary = ExampleContext.TryCreateClient();
        if (cloudinary == null)
        {
            return 1;
        }

        var videoUrl = cloudinary.Api.UrlVideoUp
            .Transform(new Transformation().Width(640).Crop("scale").Quality("auto"))
            .BuildUrl($"{ExampleContext.VideoPublicId}.mp4");

        Console.WriteLine("Scaled MP4:");
        Console.WriteLine($"  {videoUrl}");

        // Request an image extension from a video asset to get a still frame.
        var posterUrl = cloudinary.Api.UrlVideoUp
            .Transform(new Transformation().Width(400).Crop("fill").StartOffset("2"))
            .BuildUrl($"{ExampleContext.VideoPublicId}.jpg");

        Console.WriteLine("Poster frame at 2 seconds:");
        Console.WriteLine($"  {posterUrl}");

        // Adaptive bitrate: let the player switch renditions instead of serving one MP4.
        var hlsUrl = cloudinary.Api.UrlVideoUp
            .Transform(new Transformation().StreamingProfile("hd"))
            .BuildUrl($"{ExampleContext.VideoPublicId}.m3u8");

        Console.WriteLine("HLS manifest:");
        Console.WriteLine($"  {hlsUrl}");

        // A complete <video> element with three source formats and a poster.
        Console.WriteLine("Player markup:");
        Console.WriteLine($"  {cloudinary.Api.UrlVideoUp.BuildVideoTag(ExampleContext.VideoPublicId)}");

        // Streaming profiles are per-environment, so list rather than assume.
        var profiles = await cloudinary.ListStreamingProfilesAsync();
        if (ExampleContext.Failed(profiles, "Listing streaming profiles"))
        {
            return 1;
        }

        var names = profiles.Data.Select(p => p.Name).OrderBy(n => n, StringComparer.Ordinal).ToList();
        Console.WriteLine($"Streaming profiles available ({names.Count}): {string.Join(", ", names.Take(8))}...");

        return 0;
    }
}

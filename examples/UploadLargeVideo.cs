// Upload a video in chunks, and verify it was stored as a video rather than raw.
//
// Prerequisites: CLOUDINARY_URL in the environment.
// Run:           dotnet run --project Examples.csproj UploadLargeVideo
//
// Downloads a small sample video to a temp file so the example needs no fixtures. In a real
// project the file is already on disk (or streamed), and you would not re-download it.
//
// The important detail: VideoUploadParams, not RawUploadParams. With RawUploadParams this
// upload succeeds silently and stores something no transformation can ever touch.
//
// Doc page: ../docs/upload-large-video.md
namespace CloudinaryDotNet.Examples;

using CloudinaryDotNet.Actions;

internal static class UploadLargeVideo
{
    // Cloudinary rejects any non-final chunk of 5 MB or less, so stay above it.
    private const int ChunkSize = 6 * 1024 * 1024;

    public static async Task<int> RunAsync()
    {
        var cloudinary = ExampleContext.TryCreateClient();
        if (cloudinary == null)
        {
            return 1;
        }

        var videoPath = Path.Combine(Path.GetTempPath(), "cloudinary-example-video.mp4");
        if (!File.Exists(videoPath))
        {
            Console.WriteLine("Downloading a sample video...");
            using var http = new HttpClient();
            var bytes = await http.GetByteArrayAsync(ExampleContext.SampleVideoUrl);
            await File.WriteAllBytesAsync(videoPath, bytes);
        }

        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(videoPath),
            PublicId = ExampleContext.VideoPublicId,
            Overwrite = true,
        };

        UploadResult result;
        try
        {
            result = await cloudinary.UploadLargeAsync(uploadParams, ChunkSize);
        }
        catch (Exception ex)
        {
            // Unlike most of this SDK, a too-small chunk size throws instead of
            // returning an error in result.Error.
            Console.Error.WriteLine($"Chunked upload failed: {ex.Message}");
            return 1;
        }

        if (ExampleContext.Failed(result, "Video upload"))
        {
            return 1;
        }

        // Guard against the silent failure mode described above.
        var resourceType = result.JsonObj["resource_type"]?.ToString();
        if (resourceType != "video")
        {
            Console.Error.WriteLine($"Stored as '{resourceType}', not video - use VideoUploadParams.");
            return 1;
        }

        Console.WriteLine($"Public ID : {result.PublicId}");
        Console.WriteLine($"URL       : {result.SecureUrl}");
        Console.WriteLine($"Duration  : {result.JsonObj["duration"]} seconds");
        Console.WriteLine($"Size      : {result.Bytes} bytes");

        return 0;
    }
}

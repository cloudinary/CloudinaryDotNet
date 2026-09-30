// Upload into a manual moderation queue, list the queue, and record a decision.
//
// Prerequisites: CLOUDINARY_URL in the environment.
// Run:           dotnet run --project Examples.csproj ModerateUpload
//
// 'manual' is used here because it needs no add-on. Automatic moderation (aws_rek,
// webpurify, google_video_moderation, perception_point, duplicate) requires a human to
// register the add-on in the console first, and to accept the provider's terms for some -
// neither step has an API. See the doc page for the full list.
//
// IMPORTANT: a pending asset is delivered normally. Cloudinary does not gate delivery on
// moderation status by default, so the gate has to live in your own application.
//
// Doc page: ../docs/moderate-upload.md
namespace CloudinaryDotNet.Examples;

using CloudinaryDotNet.Actions;

internal static class ModerateUpload
{
    public static async Task<int> RunAsync()
    {
        var cloudinary = ExampleContext.TryCreateClient();
        if (cloudinary == null)
        {
            return 1;
        }

        // 1. Upload into the queue. The asset starts as Pending.
        //
        // A unique public ID per run, because overwriting an asset that was already
        // approved keeps its existing moderation status - the upload would report
        // "Approved" and this example would not show the queue at all.
        var publicId = $"{ExampleContext.ModeratedPublicId}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

        var uploaded = await cloudinary.UploadAsync(new ImageUploadParams
        {
            File = new FileDescription(ExampleContext.SampleImageUrl),
            PublicId = publicId,
            Overwrite = true,
            Moderation = "manual",
        });

        if (ExampleContext.Failed(uploaded, "Moderated upload"))
        {
            return 1;
        }

        // Read the status from the Moderation list: on an upload result the
        // ModerationStatus property is empty.
        Console.WriteLine($"Kind   : {uploaded.Moderation[0].Kind}");
        Console.WriteLine($"Status : {uploaded.Moderation[0].Status}");
        Console.WriteLine($"URL    : {uploaded.SecureUrl}");
        Console.WriteLine("         ^ already serving. Do not publish this until approved.");

        // 2. The review queue your moderation UI would render.
        var queue = await cloudinary.ListResourcesByModerationStatusAsync("manual", ModerationStatus.Pending);
        if (ExampleContext.Failed(queue, "Listing the moderation queue"))
        {
            return 1;
        }

        // The moderation index lags writes by a few seconds, so a just-uploaded asset
        // may not appear yet.
        Console.WriteLine($"Pending review: {queue.Resources?.Length ?? 0} asset(s)");

        // 3. A reviewer's decision.
        var decision = await cloudinary.UpdateResourceAsync(
            new UpdateParams(uploaded.PublicId) { ModerationStatus = ModerationStatus.Approved });

        if (ExampleContext.Failed(decision, "Recording the moderation decision"))
        {
            return 1;
        }

        // Unlike the upload result, a read exposes the flat status.
        var after = await cloudinary.GetResourceAsync(uploaded.PublicId);
        if (ExampleContext.Failed(after, "Re-reading the asset"))
        {
            return 1;
        }

        Console.WriteLine($"Status after review: {after.ModerationStatus}");
        Console.WriteLine("Only now would you mark it publishable in your own data model.");

        return 0;
    }
}

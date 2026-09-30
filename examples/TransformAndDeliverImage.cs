// Build image delivery URLs. No network calls and no API secret are needed for this -
// only the cloud name - so it is safe to do inside a view or a hot request path.
//
// Prerequisites: CLOUDINARY_URL in the environment. Run UploadImage first so the
//                transformed asset exists; 'sample' also ships with every new account.
// Run:           dotnet run --project Examples.csproj TransformAndDeliverImage
//
// In a real project you would emit these URLs into a template rather than printing them.
//
// Doc page: ../docs/transform-and-deliver-image.md
namespace CloudinaryDotNet.Examples;

internal static class TransformAndDeliverImage
{
    public static Task<int> RunAsync()
    {
        var cloudinary = ExampleContext.TryCreateClient();
        if (cloudinary == null)
        {
            return Task.FromResult(1);
        }

        // f_auto + q_auto: best format for the browser, tuned quality. Apply to everything.
        var thumbnail = cloudinary.Api.UrlImgUp
            .Transform(new Transformation()
                .Width(200).Height(200).Crop("thumb")
                .Gravity("auto")
                .FetchFormat("auto")
                .Quality("auto"))
            .BuildUrl("sample.jpg");

        Console.WriteLine("Optimized thumbnail:");
        Console.WriteLine($"  {thumbnail}");

        // Chained components run in order, each on the output of the previous one.
        var banner = cloudinary.Api.UrlImgUp
            .Transform(new Transformation()
                .Width(1280).Height(720).Crop("fill").Gravity("auto").Chain()
                .Overlay(new TextLayer().Text("SALE")
                    .FontFamily("Arial").FontSize(64).FontWeight("bold"))
                .Color("white").Gravity("south_east").X(24).Y(24).Chain()
                .FetchFormat("auto").Quality("auto"))
            .BuildUrl("sample.jpg");

        Console.WriteLine("Chained transformation with a text overlay:");
        Console.WriteLine($"  {banner}");

        // An <img> tag, if you want the markup rather than the URL.
        Console.WriteLine("Image tag:");
        Console.WriteLine($"  {cloudinary.Api.UrlImgUp.BuildImageTag("sample.jpg")}");

        return Task.FromResult(0);
    }
}

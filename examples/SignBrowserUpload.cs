// Sign an upload on your server so a browser can upload directly to Cloudinary
// without ever seeing your API secret.
//
// Prerequisites: CLOUDINARY_URL in the environment.
// Run:           dotnet run --project Examples.csproj SignBrowserUpload
//
// This prints what a signing endpoint would return, then performs the upload the browser
// would perform, so the whole round trip is verifiable from one command. In a real project
// the first half is an HTTP endpoint (see ../docs/use-with-aspnet-core.md) and the second
// half is JavaScript in the page.
//
// Doc page: ../docs/sign-browser-upload.md
namespace CloudinaryDotNet.Examples;

using System.Net.Http.Headers;

internal static class SignBrowserUpload
{
    private const string UploadFolder = "user-uploads";

    public static async Task<int> RunAsync()
    {
        var cloudinary = ExampleContext.TryCreateClient();
        if (cloudinary == null)
        {
            return 1;
        }

        // --- Server side: sign only the parameters the client is allowed to send. ---
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var signedParams = new SortedDictionary<string, object>
        {
            { "folder", UploadFolder },
            { "timestamp", timestamp },
        };

        var signature = cloudinary.Api.SignParameters(signedParams);
        var apiKey = cloudinary.Api.Account.ApiKey;
        var cloudName = cloudinary.Api.Account.Cloud;

        Console.WriteLine("Your signing endpoint would return:");
        Console.WriteLine($"  signature : {signature}");
        Console.WriteLine($"  timestamp : {timestamp}");
        Console.WriteLine($"  folder    : {UploadFolder}");
        Console.WriteLine($"  apiKey    : {apiKey}          (public - safe to send)");
        Console.WriteLine($"  cloudName : {cloudName}");
        Console.WriteLine("  (the api_secret never leaves the server)");

        // --- Client side: upload using that signature. ---
        using var http = new HttpClient();
        byte[] fileBytes;
        try
        {
            fileBytes = await http.GetByteArrayAsync(ExampleContext.SampleImageUrl);
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Could not fetch the sample image: {ex.Message}");
            return 1;
        }

        using var form = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        form.Add(fileContent, Quoted("file"), Quoted("upload.jpg"));
        form.Add(FormField(apiKey), Quoted("api_key"));
        form.Add(FormField(timestamp), Quoted("timestamp"));
        form.Add(FormField(signature), Quoted("signature"));
        form.Add(FormField(UploadFolder), Quoted("folder"));   // exactly what was signed

        // 'auto' lets Cloudinary detect image/video/raw from the file itself.
        var response = await http.PostAsync(
            $"https://api.cloudinary.com/v1_1/{cloudName}/auto/upload", form);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.Error.WriteLine($"Signed upload failed ({(int)response.StatusCode}): {body}");
            return 1;
        }

        Console.WriteLine("The browser's upload succeeded:");
        Console.WriteLine($"  {body}");

        return 0;
    }

    /// <summary>
    /// Cloudinary expects bare multipart fields. HttpClient adds a Content-Type to
    /// StringContent, which makes the API treat the request as an unsigned upload.
    /// </summary>
    private static StringContent FormField(string value)
    {
        var content = new StringContent(value);
        content.Headers.ContentType = null;
        return content;
    }

    /// <summary>
    /// MultipartFormDataContent emits an unquoted name= parameter, which Cloudinary's
    /// parser ignores - the request then looks like an unsigned upload and is rejected
    /// with "Upload preset must be specified". Passing the name pre-quoted produces the
    /// name="..." form the API expects. Not needed from a browser: FormData does this.
    /// </summary>
    private static string Quoted(string name) => $"\"{name}\"";
}

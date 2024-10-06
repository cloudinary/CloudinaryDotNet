using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Markdig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhotoAlbum.Data;
using UploadResult = PhotoAlbum.Data.UploadResult;
using Markdig;


namespace PhotoAlbum.Pages
{
    public class UploadModel : PageModel
    {
        private const string Tags = "backend_PhotoAlbum";

        private readonly Cloudinary _cloudinary;
        private readonly PhotosDbContext _context;
        public string RenderedMarkdown { get; private set; }



        public UploadModel(
            Cloudinary cloudinary,
            PhotosDbContext context
            )
        {
            _cloudinary = cloudinary;
            _context = context;
        }

        public void OnGet()
        {
            string markdownContent = @"

#### Server-side Upload

You can upload images, videos, or any other raw file to Cloudinary from your .NET code. Uploading is done over HTTPS using a secure protocol based on your product environment's `api_key` and `api_secret` parameters.

## .NET Image Upload

The following C# method uploads an image to Cloudinary:

### C#

```csharp
public ImageUploadResult Upload(ImageUploadParams parameters);
```

The `ImageUploadParams` class sets the image to upload with additional parameters, and the `ImageUploadResult` class provides the deserialized server response.

### Example: Uploading a Local Image File

The following code uploads a local image file named `my_image.jpg`.

#### In C#

```csharp
var uploadParams = new ImageUploadParams()
{
    File = new FileDescription(@""c:\my_image.jpg"")
};
var uploadResult = cloudinary.Upload(uploadParams);
```

#### In VB.NET

```vbnet
Dim uploadParams = New ImageUploadParams
uploadParams.File = New FileDescription(""c:\my_image.jpg"")
Dim uploadResult = m_cloudinary.Upload(uploadParams)
```

For a full list of the `Upload` method parameters, see the upload method in the [Upload API reference](https://cloudinary.com/documentation/dotnet_image_and_video_upload#server_side_upload).

> **Note:** If you need to override the default signing mechanism and supply your own signature for the upload, you can also pass the `signature` and `timestamp` parameters.

For more details, visit the [Cloudinary .NET Image and Video Upload Documentation](https://cloudinary.com/documentation/dotnet_image_and_video_upload#server_side_upload).

";
            RenderedMarkdown = Markdown.ToHtml(markdownContent);
        }

        public async Task<IActionResult> OnPostAsync(IFormFile[] images)
        {
            var results = new List<Dictionary<string, string>>();

            if (images == null || images.Length == 0)
            {
                return RedirectToPage("Upload");
            }

            IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-US");
            foreach (var image in images)
            {
                if (image.Length == 0) return RedirectToPage("Upload");

                var result = await _cloudinary.UploadAsync(new ImageUploadParams
                {
                    File = new FileDescription(image.FileName,
                        image.OpenReadStream()),
                    Tags = Tags
                }).ConfigureAwait(false);

                var imageProperties = new Dictionary<string, string>();
                foreach (var token in result.JsonObj.Children())
                {
                    if (token is JProperty prop)
                    {
                        imageProperties.Add(prop.Name, prop.Value.ToString());
                    }
                }

                results.Add(imageProperties);

                await _context.Photos.AddAsync(new Photo
                {
                    Bytes = (int) result.Bytes,
                    CreatedAt = DateTime.Now,
                    Format = result.Format,
                    Height = result.Height,
                    Path = result.Url.AbsolutePath,
                    PublicId = result.PublicId,
                    ResourceType = result.ResourceType,
                    SecureUrl = result.SecureUrl.AbsoluteUri,
                    Signature = result.Signature,
                    Type = result.JsonObj["type"]?.ToString(),
                    Url = result.Url.AbsoluteUri,
                    Version = int.Parse(result.Version, provider),
                    Width = result.Width
                }).ConfigureAwait(false);
            }

            await _context.UploadResults.AddAsync(new UploadResult { UploadResultAsJson = JsonConvert.SerializeObject(results) }).ConfigureAwait(false);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("UploadSucceeded");
        }

        private string RenderMarkdownToHtml(string markdown)
        {
            // Use Markdig to convert Markdown to HTML
            return Markdown.ToHtml(markdown);
        }
    }
}

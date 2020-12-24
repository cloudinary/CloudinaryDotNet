namespace photo_album
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class UploadModel : PageModel
    {
        private const string Tags = "backend_photo_album";

        private readonly Cloudinary m_cloudinary;
        private readonly PhotosDbContext m_context;

        public UploadModel(
            Cloudinary cloudinary,
            PhotosDbContext context
            )
        {
            m_cloudinary = cloudinary;
            m_context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(IFormFile[] images)
        {
            var results = new List<Dictionary<string, string>>();

            if (images == null || images.Length == 0)
            {
                return RedirectToPage("Upload");
            }

            foreach (IFormFile image in images)
            {
                if (image.Length == 0) return RedirectToPage("Upload");

                var result = await m_cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(image.FileName,
                    image.OpenReadStream()),
                    Tags = Tags
                });

                var imageProperties = new Dictionary<string, string>();
                foreach (var token in result.JsonObj.Children())
                {
                    if (token is JProperty prop)
                    {
                        imageProperties.Add(prop.Name, prop.Value.ToString());
                    }
                }
                results.Add(imageProperties);

                await m_context.Photos.AddAsync(new Photo()
                {
                    Bytes = (int)result.Bytes,
                    CreatedAt = DateTime.Now,
                    Format = result.Format,
                    Height = result.Height,
                    Path = result.Url.AbsolutePath,
                    PublicId = result.PublicId,
                    ResourceType = result.ResourceType,
                    SecureUrl = result.SecureUrl.AbsoluteUri,
                    Signature = result.Signature,
                    Type = result.JsonObj["type"].ToString(),
                    Url = result.Url.AbsoluteUri,
                    Version = Int32.Parse(result.Version),
                    Width = result.Width,
                });
            }

            await m_context.UploadResults.AddAsync(new UploadResult { UploadResultAsJson = JsonConvert.SerializeObject(results) });

            await m_context.SaveChangesAsync();

            return RedirectToPage("UploadSucceeded");
        }
    }
}

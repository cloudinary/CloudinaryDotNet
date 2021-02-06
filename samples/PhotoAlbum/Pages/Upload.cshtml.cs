using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhotoAlbum.Data;
using UploadResult = PhotoAlbum.Data.UploadResult;

namespace PhotoAlbum.Pages
{
    public class UploadModel : PageModel
    {
        private const string Tags = "backend_PhotoAlbum";

        private readonly Cloudinary _cloudinary;
        private readonly PhotosDbContext _context;

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
    }
}

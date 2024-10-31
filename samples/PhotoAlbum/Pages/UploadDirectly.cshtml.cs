using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PhotoAlbum.Data;
using PhotoAlbum.Infrastructure;
using static System.String;

namespace PhotoAlbum.Pages
{
    public class UploadDirectlyModel : PageModel
    {


        private readonly Cloudinary _cloudinary;
        private readonly PhotosDbContext _context;
        public const string Tags = "direct_PhotoAlbum";
        public DirectUploadType DirectUploadType { get; set; }
        public string Preset { get; set; }

        public UploadDirectlyModel(
            Cloudinary cloudinary,
            PhotosDbContext context
            )
        {
            _cloudinary = cloudinary;
            _context = context;
        }

        public async Task OnGetAsync(DirectUploadType type)
        {
            DirectUploadType = type;

            if (DirectUploadType == DirectUploadType.Signed) return;

            Preset = $"sample_{_cloudinary.Api.SignParameters(new SortedDictionary<string, object> { { "api_key", _cloudinary.Api.Account.ApiKey } }).Substring(0, 10)}";

            await _cloudinary.CreateUploadPresetAsync(new UploadPresetParams
            {
                Name = Preset,
                Unsigned = true
            }).ConfigureAwait(false);
        }

        public async Task OnPostAsync()
        {
            string content = null;
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                content = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            if (IsNullOrEmpty(content)) return;

            var parsedResult = JsonConvert.DeserializeObject<ImageUploadResult>(content);
            IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-US");
            await _context.Photos.AddAsync(new Photo
            {
                CreatedAt = parsedResult.CreatedAt,
                Format = parsedResult.Format,
                Height = parsedResult.Height,
                PublicId = parsedResult.PublicId,
                ResourceType = parsedResult.ResourceType,
                SecureUrl = parsedResult.SecureUrl.ToString(),
                Signature = parsedResult.Signature,
                Type = parsedResult.Type,
                Url = parsedResult.Url.ToString(),
                Version = int.Parse(parsedResult.Version, provider),
                Width = parsedResult.Width
            }).ConfigureAwait(false);

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

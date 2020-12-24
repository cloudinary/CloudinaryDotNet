namespace photo_album.Pages
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Newtonsoft.Json;
    using photo_album.Infrastructure;

    public class UploadDirectlyModel : PageModel
    {
        private const string FolderName = "preset_folder";
        private readonly Cloudinary m_cloudinary;
        private readonly PhotosDbContext m_context;
        public readonly string Tags = "direct_photo_album";
        public DirectUploadType DirectUploadtype { get; set; }
        public string Preset { get; set; }

        public UploadDirectlyModel(
            Cloudinary cloudinary,
            PhotosDbContext context
            )
        {
            m_cloudinary = cloudinary;
            m_context = context;
        }

        public async Task OnGetAsync(DirectUploadType type)
        {
            DirectUploadtype = type;

            if (DirectUploadtype == DirectUploadType.signed) return;

            Preset = $"sample_{m_cloudinary.Api.SignParameters(new SortedDictionary<string, object>() { { "api_key", m_cloudinary.Api.Account.ApiKey } }).Substring(0, 10)}";

            await m_cloudinary.CreateUploadPresetAsync(new UploadPresetParams()
            {
                Name = Preset,
                Unsigned = true,
                Folder = FolderName
            });
        }

        public async Task OnPostAsync()
        {
            string content = null;
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                content = await reader.ReadToEndAsync();
            }

            if (String.IsNullOrEmpty(content)) return;

            var parsedResult = JsonConvert.DeserializeObject<ImageUploadResult>(content);

            await m_context.Photos.AddAsync(new Photo()
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
                Version = Int32.Parse(parsedResult.Version),
                Width = parsedResult.Width
            });

            await m_context.SaveChangesAsync();
        }
    }
}

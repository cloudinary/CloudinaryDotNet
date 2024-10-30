using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PhotoAlbum.Pages
{
    public class PhotoDetailModel : PageModel
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;


        public PhotoDetailModel(Cloudinary cloudinary, IConfiguration configuration)
        {
            _cloudinary = cloudinary;
            _configuration = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public ImageData Photo { get; set; }
        public List<Url> TransformedUrls { get; set; }
        public string CloudName => _configuration["AccountSettings:CloudName"];

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            // Fetch the photo using the ID (could be direct API call or list resource)
            var result = await _cloudinary.GetResourceAsync(new GetResourceParams(Id));
            if (result == null)
            {
                return NotFound();
            }

            Photo = new ImageData
            {
                PublicId = result.PublicId,
                Format = result.Format
            };

            // Set up base transformation URLs
            TransformedUrls = new List<Url>
        {
            _cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(300).Height(300).Crop("fill")),
            _cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(300).Height(300).Crop("scale"))
        };

            return Page();
        }

        public class ImageData
        {
            public string PublicId { get; set; }
            public string Format { get; set; }
        }

        public Cloudinary Cloudinary => _cloudinary;
    }
}

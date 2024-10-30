using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAlbum.Pages
{
    public class MyPhotosModel : PageModel
    {
        private readonly PhotosDbContext _dbContext;

        public List<Photo> Photos { get; set; }

        public MyPhotosModel(PhotosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGetAsync()
        {
            Photos = await _dbContext.Photos.ToListAsync().ConfigureAwait(false);
        }
    }
}

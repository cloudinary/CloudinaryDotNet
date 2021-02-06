using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Data;

namespace PhotoAlbum.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PhotosDbContext _dbContext;

        public List<Photo> Photos { get; set; }

        public IndexModel(PhotosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGetAsync()
        {
            Photos = await _dbContext.Photos.ToListAsync().ConfigureAwait(false);
        }
    }
}

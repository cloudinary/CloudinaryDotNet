namespace photo_album.Pages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    public class IndexModel : PageModel
    {
        private readonly PhotosDbContext m_dbcontext;

        public List<Photo> Photos { get; set; }

        public IndexModel(PhotosDbContext dbcontext)
        {
            m_dbcontext = dbcontext;
        }

        public async Task OnGetAsync()
        {
            Photos = await m_dbcontext.Photos.ToListAsync();
        }
    }
}

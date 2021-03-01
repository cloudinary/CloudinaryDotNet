using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhotoAlbum.Data;

namespace PhotoAlbum.Pages
{
    public class UploadSucceededModel : PageModel
    {
        private readonly PhotosDbContext _dbContext;

        public List<Dictionary<string, string>> Items { get; set; } = new();

        public UploadSucceededModel(PhotosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGetAsync()
        {
            var lastUploadResult = await _dbContext.UploadResults
                .OrderByDescending(_ => _.Id)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(lastUploadResult?.UploadResultAsJson))
            {
                Items = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(lastUploadResult.UploadResultAsJson);
            }
        }
    }
}

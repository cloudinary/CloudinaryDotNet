namespace photo_album.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class UploadSucceededModel : PageModel
    {
        private readonly PhotosDbContext m_dbcontext;

        public List<Dictionary<string, string>> Items { get; set; } = new List<Dictionary<string, string>>();

        public UploadSucceededModel(PhotosDbContext dbcontext)
        {
            m_dbcontext = dbcontext;
        }

        public async Task OnGetAsync()
        {
            var lastUploadResult = await m_dbcontext.UploadResults
                .OrderByDescending(_ => _.Id)
                .FirstOrDefaultAsync();

            if (!string.IsNullOrWhiteSpace(lastUploadResult?.UploadResultAsJson))
            {
                Items = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(lastUploadResult.UploadResultAsJson);
            }
        }
    }
}

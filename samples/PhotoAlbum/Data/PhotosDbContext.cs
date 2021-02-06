using Microsoft.EntityFrameworkCore;

namespace PhotoAlbum.Data
{
    public class PhotosDbContext : DbContext
    {
        public PhotosDbContext(DbContextOptions<PhotosDbContext> options) : base (options)
        {

        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<UploadResult> UploadResults { get; set; }
    }
}

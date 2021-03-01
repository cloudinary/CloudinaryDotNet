using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Data
{
    public class UploadResult
    {
        [Key]
        public int Id { get; set; }
        public string UploadResultAsJson { get; set; }
    }
}

namespace photo_album
{
    using System.ComponentModel.DataAnnotations;

    public class UploadResult
    {
        [Key]
        public int Id { get; set; }
        public string UploadResultAsJson { get; set; }
    }
}

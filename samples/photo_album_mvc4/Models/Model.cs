using CloudinaryDotNet;
using System.Collections.Generic;

namespace photo_album_mvc4.Models
{
    public class DictionaryModel : Model
    {
        public DictionaryModel(Cloudinary cloudinary, Dictionary<string, string> dict)
            : base(cloudinary)
        {
            Dict = dict;
        }

        public Dictionary<string, string> Dict { get; set; }
    }

    public class PhotosModel : Model
    {
        public PhotosModel(Cloudinary cloudinary, List<Photo> photos)
            : base(cloudinary)
        {
            Photos = photos;
        }

        public List<Photo> Photos { get; set; }
    }

    public class Model
    {
        public Model(Cloudinary cloudinary)
        {
            Cloudinary = cloudinary;
        }

        public Cloudinary Cloudinary { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json.Linq;
using photo_album_mvc4.Models;

namespace photo_album_mvc4.Controllers
{
    public class HomeController : Controller
    {
        static Cloudinary m_cloudinary;

        static HomeController()
        {
            PhotoAlbumContainer album = new PhotoAlbumContainer();
            album.Database.Initialize(false);

            Account acc = new Account(
                    Properties.Settings.Default.CloudName,
                    Properties.Settings.Default.ApiKey,
                    Properties.Settings.Default.ApiSecret);

            m_cloudinary = new Cloudinary(acc);
        }

        public ActionResult Index()
        {
            PhotoAlbumContainer album = new PhotoAlbumContainer();

            return PartialView("List", new PhotosModel(m_cloudinary, album.Photos.ToList()));
        }

        public ActionResult Upload()
        {
            return PartialView("Upload", new Model(m_cloudinary));
        }

        public ActionResult UploadDirectly()
        {
            var model = new DictionaryModel(m_cloudinary, new Dictionary<string, string>() { { "unsigned", "false" } });

            return PartialView("UploadDirectly", model);
        }

        public ActionResult UploadDirectlyUnsigned()
        {
            var preset = "sample_" + m_cloudinary.Api.SignParameters(new SortedDictionary<string, object>() { { "api_key", m_cloudinary.Api.Account.ApiKey } }).Substring(0, 10);
            var result = m_cloudinary.CreateUploadPreset(new UploadPresetParams() { Name = preset, Unsigned = true, Folder = "preset_folder" });

            var model = new DictionaryModel(m_cloudinary, new Dictionary<string, string>() { { "unsigned", "true" }, { "preset", preset } });

            return PartialView("UploadDirectly", model);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadServer()
        {
            PhotoAlbumContainer album = new PhotoAlbumContainer();
            Dictionary<string, string> results = new Dictionary<string, string>();

            for (int i = 0; i < HttpContext.Request.Files.Count; i++)
            {
                var file = HttpContext.Request.Files[i];

                if (file.ContentLength == 0) return PartialView("Upload", new Model(m_cloudinary));

                var result = m_cloudinary.Upload(new ImageUploadParams()
                {
                    File = new CloudinaryDotNet.Actions.FileDescription(file.FileName,
                        file.InputStream),
                    Tags = "backend_photo_album"
                });

                foreach (var token in result.JsonObj.Children())
                {
                    if (token is JProperty)
                    {
                        JProperty prop = (JProperty)token;
                        results.Add(prop.Name, prop.Value.ToString());
                    }
                }

                Photo p = new Photo()
                {
                    Bytes = (int)result.Length,
                    CreatedAt = DateTime.Now,
                    Format = result.Format,
                    Height = result.Height,
                    Path = result.Uri.AbsolutePath,
                    PublicId = result.PublicId,
                    ResourceType = result.ResourceType,
                    SecureUrl = result.SecureUri.AbsoluteUri,
                    Signature = result.Signature,
                    Type = result.JsonObj["type"].ToString(),
                    Url = result.Uri.AbsoluteUri,
                    Version = Int32.Parse(result.Version),
                    Width = result.Width,
                };

                album.Photos.Add(p);
            }

            album.SaveChanges();

            return PartialView("UploadSucceeded",
                new DictionaryModel(m_cloudinary, results));
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public void UploadDirect()
        {
            var headers = HttpContext.Request.Headers;

            string content = null;
            using (StreamReader reader = new StreamReader(HttpContext.Request.InputStream))
            {
                content = reader.ReadToEnd();
            }

            if (String.IsNullOrEmpty(content)) return;

            Dictionary<string, string> results = new Dictionary<string, string>();

            string[] pairs = content.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                string[] splittedPair = pair.Split('=');

                if (splittedPair[0].StartsWith("faces"))
                    continue;

                results.Add(splittedPair[0], splittedPair[1]);
            }

            Photo p = new Photo()
            {
                Bytes = Int32.Parse(results["bytes"]),
                CreatedAt = DateTime.ParseExact(HttpUtility.UrlDecode(results["created_at"]), "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
                Format = results["format"],
                Height = Int32.Parse(results["height"]),
                Path = results["path"],
                PublicId = results["public_id"],
                ResourceType = results["resource_type"],
                SecureUrl = results["secure_url"],
                Signature = results["signature"],
                Type = results["type"],
                Url = results["url"],
                Version = Int32.Parse(results["version"]),
                Width = Int32.Parse(results["width"]),
            };

            PhotoAlbumContainer album = new PhotoAlbumContainer();

            album.Photos.Add(p);

            album.SaveChanges();
        }
    }
}

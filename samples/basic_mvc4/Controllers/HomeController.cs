using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace basic_mvc4
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Uploads images and shows their when done
        /// </summary>
        public ActionResult Index()
        {
            BackgroundUploader bgUploader = GetUploader();

            if (bgUploader.Progress < 100)
                return PartialView("Upload", bgUploader);

            // The whole uploader object will be passed to view as model
            // This is to show usage of some API helpers that builds HTML or URL
            return PartialView("Show", bgUploader);
        }

        /// <summary>
        /// Gets uploading progress
        /// </summary>
        public ActionResult Progress()
        {
            BackgroundUploader bgUploader = GetUploader();

            return Json(new { progress = bgUploader.Progress }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets uploader from session or creates a new one
        /// </summary>
        private BackgroundUploader GetUploader()
        {
            BackgroundUploader bgUploader = Session["bguploader"] as BackgroundUploader;

            if (bgUploader == null)
            {
                bgUploader = new BackgroundUploader();
                Session.Add("bguploader", bgUploader);
                bgUploader.Upload();
                return bgUploader;
            }

            return bgUploader;
        }
    }

    /// <summary>
    /// We can inherit API class to make more convenient for us
    /// These fields will not be passes to cloudinary
    /// They will be used just at showing images after uploading
    /// </summary>
    class ImageUploadParamsExt : ImageUploadParams
    {
        /// <summary>
        /// Image caption to show
        /// </summary>
        public string Caption;

        /// <summary>
        /// Transformation that will be applied at showing image, not at uploading
        /// </summary>
        public Transformation ShowTransform;
    }

    /// <summary>
    /// Model that will be passed to view
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Image caption to show
        /// </summary>
        public string Caption;

        /// <summary>
        /// Transformation that will be applied at showing image, not at uploading
        /// </summary>
        public Transformation ShowTransform;

        /// <summary>
        /// Cloudinary image URL
        /// </summary>
        public string Url;

        /// <summary>
        /// Image format
        /// </summary>
        public string Format;

        /// <summary>
        /// Cloudinary public ID of image
        /// </summary>
        public string PublicId;
    }

    public class BackgroundUploader
    {
        // Cloudinary API object
        Cloudinary m_cloudinary;

        // Parameters of uploading tasks
        List<ImageUploadParamsExt> m_uploadParams;

        // Results of uploading tasks
        List<Image> m_images;

        // Background uploading task
        Task<bool> m_task;
        int m_progress = 0;
        object m_sync = new object();

        public List<Image> Images { get { return m_images; } }

        public Api CloudinaryApi { get { return m_cloudinary.Api; } }

        public BackgroundUploader()
        {
            // Set up cloudinary object

            Account acc = new Account(
                Properties.Settings.Default.CloudName,
                Properties.Settings.Default.ApiKey,
                Properties.Settings.Default.ApiSecret);

            m_cloudinary = new Cloudinary(acc);

            // Check that application is properly installed in IIS
            string fileToCheck = HttpContext.Current.Server.MapPath("/Images/pizza.jpg");
            if (!File.Exists(fileToCheck))
                throw new ApplicationException(String.Format("Can't find file {0}!", fileToCheck));

            // Set up parameters of uploading tasks

            m_uploadParams = new List<ImageUploadParamsExt>();

            // Upload local image, public_id will be generated on Cloudinary's backend.
            m_uploadParams.Add(new ImageUploadParamsExt()
            {
                File = new FileDescription(HttpContext.Current.Server.MapPath("/Images/pizza.jpg")),
                Tags = "basic_mvc4",

                Caption = "Local file, Fill 200x150",
                ShowTransform = new Transformation().Width(200).Height(150).Crop("fill")
            });

            // Upload local image, uploaded with a public_id.
            m_uploadParams.Add(new ImageUploadParamsExt()
            {
                File = new FileDescription(HttpContext.Current.Server.MapPath("/Images/pizza.jpg")),
                PublicId = "custom_name",
                Tags = "basic_mvc4",

                Caption = "Local file, custom public ID, Fit into 200x150",
                ShowTransform = new Transformation().Width(200).Height(150).Crop("fit")
            });

            // Eager transformations are applied as soon as the file is uploaded, instead of waiting
            // for a user to request them. 
            m_uploadParams.Add(new ImageUploadParamsExt()
            {
                File = new FileDescription(HttpContext.Current.Server.MapPath("/Images/lake.jpg")),
                PublicId = "eager_custom_name",
                Tags = "basic_mvc4",
                EagerTransforms = new List<Transformation>(){
                    new EagerTransformation().Width(200).Height(150).Crop("scale")},

                Caption = "Local file, Eager transformation of scaling to 200x150",
                ShowTransform = new Transformation().Width(200).Height(150).Crop("scale")
            });

            // In the two following examples, the file is fetched from a remote URL and stored in Cloudinary.
            // This allows you to apply the same transformations, and serve those using Cloudinary's CDN layer.
            m_uploadParams.Add(new ImageUploadParamsExt()
            {
                File = new FileDescription("http://res.cloudinary.com/demo/image/upload/couple.jpg"),
                Tags = "basic_mvc4",

                Caption = "Uploaded remote image, Face detection based 200x150 thumbnail",
                ShowTransform = new Transformation().Width(200).Height(150).Crop("thumb").Gravity("faces")
            });

            m_uploadParams.Add(new ImageUploadParamsExt()
            {
                File = new FileDescription("http://res.cloudinary.com/demo/image/upload/couple.jpg"),
                Tags = "basic_mvc4",
                Transformation = new Transformation().Width(500).Height(500).Crop("fit").Effect("saturation:-70"),

                Caption = "Uploaded remote image, Fill 200x150, round corners, apply the sepia effect",
                ShowTransform = new Transformation().Width(200).Height(150).Crop("fill").Gravity("face").Radius(10).Effect("sepia")
            });

            m_images = new List<Image>();
        }

        public int Progress { get { lock (m_sync) { return m_progress; } } }

        // Performs all uploading tasks and fills results (model to apply to view)
        public void Upload()
        {
            // Upload images in background to allow user to see the progress
            m_task = Task.Factory.StartNew(() =>
            {
                try
                {
                    for (int i = 0; i < m_uploadParams.Count; i++)
                    {
                        // Using Cloudinary API to upload images
                        ImageUploadResult result = m_cloudinary.Upload(m_uploadParams[i]);

                        m_images.Add(new Image()
                        {
                            // Copy predefined caption and transformation
                            Caption = m_uploadParams[i].Caption,
                            ShowTransform = m_uploadParams[i].ShowTransform,

                            // Load data from Cloudinary response
                            PublicId = result.PublicId,
                            Url = result.Uri.ToString(),
                            Format = result.Format
                        });

                        lock (m_sync)
                        {
                            m_progress = (i + 1) * 100 / m_uploadParams.Count;
                        }
                    }

                    return true;
                }
                catch
                {
                    lock (m_sync) { m_progress = 100; }

                    return false;
                }
            });
        }
    }
}

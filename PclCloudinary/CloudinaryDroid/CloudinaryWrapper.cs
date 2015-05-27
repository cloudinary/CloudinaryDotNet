using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Net;

namespace CloudinaryDroid
{
	public class CloudinaryWrapper
	{
		Account m_account;
		Cloudinary m_cloudinary;
		string m_testImagePath;
		string m_testVideoPath;
		string m_testPdfPath;
		string m_testIconPath;

		/// <summary>
		/// A convenience method for uploading an image before testing
		/// </summary>
		/// <param name="id">The ID of the resource</param>
		/// <returns>The upload results</returns>
		private ImageUploadResult UploadTestResource( String id)
		{
			var uploadParams = new ImageUploadParams()
			{
				File = new FileDescription(m_testImagePath),
				PublicId = id,
				Tags = "test"
			};
			return m_cloudinary.Upload(uploadParams);
		}

		/// <summary>
		/// A convenience method for deleting an image in the test
		/// </summary>
		/// <param name="id">The ID of the image to delete</param>
		/// <returns>The results of the deletion</returns>
		private DelResResult DeleteTestResource( String id)
		{
			return m_cloudinary.DeleteResources(id);
		}


        public CloudinaryWrapper()
        {
            //Initialize();
        }

		public void Initialize()
		{
			m_account = new Account(
                "dt7dlkaum",
                "743578136545741",
                "wETwOKHSP6vvb4VHzZNh4qWcCGQ");

            //if (String.IsNullOrEmpty(m_account.Cloud))
            //    Console.WriteLine("Cloud name must be specified in test configuration (app.config)!");

            //if (String.IsNullOrEmpty(m_account.ApiKey))
            //    Console.WriteLine("Cloudinary API key must be specified in test configuration (app.config)!");

            //if (String.IsNullOrEmpty(m_account.ApiSecret))
            //    Console.WriteLine("Cloudinary API secret must be specified in test configuration (app.config)!");

			m_cloudinary = new Cloudinary(m_account);

//			if (!String.IsNullOrWhiteSpace(Settings.Default.ApiBaseAddress))
//				m_cloudinary.Api.ApiBaseAddress = Settings.Default.ApiBaseAddress;

            //m_testVideoPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "movie.mp4");
            //m_testImagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestImage.jpg");
            //m_testPdfPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "multipage.pdf");
            //m_testIconPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "favicon.ico");

            //Resources.TestImage.Save(m_testImagePath);
            //File.WriteAllBytes(m_testPdfPath, Resources.multipage);
            //File.WriteAllBytes(m_testVideoPath, Resources.movie);

            //using (Stream s = new FileStream(m_testIconPath, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    Resources.favicon.Save(s);
            //}
		}

		public async Task UploadUnsigned()
		{
//			var preset = m_cloudinary.CreateUploadPreset(new UploadPresetParams()
//				{
//					Folder = "upload_folder",
//					Unsigned = true
//				});

			string remoteUrl = "http://img.mangahit.com/manga/0968/054697/01.jpg";
			var webClient = new WebClient();
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string localFilename = "downloaded.png";
			string localPath = Path.Combine (documentsPath, localFilename);

			webClient.DownloadDataCompleted += (s, e) => {
				var bytes = e.Result; // get the downloaded data
				File.WriteAllBytes (localPath, bytes); // writes to local storage   
			};
			//var url = new Uri(remoteUrl);
			await webClient.DownloadFileTaskAsync (remoteUrl, localPath);

			var acc = new Account("dt7dlkaum"); //Settings.Default.CloudName);
			var cloudinary = new Cloudinary(acc);


			var upload = cloudinary.Upload(new ImageUploadParams()
				{
					File = new FileDescription(localPath),
					UploadPreset = "nqoryyty",
					Unsigned = true,
					UseFilename = true, 
					UniqueFilename = true,

				});
		}

		public async Task TestUploadLocalImage(string path)
        {

			var uploadParams = new ImageUploadParams()
			{
				File = new FileDescription("http://d152j5tfobgaot.cloudfront.net/wp-content/uploads/2015/05/Wearable-technology.jpg" ), // "http://cloudinary.com/images/old_logo.png"
				Tags = "remote"
			};

			var uploadResult = m_cloudinary.Upload(uploadParams);

//			Assert.AreEqual(3381, uploadResult.Length);
//			Assert.AreEqual(241, uploadResult.Width);
//			Assert.AreEqual(51, uploadResult.Height);
//			Assert.AreEqual("png", uploadResult.Format);


//            var uploadParams = new ImageUploadParams()
//            {
//                File = new FileDescription(path)
//            };
//
//			var uploadResult = await m_cloudinary.UploadAsync (uploadParams);
//            //var uploadResult = m_cloudinary.Upload(uploadParams);
//
//            //Assert.AreEqual(1920, uploadResult.Width);
//            //Assert.AreEqual(1200, uploadResult.Height);
//            //Assert.AreEqual("jpg", uploadResult.Format);
//
//            var checkParams = new SortedDictionary<string, object>();
//            checkParams.Add("public_id", uploadResult.PublicId);
//            checkParams.Add("version", uploadResult.Version);
//
//            var api = new Api(m_account);
//            string expectedSign = api.SignParameters(checkParams);

            //Assert.AreEqual(expectedSign, uploadResult.Signature);
        }



	}
}


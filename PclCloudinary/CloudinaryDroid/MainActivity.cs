using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;


namespace CloudinaryDroid
{
	[Activity (Label = "CloudinaryDroid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected async override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.Local);
			
			button.Click += async delegate {
				//button.Text = string.Format ("{0} clicks!", count++);
                //var cloudWrapper = new CloudinaryWrapper();
                //string remoteFilePath = @"http://d152j5tfobgaot.cloudfront.net/wp-content/uploads/2015/05/online-community.jpg";
                //await cloudWrapper.TestUploadLocalImage(remoteFilePath);
				//await cloudWrapper.UploadUnsigned();

				string remoteUrl = "http://img.mangahit.com/manga/0968/054697/01.jpg";
				var webClient = new WebClient();
				string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				string localFilename = "downloaded.png";
				string localPath = Path.Combine (documentsPath, localFilename);

				//var url = new Uri(remoteUrl);
				await webClient.DownloadFileTaskAsync (remoteUrl, localPath);

				var acc = new Account("dt7dlkaum"); //Settings.Default.CloudName);
				var cloudinary = new Cloudinary(acc);

				var upload = cloudinary.Upload(new ImageUploadParams()
					{
						File = new FileDescription(remoteUrl),
						UploadPreset = "nqoryyty",
						Unsigned = true,
//						UseFilename = true, 
//						UniqueFilename = true,

					});


                
			};
		}
	}
}



Cloudinary
==========

Cloudinary is a cloud service that offers a solution to a web application's entire image management pipeline. 

Easily upload images to the cloud. Automatically perform smart image resizing, cropping and conversion without installing any complex software. 
Integrate Facebook or Twitter profile image extraction in a snap, in any dimension and style to match your website’s graphics requirements. 
Images are seamlessly delivered through a fast CDN, and much much more. 

Cloudinary offers comprehensive APIs and administration capabilities and is easy to integrate with any web application, existing or new.

Cloudinary provides URL and HTTP based APIs that can be easily integrated with any Web development framework. 

For projects based on Microsoft .NET Framrwork, Cloudinary provides a library for simplifying the integration even further.

## Setup ######################################################################

CloudinaryDotNet is available as NuGet package at https://nuget.org/packages/CloudinaryDotNet

Please see NuGet Documentation at http://docs.nuget.org/ for instructions of how to work with NuGet packages.

### Using Visual Studio

1. Download NuGet Package Manager at http://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c
2. Use Visual Studio to create a new project and choose .NET 3.5 as target framework.
3. Right click on the project in Solution Explorer and click on Manage NuGet packages...
4. Type CloudinaryDotNet in search box at upper right corner.
5. When CloudinaryDotNet package will appears click Install button.
6. After the package will be installed click Close button.
7. You are done setup and able to use CloudinaryDotNet.

### Manual setup

1. Go to https://nuget.org/packages/CloudinaryDotNet and download NuGet package.
2. NuGet package is a ZIP archive that could be extracted using any unzip tool.
3. NuGet package contains CloudinaryDotNet.dll, xml documentation and this file.

## Try it right away

Sign up for a [free account](https://cloudinary.com/users/register/free) so you can try out image transformations and seamless image delivery through CDN.

*Note: Replace `demo` in all the following examples with your Cloudinary's `cloud name`.*  

Accessing an uploaded image with the `sample` public ID through a CDN:

    http://res.cloudinary.com/demo/image/upload/sample.jpg

![Sample](https://d3jpl91pxevbkh.cloudfront.net/demo/image/upload/w_0.4/sample.jpg "Sample")

Generating a 150x100 version of the `sample` image and downloading it through a CDN:

    http://res.cloudinary.com/demo/image/upload/w_150,h_100,c_fill/sample.jpg

![Sample 150x100](https://d3jpl91pxevbkh.cloudfront.net/demo/image/upload/w_150,h_100,c_fill/sample.jpg "Sample 150x100")

Converting to a 150x100 PNG with rounded corners of 20 pixels: 

    http://res.cloudinary.com/demo/image/upload/w_150,h_100,c_fill,r_20/sample.png

![Sample 150x150 Rounded PNG](https://d3jpl91pxevbkh.cloudfront.net/demo/image/upload/w_150,h_100,c_fill,r_20/sample.png "Sample 150x150 Rounded PNG")

For plenty more transformation options, see our [image transformations documentation](http://cloudinary.com/documentation/image_transformations).

Generating a 120x90 thumbnail based on automatic face detection of the Facebook profile picture of Bill Clinton:
 
    http://res.cloudinary.com/demo/image/facebook/c_thumb,g_face,h_90,w_120/billclinton.jpg
    
![Facebook 90x120](https://d3jpl91pxevbkh.cloudfront.net/demo/image/facebook/c_thumb,g_face,h_90,w_120/billclinton.jpg "Facebook 90x200")

For more details, see our documentation for embedding [Facebook](http://cloudinary.com/documentation/facebook_profile_pictures) and [Twitter](http://cloudinary.com/documentation/twitter_profile_pictures) profile pictures.

## Usage

### Configuration

Each request for building a URL of a remote cloud resource must have the `cloud_name` parameter set. 
Each request to our secure APIs (e.g., image uploads, eager sprite generation) must have the `api_key` and `api_secret` parameters set. 
See [API, URLs and access identifiers](http://cloudinary.com/documentation/api_and_access_identifiers) for more details.

Setting the `cloud_name`, `api_key` and `api_secret` parameters can be done either directly in each call to a Cloudinary method, 
by when initializing the Cloudinary object, or by using the CLOUDINARY_URL environment variable / system property.

The main entry point of the library is the Cloudinary object.

    CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary();

NOTE: This call assumes that CLOUDINARY_URL environment variable is set. If not, please use parameterized constructor:

	CloudinaryDotNet.Account account =
		new CloudinaryDotNet.Account("cloud_name", "api_key", "api_secret");
	
	CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);

### Embedding and transforming images

Any image uploaded to Cloudinary can be transformed and embedded using powerful view helper methods:

The following example generates the url for accessing an uploaded `sample` image while transforming it to fill a 100x150 rectangle:

    string url = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(100).Height(150).Crop("fill")).BuildUrl("sample.jpg");

Another example, emedding a smaller version of an uploaded image while generating a 90x90 face detection based thumbnail: 

    string url = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(90).Height(90).Crop("thumb").Gravity("face")).BuildUrl("woman.jpg");

You can provide either a Facebook name or a numeric ID of a Facebook profile or a fan page.  
             
Embedding a Facebook profile to match your graphic design is very simple:

    string url = cloudinary.Api.UrlImgUp.Action("facebook").Transform(new CloudinaryDotNet.Transformation().Width(130).Height(130).Crop("fill").Gravity("north_west")).BuildUrl("billclinton.jpg");
                           
Same goes for Twitter:

    string url = cloudinary.Api.UrlImgUp.Action("twitter_name").BuildUrl("billclinton.jpg");

### Upload

Assuming you have your Cloudinary configuration parameters defined (`cloud_name`, `api_key`, `api_secret`), uploading to Cloudinary is very simple.
    
The following example uploads a local JPG to the cloud: 
    
    CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
	{
		File = new CloudinaryDotNet.Actions.FileDescription(@"c:\mypicture.jpg")
	};
	
	CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
        
The uploaded image is assigned a randomly generated public ID. The image is immediately available for download through a CDN:

    string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
        
    https://res.cloudinary.com/cloud_name/image/upload/biricaezlhduexarhzsb.jpg

You can also specify your own public ID:    
    
    CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
	{
		File = new CloudinaryDotNet.Actions.FileDescription(@"c:\mypicture.jpg"),
		PublicId = "sample_remote_file"
	};
	
	CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);

    string url = cloudinary.Api.UrlImgUp.BuildUrl("sample_remote_file.jpg");

    https://res.cloudinary.com/cloud_name/image/upload/sample_remote_file.jpg
        
### imageTag

Returns an html image tag pointing to Cloudinary.

Usage:

    string tag = cloudinary.Api.UrlImgUp.Format("png").Transform(new CloudinaryDotNet.Transformation().Width(100).Height(100).Crop("fill")).BuildImageTag("sample");

	# <img src='https://res.cloudinary.com/cloud_name/image/upload/c_fill,h_100,w_100/sample.png' width='100' height='100'/>
  
## Additional resources ##########################################################

Additional resources are available at:

* [Website](http://cloudinary.com)
* [Documentation](http://cloudinary.com/documentation)
* [Image transformations documentation](http://cloudinary.com/documentation/image_transformations)
* [Upload API documentation](http://cloudinary.com/documentation/upload_images)

## Support

Contact us at [info@cloudinary.com](mailto:info@cloudinary.com)

Or via Twitter: [@cloudinary](https://twitter.com/#!/cloudinary)

## License #######################################################################

Released under the MIT license. 

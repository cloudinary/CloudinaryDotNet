[![Build status](https://ci.appveyor.com/api/projects/status/vdx8o03ethg5opt4?svg=true)](https://ci.appveyor.com/project/Cloudinary/cloudinarydotnet)
[![NuGet Badge](https://img.shields.io/nuget/v/CloudinaryDotNet)](https://www.nuget.org/packages/CloudinaryDotNet/)
![NuGet Downloads](https://img.shields.io/nuget/dt/CloudinaryDotNet)

Cloudinary .NET SDK
==================
## About
The Cloudinary .NET SDK allows you to quickly and easily integrate your application with Cloudinary.
Effortlessly optimize, transform, upload and manage your cloud's assets.


#### Note
This Readme provides basic installation and usage information.
For the complete documentation, see the [.NET SDK Guide](https://cloudinary.com/documentation/dotnet_integration).

## Table of Contents
- [Key Features](#key-features)
- [Version Support](#Version-Support)
- [Installation](#installation)
- [Usage](#usage)
    - [Setup](#Setup)
    - [Transform and Optimize Assets](#Transform-and-Optimize-Assets)


## Key Features
- [Transform](https://cloudinary.com/documentation/dotnet_video_manipulation#video_transformation_examples) and
  [optimize](https://cloudinary.com/documentation/dotnet_image_manipulation#image_optimizations) assets.
- Generate [image](https://cloudinary.com/documentation/dotnet_image_manipulation#deliver_and_transform_images) and
  [video](https://cloudinary.com/documentation/dotnet_video_manipulation#dotnet_video_transformation_code_examples) tags.
- [Asset Management](https://cloudinary.com/documentation/dotnet_asset_administration).
- [Secure URLs](https://cloudinary.com/documentation/video_manipulation_and_delivery#generating_secure_https_urls_using_sdks).



## Version Support

| SDK Version | .NET Framework 4.5.2 - 4.8 | .NET Standard 1.3 and up | .NET Core | .NET 5 - 9 |
|-------------|----------------------------|--------------------------|-----------|------------|
| 1.x         | ✔                          | ✔                        | ✔         | ✔          |

## Installation
CloudinaryDotNet is available as NuGet package [CloudinaryDotNet](https://nuget.org/packages/CloudinaryDotNet)

Install using Package Manager:
```
PM> Install-Package CloudinaryDotNet
```

# Usage

### Setup
```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

var cloudinary = new Cloudinary();
```

### Transform and Optimize Assets
- [See full documentation](https://cloudinary.com/documentation/dotnet_image_manipulation).

```csharp
var url = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(100).Height(150).Crop("fill")).BuildUrl("sample.jpg")
```

### Upload
- [See full documentation](https://cloudinary.com/documentation/dotnet_image_and_video_upload).
- [Learn more about configuring your uploads with upload presets](https://cloudinary.com/documentation/upload_presets).
```csharp
var uploadParams = new ImageUploadParams()
{
    File = new FileDescription(@"c:\mypicture.jpg")
};

var uploadResult = cloudinary.Upload(uploadParams);
```

### Code Samples

You can find our simple and ready-to-use samples projects, along with documentations in the [samples folder](https://github.com/cloudinary/CloudinaryDotNet/tree/master/samples). 

Please consult with the [README file](https://github.com/cloudinary/CloudinaryDotNet/blob/master/samples/README.md), for usage and explanations.


### Security options
- [See full documentation](https://cloudinary.com/documentation/solution_overview#security).

## Contributions
- Ensure tests run locally
- Open a PR and ensure Travis tests pass


## Get Help
If you run into an issue or have a question, you can either:
- Issues related to the SDK: [Open a GitHub issue](https://github.com/cloudinary/CloudinaryDotNet/issues).
- Issues related to your account: [Open a support ticket](https://cloudinary.com/contact)


## About Cloudinary
Cloudinary is a powerful media API for websites and mobile apps alike, Cloudinary enables developers to efficiently
manage, transform, optimize, and deliver images and videos through multiple CDNs. Ultimately, viewers enjoy responsive
and personalized visual-media experiences—irrespective of the viewing device.


## Additional Resources
- [Cloudinary Transformation and REST API References](https://cloudinary.com/documentation/cloudinary_references): Comprehensive references, including syntax and examples for all SDKs.
- [MediaJams.dev](https://mediajams.dev/): Bite-size use-case tutorials written by and for Cloudinary Developers
- [DevJams](https://www.youtube.com/playlist?list=PL8dVGjLA2oMr09amgERARsZyrOz_sPvqw): Cloudinary developer podcasts on YouTube.
- [Cloudinary Academy](https://training.cloudinary.com/): Free self-paced courses, instructor-led virtual courses, and on-site courses.
- [Code Explorers and Feature Demos](https://cloudinary.com/documentation/code_explorers_demos_index): A one-stop shop for all code explorers, Postman collections, and feature demos found in the docs.
- [Cloudinary Roadmap](https://cloudinary.com/roadmap): Your chance to follow, vote, or suggest what Cloudinary should develop next.
- [Cloudinary Facebook Community](https://www.facebook.com/groups/CloudinaryCommunity): Learn from and offer help to other Cloudinary developers.
- [Cloudinary Account Registration](https://cloudinary.com/users/register/free): Free Cloudinary account registration.
- [Cloudinary Website](https://cloudinary.com): Learn about Cloudinary's products, partners, customers, pricing, and more.


## Licence
Released under the MIT license.

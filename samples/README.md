Cloudinary - Basic .NET sample step-by-step guide
=================================================

## Run sample in debug mode - quick start

This instruction assumes that Microsoft Windows 7 and Microsoft Visual
Studio 2012 are installed.

-   Open cloudinary\_dotnet\_samples.sln with Visual Studio 2012.
-   Go to Package Manager Settings... ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen1.png)
-   ...and allow to download missing packages during build. ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen2.png)
-   Press Ok and build the solution. ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen3.png)
-   Go to settings of basic\_mvc4 project and fill in your cloudinary account data. ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen4.png)
-   Press F5 to run sample application in debug mode. Your default browser will pop up and show sample web application. ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen5.png)

## Deploying ASP.NET MVC4 application on IIS 7.5

This assumes you have successfully ran sample in debug mode. 
Note that this is just one method of deploying ASP.NET application, there are
several other methods and configurations that are better for professional use if you are familiar with Microsoft development tools.

-   If IIS is not installed, install it using Control Panel. ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen6.png)
-   Run inetmgr application. ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen7.png)
-   Right click Sites and click Add Web Site... ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen8.png)
-   Create somewhere directory that will be used to store application files. It is important to properly set permissions of this
    directory. IIS process should have permissions to read and execute files in this directory (user 'IIS AppPool\\DefaultAppPool'). Visual
    Studio should have permissions to write and modify files in this directory (to publish application - note that this only needed for
    method of publishing that is described here; other methods are
    available). ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen9.png) !(http://github.com/CloudinaryDotNet/tree/master/samples/screenshots/screen10.png)
-   Enter any unused site name, select any unused port. Enter path to directory that you have created in previous step. Press button Select...
    ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen11.png)
-   ...and select application pool named ASP.NET v4.0. ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen12.png)
-   If you don't have an application pool named ASP.NET v4.0 you need to complete the .NET installation. See [here](http://stackoverflow.com/questions/4890245/how-to-add-asp-net-4-0-as-application-pool-on-iis-7-windows-7) 
-   Return to Visual Studio, right click on basic\_mvc4 project and click Publish... ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen13.png)
-   Use publishing to File System and select directory that you have created in step 4. Click publish. ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen14.png)
-   Open browser and go to http://localhost:81/  (or other port that you have choosen in step 5. ![](https://raw.github.com/cloudinary/CloudinaryDotNet/master/samples/screenshots/screen15.png)


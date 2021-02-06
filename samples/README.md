Cloudinary - Basic .NET sample step-by-step guide
=================================================

This is a sample ASP.NET Core application showcasing the usage of Cloudinary .NET SDK. 

## Run from command line

This instruction assumes that [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) is installed.

1. Go to project directory.

2. Set Cloudinary account settings using one of the following:

    - in [PhotoAlbum/appsettings.Development.json](PhotoAlbum/appsettings.Development.json), `AccountSettings` section;

    - by using app secrets (see [docs for more details](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0):

      ```
      dotnet user-secrets set "AccountSettings:CloudName" "your_value_here"
      dotnet user-secrets set "AccountSettings:ApiKey" "your_value_here"
      dotnet user-secrets set "AccountSettings:ApiSecret" "your_value_here"
      ```

3. To restore nuget packages and build the project (nuget packages will be restored implicitly):

    ```
    dotnet restore
    ```

4. Run application:

    ```
    dotnet run
    ```

# Cloudinary - Basic .NET Sample Step-by-Step Guide

This is a sample ASP.NET Core application showcasing the usage of the Cloudinary .NET SDK with .NET 8.

## Prerequisites

Ensure that you have the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed before proceeding.

## Steps to Run from Command Line

1. **Navigate to the project directory:**

   Open a terminal window, and navigate to the root directory of the project.

2. **Set Cloudinary account settings:**

   You can configure your Cloudinary account in one of the following ways:

   - **In the `appsettings.Development.json` file:** Open `PhotoAlbum/appsettings.Development.json`, and enter your Cloudinary account details under the `AccountSettings` section.
     
   - **By using user secrets (recommended for local development):**
     Use the following commands to set your Cloudinary account settings in a secure way:

     ```bash
     dotnet user-secrets set "AccountSettings:CloudName" "your_cloud_name"
     dotnet user-secrets set "AccountSettings:ApiKey" "your_api_key"
     dotnet user-secrets set "AccountSettings:ApiSecret" "your_api_secret"
     ```

     For more information on using app secrets, see the official [Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0).

3. **Restore NuGet packages:**

   Run the following command to restore necessary NuGet packages and build the project (NuGet packages will be restored automatically):

   ```bash
   dotnet restore
   ```

4. **Run the application:**

   To start the application, use the following command:

   ```bash
   dotnet run
   ```

   This will launch the ASP.NET Core application, which can be accessed via the browser at `https://localhost:5001` (or as specified by your configuration).


using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoAlbum.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// Configure Services
var cloudName = configuration.GetValue<string>("AccountSettings:CloudName");
var apiKey = configuration.GetValue<string>("AccountSettings:ApiKey");
var apiSecret = configuration.GetValue<string>("AccountSettings:ApiSecret");

if (new[] { cloudName, apiKey, apiSecret }.Any(string.IsNullOrWhiteSpace))
{
    throw new ArgumentException("Please specify Cloudinary account details!");
}

builder.Services.AddSingleton(new Cloudinary(new Account(cloudName, apiKey, apiSecret)));

var dbFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Cloudinary\\samples";
System.IO.Directory.CreateDirectory(dbFolder);
builder.Services.AddDbContext<PhotosDbContext>(options => options.UseSqlite($"Data Source ={dbFolder}\\PhotosCoreDb.sqlite"));

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Ensure Database Creation
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<PhotosDbContext>();
    Debug.Assert(context != null, nameof(context) + " != null");
    await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
}

// Run the application
await app.RunAsync();

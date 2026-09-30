# Runnable examples

One file per task in [`../docs/`](../docs/README.md). Each is a self-contained console
program with a single entry point and no command-line arguments.

These files are **not** shipped in the NuGet package (loose `.cs` files could be picked up
by a consumer's compile glob). Every doc page carries its complete flow inline, so you
never need this directory to complete a task.

## Run one

```bash
export CLOUDINARY_URL=cloudinary://<api_key>:<api_secret>@<cloud_name>
cd examples
dotnet run --project Examples.csproj UploadImage
```

Without arguments the runner lists the available examples.

No credentials? `npx @cloudinary/cloud` provisions a working cloud with no signup — see
[Get Cloudinary credentials](../docs/get-credentials.md).

## Order matters

`UploadImage` and `UploadLargeVideo` create the assets the transform, search, and
metadata examples read. Run them first.

| Example | Doc page |
|---|---|
| `UploadImage` | [Upload an image](../docs/upload-image.md) |
| `UploadLargeVideo` | [Upload a large video](../docs/upload-large-video.md) |
| `TransformAndDeliverImage` | [Transform and deliver an image](../docs/transform-and-deliver-image.md) |
| `TransformAndDeliverVideo` | [Transform and deliver a video](../docs/transform-and-deliver-video.md) |
| `SignBrowserUpload` | [Sign a browser upload](../docs/sign-browser-upload.md) |
| `SearchAndManageAssets` | [Search and manage assets](../docs/search-and-manage-assets.md) |
| `ModerateUpload` | [Moderate an upload](../docs/moderate-upload.md) |
| `UseStructuredMetadata` | [Use structured metadata](../docs/use-structured-metadata.md) |

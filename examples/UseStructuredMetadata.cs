// Define a structured metadata field, write a value, and read it back.
//
// Prerequisites: CLOUDINARY_URL in the environment.
// Run:           dotnet run --project Examples.csproj UseStructuredMetadata
//
// Metadata field definitions are permanent and per product environment, so re-running this
// is safe: an "already exists" response is treated as success.
//
// In a real project the field would be created once during setup, not on every request.
//
// Doc page: ../docs/use-structured-metadata.md
namespace CloudinaryDotNet.Examples;

using CloudinaryDotNet.Actions;

internal static class UseStructuredMetadata
{
    private const string FieldExternalId = "sku";
    private const string SkuValue = "SKU-00042";

    public static async Task<int> RunAsync()
    {
        var cloudinary = ExampleContext.TryCreateClient();
        if (cloudinary == null)
        {
            return 1;
        }

        // 1. Define the field. Idempotent by design: an existing field is fine.
        var field = await cloudinary.AddMetadataFieldAsync(
            new StringMetadataFieldCreateParams("SKU")
            {
                ExternalId = FieldExternalId,
                Mandatory = false,
            });

        if (field.Error != null)
        {
            if (field.Error.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Field '{FieldExternalId}' already exists - reusing it.");
            }
            else
            {
                Console.Error.WriteLine(
                    $"Creating the metadata field failed ({(int)field.StatusCode}): {field.Error.Message}");
                return 1;
            }
        }
        else
        {
            Console.WriteLine($"Created metadata field '{field.ExternalId}'.");
        }

        // 2. Write the value at upload time. An undefined key here would fail the whole
        //    upload, not just the metadata.
        var uploaded = await cloudinary.UploadAsync(new ImageUploadParams
        {
            File = new FileDescription(ExampleContext.SampleImageUrl),
            PublicId = ExampleContext.MetadataPublicId,
            Overwrite = true,
            MetadataFields = new StringDictionary($"{FieldExternalId}={SkuValue}"),
        });

        if (ExampleContext.Failed(uploaded, "Upload with metadata"))
        {
            return 1;
        }

        Console.WriteLine($"Uploaded {uploaded.PublicId} with {FieldExternalId}={SkuValue}.");

        // 3. Read it back. Use a direct read, not a search: the search index lags writes
        //    by a few seconds, so querying immediately returns nothing.
        var asset = await cloudinary.GetResourceAsync(uploaded.PublicId);
        if (ExampleContext.Failed(asset, "Reading the asset"))
        {
            return 1;
        }

        // There is no typed Metadata property; read it from the raw JSON.
        Console.WriteLine($"Metadata on the asset: {asset.JsonObj["metadata"]}");

        Console.WriteLine(
            $"To query it once indexed: cloudinary.Search().Expression(\"metadata.{FieldExternalId}=\\\"{SkuValue}\\\"\")");

        return 0;
    }
}

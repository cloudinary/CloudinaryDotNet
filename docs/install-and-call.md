# Install and call the SDK

## Install

```bash
dotnet add package CloudinaryDotNet
```

## Namespaces

Two `using` directives cover almost everything. The second is the one agents forget:
every parameter and result type (`ImageUploadParams`, `UploadResult`, `UpdateParams`, …)
lives in `CloudinaryDotNet.Actions`, not in the root namespace.

```csharp
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
```

## Create the client

`Cloudinary` is the single entry point. All Upload, Admin, and Search operations hang off
it; URL building hangs off `cloudinary.Api`.

```csharp
var cloudinary = new Cloudinary("cloudinary://<api_key>:<api_secret>@<cloud_name>");

// Or read CLOUDINARY_URL from the environment:
var fromEnv = new Cloudinary();
```

See [Configure Cloudinary](configure-cloudinary.md) for all four ways to configure, and
which to prefer.

## Async and sync

Every API method has both forms. Prefer the `…Async` variants — the synchronous ones
block a thread, which matters in a request pipeline:

```csharp
var result = await cloudinary.UploadAsync(uploadParams);   // preferred
var blocking = cloudinary.Upload(uploadParams);            // sync equivalent
```

## Errors do not throw

Cloudinary API failures are returned, not thrown:

```csharp
var result = await cloudinary.UploadAsync(uploadParams);
if (result.Error != null)
{
    Console.Error.WriteLine($"Upload failed ({(int)result.StatusCode}): {result.Error.Message}");
    return;
}
Console.WriteLine(result.SecureUrl);
```

`Error` is `null` on success. See [Troubleshoot errors](troubleshoot-errors.md) for the
complete model and the few operations that *do* throw.

## Lifetime

`Cloudinary` is **not** `IDisposable`, and each instance creates its own `HttpClient`.
Create one and reuse it for the life of the process — register it as a **singleton** in
dependency injection rather than constructing one per request. See
[Use with ASP.NET Core](use-with-aspnet-core.md).

## Related

- [Configure Cloudinary](configure-cloudinary.md)
- [Use with ASP.NET Core](use-with-aspnet-core.md)
- [.NET SDK guide](https://cloudinary.com/documentation/dotnet_integration.md)

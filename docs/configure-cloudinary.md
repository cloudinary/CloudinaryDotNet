# Configure Cloudinary

## When to use

Once, when you create the `Cloudinary` instance — before any upload, admin, or
URL-generation call.

**Prerequisite:** a cloud name, API key, and API secret. If you do not have them, see
[Get Cloudinary credentials](get-credentials.md) — `npx @cloudinary/cloud` provisions a
working cloud with no signup.

## Recommended: environment variable

Set `CLOUDINARY_URL` (from Console > Settings > API Keys, or written into `.env` for you
by `npx @cloudinary/cloud`):

```bash
export CLOUDINARY_URL=cloudinary://<api_key>:<api_secret>@<cloud_name>
```

```csharp
using CloudinaryDotNet;

var cloudinary = new Cloudinary();   // reads CLOUDINARY_URL
cloudinary.Api.Secure = true;        // see "HTTPS is not the default" below
```

This keeps the secret out of source control and matches how the other Cloudinary SDKs
behave.

## Alternative: explicit `Account`

Use this when the credentials come from a configuration system rather than the
environment — for example ASP.NET Core's `IConfiguration`
(see [Use with ASP.NET Core](use-with-aspnet-core.md)):

```csharp
using CloudinaryDotNet;

var account = new Account(
    cloudName,   // e.g. configuration["Cloudinary:CloudName"]
    apiKey,
    apiSecret);

var cloudinary = new Cloudinary(account) { Api = { Secure = true } };
```

## Alternative: connection-string style

```csharp
var cloudinary = new Cloudinary("cloudinary://<api_key>:<api_secret>@<cloud_name>");
```

## Delivery only, no credentials

URL building is local and needs only the cloud name — no key, no secret. Use this in a
process that must never hold the secret:

```csharp
var delivery = new Cloudinary(new Account("<cloud_name>"));
delivery.Api.Secure = true;
var url = delivery.Api.UrlImgUp.BuildUrl("sample.jpg");
```

URL generation works. Upload and Admin calls on such an instance fail in
`result.Error.Message` — `Missing required parameter - api_key` for uploads,
`Invalid credentials` for Admin calls — rather than throwing.

## HTTPS is not the default

`Api.Secure` is **`false`** out of the box, so generated URLs start with `http://`:

```csharp
var cloudinary = new Cloudinary(url);
cloudinary.Api.UrlImgUp.BuildUrl("sample.jpg");
// http://res.cloudinary.com/<cloud>/image/upload/sample.jpg

cloudinary.Api.Secure = true;
cloudinary.Api.UrlImgUp.BuildUrl("sample.jpg");
// https://res.cloudinary.com/<cloud>/image/upload/sample.jpg
```

**Set `Api.Secure = true` immediately after constructing the client.** Browsers block
mixed content, so an `http://` image URL on an HTTPS page will not load. This differs from
the Node and Python SDKs, where HTTPS is the default — do not assume parity.

Per-URL alternative: `cloudinary.Api.UrlImgUp.Secure(true).BuildUrl(...)`.

## Behaviour you should know

- **Configuration is per instance, not process-global.** Two `Cloudinary` objects can
  point at different clouds, and setting `Api.Secure` on one does not affect the other.
- **`CloudinaryConfiguration`'s static fields are read by `new Account()`, not by
  `new Cloudinary()`.** Setting `CloudinaryConfiguration.CloudName` and then calling the
  parameterless `new Cloudinary()` throws, because that constructor only ever reads
  `CLOUDINARY_URL`. To use the statics, pass `new Account()` explicitly:

  ```csharp
  CloudinaryConfiguration.CloudName  = "my-cloud";
  CloudinaryConfiguration.ApiKey     = "...";
  CloudinaryConfiguration.ApiSecret  = "...";
  var cloudinary = new Cloudinary(new Account());   // note the Account()
  ```

- Each instance owns an `HttpClient`, and `Cloudinary` is not `IDisposable`. Create one
  per process and reuse it — a singleton in DI.
- Proxy support: `cloudinary.Api.ApiProxy = "http://proxy:8080";` — a **string**, not a
  `WebProxy`. Setting it rebuilds the internal `HttpClient`. This property is compiled
  only for the `netstandard2.0` target, so it is unavailable if your app resolves the
  `netstandard1.3` or `net452` assembly.
- Account-level (provisioning) operations use a separate `AccountApi` client and the
  `CLOUDINARY_ACCOUNT_URL` environment variable.

## Validate configuration early

Because a missing `CLOUDINARY_URL` throws only when the client is constructed — and with
a message that names the scheme rather than the absence — check explicitly at startup:

```csharp
var url = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
if (string.IsNullOrEmpty(url))
{
    Console.Error.WriteLine("Cloudinary is not configured: set CLOUDINARY_URL.");
    return 1;
}
var cloudinary = new Cloudinary(url) { Api = { Secure = true } };
```

## Troubleshooting

- `ArgumentException: Invalid CLOUDINARY_URL scheme. Expecting to start with
  'cloudinary://'` — the variable is **missing**, empty, or malformed. The message names
  the scheme even when nothing is set at all, so check for absence first. It is also what
  you get from passing a Claimable Cloud's `api_environment_variable` verbatim, since that
  string starts with `CLOUDINARY_URL=`.
- `ArgumentException: Cloud name must be specified in Account!` — you passed an `Account`
  with no cloud name, or `CloudinaryConfiguration.CloudName` was never set.
- `api_secret mismatch` / `Invalid Signature` on a call — the credentials do not belong
  together. These arrive in `result.Error.Message`, not as exceptions; re-copy all three
  values from the console.
- Images do not load on an HTTPS page — `Api.Secure` is still `false`. See
  [HTTPS is not the default](#https-is-not-the-default).

## Related

- [Get Cloudinary credentials](get-credentials.md) — if you do not have an account yet.
- [Use with ASP.NET Core](use-with-aspnet-core.md) — DI registration and options binding.
- [Sign a browser upload](sign-browser-upload.md) — keeping the secret server-side.
- [.NET SDK guide](https://cloudinary.com/documentation/dotnet_integration.md)

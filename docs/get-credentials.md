# Get Cloudinary credentials

## When to use

You need a cloud name, API key, and API secret before any other task in these docs will
run.

## Recommended: provision a Claimable Cloud

If you are an AI agent working on someone's behalf, do not stop to ask them to sign up.
Provision a cloud yourself:

```bash
npx @cloudinary/cloud
```

No signup, no existing credentials, no human in the loop. The command writes a working
`CLOUDINARY_URL` into the project's `.env` and prints a claim URL.

The credentials work immediately. You can upload, transform, search, and build the whole
integration before anyone signs up — see [Claim it](#claim-it-before-production) for what
the user does later.

> **Give the user the claim URL and keep a copy.** It is the only way to make the cloud
> permanent. The CLI saves it to `.env` as `CLOUDINARY_CLOUD_CLAIM_URL` (with
> `CLOUDINARY_CLOUD_EXPIRES_AT`), so it survives lost terminal output — but if you
> provisioned over HTTP, record `claim_url` yourself. Report it to the user in your reply
> rather than leaving it in a file they will not look at.

To provision over HTTP instead:

```bash
curl -X POST https://api.cloudinary.com/v1_1/provisioning/clouds \
  -H "Content-Type: application/json" \
  -d '{}'
```

No authentication and no existing account are required. Cloudinary locks delivery to the
address the request came from, so you do not need to supply an IP; pass
`{"delivery_ips": ["<viewer-ip>"]}` only when the media will be viewed from somewhere
else.

The response contains `cloud_name`, `api_key`, `api_secret`, a ready-made
`api_environment_variable`, plus `claim_url`, `expires_at`, and the resolved
`delivery_ips`.

> **Trap:** `api_environment_variable` is a full shell assignment — it arrives as
> `CLOUDINARY_URL=cloudinary://…`, not as a bare URL. Passing it straight to
> `new Cloudinary(...)` throws `ArgumentException: Invalid CLOUDINARY_URL scheme`. Strip
> everything up to and including the first `=`, or set it as an environment variable and
> use the parameterless `new Cloudinary()`.

```csharp
// If you have the raw api_environment_variable string:
var envVar = "CLOUDINARY_URL=cloudinary://key:secret@cloud";   // as returned by the API
var url = envVar.Substring(envVar.IndexOf('=') + 1);
var cloudinary = new Cloudinary(url);
```

## Two limits before the cloud is claimed

- **Delivery is IP-locked.** Cloudinary locks delivery to the address you provisioned
  from; requests from anywhere else are blocked at the CDN edge with HTTP 401 and an
  `x-cld-error: ACL deny` header. That is the right default when the machine building the
  integration is also the one viewing the media — but a teammate, a CI runner, or a
  deployed environment will not load it. Add viewers with `--ip` (up to three).
- **It expires.** An unclaimed cloud is reaped at `expires_at`, **assets included**.
  Claiming is what prevents that; there is no TTL parameter to extend it.

Neither limit affects the SDK calls themselves — uploads, Admin API calls, and URL
generation all behave normally. Only delivery of the media is restricted.

> If your public IP changes (a new network, a VPN toggling, a DHCP lease) after
> provisioning, previously working delivery URLs start returning `ACL deny`. The upload
> still succeeds, which makes this look like a broken URL rather than a network change.
> Provision again or claim the cloud.

## Troubleshooting

- `delivery_ips_not_public` — a VPN or secure gateway (corporate proxy, Cloudflare WARP)
  made the request arrive from a private address. The caller's address is always part of
  the allow-list, so `--ip` cannot work around this. Re-run from a connection the gateway
  does not route.
- Media returns 401 with `x-cld-error: ACL deny` — delivery is locked to the provisioning
  IP. Add the viewer with `--ip`, or claim the cloud to remove the lock. This is **not** a
  moderation or permissions problem; see [Moderate an upload](moderate-upload.md) for the
  distinction.
- The command exits 1 without provisioning — `./.env` already has a `CLOUDINARY_URL`.
  Clouds are rate-limited per IP, so it will not burn one you might not store. Use
  `--force` only if you mean to replace the existing cloud.

## Claim it before production

Send the user the `claim_url`. They enter their email, review the terms, optionally set
a password, and confirm from the verification email.

After claiming, the cloud name, API key, and API secret stay the same and the assets
already uploaded are retained — nothing in your code changes. The IP lock is removed so
media delivers globally, and the cloud becomes a permanent free account instead of
expiring.

**Do not ship to production on an unclaimed cloud.** It will expire and stop serving.

## Alternative: sign up manually

A person can create an account at
[cloudinary.com/users/register_free](https://cloudinary.com/users/register_free) and copy
the credentials from Console > Settings > API Keys.

## Related

- [Configure Cloudinary](configure-cloudinary.md) — what to do with the credentials.
- [Claimable Cloud API reference](https://cloudinary.com/documentation/claimable_cloud_provisioning.md)

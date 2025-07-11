
1.27.6 / 2025-06-17
===================

  * Fix API parameters signature

1.27.5 / 2025-03-24
=============

  * Fix missing `ModerationKind` and `ModerationStatus` fields

1.27.4 / 2025-02-12
=============

  * Fix `duplicates` moderation response

1.27.3 / 2025-02-07
=============

  * Fix `format` handling in signed URLs

1.27.2 / 2025-01-23
=============

  * Fix boolean value parsing in API responses

1.27.1 / 2024-12-20
=============

  * Fix race condition in `UploadLarge` with parallel upload

1.27.0 / 2024-12-16
=============

New functionality
-----------------

  * Add support for `RenameFolder` Admin API
  * Add support for `GetConfig` Admin API
  * Add support for `DeleteAccessKey` in Provisioning API
  * Add support for `Analyze` API

Other Changes
-------------

  * Upgrade `cloudinary-jquery-file-upload`
  * Fix epoch time conversion discrepancy
  * Upgrade sample project to .NET 8

1.26.2 / 2024-03-16
===================

  * Fix `X-Unique-Upload-Id` header duplicate value

1.26.1 / 2024-03-14
===================

  * Fix `UploadLarge` breaking large files on upload

1.26.0 / 2024-02-26
=============

New functionality
-----------------
  * Add support for `UploadChunk` and parallel uploads in Upload Api

1.25.1 / 2024-02-07
=============

  * Fix vulnerable dependencies
  * Fix URL encoding in `DownloadPrivate`

1.25.0 / 2024-01-07
===================

New functionality
-----------------

  * Add support for `OAuth` in Api calls
  * Add support for uploading non-seekable streams
  * Add support for `Signature` and `Timestamp` in `UploadParams`
  * Add support for delivery `Type` setter in URL
  * Add support for `UseFetchFormat` parameter to `BuildVideoTag`

Other Changes
-------------

  * Fix special characters encoding in `FetchLayer`

1.24.0 / 2023-12-06
=============

New functionality
-----------------

  * Add support for `Fields` parameter in Search and Admin APIs
  * Add `AccountProvisioning` constructors
  * Add support for access keys management in Account Provisioning API

Other Changes
-------------

  * Fix SearchApi tests on some environments

1.23.0 / 2023-11-06
=============

New functionality
-----------------

  * Add `ICloudinary` interface for `Cloudinary` class
  * Add support for `ImageFile` in `VisualSearch`
  * Add support for fetch video layer
  * Add support for `PlaybackUrl` in `VideoUploadResult`
  * Add support for `OnSuccess` upload parameter

1.22.0 / 2023-07-31
===================

New functionality
-----------------

  * Add support for `SearchFolders` API
  * Add support for Search URL
  * Add support for `VisualSearch` Admin API
  * Add support for related assets APIs

Other Changes
-------------

  * Bump vulnerable dependencies

1.21.0 / 2023-05-30
===================

New functionality
-----------------

  * Add support for `BackgroundRemoval` in `GetUsage` Admin API
  * Add support for `LastUpdated` field in `GetResourceResult`

Other Changes
-------------

  * Fix support for unicode public_ids in URLs
  * Fix `Context` in `VideoUploadResult`

1.20.0 / 2022-09-25
===================

New functionality
-----------------

  * Add support for `ListResourcesByAssetFolder` Admin API

1.19.0 / 2022-06-30
===================

New functionality
-----------------

  * Add support for Dynamic Folders parameters in `Explicit` Upload API

1.18.1 / 2022-06-08
===================

  * Fix `AutoUploadParams` missing properties

1.18.0 / 2022-05-29
===================

New functionality
-----------------

  * Add support for `ListResourcesByAssetIDs` Admin API
  * Add support for `GetResourceByAssetId` Admin API
  * Add support for `ReorderMetadataFields` Admin API
  * Add support for `ReorderMetadataFieldDatasource` Admin API
  * Add support for `ClearInvalid` metadata parameter
  * Add support for disabling b-frames in `VideoCodec` transformation parameter
  * Add support for `LastLogin` parameter in `ListUsers` Provisioning API
  * Add support for multiple ACLs in `AuthToken`
  * Add async versions of the `Metadata` Admin APIs

Other Changes
-------------

  * Bump dependencies to fix security vulnerabilities
  * Fix normalization of `StartOffset` and `EndOffset` transformation parameters
  * Add support for lowercase headers in API responses

1.17.0 / 2022-01-17
===================

New functionality
-----------------

  * Add support for `AutoUploadParams` in `UploadLarge`

1.16.0 / 2022-01-05
===================

New functionality
-----------------

  * Add support for folder decoupling
  * Add support for `CreateSlideshow` Upload API
  * Add support for variables in text style
  * Add support for `context` and structured `metadata` in `Rename` Admin API
  * Add support for structured metadata in `GetResources` Admin API
  * Add support for `ResourceType` from `Context` Upload API
  * Add proxy support

Other Changes
-------------

  * Remove duplicates in Search API fields
  * Fix named parameters normalization issue
  * Fix `appveyor.yml` config
  * Speedup integration tests

1.15.2 / 2021-06-08
===================

  * Fix upload of files larger than 2GB
  * Refactor Admin and Upload APIs
  * Update GitHub templates
  * Fix appveyor test reporting

1.15.1 / 2021-03-26
===================

  * Speedup JSON parsing
  * Enhance integration tests
  * Fix `set_version.ps1` script
  * Restructure solution
  * Fix validation for `AuthToken` generation

1.15.0 / 2021-03-04
===================

New functionality
-----------------

  * Add support of `filename_override` upload parameter
  * Add support of `SHA-256` in auth signatures

Other Changes
-------------

  * Fix `ArchiveResult` empty tags issue
  * Fix Transformations API call
  * Fix `Type` in `TagParams`
  * Upgrade the demo project
  * Fix broken account provisioning tests
  * Integrate with sub-account test service

1.14.0 / 2020-11-24
===================

New functionality
-----------------

  * Add `DownloadBackedUpAsset` helper
  * Add `Eval` upload parameter support
  * Add `AccessibilityAnalysis` support in `UploadPreset`

Other Changes
-------------

  * Fix Newtonsoft.Json NuGet dependency
  * Fix `SubAccountIds` in `UserResult`

1.13.2 / 2020-11-10
===================

  * Fix expression normalisation for advanced cases
  * Improve integration tests that rely on add-ons
  * Add support for list value in metadata
  * Fix code style

1.13.1 / 2020-09-21
===================

  * Add missing `MetadataFields` in `SearchResult`

1.13.0 / 2020-09-17
===================

New functionality
-----------------

  * Add support for `TargetPublicId` in `DownloadArchiveUrl`
  
Other Changes
-------------

  * Fix `ImageMetadata` list values support in `SearchResult`
  * Add tests to Provisioning API
  * Add test for context metadata as user variables

1.12.0 / 2020-08-17
===================

New functionality
-----------------

  * Add Account Provisioning - User Management API
  * Add `DownloadSprite` and `DownloadMulti` helpers
  * Add `DownloadFolder` helper
  * Add support for `Date` in `Usage` Admin API
  * Add enhanced quality scores to `QualityAnalysis` result
  
Other Changes
-------------

  * Fix unicode filename handling in upload API
  * Fix support for integer parameter value
  * Fix `UpdateTransform` Admin API
  * Fix `CustomFunction` causes exception in Transformation
  * Detect data URLs with a suffix in mime type
  * Make response objects setters public
  * Improve visibility of supported frameworks in nuspec
  * Enable code style rules
  * Normalise line endings
  * Add pull request template
  * Add an attribute that retries one test with delay
  * Add CONTRIBUTING.md

1.11.0 / 2020-05-28
===================

New functionality
-----------------
  * Add `duration` and `initial_duration` predefined variables
  * Add `CinemagraphAnalysis` parameter
  * Add `AccessibilityAnalysis` parameter
  * Add `CreateFolder` Admin API
  * Add structured metadata support
  * Add support for 32 char SHA-256 URL signatures
  * Add support for `pow` operator
  * Add support for `max_results` and `next_cursor` in Folders API

Other Changes
-------------

  * Address some issues with NuGet package creation
  * Fix API Url when private CDN is set
  * Fix special characters escaping in API urls
  * Verify protocol in CLOUDINARY_URL
  * Fix type of `Requests` data member of `UsageResult`
  * Implement more flexible way of boolean values deserialization
  * Fix for serialization of  transformation and tags for direct-upload input field
  * Fix permissions issue when getting version in restricted environment
  * Fix integration tests
  * Update issue templates
  * Fix/update admin upload api request response objects
  * Fix `normalize_expression` when variable is named as a keyword
  * Fix code style in AssemblyInfo
  * Cleanup nuspec file

1.10.0 / 2020-01-29
===================

New functionality
-----------------

  * Add response properties to `SearchResult` and `SearchResource`
  * Add `resourceType` parameter in archive methods

Other Changes
-------------

  * Fix NuGet dependency warning
  * Fix `TestUsage` unit test
  * Fix code style

1.9.1 / 2019-11-18
==================

  * Fix nuget package
  * Fix build script output paths resolution

1.9.0 / 2019-11-17
==================

New functionality
-----------------

  * Add support of `Async` API calls in .NET Core
  * Add `expiresAt` to `DownloadPrivate`
  * Add `DerivedNextCursor ` to `GetResourceParams` Admin API
  * Add `ListResourcesByContext` to Admin API
  * Add `Live` parameter to `UploadPreset`
  * Add `AudioFrequency` enumeration
  * Add `Format` parameter to `SpriteParams`
  * Add supported video codecs
  * Add supported `Gravity` values
  * Add `Named` parameter to `GetTransformResult`
  * Add `VerifyApiResponseSignature` and `VerifyNotificationSignature` functions
  * Add XML documentation
  
 Other Changes
 -------------
 
  * Fix typo in `QualityOverride` parameter name
  * Fix `acl` and `url` escaping in `AuthToken` generation
  * Fix project types for VS for Mac
  * Extract integration tests into separate assembly
  * Fix `build.ps1` script
 
1.8.0 / 2019-04-23
==================

New functionality
-----------------

  * Allow generating archive with multiple resource types
  * Add `DeleteFolder` admin API
  * Add `ForceVersion` parameter to `Url`

1.7.0 / 2019-03-14
==================

New functionality
-----------------

  * Add `Pages` to `GetResourceParams`
  * Add public setter for `FileName` attribute of the `FileDescription`
  * Support per corner values in Radius transformation param
  * Ignore URL in AuthToken generation if ACL is provided
  * Add global `secure` flag (fixes #30)

Other Changes
-------------

  * Fix base64 regex validation template
  * Fix url auth token generation

1.6.0 / 2018-12-19
==================

New functionality
-----------------

* Add custom `pre` function support
* Add streaming profile API
* Add `fps` transformation parameter
* Add support for font antialiasing and font hinting for text overlays

Other Changes
-------------

  * Fix transformation cloning
  * Fix remote file upload
  * Fix handling of null custom functions
  * Handle errors on server response parsing
  * Fix `SecurityException` on restricted environments
  * Fix `TestOcrUpdateResult` unit test
  * Remove `DotNetZip` unused vulnerable dependency

1.5.0 / 2018-11-14
==================

New functionality
-----------------

  * Add support for web assembly and lambda functions in transformations
  * Add `QualityAnalysis` parameter
  * Add `Format` parameter to `ResponsiveBreakpoint`

Other Changes
-------------

  * Fix possible NullReferenceException in tests

1.4.1 / 2018-10-11
==================

  * Fix NuGet package

1.4.0 / 2018-10-11
==================

New functionality
-----------------
  * Add support of custom codecs in video tag
  * Add Google Cloud Storage protocol support in upload
  * Add support of user defined variables and expressions
  * Add support of deleting resources by transformations
  * Support advanced OCR
  * Add support of "auto" value for `start_offset` transformation parameter
  * Support remote URLs in UploadLarge API

Other Changes
-------------
  * Fix moderation plugins response support
  * Fix sort_by and aggregate in advanced search
  * Test passing expression operators' value via fluent API
  * Fix Nuget dependency warning (fixes #116)
  * Rename type param in PublishResourceParams
  * Escape parameters in context values
  * Update keyframe_interval param serialization
  * Fix `TestExplicit` dependency on the file format of the remote image

1.3.1 / 2018-05-22
==================

Other Changes
-------------
  * Fix `AuthToken` default `startTime` calculation (#115)
  * Fix package references
  * Add `set_version.ps1` helper script
  * Fix timeout issue in `TestUploadLocalImageTimeout`
  * Fix account cleanup after tests (#110)

1.3.0 / 2018-04-17
==================

New functionality
-----------------

  * Add `ResourceType` to `ExplicitParams`
  * Add `ToType` param to rename API
  * Add `CreateZip` API (#88)
  * Add `Async` parameter to `ExplicitParams`
  * Add `Pages` to `ImageUploadResult`
  * Add `StreamingProfile` `Transformation` parameter
  * Add suffix support for private images.

Other Changes
-------------

  * Fix `Tag` API for video and raw resource types (#90) fixes #82
  * Add `FrameworkDisplayName`/`FrameworkDescription` to user agent
  * Fix `startTime` of `AuthToken` (use UTC)
  * Fix `UploadLarge` in .NET Core
  * Share tests between .Net Core and .Net Framework (#91)
  * Fix compilation warnings / notices

1.2.0 / 2018-03-15
==================

New functionality        
-----------------        
                         
  * Add `AccessControl` parameter to Update and Upload

Other Changes
-------------

  * Fix nuget package structure
  * NuGet package automatization.
  * Simplify `ListResources` test`
  * Remove `auto_tagging` failure test
  * Remove `similarity_search` test

1.1.1 / 2017-11-21
==================

  * Fix nuget package

1.1.0 / 2017-11-21
==================

# The CloudinaryDotNet now supports .net core and standard libraries.

The library has been split to a shared project, a Core project and Standard (Foundation) project.
The standard library is fully backwards compatible with the previous version.

New functionality
-----------------

  * Search API
  * Implemented async methods and tests.
  * Added `access_mode` to RawUploadParams.
  * AppVeyor CI
  * Added `quality_override` param and cover test

Other Changes
-------------

  * Upgrade project structure to VS 2017 and prepare version 1.1.0-rc1
  * Update Nuget.exe
  * Remove nupkg from git
  * Fix http timeout
  * Rearrange tests to enable `TestListResourcesByPublicIds`
  * Implemented auto width params for transformation
  * Fixed setter fo "All" parameter in delResParams class
  * Gitignore fix
  * Fixed test for upload mappings - removed unnecessary check.
  * Fix typos and tests
  * Fix `.gitignore`
  * Update Readme to point to HTTPS URLs of cloudinary.com
  * Added support for different resource_types in Rename and Tag
  * Fixed setter for "all" property in DelResParam class.
  * Removed test user credentials from appveyor configuration.
  * Updated readme file.
  * Create LICENSE
  * Added `nuget.config`
  * AppVeyor configuration.
  * Added lock files.
  * project structure rebuild to support netcore platform
  * Implemented custom configuration section for cloudinary settings.
  
1.0.31 / 2017-05-04
===================

  * Code structure refactored to support both .net classic and net core support  
  * Added .net core support in separate project
  * General code between .net core and .net classic versions moved to shared scope
  * Added base classes with common functionality for Cloudinary and Api objects
  * Added child classes fo Cloudinary and Api that contains specific differences for .net classic and .net core versions
  * Removed using of IHTMLString type
  * Added tests projects for both .net classic and .net core versions
  * Test project for .net classic adopted for working with shared code scope
  * General code cleanup, removed unused constructors and other code improvements    
  

1.0.30 / 2017-05-04
===================

  * Gitignore file adopted for working with Cloudinary.
  * Added custom configuration handler and section for Cloudinary test project
  * SettingsReader class was adopted for working with custom configuration section
  * Added configuration for working with appVeyour automatic tests service

1.0.30 / 2017-01-14
===================

  * Add varying type to createArchive test
  * Fix createArchive to support resource_type
  * Use extension inclusive filename method for supporting proper raw public IDs
  * Remove resource_type from archiveParams toParamsDictionary
  * Added raw archive test + fix faulty overwrite test

1.0.29 / 2017-01-05
===================

  * remove private distribution limitation for SEO suffix

1.0.28 / 2016-12-22
===================

  * Correct array parameters

1.0.27 / 2016-11-10
===================

  * Merge pull request #19 from RTLcoil/features/new-features
  * Simplify ability to set AllowWriteStreamBuffering for upload

1.0.26 / 2016-09-02
===================

  * Support Video in UploadLarge
  * Use generics in UploadLarge
  * Merge pull request #18 from RTLcoil/features/new-features
  * UploadLarge fixes and tests

1.0.25 / 2016-08-18
===================

  * Add UploadLarge

1.0.24 / 2016-07-15
==================
New functionality and features
------------------------------

* Upload mappings API
* Restore API
* ZIP (archive) generation and download API
* Responsive breakpoints
* Conditional transformations
* Aspect ratio transformation parameter
* Overlay and Underlay improvements. Support Line spacing, Letter spacing and Stroke in layer options.
* Put the params in the body instead of URL query for 'POST' and 'PUT' API requests
* Renamed the FontWeight parameter and added TextAlign keyword.
* New User-Agent format (CloudinaryDotNet/vvv) and support of User-Platform parameter
* Support of Invalidate flag for Explicit and Rename API
* Allow passing ad-hoc parameters

Other Changes
-------------

* .NetFramework 3.5 compatibility fixes
* Fixed values of explicit HTML attributes in Image tags (with quotes)
* Moved enums into designated files
* fixed naming conventions and dedicated class for video layer
* Modify Breakpoint test.
* Replace test image
* Disable Listing Direction test
* simplification of the code
* added additional asserts and improved asserts messages
* moved strings to constants, fixed method namings
* updated tests to match current version of the server software
* removed dependencies to keep it a unit test
* Extracted setup for UploadMapping test into designated file.
* removed twitter dependencies due to twitter API limits
* Add random number to public ID of upload test



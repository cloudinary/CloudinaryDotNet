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



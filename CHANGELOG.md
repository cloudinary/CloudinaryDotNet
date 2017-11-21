
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



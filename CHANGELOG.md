
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



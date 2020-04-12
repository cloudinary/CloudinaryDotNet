using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class ExplicitMethodsTest : IntegrationTestBase
    {
        private readonly string _cloudinaryPublicId = "cloudinary";
        private readonly string _storageTypeFacebook = StorageType.facebook.ToString();

        [Test]
        public void TestExplicit()
        {
            var explicitParams = PopulateExplicitParams();

            var expResult = m_cloudinary.Explicit(explicitParams);

            AssertExplicitAbsoluteUri(expResult);
        }

        [Test]
        public async Task TestExplicitAsync()
        {
            var explicitParams = PopulateExplicitParams();

            var expResult = await m_cloudinary.ExplicitAsync(explicitParams);

            AssertExplicitAbsoluteUri(expResult);
        }

        private ExplicitParams PopulateExplicitParams()
        {
            return new ExplicitParams(_cloudinaryPublicId)
            {
                EagerTransforms = new List<Transformation>() { m_explicitTransformation },
                Type = _storageTypeFacebook,
                Tags = m_apiTag
            };
        }

        private void AssertExplicitAbsoluteUri(ExplicitResult result)
        {
            var url = new Url(m_account.Cloud)
                .ResourceType(ApiShared.GetCloudinaryParam(ResourceType.Image))
                .Add(_storageTypeFacebook)
                .Transform(m_explicitTransformation)
                .Format(FILE_FORMAT_PNG)
                .Version(result.Version)
                .BuildUrl(_cloudinaryPublicId);

            Assert.AreEqual(url, result.Eager[0].Uri.AbsoluteUri);
        }

        [Test]
        public void TestExplicitContext()
        {
            var facebook = StorageType.facebook.ToString();

            var exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { m_explicitTransformation },
                Type = facebook,
                Context = new StringDictionary("context1=254"),
                Tags = m_apiTag
            };

            var expResult = m_cloudinary.Explicit(exp);

            Assert.IsNotNull(expResult);

            var getResult = m_cloudinary.GetResource(new GetResourceParams(expResult.PublicId) { Type = facebook });

            Assert.IsNotNull(getResult);
            Assert.AreEqual("254", getResult.Context["custom"]["context1"].ToString());
        }

        [Test]
        public void TestExplicitAsyncProcessing()
        {
            var publicId = GetUniquePublicId();
            var facebook = StorageType.facebook.ToString();

            var exp = new ExplicitParams(publicId)
            {
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                Type = facebook,
                Async = true,
            };

            var expAsyncResult = m_cloudinary.Explicit(exp);

            Assert.AreEqual("pending", expAsyncResult.Status);
            Assert.AreEqual(Api.GetCloudinaryParam(ResourceType.Image), expAsyncResult.ResourceType);
            Assert.AreEqual(facebook, expAsyncResult.Type);
            Assert.AreEqual(publicId, expAsyncResult.PublicId);
        }

        [Test]
        public void TestExplicitVideo()
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(m_testVideoPath),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            var exp = new ExplicitParams(uploadResult.PublicId)
            {
                Type = "upload",
                ResourceType = ResourceType.Video,
                Context = new StringDictionary("context1=254")
            };

            var expResult = m_cloudinary.Explicit(exp);

            Assert.IsNotNull(expResult);

            var getResult = m_cloudinary.GetResource(new GetResourceParams(expResult.PublicId) { ResourceType = ResourceType.Video });

            Assert.IsNotNull(getResult);
            Assert.AreEqual("254", getResult.Context["custom"]["context1"].ToString());
        }

        [Test]
        public void TestFaceCoordinates()
        {
            //should allow sending face coordinates

            var faceCoordinates = new List<CloudinaryDotNet.Core.Rectangle>()
            {
                new CloudinaryDotNet.Core.Rectangle(121,31,110,151),
                new CloudinaryDotNet.Core.Rectangle(120,30,109,150)
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                FaceCoordinates = faceCoordinates,
                Faces = true,
                Tags = m_apiTag
            };

            var uploadRes = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(uploadRes.Faces);
            Assert.AreEqual(2, uploadRes.Faces.Length);
            Assert.AreEqual(4, uploadRes.Faces[0].Length);
            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(faceCoordinates[i].X, uploadRes.Faces[i][0]);
                Assert.AreEqual(faceCoordinates[i].Y, uploadRes.Faces[i][1]);
                Assert.AreEqual(faceCoordinates[i].Width, uploadRes.Faces[i][2]);
                Assert.AreEqual(faceCoordinates[i].Height, uploadRes.Faces[i][3]);
            }

            var explicitParams = new ExplicitParams(uploadRes.PublicId)
            {
                FaceCoordinates = "122,32,111,152",
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            };

            m_cloudinary.Explicit(explicitParams);

            var res = m_cloudinary.GetResource(
                new GetResourceParams(uploadRes.PublicId) { Faces = true });

            Assert.NotNull(res.Faces);
            Assert.AreEqual(1, res.Faces.Length);
            Assert.AreEqual(4, res.Faces[0].Length);
            Assert.AreEqual(122, res.Faces[0][0]);
            Assert.AreEqual(32, res.Faces[0][1]);
            Assert.AreEqual(111, res.Faces[0][2]);
            Assert.AreEqual(152, res.Faces[0][3]);
        }

        [Test]
        public void TestQualityAnalysis()
        {
            //should return quality analysis information
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                QualityAnalysis = true,
                Tags = m_apiTag
            };

            var uploadRes = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(uploadRes.QualityAnalysis);
            Assert.IsInstanceOf<double>(uploadRes.QualityAnalysis.Focus);

            var explicitParams = new ExplicitParams(uploadRes.PublicId)
            {
                QualityAnalysis = true,
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            };

            var explicitResult = m_cloudinary.Explicit(explicitParams);

            Assert.NotNull(explicitResult.QualityAnalysis);
            Assert.IsInstanceOf<double>(explicitResult.QualityAnalysis.Focus);

            var res = m_cloudinary.GetResource(new GetResourceParams(uploadRes.PublicId) { QualityAnalysis = true });

            Assert.NotNull(res.QualityAnalysis);
            Assert.IsInstanceOf<double>(res.QualityAnalysis.Focus);
        }

        [Test]
        public void TestJsonObject()
        {
            var exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Type = StorageType.facebook.ToString(),
                Tags = m_apiTag
            };

            var result = m_cloudinary.Explicit(exp);
            AddCreatedPublicId(StorageType.facebook, result.PublicId);

            Assert.NotNull(result.JsonObj);
            Assert.AreEqual(result.PublicId, result.JsonObj["public_id"].ToString());
        }

        [Test]
        public void TestResourceDerivedNextCursor()
        {
            var eagerTransforms = new List<Transformation> { m_simpleTransformation, m_resizeTransformation };
            var eagerTransformStrings = new List<string> {
                m_simpleTransformationAsString,
                m_resizeTransformationAsString };

            var upResult = m_cloudinary.Upload(new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                EagerTransforms = eagerTransforms
            });

            var result = m_cloudinary.GetResource(new GetResourceParams(upResult.PublicId) { MaxResults = 1 });

            Assert.NotNull(result.DerivedNextCursor);
            CollectionAssert.Contains(eagerTransformStrings, result.Derived[0].Transformation);

            var derivedResult = m_cloudinary.GetResource(
            new GetResourceParams(upResult.PublicId) { DerivedNextCursor = result.DerivedNextCursor });

            Assert.IsNull(derivedResult.DerivedNextCursor);
            CollectionAssert.Contains(eagerTransformStrings, derivedResult.Derived[0].Transformation);

            Assert.AreNotEqual(result.Derived[0].Transformation, derivedResult.Derived[0].Transformation);
        }

        [Test]
        public void TestCustomCoordinates()
        {
            //should allow sending custom coordinates

            var coordinates = new CloudinaryDotNet.Core.Rectangle(121, 31, 110, 151);

            var upResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                CustomCoordinates = coordinates,
                Tags = m_apiTag
            });

            var result = m_cloudinary.GetResource(new GetResourceParams(upResult.PublicId) { Coordinates = true });

            Assert.NotNull(result.Coordinates);
            Assert.NotNull(result.Coordinates.Custom);
            Assert.AreEqual(1, result.Coordinates.Custom.Length);
            Assert.AreEqual(4, result.Coordinates.Custom[0].Length);
            Assert.AreEqual(coordinates.X, result.Coordinates.Custom[0][0]);
            Assert.AreEqual(coordinates.Y, result.Coordinates.Custom[0][1]);
            Assert.AreEqual(coordinates.Width, result.Coordinates.Custom[0][2]);
            Assert.AreEqual(coordinates.Height, result.Coordinates.Custom[0][3]);

            coordinates = new CloudinaryDotNet.Core.Rectangle(122, 32, 110, 152);

            var exResult = m_cloudinary.Explicit(new ExplicitParams(upResult.PublicId)
            {
                CustomCoordinates = coordinates,
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            });

            result = m_cloudinary.GetResource(new GetResourceParams(upResult.PublicId) { Coordinates = true });

            Assert.NotNull(result.Coordinates);
            Assert.NotNull(result.Coordinates.Custom);
            Assert.AreEqual(1, result.Coordinates.Custom.Length);
            Assert.AreEqual(4, result.Coordinates.Custom[0].Length);
            Assert.AreEqual(coordinates.X, result.Coordinates.Custom[0][0]);
            Assert.AreEqual(coordinates.Y, result.Coordinates.Custom[0][1]);
            Assert.AreEqual(coordinates.Width, result.Coordinates.Custom[0][2]);
            Assert.AreEqual(coordinates.Height, result.Coordinates.Custom[0][3]);
        }

        [Test]
        public void TestResponsiveBreakpoints()
        {
            var publicId = GetUniquePublicId();
            var breakpoint = new ResponsiveBreakpoint().MaxImages(5).BytesStep(20)
                                .MinWidth(200).MaxWidth(1000).CreateDerived(false);

            var breakpoint2 = new ResponsiveBreakpoint().Transformation(m_simpleTransformation).MaxImages(4)
                                .BytesStep(20).MinWidth(100).MaxWidth(900).CreateDerived(false);

            // An array of breakpoints
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag,
                ResponsiveBreakpoints = new List<ResponsiveBreakpoint> { breakpoint, breakpoint2 }
            };
            var result = m_cloudinary.Upload(uploadParams);
            Assert.Null(result.Error);
            Assert.NotNull(result.ResponsiveBreakpoints, "result should include 'ResponsiveBreakpoints'");
            Assert.AreEqual(2, result.ResponsiveBreakpoints.Count);

            Assert.AreEqual(5, result.ResponsiveBreakpoints[0].Breakpoints.Count);
            Assert.AreEqual(1000, result.ResponsiveBreakpoints[0].Breakpoints[0].Width);
            Assert.AreEqual(200, result.ResponsiveBreakpoints[0].Breakpoints[4].Width);

            Assert.AreEqual(4, result.ResponsiveBreakpoints[1].Breakpoints.Count);
            Assert.AreEqual(900, result.ResponsiveBreakpoints[1].Breakpoints[0].Width);
            Assert.AreEqual(100, result.ResponsiveBreakpoints[1].Breakpoints[3].Width);

            // responsive breakpoints for Explicit()
            var exp = new ExplicitParams(publicId)
            {
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag,
                ResponsiveBreakpoints = new List<ResponsiveBreakpoint> { breakpoint2.CreateDerived(true) }
            };

            ExplicitResult expResult = m_cloudinary.Explicit(exp);

            Assert.AreEqual(1, expResult.ResponsiveBreakpoints.Count);
            Assert.AreEqual(4, expResult.ResponsiveBreakpoints[0].Breakpoints.Count);
            Assert.AreEqual(900, expResult.ResponsiveBreakpoints[0].Breakpoints[0].Width);
            Assert.AreEqual(100, expResult.ResponsiveBreakpoints[0].Breakpoints[3].Width);
        }
        
        [Test]
        public void TestMetadata()
        {
            var uploadResult = m_cloudinary.Upload(new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
            });

            var metadataLabel = GetUniqueMetadataFieldLabel("resource_update");
            var metadataParameters = new StringMetadataFieldCreateParams(metadataLabel);
            var metadataResult = m_cloudinary.AddMetadataField(metadataParameters);

            Assert.NotNull(metadataResult);

            var metadataFieldId = metadataResult.ExternalId;
            if (!string.IsNullOrEmpty(metadataFieldId))
                m_metadataFieldsToClear.Add(metadataFieldId);

            const string metadataValue = "test value";
            var metadata = new StringDictionary
            {
                {metadataFieldId, metadataValue}
            };

            var explicitResult = m_cloudinary.Explicit(new ExplicitParams(uploadResult.PublicId)
            {
                Metadata = metadata,
                Type = STORAGE_TYPE_UPLOAD
            });

            Assert.NotNull(explicitResult);
            Assert.AreEqual(HttpStatusCode.OK, explicitResult.StatusCode);

            var getResult = m_cloudinary.GetResource(new GetResourceParams(uploadResult.PublicId));

            Assert.IsNotNull(getResult);
            Assert.NotNull(getResult.MetadataFields);
        }

        [Test]
        public void TestEager()
        {
            var publicId = GetUniquePublicId();

            // An array of breakpoints
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag
            };
            m_cloudinary.Upload(uploadParams);

            // responsive breakpoints for Explicit()
            var exp = new ExplicitParams(publicId)
            {
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            };

            ExplicitResult expResult = m_cloudinary.Explicit(exp);

            Assert.NotZero(expResult.Eager.Length);
            Assert.NotNull(expResult.Eager[0]);
            Assert.AreEqual(expResult.Eager[0].SecureUri, expResult.Eager[0].SecureUrl);
            Assert.AreEqual(expResult.Eager[0].Uri, expResult.Eager[0].Url);
            Assert.NotZero(expResult.Eager[0].Width);
            Assert.NotZero(expResult.Eager[0].Height);
            Assert.NotNull(expResult.Eager[0].Format);
            Assert.NotZero(expResult.Eager[0].Bytes);
            Assert.NotNull(expResult.Eager[0].Transformation);
        }

        [Test]
        public void TestExplicitOptionalParameters()
        {
            var explicitResult = ArrangeAndGetExplicitResult();

            Assert.NotZero(explicitResult.Colors.Length);
            Assert.NotZero(explicitResult.ImageMetadata.Count);
            Assert.NotNull(explicitResult.Phash);
            Assert.NotZero(explicitResult.Faces.Length);
            Assert.Zero(explicitResult.CinemagraphAnalysis.CinemagraphScore);
            Assert.NotZero(explicitResult.Moderation.Count);
            Assert.NotNull(explicitResult.AccessMode);
            Assert.NotNull(explicitResult.Etag);
            Assert.NotNull(explicitResult.Placeholder);
            Assert.NotNull(explicitResult.OriginalFilename);
            Assert.NotZero(explicitResult.Width);
            Assert.NotZero(explicitResult.Height);
            Assert.NotNull(explicitResult.OriginalFilename);
            Assert.NotNull(explicitResult.SlotToken);
            Assert.NotZero(explicitResult.Pages);
            Assert.Zero(explicitResult.IllustrationScore);
            Assert.AreEqual(explicitResult.Length, explicitResult.Bytes);
            Assert.AreEqual(explicitResult.SecureUri, explicitResult.SecureUrl);
            Assert.AreEqual(explicitResult.Uri, explicitResult.Url);
            Assert.IsNotNull(explicitResult.Predominant);
            Assert.NotZero(explicitResult.Predominant.Google.Length);
            Assert.NotZero(explicitResult.Predominant.Cloudinary.Length);
        }

        [Test]
        public void TestProfilingData()
        {
            var explicitResult = ArrangeAndGetExplicitResult();

            Assert.NotNull(explicitResult.ProfilingData);
            Assert.NotZero(explicitResult.ProfilingData.Length);
            Assert.NotZero(explicitResult.ProfilingData[0].Action.Postsize.Length);
            Assert.NotZero(explicitResult.ProfilingData[0].Action.Presize.Length);
        }

        [Test]
        public void TestPredominant()
        {
            var explicitResult = ArrangeAndGetExplicitResult();

            Assert.IsNotNull(explicitResult.Predominant);
            Assert.NotZero(explicitResult.Predominant.Google.Length);
            Assert.NotZero(explicitResult.Predominant.Cloudinary.Length);
        }

        private ExplicitResult ArrangeAndGetExplicitResult()
        {
            //should return quality analysis information
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                QualityAnalysis = true,
                Tags = m_apiTag
            };

            var uploadRes = m_cloudinary.Upload(uploadParams);

            var explicitParams = new ExplicitParams(uploadRes.PublicId)
            {
                Overwrite = true,
                ImageMetadata = true,
                Colors = true,
                Phash = true,
                Faces = true,
                CinemagraphAnalysis = true,
                QualityOverride = "auto:best",
                Moderation = "webpurify",
                FaceCoordinates = "122,32,111,152",
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            };

            return m_cloudinary.Explicit(explicitParams);
        }
    }
}

using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.UploadApi
{
    public class CreateSlideshowMethodsTest
    {
        private readonly MockedCloudinary _cloudinary = new MockedCloudinary();

        [Test]
        public void TestCreateSlideshowFromManifestTransformation()
        {
            const string slideshowManifest = "w_352;h_240;du_5;fps_30;vars_(slides_((media_s64:aHR0cHM6Ly9y" +
                                              "ZXMuY2xvdWRpbmFyeS5jb20vZGVtby9pbWFnZS91cGxvYWQvY291cGxl);(media_s64:aH" +
                                              "R0cHM6Ly9yZXMuY2xvdWRpbmFyeS5jb20vZGVtby9pbWFnZS91cGxvYWQvc2FtcGxl)))";

            var csParams = new CreateSlideshowParams
            {
                ManifestTransformation = new Transformation().CustomFunction(CustomFunction.Render(slideshowManifest)),
                Tags = new List<string> {"tag1", "tag2", "tag3"},
                Transformation = new Transformation().FetchFormat("auto").Quality("auto")
            };

            _cloudinary.CreateSlideshow(csParams);

            _cloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "video/create_slideshow");

            foreach (var expected in new List<string>
            {
                $"fn_render:{slideshowManifest}",
                "tag1",
                "tag2",
                "tag3",
                "f_auto,q_auto"
            })
            {
                StringAssert.Contains(expected, _cloudinary.HttpRequestContent);
            }
        }
        [Test]
        public void TestCreateSlideshowFromManifestJson()
        {
            const string expectedManifestJson =
                @"{""w"":848,""h"":480,""du"":6,""fps"":30,""vars"":{""sdur"":500,""tdur"":500,""slides"":"+
                @"[{""media"":""i:protests9""},{""media"":""i:protests8""},{""media"":""i:protests7""},"+
                @"{""media"":""i:protests6""},{""media"":""i:protests2""},{""media"":""i:protests1""}]}}";

            const string notificationUrl = "https://example.com";
            const string uploadPreset = "test_preset";
            const string testId = "test_id";


            var csParams = new CreateSlideshowParams
            {
                ManifestJson = new SlideshowManifest
                {
                    Width = 848,
                    Height = 480,
                    Duration = 6,
                    Fps = 30,
                    Variables = new Slideshow
                    {
                        SlideDuration = 500,
                        TransitionDuration = 500,
                        Slides = new List<Slide>
                        {
                            new Slide("i:protests9"), new Slide("i:protests8"), new Slide("i:protests7"),
                            new Slide("i:protests6"), new Slide("i:protests2"), new Slide("i:protests1")
                        }
                    }
                },
                PublicId = testId,
                NotificationUrl = notificationUrl,
                UploadPreset = uploadPreset,
                Overwrite = true
            };

            _cloudinary.CreateSlideshow(csParams);

            foreach (var expected in new List<string>
            {
                expectedManifestJson,
                testId,
                notificationUrl,
                uploadPreset,
                "1" // Overwrite
            })
            {
                StringAssert.Contains(expected, _cloudinary.HttpRequestContent);
            }
        }
    }
}

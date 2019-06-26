using NUnit.Framework;

namespace CloudinaryDotNet.Test.Transforms
{
    [TestFixture]
    public class GravityTest
    {
        #region OcrGravityTestCases
        [TestCase(Gravity.Center, "g_center")]
        [TestCase(Gravity.NorthWest, "g_north_west")]
        [TestCase(Gravity.North, "g_north")]
        [TestCase(Gravity.NorthEast, "g_north_east")]
        [TestCase(Gravity.West, "g_west")]
        [TestCase(Gravity.East, "g_east")]
        [TestCase(Gravity.SouthWest, "g_south_west")]
        [TestCase(Gravity.South, "g_south")]
        [TestCase(Gravity.SouthEast, "g_south_east")]
        [TestCase(Gravity.Auto, "g_auto")]
        [TestCase(Gravity.XYCenter, "g_xy_center")]
        [TestCase(Gravity.Face, "g_face")]
        [TestCase(Gravity.FaceCenter, "g_face:center")]
        [TestCase(Gravity.FaceAuto, "g_face:auto")]
        [TestCase(Gravity.Faces, "g_faces")]
        [TestCase(Gravity.FacesCenter, "g_faces:center")]
        [TestCase(Gravity.FacesAuto, "g_faces:auto")]
        [TestCase(Gravity.Body, "g_body")]
        [TestCase(Gravity.BodyFace, "g_body:face")]
        [TestCase(Gravity.Liquid, "g_liquid")]
        [TestCase(Gravity.OcrText, "g_ocr_text")]
        [TestCase(Gravity.AdvFace, "g_adv_face")]
        [TestCase(Gravity.AdvFaces, "g_adv_faces")]
        [TestCase(Gravity.AdvEyes, "g_adv_eyes")]
        [TestCase(Gravity.Custom, "g_custom")]
        [TestCase(Gravity.CustomFace, "g_custom:face")]
        [TestCase(Gravity.CustomFaces, "g_custom:faces")]
        [TestCase(Gravity.CustomAdvFace, "g_custom:adv_face")]
        [TestCase(Gravity.CustomAdvFaces, "g_custom:adv_faces")]
        #endregion
        public void TestOcrGravityTransformation(string actual, string expected)
        {
            Assert.AreEqual(expected, new Transformation().Gravity(actual).ToString());
        }

        #region OcrGravityTestCasesWithParams
        [TestCase(Gravity.OcrText, "adv_ocr", "g_ocr_text:adv_ocr")]
        [TestCase(Gravity.Auto, "good", "g_auto:good")]
        [TestCase(Gravity.Auto, "ocr_text", "g_auto:ocr_text")]
        #endregion
        public void TestOcrGravityTransformationWithParams(string actual, string param, string expected)
        {
            Assert.AreEqual(expected, new Transformation().Gravity(actual, param).ToString());
        }
    }
}

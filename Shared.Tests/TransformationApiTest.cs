using NUnit.Framework;
using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public partial class ApiTest
    {
        [Test]
        public void WithLiteral()
        {
            Transformation transformation = new Transformation().IfCondition("w_lt_200").Crop("fill").Height(120).Width(80);
            string sTransform = transformation.ToString();
            Assert.AreEqual(sTransform.IndexOf("if"), 0, "should include the if parameter as the first component in the transformation string");
            Assert.AreEqual("if_w_lt_200,c_fill,h_120,w_80", sTransform, "should be proper transformation string");

            transformation = new Transformation().Crop("fill").Height(120).IfCondition("w_lt_200").Width(80);
            sTransform = transformation.ToString();
            Assert.AreEqual(sTransform.IndexOf("if"), 0, "should include the if parameter as the first component in the transformation string");
            Assert.AreEqual("if_w_lt_200,c_fill,h_120,w_80", sTransform, "components should be in proper order");

            transformation = new Transformation().IfCondition("w_lt_200").Crop("fill").Height(120).Width(80).
                                          Chain().IfCondition("w_gt_400").Crop("fit").Height(150).Width(150).
                                          Chain().Effect("sepia");
            sTransform = transformation.ToString();
            Assert.AreEqual("if_w_lt_200,c_fill,h_120,w_80/if_w_gt_400,c_fit,h_150,w_150/e_sepia", sTransform, "should allow multiple conditions when chaining transformations");
        }

        [Test]
        public void LiteralWithSpaces()
        {
            Transformation transformation = new Transformation().IfCondition("w < 200").Crop("fill").Height(120).Width(80);
            string sTransform = transformation.ToString();
            Assert.AreEqual("if_w_lt_200,c_fill,h_120,w_80", sTransform, "should translate operators");
        }

        [Test]
        public void EndIf()
        {
            Transformation transformation = new Transformation().IfCondition("w_lt_200").Crop("fill").Height(120).Width(80).Effect("sharpen")
                .Chain().Effect("brightness", 50)
                .Chain().Effect("shadow").Color("red")
                .EndIf();
            string sTransform = transformation.ToString();
            Assert.IsTrue(sTransform.EndsWith("if_end"), "should include the if_end as the last parameter in its component");
            Assert.AreEqual("if_w_lt_200/c_fill,e_sharpen,h_120,w_80/e_brightness:50/co_red,e_shadow/if_end", sTransform, "should be proper transformation string");
        }

        [Test]
        public void IfElse()
        {
            List<Transformation> transformations = new List<Transformation>()
            {
                new Transformation().IfCondition("w_lt_200").Crop("fill").Height(120).Width(80),
                new Transformation().IfElse().Crop("fill").Height(90).Width(100)
            };
            var transformation = new Transformation(transformations);
            var sTransform = transformation.ToString();
            Assert.AreEqual("if_w_lt_200,c_fill,h_120,w_80/if_else,c_fill,h_90,w_100", sTransform, "should support if_else with transformation parameters");

            transformations = new List<Transformation>()
            {
                new Transformation().IfCondition("w_lt_200"),
                new Transformation().Crop("fill").Height(120).Width(80),
                new Transformation().IfElse(),
                new Transformation().Crop("fill").Height(90).Width(100)
            };
            transformation = new Transformation(transformations);
            sTransform = transformation.ToString();
            Assert.IsTrue(sTransform.Contains("/if_else/"), "if_else should be without any transformation parameters");
            Assert.AreEqual("if_w_lt_200/c_fill,h_120,w_80/if_else/c_fill,h_90,w_100", sTransform, "should be proper transformation string");
        }

        [Test]
        public void ChainedConditions()
        {
            Transformation transformation = new Transformation().IfCondition().AspectRatio("gt", "3:4").Then().Width(100).Crop("scale");
            Assert.AreEqual("if_ar_gt_3:4,c_scale,w_100", transformation.ToString(), "passing an operator and a value adds a condition");

            transformation = new Transformation().IfCondition().AspectRatio("gt", "3:4").And().Width("gt", 100).Then().Width(50).Crop("scale");
            Assert.AreEqual("if_ar_gt_3:4_and_w_gt_100,c_scale,w_50", transformation.ToString(), "should chaining condition with `and`");

            transformation = new Transformation().IfCondition().AspectRatio("gt", "3:4").And().Width("gt", 100).Or().Width("gt", 200).Then().Width(50).Crop("scale");
            Assert.AreEqual("if_ar_gt_3:4_and_w_gt_100_or_w_gt_200,c_scale,w_50", transformation.ToString(), "should chain conditions with `or`");

            transformation = new Transformation().IfCondition().AspectRatio(">", "3:4").And().Width("<=", 100).Or().Width("gt", 200).Then().Width(50).Crop("scale");
            Assert.AreEqual("if_ar_gt_3:4_and_w_lte_100_or_w_gt_200,c_scale,w_50", transformation.ToString(), "should translate operators");

            transformation = new Transformation().IfCondition().AspectRatio(">", "3:4").And().Width("<=", 100).Or().Width(">", 200).Then().Width(50).Crop("scale");
            Assert.AreEqual("if_ar_gt_3:4_and_w_lte_100_or_w_gt_200,c_scale,w_50", transformation.ToString(), "should translate operators");

            transformation = new Transformation().IfCondition().AspectRatio(">=", "3:4").And().PageCount(">=", 100).Or().PageCount("!=", 0).Then().Width(50).Crop("scale");
            Assert.AreEqual("if_ar_gte_3:4_and_pc_gte_100_or_pc_ne_0,c_scale,w_50", transformation.ToString(), "should translate operators");

            transformation = new Transformation().IfCondition().AspectRatio("gt", "3:4").And().InitialHeight(">", 100).And().InitialWidth("<", 500).Then().Width(100).Crop("scale");
            Assert.AreEqual("if_ar_gt_3:4_and_ih_gt_100_and_iw_lt_500,c_scale,w_100", transformation.ToString(), "passing an operator and a value adds a condition");
        }

        [Test]
        public void ShouldSupportAndTranslateOperators()
        {
            string allOperators =
                            "if_" +
                            "w_eq_0_and" +
                            "_h_ne_0_or" +
                            "_ar_lt_0_and" +
                            "_pc_gt_0_and" +
                            "_fc_lte_0_and" +
                            "_w_gte_0" +
                            ",e_grayscale";
            Assert.AreEqual(allOperators, new Transformation().IfCondition()
                            .Width("=", 0).And()
                            .Height("!=", 0).Or()
                            .AspectRatio("<", "0").And()
                            .PageCount(">", 0).And()
                            .FaceCount("<=", 0).And()
                            .Width(">=", 0)
                            .Then().Effect("grayscale").ToString(), "should support and translate operators:  '=', '!=', '<', '>', '<=', '>=', '&&', '||'");

            Assert.AreEqual(allOperators, new Transformation().IfCondition("w = 0 && height != 0 || aspectRatio < 0 and pageCount > 0 and faceCount <= 0 and width >= 0")
                        .Effect("grayscale")
                        .ToString());
        }

        [Test]
        public void EndIf2()
        {
            Transformation transformation = new Transformation().IfCondition().Width("gt", 100).And().Width("lt", 200).Then().Width(50).Crop("scale").EndIf();
            Assert.AreEqual("if_w_gt_100_and_w_lt_200/c_scale,w_50/if_end", transformation.ToString(), "should serialize to 'if_end'");

            transformation = new Transformation().IfCondition().Width("gt", 100).And().Width("lt", 200).Then().Width(50).Crop("scale").EndIf();
            Assert.AreEqual("if_w_gt_100_and_w_lt_200/c_scale,w_50/if_end", transformation.ToString(), "force the if clause to be chained");

            transformation = new Transformation().IfCondition().Width("gt", 100).And().Width("lt", 200).Then().Width(50).Crop("scale").IfElse().Width(100).Crop("crop").EndIf();
            Assert.AreEqual("if_w_gt_100_and_w_lt_200/c_scale,w_50/if_else/c_crop,w_100/if_end", transformation.ToString(), "force the if_else clause to be chained");
        }


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

        [Test]
        public void TestBlurPixelateEffectsTransformation()
        {
            var transformation = new Transformation().Effect("blur_region");
            Assert.AreEqual("e_blur_region", transformation.ToString());
            
            transformation = new Transformation().Effect("pixelate_region");
            Assert.AreEqual("e_pixelate_region", transformation.ToString());
        }
        // User-defined variables

        [Test]
        public void TestArrayShouldDefineASetOfVariables()
        {
            // using methods
            var t = new Transformation();
            t.IfCondition("face_count > 2")
                    .Variables(Expression.Variable("$z", 5), Expression.Variable("$foo", "$z * 2"))
                    .Crop("scale")
                    .Width("$foo * 200");
            Assert.AreEqual("if_fc_gt_2,$z_5,$foo_$z_mul_2,c_scale,w_$foo_mul_200", t.ToString());
        }

        [Test]
        public void TestShouldSortDefinedVariable()
        {
            var t = new Transformation().Variable("$second", 1).Variable("$first", 2);
            Assert.AreEqual("$first_2,$second_1", t.ToString());
        }

        [Test]
        public void TestShouldPlaceDefinedVariablesBeforeOrdered()
        {
            var t = new Transformation()
                    .Variables(Expression.Variable("$z", 5), Expression.Variable("$foo", "$z * 2"))
                    .Variable("$second", 1)
                    .Variable("$first", 2);
            Assert.AreEqual("$first_2,$second_1,$z_5,$foo_$z_mul_2", t.ToString());
        }

        [Test]
        public void TestVariable()
        {
            // using strings
            var t = new Transformation()
                    .Variable("$foo", 10)
                    .Chain()
                    .IfCondition(Expression.FaceCount().Gt(2))
                    .Crop("scale")
                    .Width(new Condition("$foo * 200 / faceCount"))
                    .IfElse()
                    .Width(Expression.InitialHeight().Mul(2))
                    .EndIf();
            Assert.AreEqual("$foo_10/if_fc_gt_2/c_scale,w_$foo_mul_200_div_fc/if_else/w_ih_mul_2/if_end", t.ToString());
        }

        [Test]
        public void TestShouldSupportTextVariableValues()
        {
            var t = new Transformation()
                .Effect("$efname", 100)
                .Variable("$efname", "!blur!");

            Assert.AreEqual("$efname_!blur!,e_$efname:100", t.ToString());
        }

        [Test]
        public void TestSupportStringInterpolation()
        {
            Transformation t = new Transformation()
                    .Crop("scale")
                    .Overlay(new TextLayer().Text("$(start)Hello $(name)$(ext), $(no ) $( no)$(end)")
                            .FontFamily("Arial")
                            .FontSize(18));

            StringAssert.AreEqualIgnoringCase("c_scale,l_text:Arial_18:$(start)Hello%20$(name)$(ext)%252C%20%24%28no%20%29%20%24%28%20no%29$(end)", t.ToString());
        }

        [Test]
        public void TestExpressionOperators()
        {
            const string transformationStr = "$foo_10,$foostr_!my:str:ing!/if_fc_gt_2_and" +
                                    "_pc_lt_300_or" +
                                    "_!myTag1!_in_tags_and" +
                                    "_!myTag2!_nin_tags_and" +
                                    "_w_gte_200_and" +
                                    "_h_eq_$foo_and" +
                                    "_w_ne_$foo_mul_2_and" +
                                    "_h_lt_$foo_or" +
                                    "_w_lte_500_and" +
                                    "_ils_lt_0_and" +
                                    "_cp_eq_10_and" +
                                    "_px_lt_300_and" +
                                    "_py_lt_300_and" +
                                    "_py_ne_400_and" +
                                    "_ar_gt_3:4_and" +
                                    "_iar_gt_3:4_and" +
                                    "_h_lt_iw_div_2_add_1_and" +
                                    "_w_lt_ih_sub_$foo" +
                                    "/c_scale,l_$foostr,w_$foo_mul_200_div_fc/if_end";

            var transformation = new Transformation()
                .Variable("$foo", 10)
                .Variable("$foostr", new[] { "my", "str", "ing" })
                .Chain()
                .IfCondition(
                    Expression.FaceCount().Gt().Value(2)
                        .And().Value(Expression.PageCount().Lt().Value(300))
                        .Or().Value("!myTag1!").In().Value(Expression.Tags())
                        .And().Value("!myTag2!").Nin().Value(Expression.Tags())
                        .And().Value(Expression.Width().Gte().Value(200))
                        .And().Value(Expression.Height().Eq().Value("$foo"))
                        .And().Value(Expression.Width().Ne().Value("$foo").Mul().Value(2))
                        .And().Value(Expression.Height().Lt().Value("$foo"))
                        .Or().Value(Expression.Width().Lte().Value(500))
                        .And().Value(Expression.IllustrationScore().Lt().Value(0))
                        .And().Value(Expression.CurrentPageIndex().Eq().Value(10))
                        .And().Value(Expression.XOffset().Lt().Value(300))
                        .And().Value(Expression.YOffset().Lt().Value(300))
                        .And().Value(Expression.YOffset().Ne().Value(400))
                        .And().Value(Expression.AspectRatio().Gt().Value("3:4"))
                        .And().Value(Expression.AspectRatioOfInitialImage().Gt().Value("3:4"))
                        .And().Value(Expression.Height().Lt(Expression.InitialWidth().Div().Value(2).Add().Value(1)))
                        .And().Value(Expression.Width().Lt(Expression.InitialHeight().Sub().Value("$foo")))
                )
                .Crop("scale")
                .Width(new Condition("$foo * 200 / faceCount"))
                .Overlay("$foostr")
                .EndIf();
            Assert.AreEqual(transformationStr, transformation.ToString());
        }

        [Test]
        public void TestExpressionOperatorsWithValues()
        {
            const string transformationStr = "$foo_10,$foostr_!my:str:ing!/if_fc_gt_2_and" +
                                    "_pc_lt_300_or" +
                                    "_!myTag1!_in_tags_and" +
                                    "_!myTag2!_nin_tags_and" +
                                    "_w_gte_200_and" +
                                    "_h_eq_$foo_and" +
                                    "_w_ne_$foo_mul_2_and" +
                                    "_h_lt_$foo_or" +
                                    "_w_lte_500_and" +
                                    "_ils_lt_0_and" +
                                    "_cp_eq_10_and" +
                                    "_px_lt_300_and" +
                                    "_py_lt_300_and" +
                                    "_py_ne_400_and" +
                                    "_ar_gt_3:4_and" +
                                    "_iar_gt_3:4_and" +
                                    "_h_lt_iw_div_2_add_1_and" +
                                    "_w_lt_ih_sub_$foo" +
                                    "/c_scale,l_$foostr,w_$foo_mul_200_div_fc/if_end";
            
            var transformation = new Transformation()
                .Variable("$foo", 10)
                .Variable("$foostr", new []{"my", "str", "ing"})
                .Chain()
                .IfCondition(
                    Expression.FaceCount().Gt(2)
                        .And(Expression.PageCount().Lt(300))
                        .Or("!myTag1!").In(Expression.Tags())
                        .And("!myTag2!").Nin(Expression.Tags())
                        .And(Expression.Width().Gte(200))
                        .And(Expression.Height().Eq("$foo"))
                        .And(Expression.Width().Ne("$foo").Mul(2))
                        .And(Expression.Height().Lt("$foo"))
                        .Or(Expression.Width().Lte(500))
                        .And(Expression.IllustrationScore().Lt(0))
                        .And(Expression.CurrentPageIndex().Eq(10))
                        .And(Expression.XOffset().Lt(300))
                        .And(Expression.YOffset().Lt(300))
                        .And(Expression.YOffset().Ne(400))
                        .And(Expression.AspectRatio().Gt("3:4"))
                        .And(Expression.AspectRatioOfInitialImage().Gt("3:4"))
                        .And(Expression.Height().Lt(Expression.InitialWidth().Div(2).Add(1)))
                        .And(Expression.Width().Lt(Expression.InitialHeight().Sub("$foo")))
                )
                .Crop("scale")
                .Width(new Condition("$foo * 200 / faceCount"))
                .Overlay("$foostr")
                .EndIf();
            Assert.AreEqual(transformationStr, transformation.ToString());
        }

        [Test]
        public void TestCustomFunction()
        {
            var customFunc = new Transformation().CustomFunction(CustomFunction.Wasm("blur_wasm")).Generate();
            
            Assert.AreEqual("fn_wasm:blur_wasm", customFunc);

            customFunc = new Transformation()
                .CustomFunction(
                    CustomFunction.Remote("https://df34ra4a.execute-api.us-west-2.amazonaws.com/default/cloudinaryFunction"))
                .Generate();
            
            Assert.AreEqual(
                "fn_remote:aHR0cHM6Ly9kZjM0cmE0YS5leGVjdXRlLWFwaS51cy13ZXN0LTIuYW1hem9uYXdzLmNvbS9kZWZhdWx0L2Nsb3VkaW5hcnlGdW5jdGlvbg==",
                customFunc);
            
            customFunc = new Transformation()
                .CustomFunction(null)
                .Generate();
            Assert.AreEqual("", customFunc);
        }

        [Test]
        public void TestCustomPreFunction()
        {
            var customFunc = new Transformation().CustomPreFunction(CustomFunction.Wasm("blur_wasm")).Generate();

            Assert.AreEqual("fn_pre:wasm:blur_wasm", customFunc);

            customFunc = new Transformation()
                .CustomPreFunction(
                    CustomFunction.Remote("https://df34ra4a.execute-api.us-west-2.amazonaws.com/default/cloudinaryFunction"))
                .Generate();

            Assert.AreEqual(
                "fn_pre:remote:aHR0cHM6Ly9kZjM0cmE0YS5leGVjdXRlLWFwaS51cy13ZXN0LTIuYW1hem9uYXdzLmNvbS9kZWZhdWx0L2Nsb3VkaW5hcnlGdW5jdGlvbg==",
                customFunc);
                        
            customFunc = new Transformation()
                .CustomPreFunction(null)
                .Generate();
            Assert.AreEqual("", customFunc);
        }
        
        [Test]
        public void TestCloneWithNested()
        {
            // transformation should be cloneable, including nested transformations
            Transformation transformation = new Transformation().X(100).Y(100).Width(200)
                .Crop("fill").Chain().Radius(10).Chain().Crop("crop").Width(100).Angle("12", "13", "14");

            Transformation clone = transformation.Clone();
            transformation.NestedTransforms[0].Width(300);
            transformation = transformation.Angle("22", "23").Chain().Crop("fill");

            Assert.AreEqual(transformation.Generate(), 
                "c_fill,w_300,x_100,y_100/r_10/a_22.23,c_crop,w_100/c_fill");
            Assert.AreEqual(clone.Generate(), 
                "c_fill,w_200,x_100,y_100/r_10/a_12.13.14,c_crop,w_100");
        }
        
        [Test]
        public void TestDictionaryParamsDeepClone()
        {
            // dictionary params should be cloned
            var codecParams = new Dictionary<string, string>
            {
                {"codec", VideoCodec.H264}, {"profile", "basic"}, {"level", "3.1"}
            };
            Transformation transform = new Transformation().VideoCodec(codecParams);
            
            Transformation clone = transform.Clone();
            codecParams["codec"] = VideoCodec.H265;
            
            Assert.AreEqual(transform.Generate(), "vc_h265:basic:3.1");
            Assert.AreEqual(clone.Generate(), "vc_h264:basic:3.1");
        }

        [TestCase(VideoCodec.Auto, "vc_auto")]
        [TestCase(VideoCodec.Vp8, "vc_vp8")]
        [TestCase(VideoCodec.Vp9, "vc_vp9")]
        [TestCase(VideoCodec.Prores, "vc_prores")]
        [TestCase(VideoCodec.H264, "vc_h264")]
        [TestCase(VideoCodec.H265, "vc_h265")]
        [TestCase(VideoCodec.Theora, "vc_theora")]
        public void TestVideoCodecsConstantValues(string actual, string expected)
        {
            Assert.AreEqual(expected, new Transformation().VideoCodec(actual).ToString());
        }

        [Test]
        public void TestTransformationLayersDeepClone()
        {
            // layers should be cloned
            var layer = new TextLayer("Hello").FontSize(10).FontFamily("Arial");
            var transformation = new Transformation().Overlay(layer);
            
            var clone = transformation.Clone();
            layer.FontSize(20);
            
            Assert.AreEqual(transformation.Generate(), "l_text:Arial_20:Hello");
            Assert.AreEqual(clone.Generate(), "l_text:Arial_10:Hello");
        }

        [Test]
        public void TestExpressionsClone()
        {
            const string transformationStr = "if_pc_lt_300/c_scale/if_end";
            var expression = Expression.PageCount().Lt(300);
            var transformation = new Transformation()
                .IfCondition(expression)
                .Crop("scale")
                .EndIf();

            var clone = transformation.Clone();
            expression.Gt(2);

            Assert.AreEqual(transformationStr, clone.ToString());
        }

        [Test]
        public void TestRadiusTransformation()
        {
            var radiusTestValues = new Dictionary<Transformation, string>
            {
                {new Transformation().Radius(10), "r_10"},
                {new Transformation().Radius("10:20"), "r_10:20"},
                {new Transformation().Radius(new List<object> {10, 20, "$v"}), "r_10:20:$v"},
                {new Transformation().Radius(new Radius(10)), "r_10"},
                {new Transformation().Radius(null), ""}
            };

            foreach (var test in radiusTestValues)
                Assert.AreEqual(test.Value, test.Key.ToString());
        }
    }
}

using System.Collections.Generic;
using NUnit.Framework;

namespace CloudinaryDotNet.Test.Transforms
{
    [TestFixture]
    public class ExpressionTest
    {
        [Test]
        public void WithLiteral()
        {
            var transformation = new Transformation().IfCondition("w_lt_200").Crop("fill").Height(120).Width(80);
            var sTransform = transformation.ToString();
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
            var transformation = new Transformation().IfCondition("w < 200").Crop("fill").Height(120).Width(80);
            var sTransform = transformation.ToString();
            Assert.AreEqual("if_w_lt_200,c_fill,h_120,w_80", sTransform, "should translate operators");
        }

        [Test]
        public void EndIf()
        {
            var transformation = new Transformation().IfCondition("w_lt_200").Crop("fill").Height(120).Width(80).Effect("sharpen")
                .Chain().Effect("brightness", 50)
                .Chain().Effect("shadow").Color("red")
                .EndIf();
            var sTransform = transformation.ToString();
            Assert.IsTrue(sTransform.EndsWith("if_end"), "should include the if_end as the last parameter in its component");
            Assert.AreEqual("if_w_lt_200/c_fill,e_sharpen,h_120,w_80/e_brightness:50/co_red,e_shadow/if_end", sTransform, "should be proper transformation string");
        }

        [Test]
        public void IfElse()
        {
            var transformations = new List<Transformation>()
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
            var transformation = new Transformation().IfCondition().AspectRatio("gt", "3:4").Then().Width(100).Crop("scale");
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
            var allOperators =
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
            var transformation = new Transformation().IfCondition().Width("gt", 100).And().Width("lt", 200).Then().Width(50).Crop("scale").EndIf();
            Assert.AreEqual("if_w_gt_100_and_w_lt_200/c_scale,w_50/if_end", transformation.ToString(), "should serialize to 'if_end'");

            transformation = new Transformation().IfCondition().Width("gt", 100).And().Width("lt", 200).Then().Width(50).Crop("scale").EndIf();
            Assert.AreEqual("if_w_gt_100_and_w_lt_200/c_scale,w_50/if_end", transformation.ToString(), "force the if clause to be chained");

            transformation = new Transformation().IfCondition().Width("gt", 100).And().Width("lt", 200).Then().Width(50).Crop("scale").IfElse().Width(100).Crop("crop").EndIf();
            Assert.AreEqual("if_w_gt_100_and_w_lt_200/c_scale,w_50/if_else/c_crop,w_100/if_end", transformation.ToString(), "force the if_else clause to be chained");
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
                .Variable("$foostr", new[] { "my", "str", "ing" })
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
    }
}

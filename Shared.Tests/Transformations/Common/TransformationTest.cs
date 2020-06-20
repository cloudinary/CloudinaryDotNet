using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace CloudinaryDotNet.Test.Transformations.Common
{
    [TestFixture]
    public class TransformationTest
    {
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
            var t = new Transformation()
                    .Crop("scale")
                    .Overlay(new TextLayer().Text("$(start)Hello $(name)$(ext), $(no ) $( no)$(end)")
                            .FontFamily("Arial")
                            .FontSize(18));

            StringAssert.AreEqualIgnoringCase("c_scale,l_text:Arial_18:$(start)Hello%20$(name)$(ext)%252C%20%24%28no%20%29%20%24%28%20no%29$(end)", t.ToString());
        }
        
        [Test]
        public void TestCloneWithNested()
        {
            // transformation should be cloneable, including nested transformations
            var transformation = new Transformation().X(100).Y(100).Width(200)
                .Crop("fill").Chain().Radius(10).Chain().Crop("crop").Width(100).Angle("12", "13", "14");

            var clone = transformation.Clone();
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
            var transform = new Transformation().VideoCodec(codecParams);
            
            var clone = transform.Clone();
            codecParams["codec"] = VideoCodec.H265;
            
            Assert.AreEqual(transform.Generate(), "vc_h265:basic:3.1");
            Assert.AreEqual(clone.Generate(), "vc_h264:basic:3.1");
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
        public void TestTextLayersUnmodifiableFields()
        {
            var textLayer = new TextLayer();
            const string testValue = "some value";

            Assert.Throws<InvalidOperationException>(() => textLayer.ResourceType(testValue));
            Assert.Throws<InvalidOperationException>(() => textLayer.Format(testValue));
            Assert.Throws<InvalidOperationException>(() => textLayer.Type(testValue));
        }

        [Test]
        public void TestVideoLayersUnmodifiableFields()
        {
            var textLayer = new VideoLayer();
            const string testValue = "some value";

            Assert.Throws<InvalidOperationException>(() => textLayer.ResourceType(testValue));
            Assert.Throws<InvalidOperationException>(() => textLayer.Format(testValue));
            Assert.Throws<InvalidOperationException>(() => textLayer.Type(testValue));
        }
    }
}

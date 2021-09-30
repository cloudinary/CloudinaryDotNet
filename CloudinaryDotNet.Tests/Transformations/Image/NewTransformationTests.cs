using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.NewTransformationApi
{
    public class Resize
    {
        private readonly List<string> content = new List<string>();

        private Resize Append(string Value)
        {
            content.Add(Value);
            return this;
        }

        public static Resize Scale() => new Resize().Append("c_scale");
        public static Resize Crop() => new Resize().Append("c_crop");
        public static Resize Fill() => new Resize().Append("c_fill");
        public static Resize Thumbnail() => new Resize().Append("c_thumb");
        

        public Resize Height(IntOrDoubleOrString v) => Append($"h_{v}");
        public Resize Width(IntOrDoubleOrString v) => Append($"w_{v}");
        public Resize X(IntOrDoubleOrString v) => Append($"x_{v}");
        public Resize Y(IntOrDoubleOrString v) => Append($"y_{v}");
        public Resize Gravity(Gravity v) => Append($"g_{v}");
        public Resize AspectRatio(AspectRatio v) => Append($"ar_{v}");

        public override string ToString() => string.Join(",", content.OrderBy(_ => _));
    }

    public class RoundCorners : StringWrapper
    {
        protected RoundCorners(string v) : base(v) { }

        public static RoundCorners Max = new RoundCorners("max");
    }

    public class StringWrapper
    {
        protected string v;
        protected StringWrapper(string v) => this.v = v;
        public override string ToString() => v;
    }

    public class FocusOn : StringWrapper
    {
        public FocusOn(string v) : base(v) { }
        public static FocusOn Face => new FocusOn("face");
    }

    public class AutoFocus
    {
        public static FocusOn FocusOn(string value) => new FocusOn($"auto:{value.ToLowerInvariant()}");
    }

    public class AspectRatio : StringWrapper
    {
        protected AspectRatio(string v) : base(v) { }

        public static AspectRatio _1X1 => new AspectRatio("1:1");

        public static implicit operator AspectRatio(string i) => new AspectRatio(i);
    }

    public class Gravity : StringWrapper
    {
        protected Gravity(string v) : base(v) {}

        public static Gravity Auto => new Gravity("auto");

        public Gravity AutoFocus(FocusOn focusOn)
        {
            v = focusOn.ToString();
            return this;
        }

        public static Gravity FocusOn(FocusOn focusOn) => new Gravity(focusOn.ToString());
        
        public static Gravity Compass(Compass compass) => new Gravity(compass.ToString());
        
    }

    public class Compass : StringWrapper
    {
        public static readonly Compass North = new Compass("north");

        protected Compass(string v) : base(v) { }
    }

    public class IntOrDoubleOrString : StringWrapper
    {
        protected IntOrDoubleOrString(string v) : base(v) {}

        public static implicit operator IntOrDoubleOrString(string s) => new IntOrDoubleOrString(s);

        public static implicit operator IntOrDoubleOrString(int i) => new IntOrDoubleOrString(i.ToString());

        public static implicit operator IntOrDoubleOrString(double d) => new IntOrDoubleOrString(d.ToString());
    }

    public class Source : StringWrapper
    {
        protected Source(string v) : base(v) { }

        public static Source Image(string image) => new Source(image);
    }


    public class Overlay
    {
        private readonly List<string> content = new List<string>();

        private Overlay Append(string Value)
        {
            content.Add(Value);
            return this;
        }

        public static Overlay Source(Source source) => new Overlay().Append(source.ToString());
        public override string ToString() => string.Join("/", content);
    }

    public class Transformation
    {
        private readonly List<string> content = new List<string>();

        public Transformation Resize(Resize spec) => Append(spec.ToString());

        public Transformation RoundCorners(RoundCorners v) => Append($"r_{v}");

        public Transformation Overlay(Overlay v) => Append($"l_{v}");

        public Transformation AddVariable(string varName, IntOrDoubleOrString varValue) => Append($"${varName}_{varValue}");

        public override string ToString() => string.Join("/", content);

        private Transformation Append(string Value)
        {
            content.Add(Value);
            return this;
        }
    }

    public class NewTransformationTests
    {
        [Test]
        public void TestSimpleIntTransformation()
        {
            Assert.AreEqual(
                    "c_scale,h_400,w_500",
                    new Transformation().Resize(Resize.Scale().Height(400).Width(500)).ToString());
        }

        [Test]
        public void TestSimpleDoubleTransformation()
        {
            Assert.AreEqual(
                    "c_scale,h_0.5,w_0.1",
                    new Transformation().Resize(Resize.Scale().Height(0.5).Width(0.1)).ToString());
        }

        [Test]
        public void TestCropTransformation()
        {
            Assert.AreEqual(
                    "c_crop,h_400,w_500,x_50,y_100",
                    new Transformation().Resize(Resize.Crop().Height(400).Width(500).X(50).Y(100)).ToString());
        }

        [Test]
        public void TestGravityTransformation()
        {
            Assert.AreEqual(
                    "c_crop,g_auto,w_500",
                    new Transformation().Resize(Resize.Crop().Width(500).Gravity(Gravity.Auto)).ToString());
        }

        [Test]
        public void TestAutoGravity()
        {
            Assert.AreEqual(
                    "ar_1:1,c_fill,g_auto:subject",
                    new Transformation()
                        .Resize(Resize.Fill().AspectRatio(AspectRatio._1X1)
                            .Gravity(Gravity.Auto.AutoFocus(AutoFocus.FocusOn("Subject"))))
                        .ToString());
        }

        [Test]
        public void TestSomethingComplex()
        {
            Assert.AreEqual(
                    "$widthval_200/$arval_0.8/ar_$arval,c_fill,g_face,w_$widthval",
                    new Transformation()
                        .AddVariable("widthval", 200)
                        .AddVariable("arval", 0.8)
                        .Resize(Resize
                                    .Fill()
                                    .Width("$widthval")
                                    .AspectRatio("$arval")
                                    .Gravity(Gravity.FocusOn(FocusOn.Face)))
                        .ToString());
        }

        [Test]
        public void TestGravityWithCompass()
        {
            Assert.AreEqual(
                    "c_fill,g_north,h_250,w_250",
                    new Transformation()
                        .Resize(Resize
                                    .Fill()
                                    .Width("250")
                                    .Height(250)
                                    .Gravity(Gravity.Compass(Compass.North)))
                        .ToString());
        }

        [Test]
        public void TestSimpleOverlays()
        {
            Assert.AreEqual(
                    "c_thumb,g_face,h_100,w_100/r_max/l_cloudinary_icon_white",
                    new Transformation()
                        .Resize(Resize
                            .Thumbnail()
                            .Width(100)
                            .Height(100)
                            .Gravity(Gravity.FocusOn(FocusOn.Face)))
                        .RoundCorners(RoundCorners.Max)
                        .Overlay(Overlay.Source(Source.Image("cloudinary_icon_white")))
                        .ToString());
        }
    }
}


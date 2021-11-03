using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;

namespace MyNamespace
{
    public struct Variable
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Variable(string name, NumberOrStringOrExpression value)
        {
            Name = name;
            Value = value.ToString();
        }

        public override string ToString() => $"${Name}_{Value}";
    }

    public class Transformation
    {
        private ImmutableList<object> actionGroups = ImmutableList<object>.Empty;

        public Transformation Resize(ScaleAction action) => AddActionGroup(action);
        public Transformation Resize(CropAction action) => AddActionGroup(action);
        public Transformation Resize(FillAction action) => AddActionGroup(action);
        public Transformation Resize(PadAction action) => AddActionGroup(action);
        public Transformation Resize(ThumbnailAction action) => AddActionGroup(action);
        public Transformation RoundCorners(RoundCornersValue value) => AddActionGroup(new RoundCorners(value));
        public Transformation Overlay(OverlayValue value) => AddActionGroup(new Overlay(value));

        public Transformation AddVariable(string name, NumberOrStringOrExpression value) =>
            AddActionGroup(new Variable(name, value));

        private Transformation AddActionGroup(object o) => new Transformation() { actionGroups = actionGroups.Add(o) };

        public override string ToString() => string.Join("/", actionGroups);
    }

    public class Overlay : TransformationQualifier
    {
        public Overlay(OverlayValue value) : base("l", value.ToString()) { }

        public static OverlayValue Source(Source source) => new OverlayValue(source);
    }

    public class OverlayValue : ValueKeeper<Source>
    {
        public OverlayValue(Source v) : base(v) { }
    }

    public class Source : ValueKeeper<string>
    {
        public Source(string v) : base(v) { }
        public static Source Image(string value) => new Source(value);
    }

    public class RoundCorners : TransformationQualifier
    {
        public RoundCorners(RoundCornersValue value) : base("r", value.ToString()) { }

        public static readonly RoundCornersValue Max = new RoundCornersValue("max");
    }

    public class RoundCornersValue : ValueKeeper<string>
    {
        public RoundCornersValue(string v) : base(v) { }
    }

    public class Resize
    {
        public static ScaleAction Scale() => new ScaleAction();
        public static CropAction Crop() => new CropAction();
        public static PadAction Pad() => new PadAction();
        public static FillAction Fill() => new FillAction();
        public static ThumbnailAction Thumbnail() => new ThumbnailAction();
    }

    public class ActionBase<T> where T : ActionBase<T>, new()
    {
        public ActionBase(string name) => qualifiers = qualifiers.Add(name);
        public IEnumerable<object> Qualifiers => qualifiers;

        private ImmutableList<object> qualifiers = ImmutableList<object>.Empty;

        public override string ToString() => string.Join(",", qualifiers.Select(_ => _.ToString()).OrderBy(_ => _));

        public T Height(NumberOrStringOrExpression v) => AddQualifier(new Height(v));

        public T Width(NumberOrStringOrExpression v) => AddQualifier(new Width(v));

        protected T AddQualifier(TransformationQualifier item) => new T { qualifiers = qualifiers.Add(item) };

    }

    public class ScaleAction : ActionBase<ScaleAction>
    {
        public ScaleAction() : base("c_scale") { }
    }

    public class ThumbnailAction : ActionBase<ThumbnailAction>
    {
        public ThumbnailAction() : base("c_thumb") { }

        public ThumbnailAction Gravity(GravityValue v) => AddQualifier(new Gravity(v));
    }

    public class PadAction : ActionBase<PadAction>
    {
        public PadAction() : base("c_pad") { }

        public PadAction Background(BackgroundValue v) => AddQualifier(new Background(v));
    }

    public class FillAction : ActionBase<FillAction>
    {
        public FillAction() : base("c_fill") { }

        public FillAction AspectRatio(AspectRatioValue v) => AddQualifier(v);
        public FillAction Gravity(GravityValue v) => AddQualifier(new Gravity(v));
    }

    public class AspectRatio
    {
        public static AspectRatioValue _1X1 => new AspectRatioValue("1:1");
    }

    public class AspectRatioValue : TransformationQualifier
    {
        public static implicit operator AspectRatioValue(string v) => new AspectRatioValue(v);
        public AspectRatioValue(string v) : base("ar", v) { }
    }

    public class ValueKeeper<T>
    {
        public ValueKeeper(T v) { this.v = v; }

        private readonly T v;

        public override string ToString() => v.ToString();
    }

    public class GravityValue : ValueKeeper<string>
    {
        public GravityValue(string v) : base(v) { }
    }

    public class AutoGravityValue : GravityValue
    {
        public AutoGravityValue(string v = null) : base("auto" + (!string.IsNullOrWhiteSpace(v) ? $":{v}" : "")) { }
        public AutoGravityValue AutoFocus(AutoFocusValue v) => new AutoGravityValue(v.ToString());
    }

    public class BackgroundValue : ValueKeeper<string>
    {
        public BackgroundValue(string v) : base(v) { }
    }

    public class Background : TransformationQualifier
    {
        public Background(BackgroundValue value) : base("b", value.ToString()) { }

        public static BackgroundValue Color(Color v) => new BackgroundValue(v.ToString());
    }

    public class Color : ValueKeeper<string>
    {
        public Color(string v) : base(v) { }
        public static readonly Color Black = new Color("black");
    }

    public class AutoFocusValue : ValueKeeper<string>
    {
        public AutoFocusValue(string v) : base(v) { }
    }

    public class AutoFocus
    {
        public static AutoFocusValue FocusOn(string v) => new AutoFocusValue(v.ToLower());
    }

    public class Gravity : TransformationQualifier
    {
        public static readonly AutoGravityValue Auto = new AutoGravityValue();
        public static GravityValue FocusOn(FocusOn value) => value;
        public static GravityValue Compass(Compass value) => value;
        public Gravity(GravityValue v) : base("g", v.ToString()) { }
    }

    public class Compass : GravityValue
    {
        public static readonly Compass North = new Compass("north");

        private Compass(string v) : base(v) { }
    }

    public class FocusOn : GravityValue
    {
        public static readonly FocusOn Face = new FocusOn("face");

        private FocusOn(string v) : base(v) { }
    }

    public class CropAction : ActionBase<CropAction>
    {
        public CropAction() : base("c_crop") { }

        public CropAction X(NumberOrStringOrExpression v) => AddQualifier(new X(v));
        public CropAction Y(NumberOrStringOrExpression v) => AddQualifier(new Y(v));
        public CropAction Gravity(GravityValue v) => AddQualifier(new Gravity(v));
    }

    public class X : TransformationQualifier
    {
        public X(NumberOrStringOrExpression v) : base("x", v) { }
    }

    public class Y : TransformationQualifier
    {
        public Y(NumberOrStringOrExpression v) : base("y", v) { }
    }

    public class Width : TransformationQualifier
    {
        public Width(NumberOrStringOrExpression v) : base("w", v) { }
    }

    public class Height : TransformationQualifier
    {
        public Height(NumberOrStringOrExpression v) : base("h", v) { }
    }

    public struct NumberOrStringOrExpression
    {
        private readonly string v;
        public NumberOrStringOrExpression(string v) => this.v = v;
        public static implicit operator NumberOrStringOrExpression(int v) => new NumberOrStringOrExpression(v.ToString());
        public static implicit operator NumberOrStringOrExpression(string v) => new NumberOrStringOrExpression(v);
        public static implicit operator NumberOrStringOrExpression(double v) => new NumberOrStringOrExpression(v.ToString());
        public override string ToString() => v;
    }

    public class TransformationQualifier
    {
        private readonly string name;
        private readonly NumberOrStringOrExpression @value;

        public TransformationQualifier(string name, NumberOrStringOrExpression value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString() => $"{name}_{value}";
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

        [Test]
        public void TestActionsReuse()
        {
            var action = new CropAction()
                .Height("500")
                .Width(100);

            Assert.AreEqual(
                    "c_crop,h_500,w_100",
                    new Transformation()
                        .Resize(action)
                        .ToString());
        }

        [Test]
        public void TestImmutabilityOfAction()
        {
            var action = new CropAction().Height(100);
            _ = action.Width(100); // this statemenet should not modify action

            Assert.AreEqual("c_crop,h_100", action.ToString());
        }

        [Test]
        public void TestImmutabilityOfTransformation()
        {
            var transformation = new Transformation().Resize(Resize.Crop().Height(100));
            _ = transformation.RoundCorners(RoundCorners.Max); // this statemenet should not modify transformation

            Assert.AreEqual("c_crop,h_100", transformation.ToString());
        }

        [Test]
        public void TestPadTransformations()
        {
            Assert.AreEqual(
                    "b_black,c_pad,h_150,w_150",
                    new Transformation()
                        .Resize(Resize.Pad()
                            .Height(150)
                            .Width(150)
                            .Background(Background.Color(Color.Black)))
                        .ToString());
        }
    }
}

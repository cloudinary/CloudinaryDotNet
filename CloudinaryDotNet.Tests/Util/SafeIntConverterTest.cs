using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Threading;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Util
{
    /// <summary>
    /// Unit tests exercising <see cref="SafeIntConverter"/> directly, independently of any API call.
    /// </summary>
    [TestFixture]
    public class SafeIntConverterTest
    {
        private const long WrapAround = 4294967295L;

        private JsonSerializer serializer;

        /// <summary>
        /// Target covering every integral shape the converter claims to handle.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Performance",
            "CA1812:Avoid uninstantiated internal classes",
            Justification = "Instantiated by the JSON deserializer via reflection.")]
        private sealed class Target
        {
            [JsonProperty("i")]
            public int Int { get; set; }

            [JsonProperty("ni")]
            public int? NullableInt { get; set; }

            [JsonProperty("l")]
            public long Long { get; set; }

            [JsonProperty("nl")]
            public long? NullableLong { get; set; }

            [JsonProperty("s")]
            public string Text { get; set; }

            [JsonProperty("f")]
            public float Float { get; set; }

            [JsonProperty("d")]
            public double Double { get; set; }

            [JsonProperty("b")]
            public bool Flag { get; set; }

            [JsonProperty("map")]
            public Dictionary<string, long> Map { get; set; }

            [JsonProperty("list")]
            public List<int> List { get; set; }

            [JsonProperty("nested")]
            public Target Nested { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            serializer = new JsonSerializer();
            serializer.Converters.Add(new SafeIntConverter());
        }

        private Target Bind(string json) => JToken.Parse(json).ToObject<Target>(serializer);

        // ---------------------------------------------------------------
        // CanConvert
        // ---------------------------------------------------------------
        [TestCase(typeof(int), true)]
        [TestCase(typeof(int?), true)]
        [TestCase(typeof(long), true)]
        [TestCase(typeof(long?), true)]
        [TestCase(typeof(string), false)]
        [TestCase(typeof(float), false)]
        [TestCase(typeof(double), false)]
        [TestCase(typeof(decimal), false)]
        [TestCase(typeof(bool), false)]
        [TestCase(typeof(short), false)]
        [TestCase(typeof(byte), false)]
        [TestCase(typeof(uint), false)]
        [TestCase(typeof(ulong), false)]
        [TestCase(typeof(object), false)]
        public void TestCanConvertAcceptsOnlyIntAndLong(Type type, bool expected)
        {
            Assert.AreEqual(expected, new SafeIntConverter().CanConvert(type));
        }

        [Test]
        public void TestConverterIsReadOnly()
        {
            Assert.IsFalse(new SafeIntConverter().CanWrite);
        }

        [Test]
        public void TestWriteJsonIsNotSupported()
        {
            var converter = new SafeIntConverter();
            using (var writer = new JTokenWriter())
            {
                Assert.Throws<NotImplementedException>(
                    () => converter.WriteJson(writer, 1, new JsonSerializer()));
            }
        }

        // ---------------------------------------------------------------
        // The wrap-around sentinel
        // ---------------------------------------------------------------
        [Test]
        public void TestWrapAroundBecomesUnknownForInt()
        {
            Assert.AreEqual(SafeIntConverter.UnknownValue, Bind($"{{'i':{WrapAround}}}").Int);
        }

        [Test]
        public void TestWrapAroundBecomesUnknownForLong()
        {
            Assert.AreEqual((long)SafeIntConverter.UnknownValue, Bind($"{{'l':{WrapAround}}}").Long);
        }

        [Test]
        public void TestWrapAroundBecomesUnknownForNullableInt()
        {
            Assert.AreEqual(SafeIntConverter.UnknownValue, Bind($"{{'ni':{WrapAround}}}").NullableInt);
        }

        [Test]
        public void TestWrapAroundBecomesUnknownForNullableLong()
        {
            Assert.AreEqual((long)SafeIntConverter.UnknownValue, Bind($"{{'nl':{WrapAround}}}").NullableLong);
        }

        [Test]
        public void TestWrapAroundAsStringBecomesUnknown()
        {
            Assert.AreEqual(SafeIntConverter.UnknownValue, Bind($"{{'i':'{WrapAround}'}}").Int);
        }

        [Test]
        public void TestUnknownValueIsNegativeOne()
        {
            // The sentinel is the signed reading of the upstream unsigned -1;
            // callers are documented against this exact value.
            Assert.AreEqual(-1, SafeIntConverter.UnknownValue);
        }

        [Test]
        public void TestValueAdjacentToWrapAroundIsNotTreatedAsSentinel()
        {
            // Only the exact sentinel is special: neighbours must still be rejected for int.
            Assert.Throws<JsonSerializationException>(() => Bind($"{{'i':{WrapAround - 1}}}"));
            Assert.Throws<JsonSerializationException>(() => Bind($"{{'i':{WrapAround + 1}}}"));
        }

        [Test]
        public void TestValueAdjacentToWrapAroundIsPreservedForLong()
        {
            Assert.AreEqual(WrapAround - 1, Bind($"{{'l':{WrapAround - 1}}}").Long);
            Assert.AreEqual(WrapAround + 1, Bind($"{{'l':{WrapAround + 1}}}").Long);
        }

        // ---------------------------------------------------------------
        // Ordinary values pass through untouched
        // ---------------------------------------------------------------
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(42)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void TestOrdinaryIntValuesArePreserved(int value)
        {
            Assert.AreEqual(value, Bind($"{{'i':{value}}}").Int);
        }

        [TestCase(0L)]
        [TestCase(-1L)]
        [TestCase(8589934592L)]
        [TestCase(long.MaxValue)]
        [TestCase(long.MinValue)]
        public void TestOrdinaryLongValuesArePreserved(long value)
        {
            Assert.AreEqual(value, Bind($"{{'l':{value}}}").Long);
        }

        [Test]
        public void TestGenuineNegativeOneIsPreserved()
        {
            // Indistinguishable from the sentinel by design; documented behaviour.
            Assert.AreEqual(-1, Bind("{'i':-1}").Int);
        }

        // ---------------------------------------------------------------
        // Range handling
        // ---------------------------------------------------------------
        [Test]
        public void TestValueAboveIntRangeThrowsForInt()
        {
            Assert.Throws<JsonSerializationException>(() => Bind("{'i':2147483648}"));
        }

        [Test]
        public void TestValueBelowIntRangeThrowsForInt()
        {
            Assert.Throws<JsonSerializationException>(() => Bind("{'i':-2147483649}"));
        }

        [Test]
        public void TestValueAboveIntRangeIsPreservedForLong()
        {
            Assert.AreEqual(2147483648L, Bind("{'l':2147483648}").Long);
        }

        [Test]
        public void TestValueBeyondLongRangeThrowsForInt()
        {
            Assert.Throws<JsonSerializationException>(() => Bind("{'i':18446744073709551615}"));
        }

        [Test]
        public void TestValueBeyondLongRangeThrowsForLong()
        {
            Assert.Throws<JsonSerializationException>(() => Bind("{'l':18446744073709551615}"));
        }

        [Test]
        public void TestOutOfRangeMessageNamesTheValue()
        {
            var ex = Assert.Throws<JsonSerializationException>(() => Bind("{'i':2147483648}"));
            StringAssert.Contains("2147483648", ex.Message);
        }

        // ---------------------------------------------------------------
        // Null and empty
        // ---------------------------------------------------------------
        [Test]
        public void TestNullYieldsDefaultForNonNullableInt()
        {
            Assert.AreEqual(0, Bind("{'i':null}").Int);
        }

        [Test]
        public void TestNullYieldsDefaultForNonNullableLong()
        {
            Assert.AreEqual(0L, Bind("{'l':null}").Long);
        }

        [Test]
        public void TestNullYieldsNullForNullableInt()
        {
            Assert.IsNull(Bind("{'ni':null}").NullableInt);
        }

        [Test]
        public void TestNullYieldsNullForNullableLong()
        {
            Assert.IsNull(Bind("{'nl':null}").NullableLong);
        }

        [Test]
        public void TestMissingPropertyLeavesDefault()
        {
            var result = Bind("{}");

            Assert.AreEqual(0, result.Int);
            Assert.IsNull(result.NullableInt);
        }

        [TestCase("''")]
        [TestCase("'   '")]
        public void TestEmptyStringYieldsDefaultForNonNullable(string json)
        {
            Assert.AreEqual(0, Bind($"{{'i':{json}}}").Int);
        }

        [TestCase("''")]
        [TestCase("'   '")]
        public void TestEmptyStringYieldsNullForNullable(string json)
        {
            Assert.IsNull(Bind($"{{'ni':{json}}}").NullableInt);
        }

        // ---------------------------------------------------------------
        // String-encoded numbers
        // ---------------------------------------------------------------
        [TestCase("'0'", 0)]
        [TestCase("'42'", 42)]
        [TestCase("'-7'", -7)]
        [TestCase("'2147483647'", int.MaxValue)]
        public void TestStringEncodedIntegersAreParsed(string json, int expected)
        {
            Assert.AreEqual(expected, Bind($"{{'i':{json}}}").Int);
        }

        [Test]
        public void TestStringEncodedLongIsParsed()
        {
            Assert.AreEqual(8589934592L, Bind("{'l':'8589934592'}").Long);
        }

        [TestCase("'abc'")]
        [TestCase("'12abc'")]
        [TestCase("'1.5'")]
        [TestCase("'0x1F'")]
        [TestCase("'1e3'")]
        public void TestUnparseableStringThrows(string json)
        {
            Assert.Throws<JsonSerializationException>(() => Bind($"{{'i':{json}}}"));
        }

        [Test]
        public void TestUnparseableStringMessageNamesTheValue()
        {
            var ex = Assert.Throws<JsonSerializationException>(() => Bind("{'i':'abc'}"));
            StringAssert.Contains("abc", ex.Message);
        }

        [Test]
        public void TestStringParsingIsCultureInvariant()
        {
            // A culture using '.' as a group separator must not change the reading.
            // Set via Thread: CultureInfo.CurrentCulture has no setter on net452.
            var original = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                Assert.AreEqual(1234, Bind("{'i':'1234'}").Int);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = original;
            }
        }

        // ---------------------------------------------------------------
        // Fractional values: the API reports some counters this way
        // ---------------------------------------------------------------
        // System.Convert rounds to even, which is what the default binder does;
        // these expectations mirror JsonReader.ReadAsInt32 exactly.
        [TestCase("1.47", 1)]
        [TestCase("0.9", 1)]
        [TestCase("-1.9", -2)]
        [TestCase("42.0", 42)]
        [TestCase("1.5", 2)]
        [TestCase("2.5", 2)]
        [TestCase("3.5", 4)]
        [TestCase("-2.5", -2)]
        public void TestFractionalValuesAreRoundedAsTheDefaultBinderDoes(string json, int expected)
        {
            Assert.AreEqual(expected, Bind($"{{'i':{json}}}").Int);
        }

        [Test]
        public void TestFractionalValueIsRoundedForLong()
        {
            Assert.AreEqual(1L, Bind("{'l':1.47}").Long);
            Assert.AreEqual(2L, Bind("{'l':1.5}").Long);
        }

        [Test]
        public void TestFractionalWrapAroundBecomesUnknown()
        {
            Assert.AreEqual(SafeIntConverter.UnknownValue, Bind($"{{'i':{WrapAround}.0}}").Int);
        }

        // ---------------------------------------------------------------
        // Unexpected tokens
        // ---------------------------------------------------------------
        [TestCase("true")]
        [TestCase("{}")]
        [TestCase("[]")]
        public void TestUnexpectedTokenThrows(string json)
        {
            Assert.Throws<JsonSerializationException>(() => Bind($"{{'i':{json}}}"));
        }

        // ---------------------------------------------------------------
        // The converter must not disturb non-integer properties
        // ---------------------------------------------------------------
        [Test]
        public void TestOtherTypesAreUntouched()
        {
            var result = Bind("{'s':'hello','f':1.5,'d':2.25,'b':true}");

            Assert.AreEqual("hello", result.Text);
            Assert.AreEqual(1.5f, result.Float);
            Assert.AreEqual(2.25d, result.Double);
            Assert.IsTrue(result.Flag);
        }

        [Test]
        public void TestNumericStringPropertyIsNotConverted()
        {
            // A string property holding the sentinel must stay a string.
            Assert.AreEqual(WrapAround.ToString(CultureInfo.InvariantCulture), Bind($"{{'s':'{WrapAround}'}}").Text);
        }

        // ---------------------------------------------------------------
        // Containers and nesting
        // ---------------------------------------------------------------
        [Test]
        public void TestDictionaryValuesAreConverted()
        {
            var result = Bind($"{{'map':{{'a':{WrapAround},'b':10}}}}");

            Assert.AreEqual((long)SafeIntConverter.UnknownValue, result.Map["a"]);
            Assert.AreEqual(10L, result.Map["b"]);
        }

        [Test]
        public void TestListElementsAreConverted()
        {
            var result = Bind($"{{'list':[1,{WrapAround},3]}}");

            Assert.AreEqual(new[] { 1, SafeIntConverter.UnknownValue, 3 }, result.List);
        }

        [Test]
        public void TestNestedObjectsAreConverted()
        {
            var result = Bind($"{{'nested':{{'i':{WrapAround},'l':5}}}}");

            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Nested.Int);
            Assert.AreEqual(5L, result.Nested.Long);
        }

        [Test]
        public void TestOneBadFieldDoesNotDiscardSiblings()
        {
            // The whole point of the converter: a single wrapped field must not
            // abort the bind and take the rest of the payload with it.
            var result = Bind($"{{'i':{WrapAround},'l':99,'s':'kept','b':true}}");

            Assert.AreEqual(SafeIntConverter.UnknownValue, result.Int);
            Assert.AreEqual(99L, result.Long);
            Assert.AreEqual("kept", result.Text);
            Assert.IsTrue(result.Flag);
        }

        // ---------------------------------------------------------------
        // Boxed integral inputs reaching ReadJson directly
        // ---------------------------------------------------------------
        [TestCase((short)7)]
        [TestCase((byte)8)]
        public void TestNarrowIntegralValuesAreAccepted(object value)
        {
            Assert.AreEqual(Convert.ToInt32(value, CultureInfo.InvariantCulture), ReadDirect(value, typeof(int)));
        }

        [Test]
        public void TestUnsignedIntegralValuesAreAccepted()
        {
            Assert.AreEqual(9, ReadDirect(9u, typeof(int)));
            Assert.AreEqual(10, ReadDirect(10UL, typeof(int)));
        }

        [Test]
        public void TestSentinelIsRecognisedInEveryReachableForm()
        {
            // 4294967295 reaches the reader boxed only as long, double or string.
            // These are the forms a real response can produce; nothing else needs handling.
            Assert.AreEqual(SafeIntConverter.UnknownValue, Bind($"{{'i':{WrapAround}}}").Int, "integer form");
            Assert.AreEqual(SafeIntConverter.UnknownValue, Bind($"{{'i':{WrapAround}.0}}").Int, "float form");
            Assert.AreEqual(SafeIntConverter.UnknownValue, Bind("{'i':4.294967295e9}").Int, "exponent form");
            Assert.AreEqual(SafeIntConverter.UnknownValue, Bind($"{{'i':'{WrapAround}'}}").Int, "string form");
        }

        [Test]
        public void TestSentinelIsNotRecognisedInUnreachableBoxedForms()
        {
            // Narrower and wider boxed types are never produced for this magnitude,
            // so they are deliberately not special-cased: the value converts literally.
            Assert.AreEqual(WrapAround, ReadDirect((uint)WrapAround, typeof(long)));
        }

        [Test]
        public void TestUnsignedValueBeyondLongRangeThrows()
        {
            Assert.Throws<JsonSerializationException>(() => ReadDirect(ulong.MaxValue, typeof(long)));
        }

        [Test]
        public void TestBigIntegerWithinRangeIsAccepted()
        {
            Assert.AreEqual(123L, ReadDirect(new BigInteger(123), typeof(long)));
        }

        [Test]
        public void TestBigIntegerWithinIntRangeIsConverted()
        {
            // BigInteger still reaches the conversion path; it is only the sentinel
            // check that no longer inspects it, since no such value is produced.
            Assert.AreEqual(456, ReadDirect(new BigInteger(456), typeof(int)));
        }

        [Test]
        public void TestDecimalValueIsRounded()
        {
            Assert.AreEqual(4, ReadDirect(3.9m, typeof(int)));
        }

        [Test]
        public void TestFloatValueIsRounded()
        {
            Assert.AreEqual(5, ReadDirect(4.9f, typeof(int)));
        }

        /// <summary>
        /// Drives ReadJson with a specific boxed value, bypassing Newtonsoft's own token typing.
        /// </summary>
        private static object ReadDirect(object value, Type targetType)
        {
            using (var reader = new JTokenReader(new JValue(value)))
            {
                reader.Read();
                return new SafeIntConverter().ReadJson(reader, targetType, null, new JsonSerializer());
            }
        }

        // ---------------------------------------------------------------
        // Differential comparison against the unmodified binder. Every input
        // except the sentinel must produce byte-identical results, including
        // the exception type, so the converter is a true drop-in.
        // ---------------------------------------------------------------
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("-1")]
        [TestCase("42")]
        [TestCase("2147483647")]
        [TestCase("-2147483648")]
        [TestCase("2147483648")]
        [TestCase("-2147483649")]
        [TestCase("1.47")]
        [TestCase("0.9")]
        [TestCase("1.5")]
        [TestCase("2.5")]
        [TestCase("3.5")]
        [TestCase("-1.9")]
        [TestCase("-2.5")]
        [TestCase("'0'")]
        [TestCase("'42'")]
        [TestCase("'-7'")]
        [TestCase("'abc'")]
        [TestCase("'1.5'")]
        [TestCase("true")]
        [TestCase("{}")]
        [TestCase("[]")]
        public void TestMatchesDefaultBinderForNonSentinelValues(string jsonValue)
        {
            var json = $"{{'i':{jsonValue}}}";

            var expected = Capture(() => JToken.Parse(json).ToObject<Target>(new JsonSerializer()).Int);
            var actual = Capture(() => Bind(json).Int);

            Assert.AreEqual(expected.Threw, actual.Threw, $"throw/succeed mismatch for {jsonValue}");

            if (expected.Threw)
            {
                // Both must fail as a JsonException so the caller-visible wrapping is unchanged.
                Assert.IsInstanceOf<JsonException>(expected.Error, $"baseline error for {jsonValue}");
                Assert.IsInstanceOf<JsonException>(actual.Error, $"converter error for {jsonValue}");
            }
            else
            {
                Assert.AreEqual(expected.Value, actual.Value, $"value mismatch for {jsonValue}");
            }
        }

        [TestCase("0")]
        [TestCase("-1")]
        [TestCase("8589934592")]
        [TestCase("1.47")]
        [TestCase("1.5")]
        [TestCase("'42'")]
        public void TestMatchesDefaultBinderForLongTargets(string jsonValue)
        {
            var json = $"{{'l':{jsonValue}}}";

            var expected = Capture(() => JToken.Parse(json).ToObject<Target>(new JsonSerializer()).Long);
            var actual = Capture(() => Bind(json).Long);

            Assert.AreEqual(expected.Threw, actual.Threw, $"throw/succeed mismatch for {jsonValue}");

            if (!expected.Threw)
            {
                Assert.AreEqual(expected.Value, actual.Value, $"value mismatch for {jsonValue}");
            }
        }

        // ---------------------------------------------------------------
        // Deliberate divergences from the default binder, documented here so
        // that a future change to either behaviour is a visible test failure.
        // ---------------------------------------------------------------
        [Test]
        public void TestNullIsLenientWhereDefaultBinderThrows()
        {
            // The default binder rejects null for a non-nullable target, which would
            // discard the whole response over one absent field. The converter reports
            // the default instead, matching the leniency it exists to provide.
            Assert.Throws<JsonSerializationException>(
                () => JToken.Parse("{'i':null}").ToObject<Target>(new JsonSerializer()));

            Assert.AreEqual(0, Bind("{'i':null}").Int);
        }

        [Test]
        public void TestEmptyStringIsLenientWhereDefaultBinderThrows()
        {
            Assert.Throws<JsonSerializationException>(
                () => JToken.Parse("{'i':''}").ToObject<Target>(new JsonSerializer()));

            Assert.AreEqual(0, Bind("{'i':''}").Int);
        }

        [Test]
        public void TestOversizedValueFailsAsJsonException()
        {
            // The default binder lets a raw OverflowException escape, which slips past
            // the JsonException handler wrapping API responses. The converter reports a
            // JsonException so the failure is wrapped consistently.
            Assert.Throws<OverflowException>(
                () => JToken.Parse("{'i':18446744073709551615}").ToObject<Target>(new JsonSerializer()));

            Assert.Throws<JsonSerializationException>(() => Bind("{'i':18446744073709551615}"));
        }

        // Deliberately catches everything: the baseline binder is known to throw
        // non-Json exceptions (e.g. a raw OverflowException), and the comparison
        // must observe those rather than let them escape the test.
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Design",
            "CA1031:Do not catch general exception types",
            Justification = "Captures any binder outcome, including non-Json exceptions, for differential comparison.")]
        private static CaptureResult Capture(Func<object> read)
        {
            try
            {
                return new CaptureResult { Threw = false, Value = read() };
            }
            catch (Exception e)
            {
                return new CaptureResult { Threw = true, Error = e };
            }
        }

        /// <summary>
        /// Outcome of a bind attempt: either a value or the exception it raised.
        /// </summary>
        private sealed class CaptureResult
        {
            public bool Threw { get; set; }

            public object Value { get; set; }

            public Exception Error { get; set; }
        }
    }
}

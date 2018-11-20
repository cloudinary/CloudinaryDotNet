using CloudinaryDotNet.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    public partial class Transformation : Core.ICloneable
    {
        private static readonly Regex RANGE_VALUE_RE = new Regex("^((?:\\d+\\.)?\\d+)([%pP])?$", RegexOptions.Compiled);
        private static readonly Regex RANGE_RE = new Regex("^(\\d+\\.)?\\d+[%pP]?\\.\\.(\\d+\\.)?\\d+[%pP]?$", RegexOptions.Compiled);

        public Transformation VideoCodec(params string[] codecParams)
        {
            if (codecParams.Length == 1)
            {
                return Add("video_codec", codecParams[0]);
            }
            else if (codecParams.Length > 1 && (codecParams.Length % 2) == 0)
            {
                var dict = new Dictionary<string, string>();
                for (int i = 0; i < codecParams.Length; i += 2)
                {
                    if (!dict.ContainsKey(codecParams[i]))
                        dict.Add(codecParams[i], codecParams[i + 1]);
                }
                return VideoCodec(dict);
            }

            throw new ArgumentException("codecParams: please provide either single parameter or a bunch of key-value pairs (key1, value1, key2, value2, ...).");
        }

        public Transformation VideoCodec(Dictionary<string, string> codecParams)
        {
            return Add("video_codec", codecParams);
        }

        public Transformation Fps(string value)
        {
            return Add("fps", value);
        }

        public Transformation Fps(double value)
        {
            return Fps($"{value}");
        }

        public Transformation Fps(double? min, double? max)
        {
            if (!min.HasValue && !max.HasValue)
                throw new ArgumentException("Both arguments 'min' and 'max' can not be null.");

            return Fps($"{min}-{max}");
        }

        public Transformation Fps(string min, string max)
        {
            if (string.IsNullOrEmpty(min) && string.IsNullOrEmpty(max))
                throw new ArgumentException("Both arguments 'min' and 'max' can not be null.");

            return Fps($"{min}-{max}");
        }

        public Transformation AudioCodec(string codec)
        {
            return Add("audio_codec", codec);
        }

        public Transformation BitRate(int bitRate)
        {
            return Add("bit_rate", bitRate);
        }

        public Transformation BitRate(string bitRate)
        {
            return Add("bit_rate", bitRate);
        }

        public Transformation AudioFrequency(int frequency)
        {
            return Add("audio_frequency", frequency);
        }

        public Transformation AudioFrequency(string frequency)
        {
            return Add("audio_frequency", frequency);
        }

        public Transformation VideoSampling(string value)
        {
            return Add("video_sampling", value);
        }

        public Transformation VideoSamplingFrames(int value)
        {
            return Add("video_sampling", value);
        }

        public Transformation VideoSamplingSeconds(int value)
        {
            return VideoSamplingSeconds((object)value);
        }

        public Transformation VideoSamplingSeconds(float value)
        {
            return VideoSamplingSeconds((object)value);
        }

        public Transformation VideoSamplingSeconds(double value)
        {
            return VideoSamplingSeconds((object)value);
        }

        private Transformation VideoSamplingSeconds(object value)
        {
            return Add("video_sampling", ToString(value) + "s");
        }

        public Transformation StartOffset(string value)
        {
            return Add("start_offset", value);
        }

        public Transformation StartOffset(float value)
        {
            return Add("start_offset", value);
        }

        public Transformation StartOffset(double value)
        {
            return Add("start_offset", value);
        }

        public Transformation StartOffsetPercent(float value)
        {
            return StartOffsetPercent((object)value);
        }

        public Transformation StartOffsetPercent(double value)
        {
            return StartOffsetPercent((object)value);
        }

        private Transformation StartOffsetPercent(object value)
        {
            return Add("start_offset", ToString(value) + "p");
        }

        public Transformation StartOffsetAuto()
        {
            return StartOffset("auto");
        }

        public Transformation EndOffset(string value)
        {
            return Add("end_offset", value);
        }

        public Transformation EndOffset(float value)
        {
            return Add("end_offset", value);
        }

        public Transformation EndOffset(double value)
        {
            return Add("end_offset", value);
        }

        public Transformation EndOffsetPercent(float value)
        {
            return EndOffsetPercent((object)value);
        }

        public Transformation EndOffsetPercent(double value)
        {
            return EndOffsetPercent((object)value);
        }

        private Transformation EndOffsetPercent(object value)
        {
            return Add("end_offset", ToString(value) + "p");
        }

        public Transformation Offset(string value)
        {
            return Add("offset", value);
        }

        public Transformation Offset(params string[] value)
        {
            if (value.Length < 2) throw new ArgumentException("Offset range must include at least 2 items.");
            return Add("offset", value);
        }

        public Transformation Offset(params float[] value)
        {
            if (value.Length < 2) throw new ArgumentException("Offset range must include at least 2 items.");
            var arr = new object[] { (object)value[0], (object)value[1] };
            return Offset(arr);
        }

        public Transformation Offset(params double[] value)
        {
            if (value.Length < 2) throw new ArgumentException("Offset range must include at least 2 items.");
            var arr = new object[] { (object)value[0], (object)value[1] };
            return Offset(arr);
        }

        private Transformation Offset(params object[] value)
        {
            if (value.Length < 2) throw new ArgumentException("Offset range must include at least 2 items.");
            return Add("offset", value);
        }

        public Transformation Duration(string value)
        {
            return Add("duration", value);
        }

        public Transformation Duration(float value)
        {
            return Add("duration", value);
        }

        public Transformation Duration(double value)
        {
            return Add("duration", value);
        }

        public Transformation DurationPercent(float value)
        {
            return DurationPercent((object)value);
        }

        public Transformation DurationPercent(double value)
        {
            return DurationPercent((object)value);
        }

        private Transformation DurationPercent(object value)
        {
            return Add("duration", ToString(value) + "p");
        }

        public Transformation KeyframeInterval(int value)
        {
            if(value <= 0) throw new ArgumentException("The range for keyframe interval should be greater than 0.");
            return Add("keyframe_interval", string.Format("{0:0.0}", value));
        }

        public Transformation KeyframeInterval(float value)
        {
            if (value <= 0) throw new ArgumentException("The range for keyframe interval should be greater than 0.");
            return Add("keyframe_interval", value);
        }

        public Transformation KeyframeInterval(string value)
        {
            return Add("keyframe_interval", value);
        }

        public Transformation StreamingProfile(string value)
        {
            return Add("streaming_profile", value);
        }

        private static string ProcessVideoCodec(object codecParam)
        {
            var sb = new StringBuilder();
            if (codecParam is string)
            {
                sb.Append(codecParam);
            }
            else if (codecParam is Dictionary<string, string>)
            {
                string s = null;
                var @params = (Dictionary<string, string>)codecParam;
                if (!@params.TryGetValue("codec", out s)) return null;
                sb.Append(s);
                if (@params.TryGetValue("profile", out s))
                {
                    sb.Append(":").Append(s);
                    if (@params.TryGetValue("level", out s))
                    {
                        sb.Append(":").Append(s);
                    }
                }
            }
            return sb.ToString();
        }

        private static string NormAutoRangeValue(object objectValue)
        {

            return objectValue != null && string.Equals(objectValue.ToString(), "auto")
                              ? objectValue.ToString()
                              : NormRangeValue(objectValue);
        }

        private static string NormRangeValue(object objectValue)
        {
            if (objectValue == null) return null;

            var value = ToString(objectValue);
            if (String.IsNullOrEmpty(value)) return null;

            var match = RANGE_VALUE_RE.Match(value);
            if (!match.Success) return null;

            string modifier = "";
            if (match.Groups.Count == 3 && !String.IsNullOrEmpty(match.Groups[2].Value))
            {
                modifier = "p";
            }

            return match.Groups[1] + modifier;
        }

        private static string[] SplitRange(object range)
        {
            if (range is string)
            {
                var rangeStr = (string)range;
                if (RANGE_RE.IsMatch(rangeStr))
                    return rangeStr.Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
            }
            else if (range is Array)
            {
                var rangeArr = (Array)range;
                string[] stringArr = new string[rangeArr.Length];
                for (int i = 0; i < rangeArr.Length; i++)
                {
                    stringArr[i] = ToString(rangeArr.GetValue(i));
                }
                return stringArr;
            }

            return null;
        }
    }
}

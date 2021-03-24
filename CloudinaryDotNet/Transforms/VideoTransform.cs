namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The building blocks for video assets transformation.
    /// </summary>
    public partial class Transformation : Core.ICloneable
    {
        private static readonly Regex RangeValueRe = new Regex("^((?:\\d+\\.)?\\d+)([%pP])?$", RegexOptions.Compiled);
        private static readonly Regex RangeRe = new Regex("^(\\d+\\.)?\\d+[%pP]?\\.\\.(\\d+\\.)?\\d+[%pP]?$", RegexOptions.Compiled);

        /// <summary>
        /// The video codec (with video profile and level if relevant).
        /// </summary>
        /// <param name="codecParams">Parameters of the video codec. Use the syntax: {codec}[:{profile}:[{level}]].</param>
        /// <returns>The transformation with video codec applied.</returns>
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
                    {
                        dict.Add(codecParams[i], codecParams[i + 1]);
                    }
                }

                return VideoCodec(dict);
            }

            throw new ArgumentException("codecParams: please provide either single parameter or a bunch of key-value pairs (key1, value1, key2, value2, ...).");
        }

        /// <summary>
        /// The video codec (with video profile and level if relevant).
        /// </summary>
        /// <param name="codecParams">Dictionary with parameters of the video codec. E.g. codec: h264.</param>
        /// <returns>The transformation with video codec applied.</returns>
        public Transformation VideoCodec(Dictionary<string, string> codecParams)
        {
            return Add("video_codec", codecParams);
        }

        /// <summary>
        /// Control the range of acceptable FPS (Frames Per Second) to ensure that video (even when optimized) is
        /// delivered with an expected fps level (helps with sync to audio).
        /// </summary>
        /// <param name="value">Value of the frame rate.</param>
        /// <returns>The transformation with FPS value applied.</returns>
        public Transformation Fps(string value)
        {
            return Add("fps", Expression.Normalize(value));
        }

        /// <summary>
        /// Control the range of acceptable FPS (Frames Per Second) to ensure that video (even when optimized) is
        /// delivered with an expected fps level (helps with sync to audio).
        /// </summary>
        /// <param name="value">Value of the frame rate.</param>
        /// <returns>The transformation with FPS value applied.</returns>
        public Transformation Fps(double value)
        {
            return Fps($"{value}");
        }

        /// <summary>
        /// Control the range of acceptable FPS (Frames Per Second) to ensure that video (even when optimized) is
        /// delivered with an expected fps level (helps with sync to audio).
        /// </summary>
        /// <param name="min">Minimum value of the range.</param>
        /// <param name="max">Maximum value of the range.</param>
        /// <returns>The transformation with FPS value applied.</returns>
        public Transformation Fps(double? min, double? max)
        {
            if (!min.HasValue && !max.HasValue)
            {
                throw new ArgumentException("Both arguments 'min' and 'max' can not be null.");
            }

            return Fps($"{min}-{max}");
        }

        /// <summary>
        /// Control the range of acceptable FPS (Frames Per Second) to ensure that video (even when optimized) is
        /// delivered with an expected fps level (helps with sync to audio).
        /// </summary>
        /// <param name="min">Minimum value of the range.</param>
        /// <param name="max">Maximum value of the range.</param>
        /// <returns>The transformation with FPS value applied.</returns>
        public Transformation Fps(string min, string max)
        {
            if (string.IsNullOrEmpty(min) && string.IsNullOrEmpty(max))
            {
                throw new ArgumentException("Both arguments 'min' and 'max' can not be null.");
            }

            return Fps($"{min}-{max}");
        }

        /// <summary>
        /// Control the audio codec or remove the audio channel.
        /// </summary>
        /// <param name="codec">Audio codec to set. Use 'none' to remove the audio channel.</param>
        /// <returns>The transformation with audio codec applied.</returns>
        public Transformation AudioCodec(string codec)
        {
            return Add("audio_codec", codec);
        }

        /// <summary>
        /// Advanced control of video bitrate in bits per second.
        /// </summary>
        /// <param name="bitRate">Number of bits per second as an integer (max: 500000).</param>
        /// <returns>The transformation with bit rate applied.</returns>
        public Transformation BitRate(int bitRate)
        {
            return Add("bit_rate", bitRate);
        }

        /// <summary>
        /// Advanced control of video bitrate in bits per second.
        /// </summary>
        /// <param name="bitRate">Number of bits per second as a string, supporting k and m for kilobits and megabits
        /// respectively e.g., 500k or 1m. </param>
        /// <returns>The transformation with bit rate applied.</returns>
        public Transformation BitRate(string bitRate)
        {
            return Add("bit_rate", bitRate);
        }

        /// <summary>
        /// Control audio sample frequency.
        /// </summary>
        /// <param name="frequency">An integer value in Hz and can only take one of the following values: 8000, 11025,
        /// 16000, 22050, 32000, 37800, 44056, 44100, 47250, 48000, 88200, 96000, 176400 or 192000.</param>
        /// <returns>The transformation with audio frequency defined.</returns>
        public Transformation AudioFrequency(int frequency)
        {
            return Add("audio_frequency", frequency);
        }

        /// <summary>
        /// Control audio sample frequency.
        /// </summary>
        /// <param name="frequency">A string value in Hz and can only take one of the following values: 8000, 11025,
        /// 16000, 22050, 32000, 37800, 44056, 44100, 47250, 48000, 88200, 96000, 176400 or 192000.</param>
        /// <returns>The transformation with audio frequency defined.</returns>
        public Transformation AudioFrequency(string frequency)
        {
            return Add("audio_frequency", frequency);
        }

        /// <summary>
        /// Control audio sample frequency.
        /// </summary>
        /// <param name="frequency">An enum value, that represents frequency value in Hz.</param>
        /// <returns>The transformation with audio frequency defined.</returns>
        public Transformation AudioFrequency(AudioFrequency frequency)
        {
            return Add("audio_frequency", ApiShared.GetCloudinaryParam(frequency));
        }

        /// <summary>
        /// Relevant when converting videos to animated GIF or WebP format. If not specified, the resulting GIF or WebP
        /// samples the whole video (up to 400 frames, at up to 10 frames per second).
        /// </summary>
        /// <param name="value">Value of the video sampling setting.</param>
        /// <returns>The transformation with video sampling defined.</returns>
        public Transformation VideoSampling(string value)
        {
            return Add("video_sampling", value);
        }

        /// <summary>
        /// Relevant when converting videos to animated GIF or WebP format. If not specified, the resulting GIF or WebP
        /// samples the whole video (up to 400 frames, at up to 10 frames per second).
        /// </summary>
        /// <param name="value">The total number of frames to sample from the original video.</param>
        /// <returns>The transformation with video sampling defined.</returns>
        public Transformation VideoSamplingFrames(int value)
        {
            return Add("video_sampling", value);
        }

        /// <summary>
        /// Relevant when converting videos to animated GIF or WebP format. If not specified, the resulting GIF or WebP
        /// samples the whole video (up to 400 frames, at up to 10 frames per second).
        /// </summary>
        /// <param name="value">The number of seconds between each frame to sample from the original video.</param>
        /// <returns>The transformation with video sampling defined.</returns>
        public Transformation VideoSamplingSeconds(int value)
        {
            return VideoSamplingSeconds((object)value);
        }

        /// <summary>
        /// Relevant when converting videos to animated GIF or WebP format. If not specified, the resulting GIF or WebP
        /// samples the whole video (up to 400 frames, at up to 10 frames per second).
        /// </summary>
        /// <param name="value">The number of seconds between each frame to sample from the original video.</param>
        /// <returns>The transformation with video sampling defined.</returns>
        public Transformation VideoSamplingSeconds(float value)
        {
            return VideoSamplingSeconds((object)value);
        }

        /// <summary>
        /// Relevant when converting videos to animated GIF or WebP format. If not specified, the resulting GIF or WebP
        /// samples the whole video (up to 400 frames, at up to 10 frames per second).
        /// </summary>
        /// <param name="value">The number of seconds between each frame to sample from the original video.</param>
        /// <returns>The transformation with video sampling defined.</returns>
        public Transformation VideoSamplingSeconds(double value)
        {
            return VideoSamplingSeconds((object)value);
        }

        /// <summary>
        /// Offset in seconds of a video. The start of the video to be kept after trimming.
        /// </summary>
        /// <param name="value">String representing value of.</param>
        /// <returns>The transformation with start offset defined.</returns>
        public Transformation StartOffset(string value)
        {
            return Add("start_offset", value);
        }

        /// <summary>
        /// Offset in seconds of a video. The start of the video to be kept after trimming.
        /// </summary>
        /// <param name="value">Float representing seconds.</param>
        /// <returns>The transformation with start offset defined.</returns>
        public Transformation StartOffset(float value)
        {
            return Add("start_offset", value);
        }

        /// <summary>
        /// Offset in seconds of a video. The start of the video to be kept after trimming.
        /// </summary>
        /// <param name="value">Double representing seconds.</param>
        /// <returns>The transformation with start offset defined.</returns>
        public Transformation StartOffset(double value)
        {
            return Add("start_offset", value);
        }

        /// <summary>
        /// Offset in percent of a video. The start of the video to be kept after trimming.
        /// </summary>
        /// <param name="value">Float representing a percent value.</param>
        /// <returns>The transformation with start offset percent defined.</returns>
        public Transformation StartOffsetPercent(float value)
        {
            return StartOffsetPercent((object)value);
        }

        /// <summary>
        /// Offset in percent of a video. The start of the video to be kept after trimming.
        /// </summary>
        /// <param name="value">Double representing a percent value.</param>
        /// <returns>The transformation with start offset percent defined.</returns>
        public Transformation StartOffsetPercent(double value)
        {
            return StartOffsetPercent((object)value);
        }

        /// <summary>
        /// Automatically selects a suitable frame from the first few seconds of the video (only relevant for
        /// generating image thumbnails).
        /// </summary>
        /// <returns>The transformation with auto start offset defined.</returns>
        public Transformation StartOffsetAuto()
        {
            return StartOffset("auto");
        }

        /// <summary>
        /// Offset in seconds or percent of a video, used to specify the end of the video to be kept after trimming or
        /// when an overlay ends displaying.
        ///
        /// Normally used together with the start_offset and duration parameters.
        /// </summary>
        /// <param name="value">String representing an end_offset value.</param>
        /// <returns>The transformation with end offset defined.</returns>
        public Transformation EndOffset(string value)
        {
            return Add("end_offset", value);
        }

        /// <summary>
        /// Offset in seconds or percent of a video, used to specify the end of the video to be kept after trimming or
        /// when an overlay ends displaying.
        ///
        /// Normally used together with the start_offset and duration parameters.
        /// </summary>
        /// <param name="value">Float representing an end_offset value.</param>
        /// <returns>The transformation with end offset defined.</returns>
        public Transformation EndOffset(float value)
        {
            return Add("end_offset", value);
        }

        /// <summary>
        /// Offset in seconds or percent of a video, used to specify the end of the video to be kept after trimming or
        /// when an overlay ends displaying.
        ///
        /// Normally used together with the start_offset and duration parameters.
        /// </summary>
        /// <param name="value">Double representing an end_offset value.</param>
        /// <returns>The transformation with end offset defined.</returns>
        public Transformation EndOffset(double value)
        {
            return Add("end_offset", value);
        }

        /// <summary>
        /// Offset in percent of a video, used to specify the end of the video to be kept after trimming or
        /// when an overlay ends displaying.
        ///
        /// Normally used together with the start_offset and duration parameters.
        /// </summary>
        /// <param name="value">Float representing an end_offset value.</param>
        /// <returns>The transformation with end offset percent defined.</returns>
        public Transformation EndOffsetPercent(float value)
        {
            return EndOffsetPercent((object)value);
        }

        /// <summary>
        /// Offset in percent of a video, used to specify the end of the video to be kept after trimming or
        /// when an overlay ends displaying.
        ///
        /// Normally used together with the start_offset and duration parameters.
        /// </summary>
        /// <param name="value">Double representing an end_offset value.</param>
        /// <returns>The transformation with end offset percent defined.</returns>
        public Transformation EndOffsetPercent(double value)
        {
            return EndOffsetPercent((object)value);
        }

        /// <summary>
        /// Set a shortcut to set video cutting using a combination of start_offset and end_offset values.
        ///
        /// Offset in seconds or percent of a video.
        /// </summary>
        /// <param name="value">String representing an offset value.</param>
        /// <returns>The transformation with offset defined.</returns>
        public Transformation Offset(string value)
        {
            return Add("offset", value);
        }

        /// <summary>
        /// Set a shortcut to set video cutting using a combination of start_offset and end_offset values.
        ///
        /// Offset in seconds or percent of a video.
        /// </summary>
        /// <param name="value">Range of strings representing an offset value.</param>
        /// <returns>The transformation with offset defined.</returns>
        public Transformation Offset(params string[] value)
        {
            if (value.Length < 2)
            {
                throw new ArgumentException("Offset range must include at least 2 items.");
            }

            return Add("offset", value);
        }

        /// <summary>
        /// Set a shortcut to set video cutting using a combination of start_offset and end_offset values.
        ///
        /// Offset in seconds or percent of a video.
        /// </summary>
        /// <param name="value">Range of floats representing an offset value.</param>
        /// <returns>The transformation with offset defined.</returns>
        public Transformation Offset(params float[] value)
        {
            if (value.Length < 2)
            {
                throw new ArgumentException("Offset range must include at least 2 items.");
            }

            var arr = new object[] { (object)value[0], (object)value[1] };
            return Offset(arr);
        }

        /// <summary>
        /// Set a shortcut to set video cutting using a combination of start_offset and end_offset values.
        ///
        /// Offset in seconds or percent of a video.
        /// </summary>
        /// <param name="value">Range of doubles representing an offset value.</param>
        /// <returns>The transformation with offset defined.</returns>
        public Transformation Offset(params double[] value)
        {
            if (value.Length < 2)
            {
                throw new ArgumentException("Offset range must include at least 2 items.");
            }

            var arr = new object[] { (object)value[0], (object)value[1] };
            return Offset(arr);
        }

        /// <summary>
        /// Set the duration the video/overlay displays.
        ///
        /// Offset in seconds or percent of a video, normally used together with the start_offset and end_offset
        /// parameters.
        /// </summary>
        /// <param name="value">String representing a duration value.</param>
        /// <returns>The transformation with duration defined.</returns>
        public Transformation Duration(string value)
        {
            return Add("duration", value);
        }

        /// <summary>
        /// Set the duration the video/overlay displays.
        ///
        /// Offset in seconds or percent of a video, normally used together with the start_offset and end_offset
        /// parameters.
        /// </summary>
        /// <param name="value">Float representing a duration value.</param>
        /// <returns>The transformation with duration defined.</returns>
        public Transformation Duration(float value)
        {
            return Add("duration", value);
        }

        /// <summary>
        /// Set the duration the video/overlay displays.
        ///
        /// Offset in seconds or percent of a video, normally used together with the start_offset and end_offset
        /// parameters.
        /// </summary>
        /// <param name="value">Double representing a duration value.</param>
        /// <returns>The transformation with duration defined.</returns>
        public Transformation Duration(double value)
        {
            return Add("duration", value);
        }

        /// <summary>
        /// Set the duration the video/overlay displays.
        ///
        /// Offset in percent of a video, normally used together with the start_offset and end_offset
        /// parameters.
        /// </summary>
        /// <param name="value">Float representing a duration value.</param>
        /// <returns>The transformation with duration percent defined.</returns>
        public Transformation DurationPercent(float value)
        {
            return DurationPercent((object)value);
        }

        /// <summary>
        /// Set the duration the video/overlay displays.
        ///
        /// Offset in percent of a video, normally used together with the start_offset and end_offset
        /// parameters.
        /// </summary>
        /// <param name="value">Double representing a duration value.</param>
        /// <returns>The transformation with duration percent defined.</returns>
        public Transformation DurationPercent(double value)
        {
            return DurationPercent((object)value);
        }

        /// <summary>
        /// Explicitly sets the keyframe interval (in seconds) of the delivered video.
        /// </summary>
        /// <param name="value">Int representing the time between keyframes.</param>
        /// <returns>The transformation with keyframe interval defined.</returns>
        public Transformation KeyframeInterval(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("The range for keyframe interval should be greater than 0.");
            }

            return Add("keyframe_interval", string.Format(CultureInfo.InvariantCulture, "{0:0.0}", value));
        }

        /// <summary>
        /// Explicitly sets the keyframe interval (in seconds) of the delivered video.
        /// </summary>
        /// <param name="value">Float representing the time between keyframes.</param>
        /// <returns>The transformation with keyframe interval defined.</returns>
        public Transformation KeyframeInterval(float value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("The range for keyframe interval should be greater than 0.");
            }

            return Add("keyframe_interval", value);
        }

        /// <summary>
        /// Explicitly sets the keyframe interval (in seconds) of the delivered video.
        /// </summary>
        /// <param name="value">String representing the time between keyframes.</param>
        /// <returns>The transformation with keyframe interval defined.</returns>
        public Transformation KeyframeInterval(string value)
        {
            return Add("keyframe_interval", value);
        }

        /// <summary>
        /// Set the name of the streaming profile to apply to an HLS or MPEG-DASH adaptive bitrate streaming video.
        /// </summary>
        /// <param name="value">The name of streaming profile.</param>
        /// <returns>The transformation with streaming profile applied.</returns>
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
                if (!@params.TryGetValue("codec", out s))
                {
                    return null;
                }

                sb.Append(s);
                if (@params.TryGetValue("profile", out s))
                {
                    sb.Append(':').Append(s);
                    if (@params.TryGetValue("level", out s))
                    {
                        sb.Append(':').Append(s);
                    }
                }
            }

            return sb.ToString();
        }

        private static string NormAutoRangeValue(object objectValue)
        {
            return objectValue != null && string.Equals(objectValue.ToString(), "auto", StringComparison.Ordinal)
                              ? objectValue.ToString()
                              : NormRangeValue(objectValue);
        }

        private static string NormRangeValue(object objectValue)
        {
            if (objectValue == null)
            {
                return null;
            }

            var value = ToString(objectValue);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var match = RangeValueRe.Match(value);
            if (!match.Success)
            {
                return null;
            }

            string modifier = string.Empty;
            if (match.Groups.Count == 3 && !string.IsNullOrEmpty(match.Groups[2].Value))
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
                if (RangeRe.IsMatch(rangeStr))
                {
                    return rangeStr.Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
                }
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

        /// <summary>
        /// Relevant when converting videos to animated GIF or WebP format. If not specified, the resulting GIF or WebP
        /// samples the whole video (up to 400 frames, at up to 10 frames per second).
        /// </summary>
        /// <param name="value">The number of seconds between each frame to sample from the original video.</param>
        private Transformation VideoSamplingSeconds(object value)
        {
            return Add("video_sampling", ToString(value) + "s");
        }

        /// <summary>
        /// Offset in percent of a video. The start of the video to be kept after trimming.
        /// </summary>
        /// <param name="value">Object representing a percent value.</param>
        private Transformation StartOffsetPercent(object value)
        {
            return Add("start_offset", ToString(value) + "p");
        }

        /// <summary>
        /// Offset in percent of a video, used to specify the end of the video to be kept after trimming or
        /// when an overlay ends displaying.
        ///
        /// Normally used together with the start_offset and duration parameters.
        /// </summary>
        /// <param name="value">Object representing an end_offset value.</param>
        private Transformation EndOffsetPercent(object value)
        {
            return Add("end_offset", ToString(value) + "p");
        }

        /// <summary>
        /// Set a shortcut to set video cutting using a combination of start_offset and end_offset values.
        ///
        /// Offset in seconds or percent of a video.
        /// </summary>
        /// <param name="value">Range of objects representing an offset value.</param>
        private Transformation Offset(params object[] value)
        {
            if (value.Length < 2)
            {
                throw new ArgumentException("Offset range must include at least 2 items.");
            }

            return Add("offset", value);
        }

        /// <summary>
        /// Set the duration the video/overlay displays.
        ///
        /// Offset in percent of a video, normally used together with the start_offset and end_offset
        /// parameters.
        /// </summary>
        /// <param name="value">Object representing a duration value.</param>
        private Transformation DurationPercent(object value)
        {
            return Add("duration", ToString(value) + "p");
        }
    }
}

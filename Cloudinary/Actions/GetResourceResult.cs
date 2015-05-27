using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class GetResourceResult : BaseResult
    {
        [JsonProperty(PropertyName = "resource_type")]
        protected string m_resourceType;

        [JsonProperty(PropertyName = "public_id")]
        public string PublicId { get; protected set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; protected set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; protected set; }

        [JsonIgnore]
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }

        [JsonProperty(PropertyName = "created_at")]
        public string Created { get; protected set; }

        [JsonProperty(PropertyName = "bytes")]
        public long Length { get; protected set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; protected set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; protected set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; protected set; }

        [JsonProperty(PropertyName = "secure_url")]
        public string SecureUrl { get; protected set; }

        [JsonProperty(PropertyName = "next_cursor")]
        public string NextCursor { get; protected set; }

        [JsonProperty(PropertyName = "exif")]
        public Dictionary<string, string> Exif { get; protected set; }

        [JsonProperty(PropertyName = "image_metadata")]
        public Dictionary<string, string> Metadata { get; protected set; }

        [JsonProperty(PropertyName = "faces")]
        public int[][] Faces { get; protected set; }

        [JsonProperty(PropertyName = "colors")]
        public string[][] Colors { get; protected set; }

        [JsonProperty(PropertyName = "derived")]
        public Derived[] Derived { get; protected set; }

        [JsonProperty(PropertyName = "tags")]
        public string[] Tags { get; protected set; }

        [JsonProperty(PropertyName = "moderation")]
        public List<Moderation> Moderation { get; protected set; }

        [JsonProperty(PropertyName = "context")]
        public JToken Context { get; protected set; }

        [JsonProperty(PropertyName = "phash")]
        public string Phash { get; protected set; }

        [JsonProperty(PropertyName = "predominant")]
        public Predominant Predominant { get; protected set; }

        [JsonProperty(PropertyName = "coordinates")]
        public Coordinates Coordinates { get; protected set; }

        [JsonProperty(PropertyName = "info")]
        public Info Info { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static GetResourceResult Parse(HttpWebResponse response)
        {
            return Parse<GetResourceResult>(response);
        }
    }

    //[DataContract]
    public class Coordinates
    {
        [JsonProperty(PropertyName = "custom")]
        public int[][] Custom { get; protected set; }

        [JsonProperty(PropertyName = "faces")]
        public int[][] Faces { get; protected set; }
    }

    //[DataContract]
    public class Predominant
    {
        [JsonProperty(PropertyName = "google")]
        public object[][] Google { get; protected set; }
    }

    //[DataContract]
    public class Derived
    {
        [JsonProperty(PropertyName = "transformation")]
        public string Transformation { get; protected set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; protected set; }

        [JsonProperty(PropertyName = "bytes")]
        public long Length { get; protected set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; protected set; }

        [JsonProperty(PropertyName = "secure_url")]
        public string SecureUrl { get; protected set; }
    }

    //[DataContract]
    public class Info
    {
        [JsonProperty(PropertyName = "detection")]
        public Detection Detection { get; protected set; }
    }

    //[DataContract]
    public class Detection
    {
        [JsonProperty(PropertyName = "rekognition_face")]
        public RekognitionFace RekognitionFace { get; protected set; }
    }

    //[DataContract]
    public class RekognitionFace
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; protected set; }

        [JsonProperty(PropertyName = "data")]
        public List<Face> Faces { get; protected set; }
    }

    //[DataContract]
    public class Face
    {
        [JsonProperty(PropertyName = "boundingbox")]
        public BoundingBox BoundingBox { get; protected set; }

        [JsonProperty(PropertyName = "confidence")]
        public double Confidence { get; protected set; }

        /// <summary>
        /// Estimated age of the person.
        /// </summary>
        [JsonProperty(PropertyName = "age")]
        public double Age { get; protected set; }

        /// <summary>
        /// 0.0 to 1.0 confidence score of whether the person is smiling.
        /// </summary>
        [JsonProperty(PropertyName = "smile")]
        public double Smile { get; protected set; }

        /// <summary>
        /// Score of whether the person is wearing glasses.
        /// </summary>
        [JsonProperty(PropertyName = "glasses")]
        public double Glasses { get; protected set; }

        [JsonProperty(PropertyName = "sunglasses")]
        public double Sunglasses { get; protected set; }

        [JsonProperty(PropertyName = "beard")]
        public double Beard { get; protected set; }

        [JsonProperty(PropertyName = "mustache")]
        public double Mustache { get; protected set; }

        /// <summary>
        /// Score of whether the eyes of the person are closed.
        /// </summary>
        [JsonProperty(PropertyName = "eye_closed")]
        public double EyeClosed { get; protected set; }

        /// <summary>
        /// Score of whether the mouse of the person is wide open.
        /// </summary>
        [JsonProperty(PropertyName = "mouth_open_wide")]
        public double MouthOpenWide { get; protected set; }

        [JsonProperty(PropertyName = "beauty")]
        public double Beauty { get; protected set; }

        /// <summary>
        /// Whether the person is a male or a female (high value towards 1 means male).
        /// </summary>
        [JsonProperty(PropertyName = "sex")]
        public double Gender { get; protected set; }

        [JsonProperty(PropertyName = "race")]
        public Dictionary<string, double> Race { get; protected set; }

        [JsonProperty(PropertyName = "emotion")]
        public Dictionary<string, double> Emotion { get; protected set; }

        [JsonProperty(PropertyName = "quality")]
        public Dictionary<string, double> Quality { get; protected set; }

        /// <summary>
        /// The way the face is positioned and 3D rotated.
        /// </summary>
        [JsonProperty(PropertyName = "pose")]
        public Dictionary<string, double> Pose { get; protected set; }

        /// <summary>
        /// Position of the left eye.
        /// </summary>
        [JsonProperty(PropertyName = "eye_left")]
        public Point EyeLeftPosition { get; protected set; }

        /// <summary>
        /// Position of the right eye.
        /// </summary>
        [JsonProperty(PropertyName = "eye_right")]
        public Point EyeRightPosition { get; protected set; }

        /// <summary>
        /// Left point of the left eye.
        /// </summary>
        [JsonProperty(PropertyName = "e_ll")]
        public Point EyeLeft_Left { get; protected set; }

        /// <summary>
        /// Right point of the left eye.
        /// </summary>
        [JsonProperty(PropertyName = "e_lr")]
        public Point EyeLeft_Right { get; protected set; }

        /// <summary>
        /// Up point of the left eye.
        /// </summary>
        [JsonProperty(PropertyName = "e_lu")]
        public Point EyeLeft_Up { get; protected set; }

        /// <summary>
        /// Down point of the left eye.
        /// </summary>
        [JsonProperty(PropertyName = "e_ld")]
        public Point EyeLeft_Down { get; protected set; }

        /// <summary>
        /// Left point of the right eye.
        /// </summary>
        [JsonProperty(PropertyName = "e_rl")]
        public Point EyeRight_Left { get; protected set; }

        /// <summary>
        /// Right point of the right eye.
        /// </summary>
        [JsonProperty(PropertyName = "e_rr")]
        public Point EyeRight_Right { get; protected set; }

        /// <summary>
        /// Up point of the right eye.
        /// </summary>
        [JsonProperty(PropertyName = "e_ru")]
        public Point EyeRight_Up { get; protected set; }

        /// <summary>
        /// Down point of the right eye.
        /// </summary>
        [JsonProperty(PropertyName = "e_rd")]
        public Point EyeRight_Down { get; protected set; }

        /// <summary>
        /// Position of the nose.
        /// </summary>
        [JsonProperty(PropertyName = "nose")]
        public Point NosePosition { get; protected set; }

        /// <summary>
        /// Left point of the nose.
        /// </summary>
        [JsonProperty(PropertyName = "n_l")]
        public Point NoseLeft { get; protected set; }

        /// <summary>
        /// Right point of the nose.
        /// </summary>
        [JsonProperty(PropertyName = "n_r")]
        public Point NoseRight { get; protected set; }

        /// <summary>
        /// Left point of the mouth.
        /// </summary>
        [JsonProperty(PropertyName = "mouth_l")]
        public Point MouthLeft { get; protected set; }

        /// <summary>
        /// Right point of the mouth.
        /// </summary>
        [JsonProperty(PropertyName = "mouth_r")]
        public Point MouthRight { get; protected set; }

        /// <summary>
        /// Up point of the mouth.
        /// </summary>
        [JsonProperty(PropertyName = "m_u")]
        public Point MouthUp { get; protected set; }

        /// <summary>
        /// Down point of the mouth.
        /// </summary>
        [JsonProperty(PropertyName = "m_d")]
        public Point MouthDown { get; protected set; }
    }

    //[DataContract]
    public class BoundingBox
    {
        [JsonProperty(PropertyName = "tl")]
        public Point TopLeft { get; protected set; }

        [JsonProperty(PropertyName = "size")]
        public Size Size { get; protected set; }
    }

    //[DataContract]
    public class Point
    {
        [JsonProperty(PropertyName = "x")]
        public double X { get; protected set; }

        [JsonProperty(PropertyName = "y")]
        public double Y { get; protected set; }
    }

    //[DataContract]
    public class Size
    {
        [JsonProperty(PropertyName = "width")]
        public double Width { get; protected set; }

        [JsonProperty(PropertyName = "height")]
        public double Height { get; protected set; }
    }
}

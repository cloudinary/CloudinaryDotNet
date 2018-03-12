using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class GetResourceResult : BaseResult
    {
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        [DataMember(Name = "created_at")]
        public string Created { get; protected set; }

        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        [DataMember(Name = "url")]
        public string Url { get; protected set; }

        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; protected set; }

        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }

        [DataMember(Name = "exif")]
        public Dictionary<string, string> Exif { get; protected set; }

        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> Metadata { get; protected set; }

        [DataMember(Name = "faces")]
        public int[][] Faces { get; protected set; }

        [DataMember(Name = "colors")]
        public string[][] Colors { get; protected set; }

        [DataMember(Name = "derived")]
        public Derived[] Derived { get; protected set; }

        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        [DataMember(Name = "moderation")]
        public List<Moderation> Moderation { get; protected set; }

        [DataMember(Name = "context")]
        public JToken Context { get; protected set; }

        [DataMember(Name = "phash")]
        public string Phash { get; protected set; }

        [DataMember(Name = "predominant")]
        public Predominant Predominant { get; protected set; }

        [DataMember(Name = "coordinates")]
        public Coordinates Coordinates { get; protected set; }

        [DataMember(Name = "info")]
        public Info Info { get; protected set; }
        
        [DataMember(Name = "access_control")]
        public List<AccessControlRule> AccessControl { get; protected set; }
        
    }

    [DataContract]
    public class Coordinates
    {
        [DataMember(Name = "custom")]
        public int[][] Custom { get; protected set; }

        [DataMember(Name = "faces")]
        public int[][] Faces { get; protected set; }
    }

    [DataContract]
    public class Predominant
    {
        [DataMember(Name = "google")]
        public object[][] Google { get; protected set; }
    }

    [DataContract]
    public class Derived
    {
        [DataMember(Name = "transformation")]
        public string Transformation { get; protected set; }

        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        [DataMember(Name = "id")]
        public string Id { get; protected set; }

        [DataMember(Name = "url")]
        public string Url { get; protected set; }

        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; protected set; }
    }

    [DataContract]
    public class Info
    {
        [DataMember(Name = "detection")]
        public Detection Detection { get; protected set; }
    }

    [DataContract]
    public class Detection
    {
        [DataMember(Name = "rekognition_face")]
        public RekognitionFace RekognitionFace { get; protected set; }
    }

    [DataContract]
    public class RekognitionFace
    {
        [DataMember(Name = "status")]
        public string Status { get; protected set; }

        [DataMember(Name = "data")]
        public List<Face> Faces { get; protected set; }
    }

    [DataContract]
    public class Face
    {
        [DataMember(Name = "boundingbox")]
        public BoundingBox BoundingBox { get; protected set; }

        [DataMember(Name = "confidence")]
        public double Confidence { get; protected set; }

        /// <summary>
        /// Estimated age of the person.
        /// </summary>
        [DataMember(Name = "age")]
        public double Age { get; protected set; }

        /// <summary>
        /// 0.0 to 1.0 confidence score of whether the person is smiling.
        /// </summary>
        [DataMember(Name = "smile")]
        public double Smile { get; protected set; }

        /// <summary>
        /// Score of whether the person is wearing glasses.
        /// </summary>
        [DataMember(Name = "glasses")]
        public double Glasses { get; protected set; }

        [DataMember(Name = "sunglasses")]
        public double Sunglasses { get; protected set; }

        [DataMember(Name = "beard")]
        public double Beard { get; protected set; }

        [DataMember(Name = "mustache")]
        public double Mustache { get; protected set; }

        /// <summary>
        /// Score of whether the eyes of the person are closed.
        /// </summary>
        [DataMember(Name = "eye_closed")]
        public double EyeClosed { get; protected set; }

        /// <summary>
        /// Score of whether the mouse of the person is wide open.
        /// </summary>
        [DataMember(Name = "mouth_open_wide")]
        public double MouthOpenWide { get; protected set; }

        [DataMember(Name = "beauty")]
        public double Beauty { get; protected set; }

        /// <summary>
        /// Whether the person is a male or a female (high value towards 1 means male).
        /// </summary>
        [DataMember(Name = "sex")]
        public double Gender { get; protected set; }

        [DataMember(Name = "race")]
        public Dictionary<string, double> Race { get; protected set; }

        [DataMember(Name = "emotion")]
        public Dictionary<string, double> Emotion { get; protected set; }

        [DataMember(Name = "quality")]
        public Dictionary<string, double> Quality { get; protected set; }

        /// <summary>
        /// The way the face is positioned and 3D rotated.
        /// </summary>
        [DataMember(Name = "pose")]
        public Dictionary<string, double> Pose { get; protected set; }

        /// <summary>
        /// Position of the left eye.
        /// </summary>
        [DataMember(Name = "eye_left")]
        public Point EyeLeftPosition { get; protected set; }

        /// <summary>
        /// Position of the right eye.
        /// </summary>
        [DataMember(Name = "eye_right")]
        public Point EyeRightPosition { get; protected set; }

        /// <summary>
        /// Left point of the left eye.
        /// </summary>
        [DataMember(Name = "e_ll")]
        public Point EyeLeft_Left { get; protected set; }

        /// <summary>
        /// Right point of the left eye.
        /// </summary>
        [DataMember(Name = "e_lr")]
        public Point EyeLeft_Right { get; protected set; }

        /// <summary>
        /// Up point of the left eye.
        /// </summary>
        [DataMember(Name = "e_lu")]
        public Point EyeLeft_Up { get; protected set; }

        /// <summary>
        /// Down point of the left eye.
        /// </summary>
        [DataMember(Name = "e_ld")]
        public Point EyeLeft_Down { get; protected set; }

        /// <summary>
        /// Left point of the right eye.
        /// </summary>
        [DataMember(Name = "e_rl")]
        public Point EyeRight_Left { get; protected set; }

        /// <summary>
        /// Right point of the right eye.
        /// </summary>
        [DataMember(Name = "e_rr")]
        public Point EyeRight_Right { get; protected set; }

        /// <summary>
        /// Up point of the right eye.
        /// </summary>
        [DataMember(Name = "e_ru")]
        public Point EyeRight_Up { get; protected set; }

        /// <summary>
        /// Down point of the right eye.
        /// </summary>
        [DataMember(Name = "e_rd")]
        public Point EyeRight_Down { get; protected set; }

        /// <summary>
        /// Position of the nose.
        /// </summary>
        [DataMember(Name = "nose")]
        public Point NosePosition { get; protected set; }

        /// <summary>
        /// Left point of the nose.
        /// </summary>
        [DataMember(Name = "n_l")]
        public Point NoseLeft { get; protected set; }

        /// <summary>
        /// Right point of the nose.
        /// </summary>
        [DataMember(Name = "n_r")]
        public Point NoseRight { get; protected set; }

        /// <summary>
        /// Left point of the mouth.
        /// </summary>
        [DataMember(Name = "mouth_l")]
        public Point MouthLeft { get; protected set; }

        /// <summary>
        /// Right point of the mouth.
        /// </summary>
        [DataMember(Name = "mouth_r")]
        public Point MouthRight { get; protected set; }

        /// <summary>
        /// Up point of the mouth.
        /// </summary>
        [DataMember(Name = "m_u")]
        public Point MouthUp { get; protected set; }

        /// <summary>
        /// Down point of the mouth.
        /// </summary>
        [DataMember(Name = "m_d")]
        public Point MouthDown { get; protected set; }
    }

    [DataContract]
    public class BoundingBox
    {
        [DataMember(Name = "tl")]
        public Point TopLeft { get; protected set; }

        [DataMember(Name = "size")]
        public Size Size { get; protected set; }
    }

    [DataContract]
    public class Point
    {
        [DataMember(Name = "x")]
        public double X { get; protected set; }

        [DataMember(Name = "y")]
        public double Y { get; protected set; }
    }

    [DataContract]
    public class Size
    {
        [DataMember(Name = "width")]
        public double Width { get; protected set; }

        [DataMember(Name = "height")]
        public double Height { get; protected set; }
    }
}

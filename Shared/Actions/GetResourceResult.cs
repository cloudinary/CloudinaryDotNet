using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed response with the detailed resource information.
    /// </summary>
    [DataContract]
    public class GetResourceResult : BaseResult
    {
        /// <summary>
        /// The type of file. Possible values: image, raw, video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// Public ID assigned to the resource.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// The format this resource delivered in. 
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        /// <summary>
        /// Current version of the resource.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        /// <summary>
        /// The type of resource. Possible values: image, raw, video. 
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// The storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// Date when the resource was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string Created { get; protected set; }

        /// <summary>
        /// Size of the resource in bytes.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        /// <summary>
        /// Parameter "width" of the resource. Not relevant for raw files. 
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Parameter "height" of the resource. Not relevant for raw files. 
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Url to the resource.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; protected set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; protected set; }

        /// <summary>
        /// When a listing request has more results to return than <see cref="GetResourceParams.MaxResults"/>, 
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <summary>
        /// Exif metadata of the resource.
        /// </summary>
        [DataMember(Name = "exif")]
        public Dictionary<string, string> Exif { get; protected set; }

        /// <summary>
        /// IPTC, XMP, and detailed Exif metadata. Supported for images, video, and audio.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> Metadata { get; protected set; }

        /// <summary>
        /// A list of coordinates of detected faces.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; protected set; }

        /// <summary>
        /// A quality analysis value for the image.
        /// </summary>
        [DataMember(Name = "quality_analysis")]
        public QualityAnalysis QualityAnalysis { get; protected set; }

        /// <summary>
        /// Color information: predominant colors and histogram of 32 leading colors. 
        /// </summary>
        [DataMember(Name = "colors")]
        public string[][] Colors { get; protected set; }

        /// <summary>
        /// A list of derived resources.
        /// </summary>
        [DataMember(Name = "derived")]
        public Derived[] Derived { get; protected set; }

        /// <summary>
        /// A list of tag names assigned to resource. 
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        /// <summary>
        /// Image moderation status of the resource.
        /// </summary>
        [DataMember(Name = "moderation")]
        public List<Moderation> Moderation { get; protected set; }

        /// <summary>
        /// A key-value pairs of context associated with the resource. 
        /// </summary>
        [DataMember(Name = "context")]
        public JToken Context { get; protected set; }

        /// <summary>
        /// A perceptual hash (pHash) of the uploaded resource for image similarity detection.
        /// </summary>
        [DataMember(Name = "phash")]
        public string Phash { get; protected set; }

        /// <summary>
        /// The predominant colors in the image according to both a Google palette and a Cloudinary palette.
        /// </summary>
        [DataMember(Name = "predominant")]
        public Predominant Predominant { get; protected set; }

        /// <summary>
        /// The coordinates of a single region contained in an image that is subsequently used for cropping the image using
        /// the custom gravity mode.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public Coordinates Coordinates { get; protected set; }

        /// <summary>
        /// Any requested information from executing one of the Cloudinary Add-ons on the media asset.
        /// </summary>
        [DataMember(Name = "info")]
        public Info Info { get; protected set; }
        
        /// <summary>
        /// Parameters of the asset access management.
        /// </summary>
        [DataMember(Name = "access_control")]
        public List<AccessControlRule> AccessControl { get; protected set; }
    }

    /// <summary>
    /// The coordinates of a single region contained in an image that is subsequently used for cropping the image using
    /// the custom gravity mode. 
    /// </summary>
    [DataContract]
    public class Coordinates
    {
        /// <summary>
        /// A list of custom coordinates.
        /// </summary>
        [DataMember(Name = "custom")]
        public int[][] Custom { get; protected set; }

        /// <summary>
        /// A list of coordinates of detected faces. 
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; protected set; }
    }

    /// <summary>
    /// The predominant colors in the image according to both a Google palette and a Cloudinary palette.
    /// </summary>
    [DataContract]
    public class Predominant
    {
        /// <summary>
        /// Google palette details.
        /// </summary>
        [DataMember(Name = "google")]
        public object[][] Google { get; protected set; }
    }

    /// <summary>
    /// The list of derived assets generated (and cached) from the original media asset, including the transformation
    /// applied, size and URL for accessing the derived media asset.
    /// </summary>
    [DataContract]
    public class Derived
    {
        /// <summary>
        /// The transformation applied to the asset.
        /// </summary>
        [DataMember(Name = "transformation")]
        public string Transformation { get; protected set; }

        /// <summary>
        /// Format of the derived asset.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        /// <summary>
        /// Size of the derived asset.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        /// <summary>
        /// Id of the derived resource.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; protected set; }

        /// <summary>
        /// URL for accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; protected set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; protected set; }
    }

    /// <summary>
    /// Any requested information from executing one of the Cloudinary Add-ons on the media asset.
    /// </summary>
    [DataContract]
    public class Info
    {
        /// <summary>
        /// Requested information from executing a Rekognition face add-ons.
        /// </summary>
        [DataMember(Name = "detection")]
        public Detection Detection { get; protected set; }

        /// <summary>
        /// Requested information from executing an OCR add-ons.
        /// </summary>
        [DataMember(Name = "ocr")]
        public Ocr Ocr { get; protected set; }
    }

    #region OCR
    /// <summary>
    /// Details of executing an OCR add-on.
    /// </summary>
    [DataContract]
    public class Ocr
    {
        /// <summary>
        /// Details of executing an ADV_OCR engine.
        /// </summary>
        [DataMember(Name = "adv_ocr")] 
        public AdvOcr AdvOcr { get; protected set; }
    }

    /// <summary>
    /// Details of executing an ADV_OCR engine.
    /// </summary>
    [DataContract]
    public class AdvOcr
    {
        /// <summary>
        /// The status of the OCR operation.
        /// </summary>
        [DataMember(Name = "status")] 
        public string Status { get; protected set; }

        /// <summary>
        /// Data returned by OCR plugin.
        /// </summary>
        [DataMember (Name = "data")]
        public List<AdvOcrData> Data { get; protected set; }
    }

    /// <summary>
    /// Data returned by OCR plugin.
    /// </summary>
    [DataContract]
    public class AdvOcrData
    {
        /// <summary>
        /// Annotations of the recognized text.
        /// </summary>
        [DataMember(Name = "textAnnotations")]
        public List<TextAnnotation> TextAnnotations { get; protected set; }

        /// <summary>
        /// This annotation provides the structural hierarchy for the OCR detected text. 
        /// If present, text (OCR) detection or document (OCR) text detection has completed successfully.
        /// </summary>
        [DataMember(Name = "fullTextAnnotation")]
        public FullTextAnnotation FullTextAnnotation { get; protected set; }
    }

    /// <summary>
    /// TextAnnotation contains a structured representation of OCR extracted text. 
    /// </summary>
    [DataContract]
    public class TextAnnotation
    {
        /// <summary>
        /// The detected locale of the text.
        /// </summary>
        [DataMember(Name = "locale")]
        public string Locale { get; protected set; }

        /// <summary>
        /// A description listing the entirety of the detected text content, with a newline character (\n) separating
        /// groups of text.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; protected set; }

        /// <summary>
        /// The outer bounding polygon for the detected image annotation.
        /// </summary>
        [DataMember (Name="boundingPoly")]
        public BoundingBlock BoundingPoly { get; protected set; }
    }

    /// <summary>
    /// The outer bounding polygon for the detected image annotation.
    /// </summary>
    [DataContract]
    public class BoundingBlock
    {
        /// <summary>
        /// The bounding polygon vertices.
        /// </summary>
        [DataMember(Name = "vertices")]
        public List<Point> Vertices { get; private set; }
    }

    /// <summary>
    /// For multi-page files (e.g. PDFs), a node indicating the containing page.
    /// </summary>
    [DataContract]
    public class FullTextAnnotation
    {
        /// <summary>
        /// A list of detected pages.
        /// </summary>
        [DataMember(Name = "pages")]
        public List<Page> Pages { get; protected set; }
        
        /// <summary>
        /// Recognized text.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; protected set; }
    }

    /// <summary>
    /// Detected page from OCR.
    /// </summary>
    [DataContract]
    public class Page
    {
        /// <summary>
        /// Additional information detected on the page.
        /// </summary>
        [DataMember(Name = "property")]
        public PageProperty Property { get; protected set; }

        /// <summary>
        /// Page width. For PDFs the unit is points. For images (including TIFFs) the unit is pixels.
        /// </summary>
        [DataMember(Name = "width")]
        public int? Width { get; protected set; }

        /// <summary>
        /// Page height. For PDFs the unit is points. For images (including TIFFs) the unit is pixels.
        /// </summary>
        [DataMember(Name = "height")]
        public int? Height { get; protected set; }

        /// <summary>
        /// List of text blocks on this page.
        /// </summary>
        [DataMember(Name = "blocks")]
        public List<TextBlock> Blocks { get; protected set; }
    }

    /// <summary>
    /// Additional information detected on the page.
    /// </summary>
    [DataContract]
    public class PageProperty
    {
        /// <summary>
        /// A list of detected languages together with confidence. 
        /// </summary>
        [DataMember(Name = "detectedLanguages")]
        public List<DetectedLanguage> DetectedLanguages { get; protected set; }
    }

    /// <summary>
    /// Detected language for a structural component.
    /// </summary>
    [DataContract]
    public class DetectedLanguage
    {
        /// <summary>
        /// The BCP-47 language code, such as "en-US" or "sr-Latn".
        /// For more information, see http://www.unicode.org/reports/tr35/#Unicode_locale_identifier. 
        /// </summary>
        [DataMember(Name = "languageCode")]
        public string LanguageCode { get; protected set; }
    }

    /// <summary>
    /// Logical element on the page.
    /// </summary>
    [DataContract]
    public abstract class Block
    {
        /// <summary>
        /// Additional information detected on the page.
        /// </summary>
        [DataMember(Name = "property")]
        public PageProperty Property { get; protected set; }

        /// <summary>
        /// The bounding box for the block. 
        /// The vertices are in the order of top-left, top-right, bottom-right, bottom-left.
        /// </summary>
        [DataMember(Name = "boundingBox")]
        public BoundingBlock BoundingBox { get; protected set; }
    }

    /// <summary>
    /// A text element on the page.
    /// </summary>
    [DataContract]
    public class TextBlock : Block
    {
        /// <summary>
        /// List of paragraphs in this block. 
        /// </summary>
        [DataMember(Name = "paragraphs")] 
        public List<Paragraph> Paragraphs { get; protected set; }

        /// <summary>
        /// Detected block type (text, image etc) for this block.
        /// </summary>
        [DataMember(Name = "blockType")] 
        public string BlockType { get; protected set; }
    }

    /// <summary>
    /// Structural unit of text representing a number of words in certain order.
    /// </summary>
    [DataContract]
    public class Paragraph : Block
    {
        /// <summary>
        /// List of words in this paragraph. 
        /// </summary>
        [DataMember(Name = "words")]
        public List<Word> Words { get; protected set; }

        /// <summary>
        /// The actual text representation of this paragraph. 
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; protected set; }
    }

    /// <summary>
    /// A word representation.
    /// </summary>
    [DataContract]
    public class Word : Block
    {
        /// <summary>
        /// List of symbols in the word. The order of the symbols follows the natural reading order. 
        /// </summary>
        [DataMember(Name = "symbols")]
        public List<Symbol> Symbols { get; protected set; }
    }

    /// <summary>
    /// A single symbol representation.
    /// </summary>
    [DataContract]
    public class Symbol : Block
    {
        /// <summary>
        /// The actual UTF-8 representation of the symbol. 
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; protected set; }
    }

    #endregion

    #region Rekognition face

    /// <summary>
    /// Requested information from executing a Rekognition face add-on.
    /// </summary>
    [DataContract]
    public class Detection
    {
        /// <summary>
        /// Details of the result of recognition.
        /// </summary>
        [DataMember(Name = "rekognition_face")]
        public RekognitionFace RekognitionFace { get; protected set; }
    }

    /// <summary>
    /// Details of each face found in the image. 
    /// </summary>
    [DataContract]
    public class RekognitionFace
    {
        /// <summary>
        /// Status of the recognition process.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; protected set; }

        /// <summary>
        /// Properties of each detected face.
        /// </summary>
        [DataMember(Name = "data")]
        public List<Face> Faces { get; protected set; }
    }

    /// <summary>
    /// Structure containing attributes of the face that the algorithm detected.
    /// </summary>
    [DataContract]
    public class Face
    {
        /// <summary>
        /// Bounding box of the face.
        /// </summary>
        [DataMember(Name = "boundingbox")]
        public BoundingBox BoundingBox { get; protected set; }

        /// <summary>
        /// Confidence level that the bounding box contains a face (and not a different object such as a tree). 
        /// Valid Range: Minimum value of 0. Maximum value of 100.
        /// </summary>
        [DataMember(Name = "confidence")]
        public double Confidence { get; protected set; }

        /// <summary>
        /// Estimated age of the person.
        /// </summary>
        [DataMember(Name = "age")]
        public double Age { get; protected set; }

        /// <summary>
        /// Indicates whether or not the face is smiling, and the confidence level in the determination.
        /// Float values: 0.0 to 1.0.
        /// </summary>
        [DataMember(Name = "smile")]
        public double Smile { get; protected set; }

        /// <summary>
        /// Score of whether the person is wearing glasses.
        /// </summary>
        [DataMember(Name = "glasses")]
        public double Glasses { get; protected set; }

        /// <summary>
        /// Indicates the confidence level whether the face is wearing sunglasses.
        /// </summary>
        [DataMember(Name = "sunglasses")]
        public double Sunglasses { get; protected set; }

        /// <summary>
        /// Indicates the confidence level whether the face has beard.
        /// </summary>
        [DataMember(Name = "beard")]
        public double Beard { get; protected set; }

        /// <summary>
        /// Indicates the confidence level whether the face has a mustache.
        /// </summary>
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

        /// <summary>
        /// Score of whether the detected face of the person is treated as beautiful.
        /// </summary>
        [DataMember(Name = "beauty")]
        public double Beauty { get; protected set; }

        /// <summary>
        /// Whether the person is a male or a female (high value towards 1 means male).
        /// </summary>
        [DataMember(Name = "sex")]
        public double Gender { get; protected set; }

        /// <summary>
        /// Detected data about the person's race.
        /// </summary>
        [DataMember(Name = "race")]
        public Dictionary<string, double> Race { get; protected set; }

        /// <summary>
        /// The emotions detected on the face, and the confidence level in the determination. 
        /// For example, HAPPY, SAD, and ANGRY. 
        /// </summary>
        [DataMember(Name = "emotion")]
        public Dictionary<string, double> Emotion { get; protected set; }

        /// <summary>
        /// Identifies image brightness and sharpness.
        /// </summary>
        [DataMember(Name = "quality")]
        public Dictionary<string, double> Quality { get; protected set; }

        /// <summary>
        /// Indicates the pose of the face as determined by its pitch, roll, and yaw.
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

    /// <summary>
    /// Identifies the bounding box around the face.
    /// </summary>
    [DataContract]
    public class BoundingBox
    {
        /// <summary>
        /// Top left point of the bounding box.
        /// </summary>
        [DataMember(Name = "tl")]
        public Point TopLeft { get; protected set; }

        /// <summary>
        /// Size of the bounding box.
        /// </summary>
        [DataMember(Name = "size")]
        public Size Size { get; protected set; }
    }

    /// <summary>
    /// Point, represented by X and Y coordinates.
    /// </summary>
    [DataContract]
    public class Point
    {
        /// <summary>
        /// X - coordinate.
        /// </summary>
        [DataMember(Name = "x")]
        public double X { get; protected set; }

        /// <summary>
        /// Y - coordinate.
        /// </summary>
        [DataMember(Name = "y")]
        public double Y { get; protected set; }
    }

    /// <summary>
    /// A size of the block, represented by width and height.
    /// </summary>
    [DataContract]
    public class Size
    {
        /// <summary>
        /// Width of the block.
        /// </summary>
        [DataMember(Name = "width")]
        public double Width { get; protected set; }

        /// <summary>
        /// Width of the block.
        /// </summary>
        [DataMember(Name = "height")]
        public double Height { get; protected set; }
    }

    #endregion
    
    /// <summary>
    /// Details of the quality analysis.
    /// </summary>
    [DataContract]
    public class QualityAnalysis
    {
        /// <summary>
        /// Focus value.
        /// </summary>
        [DataMember(Name = "focus")]
        public double Focus { get; protected set; }
    }
}

namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

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
        /// Gets or sets public ID assigned to the resource.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the format this resource is delivered in.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets current version of the resource.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets the type of resource. Possible values: image, raw, video.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// Gets or sets the storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets date when the resource was created.
        /// </summary>
        [Obsolete("Property Created is deprecated, please use CreatedAt instead")]
        public string Created
        {
            get { return CreatedAt; }
            set { CreatedAt = value; }
        }

        /// <summary>
        /// Gets or sets date when the resource was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets size of the resource in bytes.
        /// </summary>
        [Obsolete("Property Length is deprecated, please use Bytes instead")]
        public long Length
        {
            get { return Bytes; }
            set { Bytes = value; }
        }

        /// <summary>
        /// Gets or sets size of the resource in bytes.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets parameter "width" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets parameter "height" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets URL to the resource.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets when a listing request has more results to return than <see cref="GetResourceParams.MaxResults"/>,
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }

        /// <summary>
        /// Gets or sets if there are more derived images than <see cref="GetResourceParams.MaxResults"/>,
        /// the <see cref="DerivedNextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="DerivedNextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "derived_next_cursor")]
        public string DerivedNextCursor { get; set; }

        /// <summary>
        /// Gets or sets exif metadata of the resource.
        /// </summary>
        [DataMember(Name = "exif")]
        public Dictionary<string, string> Exif { get; set; }

        /// <summary>
        /// Gets or sets iPTC, XMP, and detailed Exif metadata. Supported for images, video, and audio.
        /// </summary>
        [Obsolete("Property Metadata is deprecated, please use ImageMetadata instead")]
        public Dictionary<string, string> Metadata
        {
            get { return ImageMetadata; }
            set { ImageMetadata = value; }
        }

        /// <summary>
        /// Gets or sets iPTC, XMP, and detailed Exif metadata. Supported for images, video, and audio.
        /// </summary>
        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> ImageMetadata { get; set; }

        /// <summary>
        /// Gets or sets a list of coordinates of detected faces.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; set; }

        /// <summary>
        /// Gets or sets a quality analysis value for the image.
        /// </summary>
        [DataMember(Name = "quality_analysis")]
        public QualityAnalysis QualityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the quality score.
        /// </summary>
        [DataMember(Name = "quality_score")]
        public double QualityScore { get; set; }

        /// <summary>
        /// Gets or sets color information: predominant colors and histogram of 32 leading colors.
        /// </summary>
        [DataMember(Name = "colors")]
        public string[][] Colors { get; set; }

        /// <summary>
        /// Gets or sets a list of derived resources.
        /// </summary>
        [DataMember(Name = "derived")]
        public Derived[] Derived { get; set; }

        /// <summary>
        /// Gets or sets a list of tag names assigned to resource.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets image moderation status of the resource.
        /// </summary>
        [DataMember(Name = "moderation")]
        public List<Moderation> Moderation { get; set; }

        /// <summary>
        /// Gets or sets a key-value pairs of context associated with the resource.
        /// </summary>
        [DataMember(Name = "context")]
        public JToken Context { get; set; }

        /// <summary>
        /// Gets or sets a key-value pairs of custom metadata fields associated with the resource.
        /// </summary>
        [DataMember(Name = "metadata")]
        public JToken MetadataFields { get; set; }

        /// <summary>
        /// Gets or sets a perceptual hash (pHash) of the uploaded resource for image similarity detection.
        /// </summary>
        [DataMember(Name = "phash")]
        public string Phash { get; set; }

        /// <summary>
        /// Gets or sets the predominant colors in the image according to both a Google palette and a Cloudinary palette.
        /// </summary>
        [DataMember(Name = "predominant")]
        public Predominant Predominant { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of a single region contained in an image that is subsequently used for cropping the image using
        /// the custom gravity mode.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public Coordinates Coordinates { get; set; }

        /// <summary>
        /// Gets or sets any requested information from executing one of the Cloudinary Add-ons on the media asset.
        /// </summary>
        [DataMember(Name = "info")]
        public Info Info { get; set; }

        /// <summary>
        /// Gets or sets parameters of the asset access management.
        /// </summary>
        [DataMember(Name = "access_control")]
        public List<AccessControlRule> AccessControl { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the asset: included if the asset has multiple pages (e.g., PDF or animated GIF).
        /// </summary>
        [DataMember(Name = "pages")]
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets the accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; set; }

        /// <summary>
        /// Gets or sets details of cinemagraph analysis for the resource.
        /// </summary>
        [DataMember(Name = "cinemagraph_analysis")]
        public CinemagraphAnalysis CinemagraphAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the color ambiguity score that indicate how good\bad an image is for colorblind people.
        /// Will they be able to differentiate between different elements in the image.
        /// </summary>
        [DataMember(Name = "accessibility_analysis")]
        public AccessibilityAnalysis AccessibilityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets asset identifier.
        /// </summary>
        [DataMember(Name = "asset_id")]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets list of asset versions.
        /// </summary>
        [DataMember(Name = "versions")]
        public List<AssetVersion> Versions { get; set; }
    }

    /// <summary>
    /// The coordinates of a single region contained in an image that is subsequently used for cropping the image using
    /// the custom gravity mode.
    /// </summary>
    [DataContract]
    public class Coordinates
    {
        /// <summary>
        /// Gets or sets a list of custom coordinates.
        /// </summary>
        [DataMember(Name = "custom")]
        public int[][] Custom { get; set; }

        /// <summary>
        /// Gets or sets a list of coordinates of detected faces.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; set; }
    }

    /// <summary>
    /// The predominant colors in the image according to both a Google palette and a Cloudinary palette.
    /// </summary>
    [DataContract]
    public class Predominant
    {
        /// <summary>
        /// Gets or sets google palette details.
        /// </summary>
        [DataMember(Name = "google")]
        public object[][] Google { get; set; }

        /// <summary>
        /// Gets or sets cloudinary palette details.
        /// </summary>
        [DataMember(Name = "cloudinary")]
        public object[][] Cloudinary { get; set; }
    }

    /// <summary>
    /// The list of derived assets generated (and cached) from the original media asset, including the transformation
    /// applied, size and URL for accessing the derived media asset.
    /// </summary>
    [DataContract]
    public class Derived
    {
        /// <summary>
        /// Gets or sets the transformation applied to the asset.
        /// </summary>
        [DataMember(Name = "transformation")]
        public string Transformation { get; set; }

        /// <summary>
        /// Gets or sets format of the derived asset.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets size of the derived asset.
        /// </summary>
        [Obsolete("Property Length is deprecated, please use Bytes instead")]
        public long Length
        {
            get { return Bytes; }
            set { Bytes = value; }
        }

        /// <summary>
        /// Gets or sets size of the derived asset.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets id of the derived resource.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets uRL for accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; set; }
    }

    /// <summary>
    /// Any requested information from executing one of the Cloudinary Add-ons on the media asset.
    /// </summary>
    [DataContract]
    public class Info
    {
        /// <summary>
        /// Gets or sets requested information from executing a Rekognition face add-ons.
        /// </summary>
        [DataMember(Name = "detection")]
        public Detection Detection { get; set; }

        /// <summary>
        /// Gets or sets requested information from executing an OCR add-ons.
        /// </summary>
        [DataMember(Name = "ocr")]
        public Ocr Ocr { get; set; }
    }

    /// <summary>
    /// Details of executing an OCR add-on.
    /// </summary>
    [DataContract]
    public class Ocr
    {
        /// <summary>
        /// Gets or sets details of executing an ADV_OCR engine.
        /// </summary>
        [DataMember(Name = "adv_ocr")]
        public AdvOcr AdvOcr { get; set; }
    }

    /// <summary>
    /// Details of executing an ADV_OCR engine.
    /// </summary>
    [DataContract]
    public class AdvOcr
    {
        /// <summary>
        /// Gets or sets the status of the OCR operation.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets data returned by OCR plugin.
        /// </summary>
        [DataMember(Name = "data")]
        public List<AdvOcrData> Data { get; set; }
    }

    /// <summary>
    /// Data returned by OCR plugin.
    /// </summary>
    [DataContract]
    public class AdvOcrData
    {
        /// <summary>
        /// Gets or sets annotations of the recognized text.
        /// </summary>
        [DataMember(Name = "textAnnotations")]
        public List<TextAnnotation> TextAnnotations { get; set; }

        /// <summary>
        /// Gets or sets this annotation provides the structural hierarchy for the OCR detected text.
        /// If present, text (OCR) detection or document (OCR) text detection has completed successfully.
        /// </summary>
        [DataMember(Name = "fullTextAnnotation")]
        public FullTextAnnotation FullTextAnnotation { get; set; }
    }

    /// <summary>
    /// TextAnnotation contains a structured representation of OCR extracted text.
    /// </summary>
    [DataContract]
    public class TextAnnotation
    {
        /// <summary>
        /// Gets or sets the detected locale of the text.
        /// </summary>
        [DataMember(Name = "locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets a description listing the entirety of the detected text content, with a newline character (\n) separating
        /// groups of text.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the outer bounding polygon for the detected image annotation.
        /// </summary>
        [DataMember(Name="boundingPoly")]
        public BoundingBlock BoundingPoly { get; set; }
    }

    /// <summary>
    /// The outer bounding polygon for the detected image annotation.
    /// </summary>
    [DataContract]
    public class BoundingBlock
    {
        /// <summary>
        /// Gets or sets the bounding polygon vertices.
        /// </summary>
        [DataMember(Name = "vertices")]
        public List<Point> Vertices { get; set; }
    }

    /// <summary>
    /// For multi-page files (e.g. PDFs), a node indicating the containing page.
    /// </summary>
    [DataContract]
    public class FullTextAnnotation
    {
        /// <summary>
        /// Gets or sets a list of detected pages.
        /// </summary>
        [DataMember(Name = "pages")]
        public List<Page> Pages { get; set; }

        /// <summary>
        /// Gets or sets recognized text.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

    /// <summary>
    /// Detected page from OCR.
    /// </summary>
    [DataContract]
    public class Page
    {
        /// <summary>
        /// Gets or sets additional information detected on the page.
        /// </summary>
        [DataMember(Name = "property")]
        public PageProperty Property { get; set; }

        /// <summary>
        /// Gets or sets page width. For PDFs the unit is points. For images (including TIFFs) the unit is pixels.
        /// </summary>
        [DataMember(Name = "width")]
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets page height. For PDFs the unit is points. For images (including TIFFs) the unit is pixels.
        /// </summary>
        [DataMember(Name = "height")]
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets list of text blocks on this page.
        /// </summary>
        [DataMember(Name = "blocks")]
        public List<TextBlock> Blocks { get; set; }
    }

    /// <summary>
    /// Additional information detected on the page.
    /// </summary>
    [DataContract]
    public class PageProperty
    {
        /// <summary>
        /// Gets or sets a list of detected languages together with confidence.
        /// </summary>
        [DataMember(Name = "detectedLanguages")]
        public List<DetectedLanguage> DetectedLanguages { get; set; }
    }

    /// <summary>
    /// Detected language for a structural component.
    /// </summary>
    [DataContract]
    public class DetectedLanguage
    {
        /// <summary>
        /// Gets or sets the BCP-47 language code, such as "en-US" or "sr-Latn".
        /// For more information, see http://www.unicode.org/reports/tr35/#Unicode_locale_identifier.
        /// </summary>
        [DataMember(Name = "languageCode")]
        public string LanguageCode { get; set; }
    }

    /// <summary>
    /// Logical element on the page.
    /// </summary>
    [DataContract]
    public abstract class Block
    {
        /// <summary>
        /// Gets or sets additional information detected on the page.
        /// </summary>
        [DataMember(Name = "property")]
        public PageProperty Property { get; set; }

        /// <summary>
        /// Gets or sets the bounding box for the block.
        /// The vertices are in the order of top-left, top-right, bottom-right, bottom-left.
        /// </summary>
        [DataMember(Name = "boundingBox")]
        public BoundingBlock BoundingBox { get; set; }
    }

    /// <summary>
    /// A text element on the page.
    /// </summary>
    [DataContract]
    public class TextBlock : Block
    {
        /// <summary>
        /// Gets or sets list of paragraphs in this block.
        /// </summary>
        [DataMember(Name = "paragraphs")]
        public List<Paragraph> Paragraphs { get; set; }

        /// <summary>
        /// Gets or sets detected block type (text, image etc) for this block.
        /// </summary>
        [DataMember(Name = "blockType")]
        public string BlockType { get; set; }
    }

    /// <summary>
    /// Structural unit of text representing a number of words in certain order.
    /// </summary>
    [DataContract]
    public class Paragraph : Block
    {
        /// <summary>
        /// Gets or sets list of words in this paragraph.
        /// </summary>
        [DataMember(Name = "words")]
        public List<Word> Words { get; set; }

        /// <summary>
        /// Gets or sets the actual text representation of this paragraph.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

    /// <summary>
    /// A word representation.
    /// </summary>
    [DataContract]
    public class Word : Block
    {
        /// <summary>
        /// Gets or sets list of symbols in the word. The order of the symbols follows the natural reading order.
        /// </summary>
        [DataMember(Name = "symbols")]
        public List<Symbol> Symbols { get; set; }
    }

    /// <summary>
    /// A single symbol representation.
    /// </summary>
    [DataContract]
    public class Symbol : Block
    {
        /// <summary>
        /// Gets or sets the actual UTF-8 representation of the symbol.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

    /// <summary>
    /// Requested information from executing a Rekognition face add-on.
    /// </summary>
    [DataContract]
    public class Detection
    {
        /// <summary>
        /// Gets or sets details of the result of recognition.
        /// </summary>
        [DataMember(Name = "rekognition_face")]
        public RekognitionFace RekognitionFace { get; set; }
    }

    /// <summary>
    /// Details of each face found in the image.
    /// </summary>
    [DataContract]
    public class RekognitionFace
    {
        /// <summary>
        /// Gets or sets status of the recognition process.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets properties of each detected face.
        /// </summary>
        [DataMember(Name = "data")]
        public List<Face> Faces { get; set; }
    }

    /// <summary>
    /// Structure containing attributes of the face that the algorithm detected.
    /// </summary>
    [DataContract]
    public class Face
    {
        /// <summary>
        /// Gets or sets bounding box of the face.
        /// </summary>
        [DataMember(Name = "boundingbox")]
        public BoundingBox BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets confidence level that the bounding box contains a face (and not a different object such as a tree).
        /// Valid Range: Minimum value of 0. Maximum value of 100.
        /// </summary>
        [DataMember(Name = "confidence")]
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets estimated age of the person.
        /// </summary>
        [DataMember(Name = "age")]
        public double Age { get; set; }

        /// <summary>
        /// Gets or sets indication whether or not the face is smiling, and the confidence level in the determination.
        /// Float values: 0.0 to 1.0.
        /// </summary>
        [DataMember(Name = "smile")]
        public double Smile { get; set; }

        /// <summary>
        /// Gets or sets score of whether the person is wearing glasses.
        /// </summary>
        [DataMember(Name = "glasses")]
        public double Glasses { get; set; }

        /// <summary>
        /// Gets or sets indication the confidence level whether the face is wearing sunglasses.
        /// </summary>
        [DataMember(Name = "sunglasses")]
        public double Sunglasses { get; set; }

        /// <summary>
        /// Gets or sets indication the confidence level whether the face has beard.
        /// </summary>
        [DataMember(Name = "beard")]
        public double Beard { get; set; }

        /// <summary>
        /// Gets or sets indication the confidence level whether the face has a mustache.
        /// </summary>
        [DataMember(Name = "mustache")]
        public double Mustache { get; set; }

        /// <summary>
        /// Gets or sets score of whether the eyes of the person are closed.
        /// </summary>
        [DataMember(Name = "eye_closed")]
        public double EyeClosed { get; set; }

        /// <summary>
        /// Gets or sets score of whether the mouse of the person is wide open.
        /// </summary>
        [DataMember(Name = "mouth_open_wide")]
        public double MouthOpenWide { get; set; }

        /// <summary>
        /// Gets or sets score of whether the detected face of the person is treated as beautiful.
        /// </summary>
        [DataMember(Name = "beauty")]
        public double Beauty { get; set; }

        /// <summary>
        /// Gets or sets whether the person is a male or a female (high value towards 1 means male).
        /// </summary>
        [DataMember(Name = "sex")]
        public double Gender { get; set; }

        /// <summary>
        /// Gets or sets detected data about the person's race.
        /// </summary>
        [DataMember(Name = "race")]
        public Dictionary<string, double> Race { get; set; }

        /// <summary>
        /// Gets or sets the emotions detected on the face, and the confidence level in the determination.
        /// For example, HAPPY, SAD, and ANGRY.
        /// </summary>
        [DataMember(Name = "emotion")]
        public Dictionary<string, double> Emotion { get; set; }

        /// <summary>
        /// Gets or sets identifies image brightness and sharpness.
        /// </summary>
        [DataMember(Name = "quality")]
        public Dictionary<string, double> Quality { get; set; }

        /// <summary>
        /// Gets or sets indication the pose of the face as determined by its pitch, roll, and yaw.
        /// </summary>
        [DataMember(Name = "pose")]
        public Dictionary<string, double> Pose { get; set; }

        /// <summary>
        /// Gets or sets position of the left eye.
        /// </summary>
        [DataMember(Name = "eye_left")]
        public Point EyeLeftPosition { get; set; }

        /// <summary>
        /// Gets or sets position of the right eye.
        /// </summary>
        [DataMember(Name = "eye_right")]
        public Point EyeRightPosition { get; set; }

        /// <summary>
        /// Gets or sets left point of the left eye.
        /// </summary>
        [DataMember(Name = "e_ll")]
        public Point EyeLeft_Left { get; set; }

        /// <summary>
        /// Gets or sets right point of the left eye.
        /// </summary>
        [DataMember(Name = "e_lr")]
        public Point EyeLeft_Right { get; set; }

        /// <summary>
        /// Gets or sets up point of the left eye.
        /// </summary>
        [DataMember(Name = "e_lu")]
        public Point EyeLeft_Up { get; set; }

        /// <summary>
        /// Gets or sets down point of the left eye.
        /// </summary>
        [DataMember(Name = "e_ld")]
        public Point EyeLeft_Down { get; set; }

        /// <summary>
        /// Gets or sets left point of the right eye.
        /// </summary>
        [DataMember(Name = "e_rl")]
        public Point EyeRight_Left { get; set; }

        /// <summary>
        /// Gets or sets right point of the right eye.
        /// </summary>
        [DataMember(Name = "e_rr")]
        public Point EyeRight_Right { get; set; }

        /// <summary>
        /// Gets or sets up point of the right eye.
        /// </summary>
        [DataMember(Name = "e_ru")]
        public Point EyeRight_Up { get; set; }

        /// <summary>
        /// Gets or sets down point of the right eye.
        /// </summary>
        [DataMember(Name = "e_rd")]
        public Point EyeRight_Down { get; set; }

        /// <summary>
        /// Gets or sets position of the nose.
        /// </summary>
        [DataMember(Name = "nose")]
        public Point NosePosition { get; set; }

        /// <summary>
        /// Gets or sets left point of the nose.
        /// </summary>
        [DataMember(Name = "n_l")]
        public Point NoseLeft { get; set; }

        /// <summary>
        /// Gets or sets right point of the nose.
        /// </summary>
        [DataMember(Name = "n_r")]
        public Point NoseRight { get; set; }

        /// <summary>
        /// Gets or sets left point of the mouth.
        /// </summary>
        [DataMember(Name = "mouth_l")]
        public Point MouthLeft { get; set; }

        /// <summary>
        /// Gets or sets right point of the mouth.
        /// </summary>
        [DataMember(Name = "mouth_r")]
        public Point MouthRight { get; set; }

        /// <summary>
        /// Gets or sets up point of the mouth.
        /// </summary>
        [DataMember(Name = "m_u")]
        public Point MouthUp { get; set; }

        /// <summary>
        /// Gets or sets down point of the mouth.
        /// </summary>
        [DataMember(Name = "m_d")]
        public Point MouthDown { get; set; }
    }

    /// <summary>
    /// Identifies the bounding box around the face.
    /// </summary>
    [DataContract]
    public class BoundingBox
    {
        /// <summary>
        /// Gets or sets top left point of the bounding box.
        /// </summary>
        [DataMember(Name = "tl")]
        public Point TopLeft { get; set; }

        /// <summary>
        /// Gets or sets size of the bounding box.
        /// </summary>
        [DataMember(Name = "size")]
        public Size Size { get; set; }
    }

    /// <summary>
    /// Point, represented by X and Y coordinates.
    /// </summary>
    [DataContract]
    public class Point
    {
        /// <summary>
        /// Gets or sets x - coordinate.
        /// </summary>
        [DataMember(Name = "x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets y - coordinate.
        /// </summary>
        public double Y { get; set; }
    }

    /// <summary>
    /// A size of the block, represented by width and height.
    /// </summary>
    [DataContract]
    public class Size
    {
        /// <summary>
        /// Gets or sets width of the block.
        /// </summary>
        [DataMember(Name = "width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets width of the block.
        /// </summary>
        [DataMember(Name = "height")]
        public double Height { get; set; }
    }

    /// <summary>
    /// Details of the quality analysis.
    /// </summary>
    [DataContract]
    public class QualityAnalysis
    {
        /// <summary>
        /// Gets or sets jpeg quality value.
        /// </summary>
        [DataMember(Name = "jpeg_quality")]
        public double JpegQuality { get; set; }

        /// <summary>
        /// Gets or sets jpeg chroma value.
        /// </summary>
        [DataMember(Name = "jpeg_chroma")]
        public double JpegChroma { get; set; }

        /// <summary>
        /// Gets or sets focus value.
        /// </summary>
        [DataMember(Name = "focus")]
        public double Focus { get; set; }

        /// <summary>
        /// Gets or sets noise value.
        /// </summary>
        [DataMember(Name = "noise")]
        public double Noise { get; set; }

        /// <summary>
        /// Gets or sets contrast value.
        /// </summary>
        [DataMember(Name = "contrast")]
        public double Contrast { get; set; }

        /// <summary>
        /// Gets or sets exposure value.
        /// </summary>
        [DataMember(Name = "exposure")]
        public double Exposure { get; set; }

        /// <summary>
        /// Gets or sets saturation value.
        /// </summary>
        [DataMember(Name = "saturation")]
        public double Saturation { get; set; }

        /// <summary>
        /// Gets or sets lighting value.
        /// </summary>
        [DataMember(Name = "lighting")]
        public double Lighting { get; set; }

        /// <summary>
        /// Gets or sets pixel score value.
        /// </summary>
        [DataMember(Name = "pixel_score")]
        public double PixelScore { get; set; }

        /// <summary>
        /// Gets or sets color score value.
        /// </summary>
        [DataMember(Name = "color_score")]
        public double ColorScore { get; set; }

        /// <summary>
        /// Gets or sets DCT value.
        /// </summary>
        [DataMember(Name = "dct")]
        public double Dct { get; set; }

        /// <summary>
        /// Gets or sets blockiness value.
        /// </summary>
        [DataMember(Name = "blockiness")]
        public double Blockiness { get; set; }

        /// <summary>
        /// Gets or sets chroma subsampling value.
        /// </summary>
        [DataMember(Name = "chroma_subsampling")]
        public double ChromaSubsampling { get; set; }

        /// <summary>
        /// Gets or sets resolution value.
        /// </summary>
        [DataMember(Name = "resolution")]
        public double Resolution { get; set; }
    }

    /// <summary>
    /// Details of the cinemagraph analysis.
    /// </summary>
    [DataContract]
    public class CinemagraphAnalysis
    {
        /// <summary>
        /// Gets or sets value between 0-1, where 0 means definitely not a cinemagraph
        /// and 1 means definitely a cinemagraph).
        /// </summary>
        [DataMember(Name = "cinemagraph_score")]
        public double CinemagraphScore { get; set; }
    }

    /// <summary>
    /// Details of the accessibility analysis.
    /// </summary>
    [DataContract]
    public class AccessibilityAnalysis
    {
        /// <summary>
        /// Gets or sets details of colorblind accessibility analysis.
        /// </summary>
        [DataMember(Name = "colorblind_accessibility_analysis")]
        public ColorblindAccessibilityAnalysis ColorblindAccessibilityAnalysis { get; set; }

        /// <summary>
        /// Gets or sets value between 0-1.
        /// </summary>
        [DataMember(Name = "colorblind_accessibility_score")]
        public double ColorblindAccessibilityScore { get; set; }
    }

    /// <summary>
    /// Details of colorblind accessibility analysis.
    /// </summary>
    [DataContract]
    public class ColorblindAccessibilityAnalysis
    {
        /// <summary>
        /// Gets or sets distinct edges value between 0-1.
        /// </summary>
        [DataMember(Name = "distinct_edges")]
        public double DistinctEdges { get; set; }

        /// <summary>
        /// Gets or sets distinct colors value between 0-1.
        /// </summary>
        [DataMember(Name = "distinct_colors")]
        public double DistinctColors { get; set; }

        /// <summary>
        /// Gets or sets most indistinct pair of colors.
        /// </summary>
        [DataMember(Name = "most_indistinct_pair")]
        public string[] MostIndistinctPair { get; set; }
    }

    /// <summary>
    /// Details of asset version.
    /// </summary>
    [DataContract]
    public class AssetVersion
    {
        /// <summary>
        /// Gets or sets asset version identifier.
        /// </summary>
        [DataMember(Name = "version_id")]
        public string VersionId { get; set; }

        /// <summary>
        /// Gets or sets asset version number.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets asset size in bytes.
        /// </summary>
        [DataMember(Name = "size")]
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets time when version created.
        /// </summary>
        [DataMember(Name = "time")]
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether asset version can be restored.
        /// </summary>
        [DataMember(Name = "restorable")]
        public bool Restorable { get; set; }

        /// <summary>
        /// Gets or sets asset version url.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}

namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

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
}
using Shared.Transforms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Coudinary.NetCoreShared.Transforms
{
    /// <summary>
    /// Class that provide User Defined Variable functionality.
    /// See https://cloudinary.com/documentation/image_transformations#using_user_defined_variables for more info.
    /// </summary>
    public class Variable
    {
        string m_name;
        object m_value;

        /// <summary>
        /// User defined variable constructor by name and value.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="value">The value by which the variable will be initialized. Can be any scalar type.</param>
        public Variable(string name, object value)
        {
            if(!CheckName(name))
                throw new ArgumentException("The name can include only alphanumeric characters and must begin with a letter.");

            m_name = name;
            m_value = value ?? throw new ArgumentException("The value cannot be null."); 
        }

        /// <summary>
        /// User defined variable constructor by name and Expression.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="value">The Expression object by which the variable will be initialized.</param>
        public Variable(string name, Expression value)
        {
            if (!CheckName(name))
                throw new ArgumentException("The name can include only alphanumeric characters and must begin with a letter.");
                            
            m_name = name;
            m_value = value ?? throw new ArgumentException("The value cannot be null.");
        }

        /// <summary>
        /// User defined variable constructor by name and string list.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="multipleStringValues">The string list by which the variable will be initialized.</param>
        public Variable(string name, List<string> multipleStringValues)
        {
            if (!CheckName(name))
                throw new ArgumentException("The name can include only alphanumeric characters and must begin with a letter.");

            m_name = name;

            if (multipleStringValues != null)
                m_value = string.Join(":", multipleStringValues);
            else
                m_value = string.Empty;
        }

        private bool CheckName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (char.IsDigit(name[0]))
                return false;

            if (name.StartsWith("$"))
                return false;

            Regex alphanumericPattern = new Regex("^[a-zA-Z0-9]*$");

            return alphanumericPattern.IsMatch(name);
        }

        public string Key
        {
            get
            {
                return string.Format("${0}", m_name);
            }
        }

        public string TextLayerKey
        {
            get
            {
                return string.Format("$({0})", m_name);
            }
        }

        public string Value
        {
            get
            {
                return GetVariableValue();
            }
        }

        public override string ToString()
        {
            return string.Format("${0}_{1}", m_name, GetVariableValue());
        }

        private string GetVariableValue()
        {
            string result = m_value.ToString();

            var @types = new Dictionary<Type, Action> {
                { typeof(Expression), () => { result = m_value.ToString(); } },
                { typeof(string), () => { result = string.Format("!{0}!", m_value.ToString()); } }
            };

            @types[m_value.GetType()]();

            return result;
        }

        #region Predefined Variables
        public static string Width
        {
            get { return "w"; }
        }

        public static string Height
        {
            get { return "h"; }
        }

        public static string InitialWidth
        {
            get { return "iw"; }
        }

        public static string InitialHeight
        {
            get { return "ih"; }
        }

        public static string PageCount
        {
            get { return "pc"; }
        }

        public static string FaceCount
        {
            get { return "fc"; }
        }

        public static string IllustrationScore
        {
            get { return "ils"; }
        }

        public static string CurentPageIndex
        {
            get { return "cp"; }
        }

        public static string Tags
        {
            get { return "tags"; }
        }

        public static string XOffset
        {
            get { return "px"; }
        }

        public static string YOffset
        {
            get { return "py"; }
        }

        public static string AspectRatio
        {
            get { return "ar"; }
        }

        public static string AspectRatioInitial
        {
            get { return "iar"; }
        }

        #endregion
    }
}

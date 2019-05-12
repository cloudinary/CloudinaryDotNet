using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents expression object that can be used in user defined variables and conditional transformations.
    /// </summary>
    public class Expression : BaseExpression<Expression>
    {
        /// <summary>
        /// Regular expression to check variable name.
        /// </summary>
        public const string VARIABLE_NAME_REGEX = "^\\$[a-zA-Z][a-zA-Z0-9]*$";

        /// <summary>
        /// Default parameterless constructor. Instantiates the <see cref="Expression"/> object.
        /// </summary>
        public Expression() { }

        /// <summary>
        /// Instantiates the <see cref="Expression"/> object with name.
        /// </summary>
        public Expression(string name) : this()
        {
            if (!string.IsNullOrEmpty(name))
                m_expressions.Add(name);
        }

        /// <summary>
        /// Define a user defined variable.
        /// </summary>
        /// <param name="name">The name of variable.</param>
        /// <param name="value">The value.</param>
        public static Expression Variable(string name, object value)
        {
            CheckVariableName(name);

            var expression = new Expression(name);
            expression.m_expressions.Add(value.ToString());
            return expression;
        }

        /// <summary>
        /// Check the variable name.
        /// </summary>
        /// <param name="name">The name to check.</param>
        public static void CheckVariableName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, VARIABLE_NAME_REGEX))
                throw new ArgumentException(
                    $"The name `{name}` can include only alphanumeric characters and must begin with a letter."
                );
        }

        /// <summary>
        /// Check if the value contains user defined variable or predefined variable.
        /// </summary>
        /// <param name="value">The value to check.</param>
        public static bool ValueContainsVariable(string value)
        {
            return !string.IsNullOrEmpty(value) &&
                   (value.IndexOf("$", StringComparison.Ordinal) != -1 ||
                    Parameters.Any(v => value.Contains($"_{v.Value}") || value.Contains($"{v.Value}_")));
        }

        #region Predefined variables

        /// <summary>
        /// Predefined variable 'width'.
        /// </summary>
        public static Expression Width()
        {
            return new Expression("w");
        }

        /// <summary>
        /// Predefined variable 'height'.
        /// </summary>
        public static Expression Height()
        {
            return new Expression("h");
        }

        /// <summary>
        /// Predefined variable 'initial width'.
        /// </summary>
        public static Expression InitialWidth()
        {
            return new Expression("iw");
        }

        /// <summary>
        /// Predefined variable 'initial height'.
        /// </summary>
        public static Expression InitialHeight()
        {
            return new Expression("ih");
        }

        /// <summary>
        /// Predefined variable 'page count'.
        /// </summary>
        public static Expression PageCount()
        {
            return new Expression("pc");
        }

        /// <summary>
        /// Predefined variable 'face count'.
        /// </summary>
        public static Expression FaceCount()
        {
            return new Expression("fc");
        }

        /// <summary>
        /// Predefined variable 'illustration score'.
        /// </summary>
        public static Expression IllustrationScore() 
        {
            return new Expression("ils");
        }

        /// <summary>
        /// Predefined variable 'current page index'.
        /// </summary>
        public static Expression CurrentPageIndex()
        {
            return new Expression("cp");
        }

        /// <summary>
        /// Predefined variable 'tags'.
        /// </summary>
        public static Expression Tags()
        {
            return new Expression("tags");
        }

        /// <summary>
        /// Predefined variable 'x-offset'.
        /// </summary>
        public static Expression XOffset()
        {
            return new Expression("px");
        }

        /// <summary>
        /// Predefined variable 'y-offset'.
        /// </summary>
        public static Expression YOffset()
        {
            return new Expression("py");
        }

        /// <summary>
        /// Predefined variable 'aspect ratio'.
        /// </summary>
        public static Expression AspectRatio()
        {
            return new Expression("ar");
        }

        /// <summary>
        /// Predefined variable 'aspect ratio of initial image'.
        /// </summary>
        public static Expression AspectRatioOfInitialImage()
        {
            return new Expression("iar");
        }

        #endregion
    }
}

namespace CloudinaryDotNet
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents expression object that can be used in user defined variables and conditional transformations.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    public class Expression : BaseExpression<Expression>
    {
        /// <summary>
        /// Regular expression to check variable name.
        /// </summary>
        public const string VARIABLE_NAME_REGEX = "^\\$[a-zA-Z][a-zA-Z0-9]*$";

        /// <summary>
        /// Initializes a new instance of the <see cref="Expression"/> class.
        /// Default parameterless constructor.
        /// </summary>
        public Expression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Expression"/> class with name.
        /// </summary>
        /// <param name="name">Name of the new expression.</param>
        public Expression(string name)
            : this()
        {
            if (!string.IsNullOrEmpty(name))
            {
                m_expressions.Add(name);
            }
        }

        /// <summary>
        /// Define a user defined variable.
        /// </summary>
        /// <param name="name">The name of variable.</param>
        /// <param name="value">The value.</param>
        /// <returns>An expression that represents the variable.</returns>
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
            {
                throw new ArgumentException(
                   $"The name `{name}` can include only alphanumeric characters and must begin with a letter.");
            }
        }

        /// <summary>
        /// Check if the value contains user defined variable or predefined variable.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value contains the variable; otherwise, false.</returns>
        public static bool ValueContainsVariable(string value)
        {
            return !string.IsNullOrEmpty(value) &&
                   (value.IndexOf("$", StringComparison.Ordinal) != -1 ||
                    parameters.Any(v => value.Contains($"_{v.Value}") || value.Contains($"{v.Value}_")));
        }

        /// <summary>
        /// Predefined variable 'width'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression Width()
        {
            return new Expression("w");
        }

        /// <summary>
        /// Predefined variable 'height'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression Height()
        {
            return new Expression("h");
        }

        /// <summary>
        /// Predefined variable 'initial width'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression InitialWidth()
        {
            return new Expression("iw");
        }

        /// <summary>
        /// Predefined variable 'initial height'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression InitialHeight()
        {
            return new Expression("ih");
        }

        /// <summary>
        /// Predefined variable 'page count'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression PageCount()
        {
            return new Expression("pc");
        }

        /// <summary>
        /// Predefined variable 'face count'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression FaceCount()
        {
            return new Expression("fc");
        }

        /// <summary>
        /// Predefined variable 'illustration score'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression IllustrationScore()
        {
            return new Expression("ils");
        }

        /// <summary>
        /// Predefined variable 'current page index'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression CurrentPageIndex()
        {
            return new Expression("cp");
        }

        /// <summary>
        /// Predefined variable 'tags'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression Tags()
        {
            return new Expression("tags");
        }

        /// <summary>
        /// Predefined variable 'x-offset'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression XOffset()
        {
            return new Expression("px");
        }

        /// <summary>
        /// Predefined variable 'y-offset'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression YOffset()
        {
            return new Expression("py");
        }

        /// <summary>
        /// Predefined variable 'aspect ratio'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression AspectRatio()
        {
            return new Expression("ar");
        }

        /// <summary>
        /// Predefined variable 'aspect ratio of initial image'.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression AspectRatioOfInitialImage()
        {
            return new Expression("iar");
        }

        /// <summary>
        /// Predefined variable 'duration'.
        /// The current duration of the video.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression Duration()
        {
            return new Expression("du");
        }

        /// <summary>
        /// Predefined variable 'initial duration'.
        /// The video's initial duration.
        /// </summary>
        /// <returns>An expression that represents the variable.</returns>
        public static Expression InitialDuration()
        {
            return new Expression("idu");
        }
    }
}

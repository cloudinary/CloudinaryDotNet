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
        public const string VARIABLE_NAME_REGEX = "^\\$[a-zA-Z][a-zA-Z0-9]*$";

        public Expression() { }

        public Expression(string name) : this()
        {
            if (!string.IsNullOrEmpty(name))
                m_expressions.Add(name);
        }

        public static Expression Variable(string name, object value)
        {
            CheckVariableName(name);

            var expression = new Expression(name);
            expression.m_expressions.Add(value.ToString());
            return expression;
        }

        public static void CheckVariableName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, VARIABLE_NAME_REGEX))
                throw new ArgumentException(
                    $"The name `{name}` can include only alphanumeric characters and must begin with a letter."
                );
        }

        /// <summary>
        /// Check if the value contains user defined variable or predefined variable
        /// </summary>
        public static bool ValueContainsVariable(string value)
        {
            return !string.IsNullOrEmpty(value) &&
                   (value.IndexOf("$", StringComparison.Ordinal) != -1 ||
                    Parameters.Any(v => value.Contains($"_{v.Value}") || value.Contains($"{v.Value}_")));
        }

        #region Predefined variables
        public static Expression Width()
        {
            return new Expression("w");
        }

        public static Expression Height()
        {
            return new Expression("h");
        }

        public static Expression InitialWidth()
        {
            return new Expression("iw");
        }

        public static Expression InitialHeight()
        {
            return new Expression("ih");
        }

        public static Expression PageCount()
        {
            return new Expression("pc");
        }

        public static Expression FaceCount()
        {
            return new Expression("fc");
        }

        public static Expression IllustrationScore() 
        {
            return new Expression("ils");
        }

        public static Expression CurrentPageIndex()
        {
            return new Expression("cp");
        }

        public static Expression Tags()
        {
            return new Expression("tags");
        }

        public static Expression XOffset()
        {
            return new Expression("px");
        }

        public static Expression YOffset()
        {
            return new Expression("py");
        }

        public static Expression AspectRatio()
        {
            return new Expression("ar");
        }

        public static Expression AspectRatioOfInitialImage()
        {
            return new Expression("iar");
        }

        #endregion
    }
}

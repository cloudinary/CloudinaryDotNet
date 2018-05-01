using System;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents expression object that can be used in user defined variables and conditional transformations.
    /// </summary>
    public class Expression : BaseExpression<Expression>
    {
        public const string VARIABLE_NAME_REGEX = "^\\$[a-zA-Z][a-zA-Z0-9]*$";

        public Expression() : base() { }

        public Expression(string name) : this()
        {
            if (!string.IsNullOrEmpty(name))
                m_expressions.Add(name);
        }

        public static Expression Variable(string name, object value)
        {
            CheckVariableName(name);

            Expression var = new Expression(name);
            var.m_expressions.Add(value.ToString());
            return var;
        }

        public static void CheckVariableName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, VARIABLE_NAME_REGEX))
                throw new ArgumentException(
                    $"The name `{name}` can include only alphanumeric characters and must begin with a letter."
                );
        }

        #region Expressions with predefined variables
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

        public static Expression CurentPageIndex()
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

        public static Expression AspectRatioInitial()
        {
            return new Expression("iar");
        }

        #endregion
    }
}

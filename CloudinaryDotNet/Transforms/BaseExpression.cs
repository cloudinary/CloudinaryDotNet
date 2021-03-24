namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Base abstract non-generic class for creating Expressions.
    /// </summary>
    public abstract class BaseExpression
    {
    }

    /// <summary>
    /// Represents expression object that can be used in user defined variables and conditional transformations.
    /// </summary>
    /// <typeparam name="T">Type of the expression.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public abstract class BaseExpression<T> : BaseExpression
        where T : BaseExpression<T>
    {
        /// <summary>
        /// A dictionary with available operators.
        /// </summary>
        protected static Dictionary<string, string> operators = new Dictionary<string, string>()
        {
            { "=", "eq" },
            { "!=", "ne" },
            { "<", "lt" },
            { ">", "gt" },
            { "<=", "lte" },
            { ">=", "gte" },
            { "&&", "and" },
            { "||", "or" },
            { "*", "mul" },
            { "/", "div" },
            { "+", "add" },
            { "-", "sub" },
            { "^", "pow" },
        };

        /// <summary>
        /// A dictionary with available parameters.
        /// </summary>
        protected static Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "width", "w" },
            { "height", "h" },
            { "initial_width", "iw" },
            { "initialWidth", "iw" },
            { "initial_height", "ih" },
            { "initialHeight", "ih" },
            { "aspect_ratio", "ar" },
            { "aspectRatio", "ar" },
            { "initial_aspect_ratio", "iar" },
            { "initialAspectRatio", "iar" },
            { "page_count", "pc" },
            { "pageCount", "pc" },
            { "face_count", "fc" },
            { "faceCount", "fc" },
            { "illustration_score", "ils" },
            { "illustrationScore", "ils" },
            { "current_page", "cp" },
            { "currentPage", "cp" },
            { "tags", "tags" },
            { "pageX", "px" },
            { "pageY", "py" },
        };

        /// <summary>
        /// A list of expressions.
        /// </summary>
        protected List<string> m_expressions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseExpression{T}"/> class.
        /// Default paramaterless constructor.
        /// </summary>
        protected BaseExpression()
        {
            m_expressions = new List<string>();
        }

        /// <summary>
        /// Gets parent transformation this expression belongs to.
        /// </summary>
        protected Transformation Parent { get; private set; }

        /// <summary>
        /// Normalize an expression string, replace "nice names" with their coded values and spaces with "_"
        /// e.g. "width > 0" => "w_lt_0".
        /// </summary>
        /// <param name="expression">An expression.</param>
        /// <returns>A parsed expression.</returns>
        public static string Normalize(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            expression = Regex.Replace(expression, "[ _]+", "_");
            const string userVariablePattern = "\\$_*[^_]+";

            var generalPattern = GetPattern();
            var matcher = new Regex(userVariablePattern, RegexOptions.IgnoreCase).Match(expression);
            var sb = new StringBuilder();
            var lastMatchEnd = 0;
            while (matcher.Success)
            {
                var matcherGroup = matcher.Groups[0];
                var beforeMatch = expression.Substring(lastMatchEnd, matcherGroup.Index - lastMatchEnd);
                sb.Append(Regex.Replace(beforeMatch, generalPattern, m => GetOperatorReplacement(m.Value)));
                sb.Append(matcherGroup.Value);
                lastMatchEnd = matcherGroup.Index + matcherGroup.Length;
                matcher = matcher.NextMatch();
            }

            var tail = expression.Substring(lastMatchEnd);
            sb.Append(Regex.Replace(tail, generalPattern, m => GetOperatorReplacement(m.Value)));
            return sb.ToString();
        }

        /// <summary>
        /// Set parent transformation.
        /// </summary>
        /// <param name="parent">A parent transformation.</param>
        /// <returns>The expression with set parameter.</returns>
        public T SetParent(Transformation parent)
        {
            Parent = parent;
            return (T)this;
        }

        /// <summary>
        /// Get a serialized list of predicates.
        /// </summary>
        /// <returns>Serialized list of predicates.</returns>
        public override string ToString()
        {
            return Serialize();
        }

        /// <summary>
        /// Set expression value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with set parameter.</returns>
        public virtual T Value(object value)
        {
            m_expressions.Add(Convert.ToString(value, CultureInfo.InvariantCulture));
            return (T)this;
        }

        /// <summary>
        /// Add 'multiply' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Mul(object value)
        {
            return Mul().Value(value);
        }

        /// <summary>
        /// Add 'multiply' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Mul()
        {
            m_expressions.Add("mul");
            return (T)this;
        }

        /// <summary>
        /// Add 'greater than' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Gt(object value)
        {
            return Gt().Value(value);
        }

        /// <summary>
        /// Add 'greater than' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Gt()
        {
            m_expressions.Add("gt");
            return (T)this;
        }

        /// <summary>
        /// Add 'and' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T And(object value)
        {
            return And().Value(value);
        }

        /// <summary>
        /// Add 'and' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T And()
        {
            m_expressions.Add("and");
            return (T)this;
        }

        /// <summary>
        /// Add 'or' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Or(object value)
        {
            return Or().Value(value);
        }

        /// <summary>
        /// Add 'or' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Or()
        {
            m_expressions.Add("or");
            return (T)this;
        }

        /// <summary>
        /// Add 'equal to' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Eq(object value)
        {
            return Eq().Value(value);
        }

        /// <summary>
        /// Add 'equal to' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Eq()
        {
            m_expressions.Add("eq");
            return (T)this;
        }

        /// <summary>
        /// Add 'not equal to' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Ne(object value)
        {
            return Ne().Value(value);
        }

        /// <summary>
        /// Add 'not equal to' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Ne()
        {
            m_expressions.Add("ne");
            return (T)this;
        }

        /// <summary>
        /// Add 'less than' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Lt(object value)
        {
            return Lt().Value(value);
        }

        /// <summary>
        /// Add 'less than' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Lt()
        {
            m_expressions.Add("lt");
            return (T)this;
        }

        /// <summary>
        /// Add 'less than or equal to' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Lte(object value)
        {
            return Lte().Value(value);
        }

        /// <summary>
        /// Add 'less than or equal to' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Lte()
        {
            m_expressions.Add("lte");
            return (T)this;
        }

        /// <summary>
        /// Add 'greater than or equal to' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Gte(object value)
        {
            return Gte().Value(value);
        }

        /// <summary>
        /// Add 'greater than or equal to' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Gte()
        {
            m_expressions.Add("gte");
            return (T)this;
        }

        /// <summary>
        /// Add 'divide' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Div(object value)
        {
            return Div().Value(value);
        }

        /// <summary>
        /// Add 'divide' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Div()
        {
            m_expressions.Add("div");
            return (T)this;
        }

        /// <summary>
        /// Add 'add' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Add(object value)
        {
            return Add().Value(value);
        }

        /// <summary>
        /// Add 'add' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Add()
        {
            m_expressions.Add("add");
            return (T)this;
        }

        /// <summary>
        /// Add 'subtract' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Sub(object value)
        {
            return Sub().Value(value);
        }

        /// <summary>
        /// Add 'subtract' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Sub()
        {
            m_expressions.Add("sub");
            return (T)this;
        }

        /// <summary>
        /// Add 'included in' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T In()
        {
            m_expressions.Add("in");
            return (T)this;
        }

        /// <summary>
        /// Add 'included in' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T In(object value)
        {
            return In().Value(value);
        }

        /// <summary>
        /// Add 'not included in' operation.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Nin()
        {
            m_expressions.Add("nin");
            return (T)this;
        }

        /// <summary>
        /// Add 'not included in' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The expression with operation added.</returns>
        public T Nin(object value)
        {
            return Nin().Value(value);
        }

        /// <summary>
        /// Adds "to the power of" sub-expression to the end of the list
        /// of already present sub-expressions in this expression instance.
        /// </summary>
        /// <returns>The expression with operation added.</returns>
        public T Pow()
        {
            m_expressions.Add("pow");
            return (T)this;
        }

        /// <summary>
        /// Utility shortcut method which invokes on this Expression instance method,
        /// takes its result and invokes method on it. Effectively, invocation of this shortcut results in
        /// "to the power of value" sub-expression added to the end of current expression instance.
        /// </summary>
        /// <param name="value">Value argument for the call.</param>
        /// <returns>The result of the call.</returns>
        public T Pow(object value)
        {
            return Pow().Value(value);
        }

        /// <summary>
        /// Helper method to replace the operator to the Cloudinary URL syntax.
        /// </summary>
        /// <param name="value">An operator to replace.</param>
        /// <returns>An operator replaced to the Cloudinary URL syntax.</returns>
        protected static string GetOperatorReplacement(string value)
        {
            if (operators.ContainsKey(value))
            {
                return operators[value];
            }

            return parameters.ContainsKey(value) ? parameters[value] : value;
        }

        /// <summary>
        /// Serialize a list of predicates.
        /// </summary>
        /// <returns>A string that represents serialized predicates list.</returns>
        protected string Serialize()
        {
            return Normalize(string.Join("_", m_expressions));
        }

        /// <summary>
        /// Get regex pattern for operators and predefined vars as /((operators)(?=[ _])|variables)/.
        /// </summary>
        /// <returns>A regex pattern.</returns>
        private static string GetPattern()
        {
            var operators = new List<string>(BaseExpression<T>.operators.Keys);
            operators.Reverse();
            var sb = new StringBuilder("((");
            foreach (string op in operators)
            {
                sb.Append(Regex.Escape(op)).Append('|');
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(")(?=[ _])|(?<!\\$)(").Append(string.Join("|", parameters.Keys.ToArray())).Append("))");
            return sb.ToString();
        }
    }
}

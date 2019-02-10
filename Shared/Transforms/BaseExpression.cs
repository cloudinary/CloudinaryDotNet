using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Base abstract non-generic class for creating Expressions.
    /// </summary>
    public abstract class BaseExpression { }

    /// <summary>
    /// Represents expression object that can be used in user defined variables and conditional transformations.
    /// </summary>
    public abstract class BaseExpression<T>: BaseExpression where T: BaseExpression<T>
    {
        /// <summary>
        /// A dictionary with available operators.
        /// </summary>
        protected static Dictionary<string, string> Operators = new Dictionary<string, string>()
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
            { "-", "sub" }
        };

        /// <summary>
        /// A dictionary with available parameters.
        /// </summary>
        protected static Dictionary<string, string> Parameters = new Dictionary<string, string>()
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
            { "pageY", "py" }
        };

        /// <summary>
        /// A list of expressions.
        /// </summary>
        protected List<string> m_expressions;

        /// <summary>
        /// Default paramaterless constructor. Instantiates the <see cref="BaseExpression"/> object.
        /// </summary>
        protected BaseExpression()
        {
            m_expressions = new List<string>();
        }

        /// <summary>
        /// Normalize an expression string, replace "nice names" with their coded values and spaces with "_"
        /// e.g. "width > 0" => "w_lt_0".
        /// </summary>
        /// <param name="expression">An expression.</param>
        /// <returns>A parsed expression.</returns>
        public static string Normalize(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return null;

            expression = Regex.Replace(expression, "[ _]+", "_");
            string pattern = GetPattern();
            return Regex.Replace(expression, pattern, m => GetOperatorReplacement(m.Value));
        }

        /// <summary>
        /// Helper method to replace the operator to the Cloudinary Url syntax.
        /// </summary>
        /// <param name="value">An operator to replace.</param>
        /// <returns>An operator replaced to the Cloudinary Url syntax.</returns>
        protected static string GetOperatorReplacement(string value)
        {
            if (Operators.ContainsKey(value))
                return Operators[value];

            return Parameters.ContainsKey(value) ? Parameters[value] : value;
        }

        /// <summary>
        /// Get regex pattern for operators and predefined vars as /((operators)(?=[ _])|variables)/.
        /// </summary>
        /// <returns>A regex pattern.</returns>
        private static string GetPattern()
        {
            var operators = new List<string>(Operators.Keys);
            operators.Reverse();
            var sb = new StringBuilder("((");
            foreach (string op in operators)
            {
                sb.Append(Regex.Escape(op)).Append("|");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")(?=[ _])|").Append(string.Join("|", Parameters.Keys.ToArray())).Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Parent transformation this expression belongs to.
        /// </summary>
        protected Transformation Parent { get; private set; }

        /// <summary>
        /// Set parent transformation.
        /// </summary>
        /// <param name="parent">A parent transformation.</param>
        public T SetParent(Transformation parent)
        {
            Parent = parent;
            return (T) this;
        }

        /// <summary>
        /// Serialize a list of predicates.
        /// </summary>
        protected string Serialize()
        {
            return Normalize(string.Join("_", m_expressions));
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
        public virtual T Value(object value)
        {
            m_expressions.Add(Convert.ToString(value));
            return (T)this;
        }

        #region Predefined operators

        /// <summary>
        /// Add 'multiply' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Mul(object value)
        {
            return Mul().Value(value);
        }

        /// <summary>
        /// Add 'multiply' operation.
        /// </summary>
        public T Mul()
        {
            m_expressions.Add("mul");
            return (T)this;
        }

        /// <summary>
        /// Add 'greater than' operation with value.
        /// </summary>
        /// <param name="value">The value</param>
        public T Gt(object value)
        {
            return Gt().Value(value);
        }

        /// <summary>
        /// Add 'greater than' operation.
        /// </summary>
        public T Gt()
        {
            m_expressions.Add("gt");
            return (T)this;
        }

        /// <summary>
        /// Add 'and' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T And(object value)
        {
            return And().Value(value);
        }

        /// <summary>
        /// Add 'and' operation.
        /// </summary>
        public T And()
        {
            m_expressions.Add("and");
            return (T)this;
        }

        /// <summary>
        /// Add 'or' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Or(object value)
        {
            return Or().Value(value);
        }


        /// <summary>
        /// Add 'or' operation.
        /// </summary>
        public T Or()
        {
            m_expressions.Add("or");
            return (T)this;
        }

        /// <summary>
        /// Add 'equal to' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Eq(object value)
        {
            return Eq().Value(value);
        }

        /// <summary>
        /// Add 'equal to' operation.
        /// </summary>
        public T Eq()
        {
            m_expressions.Add("eq");
            return (T)this;
        }

        /// <summary>
        /// Add 'not equal to' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Ne(object value)
        {
            return Ne().Value(value);
        }

        /// <summary>
        /// Add 'not equal to' operation.
        /// </summary>
        public T Ne()
        {
            m_expressions.Add("ne");
            return (T)this;
        }

        /// <summary>
        /// Add 'less than' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Lt(object value)
        {
            return Lt().Value(value);
        }

        /// <summary>
        /// Add 'less than' operation.
        /// </summary>
        public T Lt()
        {
            m_expressions.Add("lt");
            return (T)this;
        }

        /// <summary>
        /// Add 'less than or equal to' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Lte(object value)
        {
            return Lte().Value(value);
        }

        /// <summary>
        /// Add 'less than or equal to' operation.
        /// </summary>
        public T Lte()
        {
            m_expressions.Add("lte");
            return (T)this;
        }

        /// <summary>
        /// Add 'greater than or equal to' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Gte(object value)
        {
            return Gte().Value(value);
        }

        /// <summary>
        /// Add 'greater than or equal to' operation.
        /// </summary>
        public T Gte()
        {
            m_expressions.Add("gte");
            return (T)this;
        }

        /// <summary>
        /// Add 'divide' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Div(object value)
        {
            return Div().Value(value);
        }

        /// <summary>
        /// Add 'divide' operation.
        /// </summary>
        public T Div()
        {
            m_expressions.Add("div");
            return (T)this;
        }

        /// <summary>
        /// Add 'add' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Add(object value)
        {
            return Add().Value(value);
        }

        /// <summary>
        /// Add 'add' operation.
        /// </summary>
        public T Add()
        {
            m_expressions.Add("add");
            return (T)this;
        }

        /// <summary>
        /// Add 'subtract' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Sub(object value)
        {
            return Sub().Value(value);
        }

        /// <summary>
        /// Add 'subtract' operation.
        /// </summary>
        public T Sub()
        {
            m_expressions.Add("sub");
            return (T)this;
        }

        /// <summary>
        /// Add 'included in' operation.
        /// </summary>
        public T In()
        {
            m_expressions.Add("in");
            return (T) this;
        }

        /// <summary>
        /// Add 'included in' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T In(object value)
        {
            return In().Value(value);
        }

        /// <summary>
        /// Add 'not included in' operation.
        /// </summary>
        public T Nin()
        {
            m_expressions.Add("nin");
            return (T) this;
        }

        /// <summary>
        /// Add 'not included in' operation with value.
        /// </summary>
        /// <param name="value">The value.</param>
        public T Nin(object value)
        {
            return Nin().Value(value);
        }
        
        #endregion
    }
}

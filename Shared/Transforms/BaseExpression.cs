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
        public static Dictionary<string, string> Operators = new Dictionary<string, string>()
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

        public static Dictionary<string, string> Parameters = new Dictionary<string, string>()
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
            { "current_page", "cp" },
            { "currentPage", "cp" },
            { "tags", "tags" },
            { "pageX", "px" },
            { "pageY", "py" }
        };

        protected List<string> m_expressions;

        public BaseExpression()
        {
            m_expressions = new List<string>();
        }

        /// <summary>
        /// Normalize an expression string, replace "nice names" with their coded values and spaces with "_"
        /// e.g. "width > 0" => "w_lt_0"
        /// </summary>
        /// <param name="expression">An expression</param>
        /// <returns>A parsed expression</returns>
        public static string Normalize(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return null;

            expression = Regex.Replace(expression, "[ _]+", "_");
            string pattern = GetPattern();
            return Regex.Replace(expression, pattern, m => GetOperatorReplacement(m.Value));
        }

        private static string GetOperatorReplacement(string value)
        {
            if (Operators.ContainsKey(value))
            {
                return Operators[value];
            }
            else if (Parameters.ContainsKey(value))
            {
                return Parameters[value];
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Get regex pattern for operators and predefined vars as /((operators)(?=[ _])|variables)/
        /// </summary>
        /// <returns>A regex pattern</returns>
        private static string GetPattern()
        {
            string pattern;
            List<string> operators = new List<string>(Operators.Keys);
            operators.Reverse();
            StringBuilder sb = new StringBuilder("((");
            foreach (string op in operators)
            {
                sb.Append(Regex.Escape(op)).Append("|");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")(?=[ _])|").Append(string.Join("|", Parameters.Keys.ToArray())).Append(")");
            pattern = sb.ToString();
            return pattern;
        }

        /// <summary>
        /// Parent transformation this expression belongs to.
        /// </summary>
        public Transformation Parent { get; private set; }

        public T SetParent(Transformation parent)
        {
            Parent = parent;
            return (T) this;
        }

        /// <summary>
        /// Serialize a list of predicates.
        /// </summary>
        public string Serialize()
        {
            return Normalize(string.Join("_", m_expressions));
        }

        public override string ToString()
        {
            return Serialize();
        }

        public virtual T Value(object value)
        {
            m_expressions.Add(Convert.ToString(value));
            return (T)this;
        }

        #region Predefined operators

        public T Mul(object value)
        {
            return Mul().Value(value);
        }

        public T Mul()
        {
            m_expressions.Add("mul");
            return (T)this;
        }

        public T Gt(object value)
        {
            return Gt().Value(value);
        }

        public T Gt()
        {
            m_expressions.Add("gt");
            return (T)this;
        }

        public T And(object value)
        {
            return And().Value(value);
        }

        public T And()
        {
            m_expressions.Add("and");
            return (T)this;
        }

        public T Or(object value)
        {
            return Or().Value(value);
        }

        public T Or()
        {
            m_expressions.Add("or");
            return (T)this;
        }

        public T Eq(object value)
        {
            return Eq().Value(value);
        }

        public T Eq()
        {
            m_expressions.Add("eq");
            return (T)this;
        }

        public T Ne(object value)
        {
            return Ne().Value(value);
        }

        public T Ne()
        {
            m_expressions.Add("ne");
            return (T)this;
        }

        public T Lt(object value)
        {
            return Lt().Value(value);
        }

        public T Lt()
        {
            m_expressions.Add("lt");
            return (T)this;
        }

        public T Lte(object value)
        {
            return Lte().Value(value);
        }

        public T Lte()
        {
            m_expressions.Add("lte");
            return (T)this;
        }

        public T Gte(object value)
        {
            return Gte().Value(value);
        }

        public T Gte()
        {
            m_expressions.Add("gte");
            return (T)this;
        }

        public T Div(object value)
        {
            return Div().Value(value);
        }

        public T Div()
        {
            m_expressions.Add("div");
            return (T)this;
        }

        public T Add(object value)
        {
            return Add().Value(value);
        }

        public T Add()
        {
            m_expressions.Add("add");
            return (T)this;
        }

        public T Sub(object value)
        {
            return Sub().Value(value);
        }

        public T Sub()
        {
            m_expressions.Add("sub");
            return (T)this;
        }

        #endregion
    }
}

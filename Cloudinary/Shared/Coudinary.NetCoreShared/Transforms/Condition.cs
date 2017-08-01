  using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    public class Condition
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
            { "||", "or" }
        };

        public static Dictionary<string, string> Parameters = new Dictionary<string, string>()
        {
            { "width", "w" },
            { "height", "h" },
            { "initial_width", "iw" },
            { "initial_height", "ih" },
            { "aspect_ratio", "ar" },
            { "aspectRatio", "ar" },
            { "page_count", "pc" },
            { "pageCount", "pc" },
            { "face_count", "fc" },
            { "faceCount", "fc" }
        };

        protected List<string> predicateList = null;

        public Condition()
        {
            predicateList = new List<string>();
        }

        /// <summary>
        /// Create a Condition object. The condition string will be translated to a serialized condition.
        /// </summary>
        /// <param name="condition">Condition in string format.</param>
        public Condition(string condition) : this()
        {
            if (!string.IsNullOrEmpty(condition))
            {
                predicateList.Add(Literal(condition));
            }
        }

        /// <summary>
        /// Convert incoming condition string into Literal in the URL format.
        /// e.g. "width > 0" => "w_lt_0"
        /// </summary>
        /// <param name="condition">Condition in string format.</param>
        private string Literal(string condition)
        {
            condition = Regex.Replace(condition, "[ _]+", "_");
            string pattern = string.Format("({0}|[=<>&|!]+)", string.Join("|", Parameters.Keys.ToArray()));
            return Regex.Replace(condition, pattern, m => GetOperatorReplacement(m.Value));
        }

        private string GetOperatorReplacement(string value)
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
        /// Parent transformation this condition belongs to.
        /// </summary>
        public Transformation Parent { get; private set; }

        public Condition SetParent(Transformation parent)
        {
            Parent = parent;
            return this;
        }

        /// <summary>
        /// Serialize a list of predicates.
        /// </summary>
        public string Serialize()
        {
            return string.Join("_", predicateList.ToArray());
        }

        public override string ToString()
        {
            return Serialize();
        }

        /// <summary>
        /// Create a predicate for binary operators
        /// </summary>
        protected Condition Predicate(string name, string @operator, string value)
        {
            if (Operators.ContainsKey(@operator))
            {
                @operator = Operators[@operator];
            }
            predicateList.Add(string.Format("{0}_{1}_{2}", name, @operator, value));
            return this;
        }

        public Condition And()
        {
            predicateList.Add("and");
            return this;
        }

        public Condition Or()
        {
            predicateList.Add("or");
            return this;
        }

        /// <summary>
        /// Terminates the definition of the condition and continue with Transformation definition.
        /// </summary>
        /// <returns>The Transformation object this Condition is attached to.</returns>
        public Transformation Then()
        {
            Parent.IfCondition(Serialize());
            return Parent;
        }

        public Condition Width(string @operator, object value)
        {
            predicateList.Add(string.Format("w_{0}_{1}", @operator, value));
            return this;
        }

        public Condition InitialWidth(string @operator, object value)
        {
            predicateList.Add(string.Format("iw_{0}_{1}", @operator, value));
            return this;
        }

        public Condition Height(string @operator, object value)
        {
            predicateList.Add(string.Format("h_{0}_{1}", @operator, value));
            return this;
        }

        public Condition InitialHeight(string @operator, object value)
        {
            predicateList.Add(string.Format("ih_{0}_{1}", @operator, value));
            return this;
        }

        public Condition AspectRatio(string @operator, string value)
        {
            predicateList.Add(string.Format("ar_{0}_{1}", @operator, value));
            return this;
        }

        public Condition FaceCount(string @operator, object value)
        {
            predicateList.Add(string.Format("fc_{0}_{1}", @operator, value));
            return this;
        }

        public Condition PageCount(string @operator, object value)
        {
            predicateList.Add(string.Format("pc_{0}_{1}", @operator, value));
            return this;
        }
    }
}

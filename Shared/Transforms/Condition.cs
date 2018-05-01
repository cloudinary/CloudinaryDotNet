using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    public class Condition : BaseExpression<Condition>
    {

        public Condition() : base() { }

        /// <summary>
        /// Create a Condition object. The condition string will be translated to a serialized condition.
        /// </summary>
        /// <param name="condition">Condition in string format.</param>
        public Condition(string condition) : this()
        {
            if (!string.IsNullOrEmpty(condition))
                m_expressions.Add(Normalize(condition));
        }


        /// <summary>
        /// Create a predicate for binary operators
        /// </summary>
        protected Condition Predicate(string name, string @operator, object value)
        {
            if (Operators.ContainsKey(@operator))
            {
                @operator = Operators[@operator];
            }
            m_expressions.Add(string.Format("{0}_{1}_{2}", name, @operator, value));
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
            return Predicate("w", @operator, value);
        }

        public Condition InitialWidth(string @operator, object value)
        {
            return Predicate("iw", @operator, value);
        }

        public Condition Height(string @operator, object value)
        {
            return Predicate("h", @operator, value);
        }

        public Condition InitialHeight(string @operator, object value)
        {
            return Predicate("ih", @operator, value);
        }

        public Condition AspectRatio(string @operator, string value)
        {
            return Predicate("ar", @operator, value);
        }

        public Condition FaceCount(string @operator, object value)
        {
            return Predicate("fc", @operator, value);
        }

        public Condition PageCount(string @operator, object value)
        {
            return Predicate("pc", @operator, value);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Represents Condition object that can be used in user defined variables and conditional transformations.
    /// </summary>
    public class Condition : BaseExpression<Condition>
    {
        /// <summary>
        /// Default parameterless constructor. Instantiates the <see cref="Condition"/> object.
        /// </summary>
        public Condition() { }

        /// <summary>
        /// Create a <see cref="Condition"/> object. The condition string will be translated to a serialized condition.
        /// </summary>
        /// <param name="condition">Condition in string format.</param>
        public Condition(string condition) : this()
        {
            if (!string.IsNullOrEmpty(condition))
                m_expressions.Add(Normalize(condition));
        }


        /// <summary>
        /// Creates a predicate for binary operators.
        /// </summary>
        /// <param name="name">A name of parameter.</param>
        /// <param name="operator">An operator.</param>
        /// <param name="value">A value.</param>
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

        /// <summary>
        /// Gets a predicate for width.
        /// </summary>
        /// <param name="operator">Applied operator.</param>
        /// <param name="value">The compared value.</param>
        public Condition Width(string @operator, object value)
        {
            return Predicate("w", @operator, value);
        }

        /// <summary>
        /// Gets a predicate for initial width.
        /// </summary>
        /// <param name="operator">Applied operator.</param>
        /// <param name="value">The compared value.</param>
        public Condition InitialWidth(string @operator, object value)
        {
            return Predicate("iw", @operator, value);
        }

        /// <summary>
        /// Gets a predicate for height.
        /// </summary>
        /// <param name="operator">Applied operator.</param>
        /// <param name="value">The compared value.</param>
        public Condition Height(string @operator, object value)
        {
            return Predicate("h", @operator, value);
        }

        /// <summary>
        /// Gets a predicate for initial height.
        /// </summary>
        /// <param name="operator">Applied operator.</param>
        /// <param name="value">The compared value.</param>
        public Condition InitialHeight(string @operator, object value)
        {
            return Predicate("ih", @operator, value);
        }

        /// <summary>
        /// Gets a predicate for aspect ratio.
        /// </summary>
        /// <param name="operator">Applied operator.</param>
        /// <param name="value">The compared value.</param>
        public Condition AspectRatio(string @operator, string value)
        {
            return Predicate("ar", @operator, value);
        }

        /// <summary>
        /// Get a predicate for face count.
        /// </summary>
        /// <param name="operator">Applied operator.</param>
        /// <param name="value">The compared value.</param>
        public Condition FaceCount(string @operator, object value)
        {
            return Predicate("fc", @operator, value);
        }

        /// <summary>
        /// Gets a predicate for page count.
        /// </summary>
        /// <param name="operator">Applied operator.</param>
        /// <param name="value">The compared value.</param>
        public Condition PageCount(string @operator, object value)
        {
            return Predicate("pc", @operator, value);
        }
    }
}

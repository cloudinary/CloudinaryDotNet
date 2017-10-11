using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Coudinary.NetCoreShared.Transforms
{
    /// <summary>
    /// Class that provide User Defined Variable functionality.
    /// See https://cloudinary.com/documentation/image_transformations#using_user_defined_variables for more info.
    /// </summary>
    public class Variable
    {
        string m_name;
        object m_value;

        /// <summary>
        /// User defined variable constructor by name and value.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="value">The value by which the variable will be initialized. Can be any scalar type.</param>
        public Variable(string name, object value)
        {
            if(!CheckName(name))
                throw new ArgumentException("The name can include only alphanumeric characters and must begin with a letter.");

            m_name = name;
            m_value = value ?? throw new ArgumentException("The value cannot be null."); 
        }

        /// <summary>
        /// User defined variable constructor by name and Expression.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="value">The Expression object by which the variable will be initialized.</param>
        public Variable(string name, Expression value)
        {
            if (!CheckName(name))
                throw new ArgumentException("The name can include only alphanumeric characters and must begin with a letter.");
                            
            m_name = name;
            m_value = value ?? throw new ArgumentException("The value cannot be null.");
        }

        /// <summary>
        /// User defined variable constructor by name and string list.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="multipleStringValues">The string list by which the variable will be initialized.</param>
        public Variable(string name, List<string> multipleStringValues)
        {
            if (!CheckName(name))
                throw new ArgumentException("The name can include only alphanumeric characters and must begin with a letter.");

            m_name = name;

            if (multipleStringValues != null)
                m_value = string.Join(":", multipleStringValues);
            else
                m_value = string.Empty;
        }

        private bool CheckName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (char.IsDigit(name[0]))
                return false;

            Regex alphanumericPattern = new Regex("^[a-zA-Z0-9]*$");

            return alphanumericPattern.IsMatch(name);
        }

        public string Key
        {
            get
            {
                return string.Format("${0}", m_name);
            }
        }

        public string TextLayerKey
        {
            get
            {
                return string.Format("$({0})", m_name);
            }
        }

        public string Value
        {
            get
            {
                return GetVariableValue();
            }
        }

        public override string ToString()
        {
            return string.Format("${0}_{1}", m_name, GetVariableValue());
        }

        private string GetVariableValue()
        {
            string result = m_value.ToString();

            var @types = new Dictionary<Type, Action> {
                { typeof(int), () => { result = m_value.ToString(); } },
                { typeof(double), () => {result = m_value.ToString(); } },
                { typeof(float), () => { result = m_value.ToString(); } },
                { typeof(long), () => { result = m_value.ToString(); } },
                { typeof(decimal), () => { result = m_value.ToString(); } },
                { typeof(Expression), () => { result = m_value.ToString(); } },
                { typeof(string), () => { result = string.Format("!{0}!", m_value.ToString()); } }
            };

            @types[m_value.GetType()]();

            return result;
        }
    }

    /// <summary>
    /// Represents expression object that can be used in user defined variables and conditional transformations.
    /// </summary>
    public class Expression
    {
        private string m_expression;
        private int m_expr_length;
        private ExpreAction m_lastAction = ExpreAction.Undefined;

        private enum Expressions
        {
            Add,
            Sub,
            Mul,
            Div,
            Mod
        }

        private enum ExpreAction
        {
            Value,
            Operation,
            Undefined
        }

        private string GetExpressionKey(Expressions exp)
        {
            return exp.ToString().ToLower();
        }

        /// <summary>
        /// Represent addition operation for expression.
        /// </summary>
        public Expression Add()
        {
            if (m_lastAction == ExpreAction.Operation || m_lastAction == ExpreAction.Undefined)
                throw new InvalidOperationException("Operations can be used only after value defenition.");
            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", GetExpressionKey(Expressions.Add));
            m_expr_length++;
            m_lastAction = ExpreAction.Operation;

            return this;
        }

        /// <summary>
        /// Represent subtraction operation for expression.
        /// </summary>
        public Expression Sub()
        {
            if (m_lastAction == ExpreAction.Operation || m_lastAction == ExpreAction.Undefined)
                throw new InvalidOperationException("Operations can be used only after value defenition.");
            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", GetExpressionKey(Expressions.Sub));
            m_expr_length++;
            m_lastAction = ExpreAction.Operation;

            return this;
        }

        /// <summary>
        /// Represent multiplication operation for expression.
        /// </summary>
        public Expression Mul()
        {
            if (m_lastAction == ExpreAction.Operation || m_lastAction == ExpreAction.Undefined)
                throw new InvalidOperationException("Operations can be used only after value defenition.");
            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", GetExpressionKey(Expressions.Mul));
            m_expr_length++;
            m_lastAction = ExpreAction.Operation;

            return this;
        }

        /// <summary>
        /// Represent division operation for expression.
        /// </summary>
        public Expression Div()
        {
            if (m_lastAction == ExpreAction.Operation || m_lastAction == ExpreAction.Undefined)
                throw new InvalidOperationException("Operations can be used only after value defenition.");
            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", GetExpressionKey(Expressions.Div));
            m_expr_length++;
            m_lastAction = ExpreAction.Operation;

            return this;
        }

        /// <summary>
        /// Represent modulo operation for expression.
        /// </summary>
        public Expression Mod()
        {
            if (m_lastAction == ExpreAction.Operation || m_lastAction == ExpreAction.Undefined)
                throw new InvalidOperationException("Operations can be used only after value defenition.");
            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", GetExpressionKey(Expressions.Mod));
            m_expr_length++;
            m_lastAction = ExpreAction.Operation;

            return this;
        }

        /// <summary>
        /// Inserts arbitrary value into Expression.
        /// </summary>
        /// <param name="value">The value that will be added into expression. The type of value can be scalar only.</param>
        public Expression Value(object value)
        {
            if (m_lastAction == ExpreAction.Value)
                throw new InvalidOperationException("Value definition can be used only at expression start or after operations.");

            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", value.ToString());
            m_expr_length++;
            m_lastAction = ExpreAction.Value;

            return this;
        }

        /// <summary>
        /// Inserts variable into Expression.
        /// </summary>
        /// <param name="var">The variable that will be added into expression.</param>
        public Expression Value(Variable var)
        {
            if (m_lastAction == ExpreAction.Value)
                throw new InvalidOperationException("Value definition can be used only at expression start or after operations.");

            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", var.Key);
            m_expr_length++;
            m_lastAction = ExpreAction.Value;

            return this;
        }

        public override string ToString()
        {
            return m_expression;
        }
    }

    /// <summary>
    /// Predefined variables storage class that can be used in transformations.
    /// </summary>
    public static class PredefinedVariable
    {

        public static string Width
        {
            get { return "w"; }
        }

        public static string Height
        {
            get { return "h"; }
        }

        public static string InitialWidth
        {
            get { return "iw"; }
        }

        public static string InitialHeight
        {
            get { return "ih"; }
        }

        public static string PageCount
        {
            get { return "pc"; }
        }

        public static string FaceCount
        {
            get { return "fc"; }
        }

        public static string IllustrationScore
        {
            get { return "ils"; }
        }

        public static string CurentPageIndex
        {
            get { return "cp"; }
        }

        public static string Tags
        {
            get { return "tags"; }
        }

        public static string XOffset
        {
            get { return "px"; }
        }

        public static string YOffset
        {
            get { return "py"; }
        }

        public static string AspectRatio
        {
            get { return "ar"; }
        }

        public static string AspectRatioInitial
        {
            get { return "iar"; }
        }
    }
}

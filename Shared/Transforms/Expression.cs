using Coudinary.NetCoreShared.Transforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Transforms
{
    /// <summary>
    /// Represents expression object that can be used in user defined variables and conditional transformations.
    /// </summary>
    public class Expression
    {
        private string m_expression;
        private int m_expr_length;
        private ExprAction m_lastAction = ExprAction.Undefined;

        public enum Operation
        {
            Add,
            Sub,
            Mul,
            Div,
            Mod
        }

        private enum ExprAction
        {
            Value,
            Operation,
            Undefined
        }

        private string GetExpressionKey(Operation operation)
        {
            return operation.ToString().ToLower();
        }

        /// <summary>
        /// Represent arithmetic operation for expression.
        /// </summary>
        public Expression Operator(Operation operation)
        {
            if (m_lastAction == ExprAction.Operation || m_lastAction == ExprAction.Undefined)
                throw new InvalidOperationException("Operations can be used only after value defenition.");
            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", GetExpressionKey(operation));
            m_expr_length++;
            m_lastAction = ExprAction.Operation;

            return this;
        }

        /// <summary>
        /// Inserts arbitrary value into Expression.
        /// </summary>
        /// <param name="value">The value that will be added into expression. The type of value can be scalar only.</param>
        public Expression Value(object value)
        {
            if (m_lastAction == ExprAction.Value)
                throw new InvalidOperationException("Value definition can be used only at expression start or after operations.");

            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", value.ToString());
            m_expr_length++;
            m_lastAction = ExprAction.Value;

            return this;
        }

        /// <summary>
        /// Inserts variable into Expression.
        /// </summary>
        /// <param name="var">The variable that will be added into expression.</param>
        public Expression Value(Variable var)
        {
            if (m_lastAction == ExprAction.Value)
                throw new InvalidOperationException("Value definition can be used only at expression start or after operations.");

            m_expression += string.Format((m_expr_length > 0 ? "_" : string.Empty) + "{0}", var.Key);
            m_expr_length++;
            m_lastAction = ExprAction.Value;

            return this;
        }

        public override string ToString()
        {
            return m_expression;
        }
    }
}

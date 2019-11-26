using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// An operation between two values (if not unary)
    /// </summary>
    public class Operator : IValue
    {
        #region Public Enums

        /// <summary>
        /// Enumeration that lists the available operators
        /// </summary>
        public enum Tag
        {
            /// <summary>
            /// the MINUS operator has only a right value and no left value.
            /// </summary>
            MINUS,

            /// <summary>
            /// Addition
            /// </summary>
            ADD,

            /// <summary>
            /// Subtraction
            /// </summary>
            SUB,

            /// <summary>
            /// Division (floating point)
            /// </summary>
            DIV,

            /// <summary>
            /// Multiplication
            /// </summary>
            MULT,

            /// <summary>
            /// Division (integer)
            /// </summary>
            INT_DIV,

            /// <summary>
            /// Modulo
            /// </summary>
            MOD,

            /// <summary>
            /// and (&amp;&amp;)
            /// </summary>
            AND,

            /// <summary>
            /// or (||)
            /// </summary>
            OR,

            /// <summary>
            /// the NOT operator has only a right value and no left value.
            /// </summary>
            NOT,

            /// <summary>
            /// equal (==)
            /// </summary>
            EQUAL,

            /// <summary>
            /// different (!=)
            /// </summary>
            DIFFRENT,

            /// <summary>
            /// bigger than (&gt;)
            /// </summary>
            BIGGER,

            /// <summary>
            /// bigger or equal than (&gt;=)
            /// </summary>
            BIGGER_EQUAL,

            /// <summary>
            /// smaller than (&lt;)
            /// </summary>
            SMALLER,

            /// <summary>
            /// smaller or eequal than (&lt;=)
            /// </summary>
            SMALLER_EQUAL
        }

        #endregion Public Enums

        #region Public Properties

        /// <summary>
        /// Left operand of the operation
        /// </summary>
        public IValue Left { get; set; }

        /// <summary>
        /// The type of operator this object is
        /// </summary>
        public Tag OperatorType { get; set; }

        /// <summary>
        /// Right operand of the operation
        /// </summary>
        public IValue Right { get; set; }

        #endregion Public Properties

        #region Internal Properties

        VarType IValue.Type => Type;

        #endregion Internal Properties

        internal VarType Type { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            if (Left != null)
            {
                textWriter.Write('(');
                Left.WriteILA(textWriter);
            }
            switch (OperatorType)
            {
                case Tag.ADD:
                    textWriter.Write(" + ");
                    break;

                case Tag.MINUS:
                    textWriter.Write("-");
                    break;

                case Tag.SUB:
                    textWriter.Write(" - ");
                    break;

                case Tag.DIV:
                    textWriter.Write(" / ");
                    break;

                case Tag.MULT:
                    textWriter.Write(" * ");
                    break;

                case Tag.INT_DIV:
                    textWriter.Write(" div ");
                    break;

                case Tag.MOD:
                    textWriter.Write(" mod ");
                    break;

                case Tag.AND:
                    textWriter.Write(" et ");
                    break;

                case Tag.OR:
                    textWriter.Write(" ou ");
                    break;

                case Tag.NOT:
                    textWriter.Write("non ");
                    break;

                case Tag.EQUAL:
                    textWriter.Write(" = ");
                    break;

                case Tag.DIFFRENT:
                    textWriter.Write(" != ");
                    break;

                case Tag.BIGGER:
                    textWriter.Write(" > ");
                    break;

                case Tag.BIGGER_EQUAL:
                    textWriter.Write(" >= ");
                    break;

                case Tag.SMALLER:
                    textWriter.Write(" < ");
                    break;

                case Tag.SMALLER_EQUAL:
                    textWriter.Write(" <= ");
                    break;
            }
            Right.WriteILA(textWriter);
            if (Left != null)
                textWriter.Write(')');
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}
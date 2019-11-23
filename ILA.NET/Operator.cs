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
        /// Right operand of the operation
        /// </summary>
        public IValue Right { get; set; }

        #endregion Public Properties

        #region Internal Properties

        VarType IValue.Type => Type;

        #endregion Internal Properties

        internal VarType Type { get; set; }

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
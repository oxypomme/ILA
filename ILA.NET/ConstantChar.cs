using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A hard coded char
    /// </summary>
    public class ConstantChar : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ConstantChar()
        {
            Type = GenericType.Char;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type { get => Type; }

        /// <summary>
        /// Value of the char
        /// </summary>
        public char Value { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write('\'');
            textWriter.Write(Value);
            textWriter.Write('\'');
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write("'" + Value.ToString() + "'");
        }

        #endregion Public Properties
    }
}
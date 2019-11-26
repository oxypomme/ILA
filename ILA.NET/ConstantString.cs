using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A hard coded string value
    /// </summary>
    public class ConstantString : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ConstantString()
        {
            Type = GenericType.String;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type => Type;

        /// <summary>
        /// Value of the string
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write('"');
            textWriter.Write(Value);
            textWriter.Write('"');
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write("\"" + Value.ToString() + "\"");
        }

        #endregion Public Properties
    }
}